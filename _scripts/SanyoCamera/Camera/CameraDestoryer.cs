using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SanyoCamera.Camera.Tracker;


namespace SanyoCamera.Camera
{
  public class CameraDestoryer : MonoBehaviour
  {
    private CameraManager manager;

    public static CameraDestoryer Create(CameraManager manager){
      GameObject gameObject = new GameObject("SanyoCamera-CameraDestoryer");
      CameraDestoryer destroyer = gameObject.AddComponent<CameraDestoryer>();
      destroyer.manager = manager;
      return destroyer;
    }

    public void Harmless(){
      this.manager = null;
      UnityEngine.Object.DestroyImmediate(this.gameObject);
    }

    public void OnDestroy()
    {
      if(this.manager != null)
        this.manager.DestroyCamera();
    }
  }
}