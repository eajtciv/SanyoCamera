using System;
using UnityEngine;
using SanyoCamera;
using SanyoCamera.Camera;
using SanyoCamera.Light;
using SanyoLib;

namespace SanyoCamera.GUI
{
  public class SettingGUI : MonoBehaviour
  {
    private GUIStyleManager style = new GUIStyleManager();
    private int width = 640;
    private int height = 330;
    private GuiTab tab = new GuiTab();
    private CameraManager camera;
    private LightManager light;
    private ConfigManager config;
    private Core core;

    private void OnEnable() {
      tab.Enabled = camera.Setting = true;
    }
    private void OnDisable() {
      tab.Enabled = camera.Setting = false;
    }

    private void Awake() {
      if(this.gameObject.name.StartsWith("SanyoCameraScript") == false){
        UnityEngine.Object.DestroyImmediate(this);
        return;
      }
      this.enabled = false;
      this.style.LoadStyles(Application.dataPath + "/../UserData/_scripts/SanyoCamera/.style/");
    }

    public void Init(Core core, CameraManager camera, LightManager light, ConfigManager config){
      this.camera = camera;
      this.light = light;
      this.config = config;
      this.core = core;
      
      this.tab.Add(new CameraTab(style, camera));
      this.tab.Add(new LightTab(style, light, camera));
      this.tab.Add(new ConfigTab(style, core));
    }
    private void OnDestroy() {
      this.style.Dispose();
    }
    
    public void OnGUI()
    {
      UnityEngine.GUI.BeginGroup(new Rect(Screen.width / 2 - width / 2-3, Screen.height / 2 - height / 2-3, width+6, height+6), style["window"]);
      GUILayout.BeginArea(new Rect(3,3, width, height));  
      this.tab.Mount(style);
      GUILayout.EndArea();
      UnityEngine.GUI.EndGroup();
    }
  }
}