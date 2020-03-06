using System;
using System.Collections.Generic;
using UnityEngine;
using SanyoCamera;
using SanyoLib;

namespace SanyoCamera.GUI
{
  public class Vector3Controll
  {
    private string Name;
    private int accuracy = 0; 
    private Vector3 vector3;
    public Vector3 Vector3 {
      set {
        this.vector3 = value;
        this.x = this.vector3.x.ToString("#0."+new string('0', accuracy));
        this.y = this.vector3.y.ToString("#0."+new string('0', accuracy));
        this.z = this.vector3.z.ToString("#0."+new string('0', accuracy));
      }
      get {
        float x,y,z;
        return new Vector3(
          float.TryParse(this.x, out x) ? x : vector3.x,
          float.TryParse(this.y, out y) ? y : vector3.y,
          float.TryParse(this.z, out z) ? z : vector3.z
         );
      }
    }
    private string x, y, z;
    public Vector3Controll(string Name){
      this.Name = Name;
      this.Vector3 = Vector3.zero;
    }
    public Vector3Controll(string Name, int accuracy){
      this.Name = Name;
      this.Vector3 = Vector3.zero;
      this.accuracy = accuracy;
    }
    
    public void Mount(GUIStyleManager style, bool controll = false){
        GUILayout.BeginVertical(style["fill"], GUILayout.MinHeight(0), GUILayout.MinWidth(0));
        if(controll){
          GUILayout.BeginHorizontal();
          GUILayout.Label(this.Name, style["label"]);
          if(GUILayout.Button("Zero", style["button-mini"], GUILayout.Width(40), GUILayout.Height(19))){
            this.Vector3 = Vector3.zero;
          }
          GUILayout.EndHorizontal();
        } else {
          GUILayout.Label(this.Name, style["label"]);
        }

        GUILayout.BeginHorizontal(style["fill-controll"]);
        GUILayout.FlexibleSpace();
        this.x = GUILayout.TextField(this.x, style["text"], GUILayout.Width(50));
        GUILayout.FlexibleSpace();
        this.y = GUILayout.TextField(this.y, style["text"], GUILayout.Width(50));
        GUILayout.FlexibleSpace();
        this.z = GUILayout.TextField(this.z, style["text"], GUILayout.Width(50));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }
  }
}