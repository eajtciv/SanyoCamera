using System;
using System.Collections.Generic;
using UnityEngine;
using SanyoCamera;
using SanyoCamera.Camera;
using SanyoLib;

namespace SanyoCamera.GUI
{
  public class ConfigTab : IGuiTab
  {
    public string Name() => "Config";

    public void OnTabEnter(){}
    
    public void OnTabExit(){}

    private Core core;
    private ConfigManager config;
    private GUIStyleManager style;
    private KeyInputer inputer = new KeyInputer();
    private string listenTarget = null;
    public Dictionary<string, string> KeyItemNames = new Dictionary<string, string>();

    public ConfigTab(GUIStyleManager style, Core core){
      this.style = style;
      this.core = core;
      this.config = core.config;

      this.KeyItemNames.Add("CAMERA_MENU_KEY", "Camera Menu");
      this.KeyItemNames.Add("CAMERA_SWITCH_KEY", "Camera Switch");
      this.KeyItemNames.Add("CAMERA_SMOOTH_KEY", "Camera Smooth Move");
      this.KeyItemNames.Add("CAMERA_HIGHMOVE_KEY", "Camera HightSpeed Move");
      this.KeyItemNames.Add("LIGHT_ADD_KEY", "Light Add");
      this.KeyItemNames.Add("HD_SCREENSHOT_KEY", "Script Screenshot");
    }

    public void Mount(){
      inputer.Listen(key => {
        config.Key[listenTarget] = key;
        listenTarget = null;
        config.ConfigSave();
      });

      GUILayout.Space(10);

      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      {
        GUILayout.BeginVertical(style["fill"], GUILayout.Width(620));
        GUILayout.Label("Key Config", style["label"]);
        GUILayout.BeginHorizontal(style["fill-controll"]);
        {
          List<string> keyList = new List<string>(KeyItemNames.Keys);
          GUILayout.BeginVertical();
          for(int i=0;i < keyList.Count;){
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            while(i < keyList.Count){
              GUILayout.BeginHorizontal(GUILayout.Width(260));
              GUILayout.Label(KeyItemNames[keyList[i]]);
              if(GUILayout.Button(inputer.ToName(config.Key[keyList[i]], "None", "[PRESS]...", (keyList[i] == listenTarget)), this.style["button-big"], GUILayout.Width(100))){
                listenTarget = keyList[i];
                inputer.Listening = true;
              }
              GUILayout.EndHorizontal();
              i++;
              if(i%2 == 0)
                break;
              else
                GUILayout.FlexibleSpace();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
          }
          GUILayout.EndVertical();
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
      }
      GUILayout.FlexibleSpace();
      GUILayout.EndHorizontal();

      GUILayout.Space(10);

      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      {
        GUILayout.BeginVertical(style["fill"], GUILayout.Width(620));
        GUILayout.Label("Script Screenshot Config", style["label"]);
        GUILayout.BeginHorizontal(style["fill-controll"]);
        {
          GUILayout.BeginVertical();

          GUILayout.BeginHorizontal(GUILayout.Width(300));
          GUILayout.Label("Render Screenshot: ", GUILayout.Width(120));
          bool NowMode = GUILayout.Toggle (config.ScreenshotRenderMode, config.ScreenshotRenderMode?" Enable":" Disable"); 
          GUILayout.EndHorizontal();

          GUILayout.BeginHorizontal(GUILayout.Width(300));
          GUILayout.Label("Resolution: ", GUILayout.Width(120));
          int Resolution = config.ScreenshotResolution;
          string SSWidth = this.config.ScreenshotWidth.ToString();
          string SSHeight = this.config.ScreenshotHeight.ToString();
          if(config.ScreenshotRenderMode){
            SSWidth = GUILayout.TextField(SSWidth, style["text"], GUILayout.Width(60));
            GUILayout.Label("X", GUILayout.Width(10));
            SSHeight = GUILayout.TextField(SSHeight, style["text"], GUILayout.Width(60));
          }else{
            GUILayout.BeginHorizontal(this.style["frame"]);
            Resolution = GUILayout.SelectionGrid(config.ScreenshotResolution-1, new string[] { "x1", "x2", "x3", "x4" }, 4, this.style["select"])+1;
            GUILayout.EndHorizontal();
          }

          GUILayout.EndHorizontal();

          GUILayout.BeginHorizontal();
          GUILayout.Label("Path: ", GUILayout.Width(120));
          string Path = GUILayout.TextField(this.config.ScreenshotPath, style["text"], GUILayout.Width(380));
          if(GUILayout.Button("Open Dir" + "ectory", this.style["button"], GUILayout.Width(100))){
            System.Diagnostics.Process.Start(this.config.ScreenshotPath);
          }
          GUILayout.EndHorizontal();


          GUILayout.EndVertical();


          if(config.ScreenshotRenderMode != NowMode 
          || config.ScreenshotResolution != Resolution
          || SSWidth != this.config.ScreenshotWidth.ToString()
          || SSHeight != this.config.ScreenshotHeight.ToString()
          || config.ScreenshotPath != Path
          )
          {
            config.ScreenshotRenderMode = NowMode;
            config.ScreenshotResolution = Resolution;
            this.config.ScreenshotPath = Path;
            int width, height;
            if(int.TryParse( SSWidth, out width))
              this.config.ScreenshotWidth = width;
            if(int.TryParse( SSHeight, out height))
              this.config.ScreenshotHeight = height;

            config.ConfigSave();
          }
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
      }
      GUILayout.FlexibleSpace();
      GUILayout.EndHorizontal();

      GUILayout.FlexibleSpace();

      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      GUILayout.Label(string.Format("Version {0}", this.core.Version));
      GUILayout.EndHorizontal();
    }
  }
}