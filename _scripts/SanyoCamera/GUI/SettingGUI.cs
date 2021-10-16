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
    public GUIStyleManager Style { get; } = new GUIStyleManager();
    public GuiTab Tab { get; } = new GuiTab();

    private int width = 640;
    private int height = 330;
    private CameraManager camera;
    private LightManager light;
    private ConfigManager config;
    private Core core;

    private void OnEnable() {
      Tab.Enabled = camera.Setting = true;
    }
    private void OnDisable() {
      Tab.Enabled = camera.Setting = false;
    }

    private void Awake() {
      if(this.gameObject.name.StartsWith("SanyoCameraScript") == false){
        UnityEngine.Object.DestroyImmediate(this);
        return;
      }
      this.enabled = false;
      this.Style.LoadStyles(Application.dataPath + "/../UserData/_scripts/SanyoCamera/.Style/");
    }

    public void Init(Core core, CameraManager camera, LightManager light, ConfigManager config){
      this.camera = camera;
      this.light = light;
      this.config = config;
      this.core = core;
      
      this.Tab.Add(new CameraTab(Style, camera));
      this.Tab.Add(new LightTab(Style, light, camera));
      this.Tab.Add(new ConfigTab(Style, core));
    }
    private void OnDestroy() {
      this.Style.Dispose();
    }
    
    public void OnGUI()
    {
      UnityEngine.GUI.BeginGroup(new Rect(Screen.width / 2 - width / 2-3, Screen.height / 2 - height / 2-3, width+6, height+6), Style["window"]);
      GUILayout.BeginArea(new Rect(3,3, width, height));  
      this.Tab.Mount(Style);
      GUILayout.EndArea();
      UnityEngine.GUI.EndGroup();
    }
  }
}