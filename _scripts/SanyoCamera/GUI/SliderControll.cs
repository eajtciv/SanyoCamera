using System;
using System.Collections.Generic;
using UnityEngine;
using SanyoCamera;
using SanyoLib;

namespace SanyoCamera.GUI
{
  public class SliderControll
  {
    private string Name;
    private float value,Min, Max;
    private String StringValue = "";
    private int accuracy = 2; 
    public float Value {
      set {
        UpdateSlider(value);
      }
      get {
        return value;
      }
    }
    public SliderControll(string Name, float Min, float Max){
      this.Name = Name;
      this.Min = Min;
      this.Max = Max;
      this.Value = Min;
    }

    public SliderControll(string Name, float Min, float Max, int accuracy){
      this.Name = Name;
      this.Min = Min;
      this.Max = Max;
      this.Value = Min;
      this.accuracy = accuracy;
    }

    public void Mount(GUIStyleManager style){
        GUILayout.BeginVertical(style["fill"], GUILayout.MinHeight(0), GUILayout.MinWidth(0));
        GUILayout.Label(this.Name, style["label"]);
        GUILayout.BeginHorizontal(style["fill-controll"]);
        GUILayout.BeginVertical();
        GUILayout.Space(5);
        float NowValue = GUILayout.HorizontalSlider(this.value, this.Min, this.Max);
        GUILayout.EndVertical();
        if(this.value != NowValue)
          UpdateSlider(NowValue);
        string NowStringValue = GUILayout.TextField(this.StringValue, style["text"], GUILayout.Width(50));
        if(this.StringValue != NowStringValue)
          UpdateText(NowStringValue);
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }

    private void UpdateSlider(float NowValue){
      this.value = NowValue;
      this.StringValue = this.value.ToString("0."+new string('0', accuracy));
    }

    private void UpdateText(string NowStringValue){
      this.StringValue = NowStringValue;
      float Value;
      if(float.TryParse(this.StringValue, out Value))
        this.value = Value;
    }
  }
}