using System;
using System.Collections.Generic;
using UnityEngine;

namespace SanyoCamera.GUI
{
  public class KeyInputer
  {
    private bool listening = false;
    public bool Listening {
      get{
        return this.listening;
      }
      set{
        if(value)
          this.escapeWait = -1;
        this.listening = value;
      }
    }
    private float escapeWait = 0;

    public void Listen(Action<KeyCode> action){
      if(Listening == false)
        return;
      if(Input.GetKey(KeyCode.Escape)){
        if(escapeWait == -1)
            escapeWait = Time.realtimeSinceStartup+.5f;
        else if(escapeWait < Time.realtimeSinceStartup){
          Listening = false;
          escapeWait = 0;
          action.Invoke(KeyCode.None);
        }
      }else{
        if(escapeWait != -1){
          Listening = false;
          escapeWait = 0;
          action.Invoke(KeyCode.Escape);
        }else if(Input.anyKeyDown){
          foreach (KeyCode key in Enum.GetValues(typeof(KeyCode))){
            if (Input.GetKeyDown(key) && key != KeyCode.None){
              Listening = false;
              escapeWait = 0;
              action.Invoke(key);
            }
          }
        }
      }
    }

    public string ToName(KeyCode code, string name, string listenName){
      return ToName(code, name, listenName, this.Listening);
    }

    public string ToName(KeyCode code, string name, string listenName, bool listening){
      if(listening)
        return listenName;
      if(code == KeyCode.None)
        return name;
      return code.ToString();
    }
  }
}