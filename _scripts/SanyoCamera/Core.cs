using System;
using UnityEngine;
using UnityEngine.EventSystems;
using SanyoCamera;
using SanyoCamera.Camera;
using SanyoCamera.Light;
using SanyoCamera.GUI;

namespace SanyoCamera
{
  public class Core : MonoBehaviour
  {
    public string Version;

    public ConfigManager config { private set; get; }
    public LightManager light { private set; get; }
    public CameraManager camera { private set; get; }

    public SettingGUI Gui { private set; get; }

    private void Awake() {
      if(this.gameObject.name.StartsWith("SanyoCameraScript") == false){
        UnityEngine.Object.DestroyImmediate(this);
        return;
      }
      this.config = new ConfigManager();
      this.config.ConfigSaveDefault();
      this.config.ConfigLoad();
      this.camera = new CameraManager(config);
      this.light = new LightManager();
      this.Gui = this.gameObject.AddComponent<SettingGUI>();
      this.Gui.camera = camera;
      this.Gui.light = light;
      this.Gui.config = config;
      this.Gui.core = this;
      this.Gui.Init();
      this.camera.RootObject = this.gameObject;
    }

    public void Update(){
      if ((EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null))
        return;

      if (Input.GetKeyDown(config.Key["CAMERA_MENU_KEY"])){
        if(CameraManager.GetMachine() == null)
          return;
        // Cursor [None / Locked] switch
        new AutoPilot().FreeCursor(this.Gui.enabled = !this.Gui.enabled);
      }

      this.camera.Update();

      if(this.Gui.enabled)
        return;

      this.light.ListenSwitch();

      if (Input.GetKeyDown(config.Key["CAMERA_SWITCH_KEY"])){
        if(this.camera.Enabled() == false){
          this.camera.CreateCamera();
        } else {
          this.camera.DestroyCamera();
        }
      }

      if (Input.GetKeyDown(config.Key["CAMERA_SMOOTH_KEY"])){
        this.camera.SmoothMove = !this.camera.SmoothMove;
      }

      if (Input.GetKeyDown(config.Key["LIGHT_ADD_KEY"])){
        this.light.Create(CameraManager.GetMachine());
      }

      if (Input.GetKeyDown(config.Key["HD_SCREENSHOT_KEY"])){
        if (!SanyoLib.DirUtil.Exists(config.ScreenshotPath))
          SanyoLib.DirUtil.Create(config.ScreenshotPath);
        string filePath = string.Format("{0}/{1}.png", config.ScreenshotPath, DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));
        if(config.ScreenshotRenderMode){
          ScreenCaptureUtil.Capture(filePath, config.ScreenshotWidth, config.ScreenshotHeight, camera.Camera, true);
        }else{
          Application.CaptureScreenshot(filePath, config.ScreenshotResolution);
        }
      }
    }
    
    public void OnDestroy()
    {
      new AutoPilot().FreeCursor(false);
      this.camera.DestroyCamera();
      this.light.DestroyLights();
    }
  }
}