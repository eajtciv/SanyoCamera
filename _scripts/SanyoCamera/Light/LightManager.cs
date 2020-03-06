using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SanyoCamera.Light
{
  public class LightManager : List<GameObject>
  {
    public float DefaultRange {get;set;} = 150;
    public float DefaultSpotAngle {get;set;} = 100;
    public float DefaultIntensity {get;set;} = 3f;
    public Color DefaultColor {get;set;} = new Color((255f / 255f), (220f / 255f), (160f / 255f));
    public Vector3 DefaultPosition {get;set;} = Vector3.zero;
    public Quaternion DefaultRotation {get;set;} = Quaternion.Euler(Vector3.zero);
    public Transform DefaultAttach {get;set;} = null;

    public UnityEngine.Light Create(Transform transform){
      GameObject gameObject = new GameObject();
      UnityEngine.Light light = gameObject.AddComponent<UnityEngine.Light>();
      light.type = LightType.Spot;
      light.range = DefaultRange;
      light.spotAngle = DefaultSpotAngle;
      light.intensity = DefaultIntensity;
      light.color = DefaultColor;
      light.renderMode = LightRenderMode.ForcePixel;
      this.Add(gameObject);
      if(DefaultAttach == null){
        if(transform != null){
          gameObject.transform.localPosition = transform.position;
          gameObject.transform.localRotation = transform.rotation;
        } else {
          gameObject.transform.localPosition = DefaultPosition;
          gameObject.transform.localRotation = DefaultRotation;
        }
      }else{
        gameObject.transform.parent = DefaultAttach;
        gameObject.transform.localPosition = DefaultPosition;
        gameObject.transform.localRotation = DefaultRotation;
      }
      return light;
    }

    public void DestroyLights() {
      foreach(GameObject gameObject in this){
        UnityEngine.Object.DestroyImmediate(gameObject);
      }
      this.Clear();
    }

    public void ListenSwitch() {
      foreach (GameObject gameObject in this)
      {
        if (gameObject == null)
          continue;
        LightSwitcher switcher = gameObject.GetComponent<LightSwitcher>();
        if (switcher != null && Input.GetKeyDown(switcher.Key)){
          UnityEngine.Light light = gameObject.GetComponent<UnityEngine.Light>();
          light.enabled = !light.enabled;
        }
      }
    }

    public void SetSwitch(UnityEngine.Light light, KeyCode code) {
      LightSwitcher switcher = light.gameObject.GetComponent<LightSwitcher>();
      if(code != KeyCode.None){
        if(switcher == null)
          switcher = light.gameObject.AddComponent<LightSwitcher>();
        switcher.Key = code;
      } else {
        UnityEngine.Object.DestroyImmediate(switcher);
      }
    }

    public KeyCode GetSwitch(UnityEngine.Light light) {
      if(light == null)
        return KeyCode.None;
      LightSwitcher switcher = light.gameObject.GetComponent<LightSwitcher>();
      if(switcher != null)
        return switcher.Key;
      else
        return KeyCode.None;
    }
  }
}