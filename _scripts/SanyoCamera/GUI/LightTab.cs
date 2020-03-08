using System;
using UnityEngine;
using SanyoCamera;
using SanyoCamera.Light;
using SanyoCamera.Camera;
using SanyoLib;

namespace SanyoCamera.GUI
{
  public class LightTab : IGuiTab
  {
    public string Name() => "Light";

    private GUIStyleManager style;
    private LightManager light;
    private int Target = -1;
    private UnityEngine.Light TargetLight;
    private Vector2 scrollPosition, AttachScrollPosition;
    private KeyInputer inputer = new KeyInputer();

    public LightTab(GUIStyleManager style, LightManager light){
      this.style = style;
      this.light = light;
    }

    private SliderControll Range = new SliderControll("Range", 10, 640f, 1);
    private SliderControll SpotAngle = new SliderControll("SpotAngle", 0.1f, 180.1f, 1);
    private SliderControll Intensity = new SliderControll("Intensity", 0.1f, 32f);
    private Vector3Controll Color = new Vector3Controll("Color (RGB)");
    private Vector3Controll Angle = new Vector3Controll("Angle", 1);
    private Vector3Controll Position = new Vector3Controll("Position", 2);
    private bool lightSwitchInput = false;
    private float lightSwitchInputEscapeWait = 0;

    private Vector3 ColorToVector3(Color color){
      return new Vector3(color.r, color.g, color.b) * 255f;
    }
    private Color Vector3ToColor(Vector3 vector3){
      vector3 /= 255f;
      return new Color(Mathf.Min(Mathf.Max(vector3.x,0f),255f), Mathf.Min(Mathf.Max(vector3.y,0f),255f), Mathf.Min(Mathf.Max(vector3.z,0f),255f));
    }

    public void Mount(){
      try{
        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical();
        {
          scrollPosition = GUILayout.BeginScrollView (scrollPosition);
          int PreTarget = this.Target;
          int NowTarget = GUILayout.SelectionGrid(this.Target, Array.ConvertAll(light.ToArray(), i => {
            Vector3 pos = i.transform.localPosition;
            int id = i.gameObject.GetInstanceID (); 
            string content =  string.Format("{0} | {1} {2}",id , i.transform.parent?.name ?? "world", string.Format("[{0:0.0} {1:0.0} {2:0.0}]", pos.x, pos.y, pos.z));
            if(i.GetComponent<UnityEngine.Light>().enabled)
              return content;
            else
              return string.Format("<color=#808080>{0}</color>", content);
          }), 1, this.style["select"]);
          if(this.Target != NowTarget){
            this.Target = NowTarget;
            this.TargetLight = light[this.Target].GetComponent<UnityEngine.Light>();
          }
          GUILayout.BeginVertical(this.style["fill"], GUILayout.MinHeight(0), GUILayout.MinWidth(0));
          GUILayout.FlexibleSpace();
          GUILayout.EndVertical();
          GUILayout.EndScrollView();

          string NonSelect = (this.Target >= 0 && this.Target < light.Count) ? "" : "-disable";
          GUILayout.BeginHorizontal();
          if(GUILayout.Button("Delete", this.style["button" + NonSelect])){
            UnityEngine.Object.Destroy(light[this.Target]);
            light.RemoveAt(this.Target);
            this.Target -= 1;
            this.TargetLight = null;
          }
          if(GUILayout.Button("Create", this.style["button"], GUILayout.Width(80))){
            this.light.Create(CameraManager.GetMachine());
          }
          GUILayout.EndHorizontal();
          
          GUILayout.Space(3);
          GUILayout.BeginVertical(this.style["fill"], GUILayout.MinHeight(0), GUILayout.MinWidth(0));
          GUILayout.BeginHorizontal();
          GUILayout.Label("Enabled", this.style["label"], GUILayout.Height(19));

          inputer.Listen(key => light.SetSwitch(TargetLight, key));
          string switchName = inputer.ToName(this.light.GetSwitch(this.TargetLight), "Switch Key", "[PRESS]...");
          if(GUILayout.Button(switchName, this.style["button-mini" + NonSelect], GUILayout.Width(100), GUILayout.Height(19))){
            if(this.TargetLight != null)
              inputer.Listening = true;
          }
          GUILayout.EndHorizontal();
          GUILayout.BeginHorizontal(this.style["frame-margin"]);
          if(this.TargetLight != null)
            this.TargetLight.enabled = !Convert.ToBoolean(GUILayout.SelectionGrid(Convert.ToInt32(!this.TargetLight.enabled), new string[] { "ON", "OFF" }, 2, this.style["select"]));
          else
            GUILayout.SelectionGrid(-1, new string[] { "ON", "OFF" }, 2, this.style["select"]);
          GUILayout.EndHorizontal();
          GUILayout.EndVertical();
        }

        if(this.TargetLight != null){
          if(this.TargetLight.range != this.Range.Value){
            this.Range.Value = this.TargetLight.range;
          }
          if(this.TargetLight.spotAngle != this.SpotAngle.Value){
            this.SpotAngle.Value = this.TargetLight.spotAngle;
          }
          if(this.TargetLight.intensity != this.Intensity.Value){
            this.Intensity.Value = this.TargetLight.intensity;
          }
          if(ColorToVector3(this.TargetLight.color) != this.Color.Vector3){
            Color.Vector3 = ColorToVector3(this.TargetLight.color);
          }
          if(this.TargetLight.transform.localEulerAngles != this.Angle.Vector3){
            Angle.Vector3 = this.TargetLight.transform.localEulerAngles;
          }
          if(this.TargetLight.transform.localPosition != this.Position.Vector3){
            Position.Vector3 = this.TargetLight.transform.localPosition;
          }
        }else{
          if(this.light.DefaultRange != this.Range.Value){
            this.Range.Value = this.light.DefaultRange;
          }
          if(this.light.DefaultSpotAngle != this.SpotAngle.Value){
            this.SpotAngle.Value = this.light.DefaultSpotAngle;
          }
          if(this.light.DefaultIntensity != this.Intensity.Value){
            this.Intensity.Value = this.light.DefaultIntensity;
          }
          if(ColorToVector3(this.light.DefaultColor) != this.Color.Vector3){
            Color.Vector3 = ColorToVector3(this.light.DefaultColor);
          }
          if(this.light.DefaultRotation.eulerAngles != this.Angle.Vector3){
            Angle.Vector3 = this.light.DefaultRotation.eulerAngles;
          }
          if(this.light.DefaultPosition != this.Position.Vector3){
            Position.Vector3 = this.light.DefaultPosition;
          }
        }

        GUILayout.EndVertical();
        GUILayout.Space(3);
        GUILayout.BeginVertical(GUILayout.Width(200));
        {
          Range.Mount(this.style);
          GUILayout.FlexibleSpace();
          SpotAngle.Mount(this.style);
          GUILayout.FlexibleSpace();
          Intensity.Mount(this.style);
          GUILayout.FlexibleSpace();
          Color.Mount(this.style);
          GUILayout.FlexibleSpace();
          Angle.Mount(this.style, true);
          GUILayout.FlexibleSpace();
          Position.Mount(this.style, true);

          if(Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1)){
            Vector3 move = Vector3.zero;
            if (Input.GetKey(KeyCode.W))
              move += Vector3.forward;
            if (Input.GetKey(KeyCode.S))
              move += Vector3.back;
            if (Input.GetKey(KeyCode.A))
              move += Vector3.left;
            if (Input.GetKey(KeyCode.D))
              move += Vector3.right;
            if (Input.GetKey(KeyCode.Space))
              move += Vector3.up;
            if (Input.GetKey(KeyCode.LeftShift))
              move += Vector3.down;
            if(move != Vector3.zero){
              if(Input.GetKey(KeyCode.Mouse0)){
                this.Position.Vector3 += move/15;
              }else if(Input.GetKey(KeyCode.Mouse1)){
                this.Angle.Vector3 += new Vector3(-move.z,move.x,move.y)/5;
              }
            }
          }
        }

        if(this.TargetLight != null){
          if(this.TargetLight.range != this.Range.Value){
            this.light.DefaultRange = this.TargetLight.range = this.Range.Value;
          }
          if(this.TargetLight.spotAngle != this.SpotAngle.Value){
            this.light.DefaultSpotAngle = this.TargetLight.spotAngle = this.SpotAngle.Value;
          }
          if(this.TargetLight.intensity != this.Intensity.Value){
            this.light.DefaultIntensity = this.TargetLight.intensity = this.Intensity.Value;
          }
          if(ColorToVector3(this.TargetLight.color) != this.Color.Vector3){
            this.light.DefaultColor = this.TargetLight.color = Vector3ToColor(this.Color.Vector3);
          }
          if(this.TargetLight.transform.localEulerAngles != this.Angle.Vector3){
            this.light.DefaultRotation = this.TargetLight.transform.localRotation = Quaternion.Euler(this.Angle.Vector3);
          }
          if(this.TargetLight.transform.localPosition != this.Position.Vector3){
            this.light.DefaultPosition = this.TargetLight.transform.localPosition = this.Position.Vector3;
          }
        }else{
          if(this.light.DefaultRange != this.Range.Value){
            this.light.DefaultRange = this.Range.Value;
          }
          if(this.light.DefaultSpotAngle != this.SpotAngle.Value){
            this.light.DefaultSpotAngle = this.SpotAngle.Value;
          }
          if(this.light.DefaultIntensity != this.Intensity.Value){
            this.light.DefaultIntensity = this.Intensity.Value;
          }
          if(ColorToVector3(this.light.DefaultColor) != this.Color.Vector3){
            this.light.DefaultColor = Vector3ToColor(this.Color.Vector3);
          }
          if(this.light.DefaultRotation.eulerAngles != this.Angle.Vector3){
            this.light.DefaultRotation = Quaternion.Euler(this.Angle.Vector3);
          }
          if(this.light.DefaultPosition != this.Position.Vector3){
            this.light.DefaultPosition = Position.Vector3;
          }
        }

        GUILayout.EndVertical();
        GUILayout.Space(3);
        GUILayout.BeginVertical(GUILayout.Width(200));
        {
          GUILayout.BeginVertical(style["fill"], GUILayout.MinHeight(0), GUILayout.MinWidth(0));
          GUILayout.Label("Attach", style["label"]);
          GUILayout.BeginHorizontal(style["fill-controll"]);
          AttachScrollPosition = GUILayout.BeginScrollView (AttachScrollPosition);
          GUILayout.BeginVertical();
          GameObject[] players = CameraManager.GetPlayers();
          GameObject[] newPlayers = new GameObject[players.Length + 1];
          Array.Copy(players, 0, newPlayers, 1, players.Length);
          newPlayers[0] = CameraManager.GetMachine().gameObject;

          string[] MountbleTargets = new string[newPlayers.Length + 1];
          MountbleTargets[0] = "-------------[ World ]-------------";
          Array.Copy(Array.ConvertAll(newPlayers, i => i.name), 0, MountbleTargets, 1, newPlayers.Length);
          
          int attachTarget = Array.FindIndex(newPlayers, i => i.transform == (this.TargetLight != null ? this.TargetLight.transform.parent : this.light.DefaultAttach))+1;
          int nowAttachTarget = GUILayout.SelectionGrid(attachTarget, MountbleTargets, 1, this.style["select"]);
          if (nowAttachTarget != attachTarget)
          {
            if(nowAttachTarget > 0 && nowAttachTarget <= newPlayers.Length){
              this.light.DefaultAttach = newPlayers[nowAttachTarget-1].transform;
              if(this.TargetLight != null)
                this.TargetLight.transform.parent = this.light.DefaultAttach;
            } else {
              this.light.DefaultAttach = null;
              if(this.TargetLight != null)
                this.TargetLight.transform.parent = null;
            }
          }
          GUILayout.FlexibleSpace();
          GUILayout.EndVertical();
          GUILayout.EndScrollView();
          GUILayout.EndHorizontal();
          GUILayout.EndVertical();
        }
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
      }catch(Exception e){
        this.TargetLight = null;
        this.Target = -1;
        for(int i=0;i < this.light.Count;i++){
          if(light[i] == null){
            light.RemoveAt(i);
            i--;
          }
        }
      }
    }
  }
}