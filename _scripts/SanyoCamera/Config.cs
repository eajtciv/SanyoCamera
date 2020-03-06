using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SanyoCamera
{
  [Serializable]
  public class Config
  {
    public KeyCode CAMERA_SWITCH_KEY = KeyCode.T;
    public KeyCode CAMERA_HIGHMOVE_KEY = KeyCode.LeftAlt;
    public KeyCode CAMERA_SMOOTH_KEY = KeyCode.None;
    public KeyCode CAMERA_MENU_KEY = KeyCode.Comma;
    public KeyCode LIGHT_ADD_KEY = KeyCode.None;
    public KeyCode HD_SCREENSHOT_KEY = KeyCode.Minus;

    public string HD_SCREENSHOT_DIR = Path.GetFullPath(Application.dataPath + "/../HD_SS");
    public int HD_SCREENSHOT_WIDTH = 4096;
    public int HD_SCREENSHOT_HEIGHT = 2160;
    public int HD_SCREENSHOT_RESOLUTION = 1;
    public bool HD_SCREENSHOT_MODE = true;
  }
}