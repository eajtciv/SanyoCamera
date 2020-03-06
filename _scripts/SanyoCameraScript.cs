using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using SanyoCamera;
/*
 * @anther sanyo[JP]
 * @version v1.1.0
 */
public class SanyoCameraScript : UserScript
{
  private const string Version = "1.1.0-2 beta";

  private GameObject ScriptObject;

  public override string GetUserName()
  {
    using (StreamReader sr = new StreamReader(Application.dataPath + "/../UserData/User.mcsd"))
      return Regex.Unescape(Regex.Match(sr.ReadToEnd(), "\"userName\":\"(?<name>.*?)\"").Groups["name"].Value);
  }
  
  public override void OnStart(AutoPilot ap)
  {
    string name = string.Format("SanyoCameraScript v{0}", Version);
    DeepDestory(GameObject.Find(name));
    this.ScriptObject = new GameObject(name);
    Core core = this.ScriptObject.AddComponent<Core>();
    core.Version = Version;
    GameObject.DontDestroyOnLoad(this.ScriptObject);
    ap.LogC("Loaded: "+this.ScriptObject.name);
  }

  public void OnDestroy()
  {
    //DeepDestory(this.ScriptObject);
  }
  
  private void DeepDestory(GameObject gameObject){
    if(gameObject == null)
      return;
    foreach(Component component in gameObject.GetComponents<Component>()) {
      UnityEngine.Object.DestroyImmediate(component);
    }
    UnityEngine.Object.DestroyImmediate(gameObject);
  }
}