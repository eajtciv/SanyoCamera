using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using SanyoLib;

namespace SanyoCamera
{
  public class ConfigManager
  {
    public Dictionary<string, KeyCode> Key = new Dictionary<string, KeyCode>();
    public string ScreenshotPath { set; get; }
    public int ScreenshotWidth { set; get; }
    public int ScreenshotHeight { set; get; }
    public int ScreenshotResolution { set; get; }
    public bool ScreenshotRenderMode { set; get; }

    private Config config = new Config();
    private string configFile = Application.dataPath + "/../UserData/_scripts/config/SanyoCamera.json";

    public void ConfigSaveDefault(){
      if(File.Exists(configFile) == false){
        if (!DirUtil.Exists(Application.dataPath + "/../UserData/_scripts/config/"))
          DirUtil.Create(Application.dataPath + "/../UserData/_scripts/config/");
        this.ConfigRead(this.config);
        this.ConfigSave();
      }
    }

    public void ConfigSave()
    {
      this.config.CAMERA_SWITCH_KEY = this.Key["CAMERA_SWITCH_KEY"];
      this.config.CAMERA_MENU_KEY = this.Key["CAMERA_MENU_KEY"];
      this.config.LIGHT_ADD_KEY = this.Key["LIGHT_ADD_KEY"];
      this.config.CAMERA_SMOOTH_KEY = this.Key["CAMERA_SMOOTH_KEY"];
      this.config.CAMERA_HIGHMOVE_KEY = this.Key["CAMERA_HIGHMOVE_KEY"];
      this.config.HD_SCREENSHOT_KEY = this.Key["HD_SCREENSHOT_KEY"];

      this.config.HD_SCREENSHOT_DIR = this.ScreenshotPath;
      this.config.HD_SCREENSHOT_WIDTH = this.ScreenshotWidth;
      this.config.HD_SCREENSHOT_HEIGHT = this.ScreenshotHeight;
      this.config.HD_SCREENSHOT_RESOLUTION = this.ScreenshotResolution;
      this.config.HD_SCREENSHOT_MODE = this.ScreenshotRenderMode;
      
      using (StreamWriter stream = new StreamWriter(configFile))
      {
        stream.Write(JsonUtility.ToJson(this.config));
      }
    }

    public void ConfigLoad()
    {
      using (StreamReader stream = new StreamReader(configFile))
      {
        this.config = JsonUtility.FromJson<Config>(stream.ReadToEnd());
        ConfigRead(this.config);
      }
    }

    public void ConfigRead(Config config)
    {
      this.Key["CAMERA_SWITCH_KEY"] = config.CAMERA_SWITCH_KEY;
      this.Key["CAMERA_MENU_KEY"] = config.CAMERA_MENU_KEY;
      this.Key["LIGHT_ADD_KEY"] = config.LIGHT_ADD_KEY;
      this.Key["CAMERA_SMOOTH_KEY"] = config.CAMERA_SMOOTH_KEY;
      this.Key["CAMERA_HIGHMOVE_KEY"] = config.CAMERA_HIGHMOVE_KEY;
      this.Key["HD_SCREENSHOT_KEY"] = config.HD_SCREENSHOT_KEY;

      this.ScreenshotPath = config.HD_SCREENSHOT_DIR;
      this.ScreenshotWidth = config.HD_SCREENSHOT_WIDTH;
      this.ScreenshotHeight = config.HD_SCREENSHOT_HEIGHT;
      this.ScreenshotResolution = config.HD_SCREENSHOT_RESOLUTION;
      this.ScreenshotRenderMode = config.HD_SCREENSHOT_MODE;
    }
  }
}