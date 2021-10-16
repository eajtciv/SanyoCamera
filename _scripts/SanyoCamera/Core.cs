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

    public ConfigManager Config { private set; get; }
    public LightManager Light { private set; get; }
    public CameraManager Camera { private set; get; }

    public SettingGUI Gui { private set; get; }

    private void Awake() {
      if(this.gameObject.name.StartsWith("SanyoCameraScript") == false){
        UnityEngine.Object.DestroyImmediate(this);
        return;
      }
      this.Config = new ConfigManager();
      this.Config.ConfigSaveDefault();
      this.Config.ConfigLoad();
      this.Camera = new CameraManager(Config);
      this.Light = new LightManager();
      this.Gui = this.gameObject.AddComponent<SettingGUI>();
      this.Gui.Init(this, this.Camera, this.Light, this.Config);
      this.Camera.RootObject = this.gameObject;
    }

    public void Update(){
      if ((EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null))
        return;

      if (Input.GetKeyDown(Config.Key["CAMERA_MENU_KEY"])){
        if(CameraManager.GetMachine() == null)
          return;
        // Cursor [None / Locked] switch
        new AutoPilot().FreeCursor(this.Gui.enabled = !this.Gui.enabled);
      }

      this.Camera.Update();

      if(this.Gui.enabled)
        return;

      this.Light.ListenSwitch();

      if (Input.GetKeyDown(Config.Key["CAMERA_SWITCH_KEY"])){
        if(this.Camera.Enabled() == false){
          this.Camera.CreateCamera();
        } else {
          this.Camera.DestroyCamera();
        }
      }

      if (Input.GetKeyDown(Config.Key["CAMERA_SMOOTH_KEY"])){
        this.Camera.SmoothMove = !this.Camera.SmoothMove;
      }

      if (Input.GetKeyDown(Config.Key["Light_ADD_KEY"])){
        this.Light.Create(CameraManager.GetMachine());
      }

      if (Input.GetKeyDown(Config.Key["HD_SCREENSHOT_KEY"])){
        if (!SanyoLib.DirUtil.Exists(Config.ScreenshotPath))
          SanyoLib.DirUtil.Create(Config.ScreenshotPath);
        string filePath = string.Format("{0}/{1}.png", Config.ScreenshotPath, DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));
        if(Config.ScreenshotRenderMode){
          ScreenCaptureUtil.Capture(filePath, Config.ScreenshotWidth, Config.ScreenshotHeight, this.Camera.Camera, true);
        }else{
          Application.CaptureScreenshot(filePath, Config.ScreenshotResolution);
        }
      }
    }
    
    public void OnDestroy()
    {
      new AutoPilot().FreeCursor(false);
      this.Camera.DestroyCamera();
      this.Light.DestroyLights();
    }
  }
}