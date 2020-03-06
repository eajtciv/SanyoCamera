using System;
using UnityEngine;
using SanyoCamera;
using SanyoCamera.Camera;
using SanyoLib;

namespace SanyoCamera.GUI
{
  public class CameraTab : IGuiTab
  {
    private GUIStyleManager style;
    private CameraManager camera;

    public CameraTab(){}// don't call

    public CameraTab(GUIStyleManager style, CameraManager camera){
      this.style = style;
      this.camera = camera;
    }

    public string Name() => "Camera";

    private int Target = 0;
    private Vector2 scrollPosition;

    private Vector3Controll Angle = new Vector3Controll("Angle");
    private Vector3Controll Offset = new Vector3Controll("Offset Position", 1);
    private Vector3Controll OffsetAngle = new Vector3Controll("Offset Angle");

    private SliderControll MoveSpeed = new SliderControll("Move Speed", 0.01f, 10f);
    private SliderControll StalkingDistance = new SliderControll("Stalking Distance", 10f, 1000f);
    private SliderControll FieldOfView = new SliderControll("FieldOfView", 0.01f, 179f);
    private SliderControll TrackingResponse = new SliderControll("Tracking Response", 0f, 100f);
    public void Mount(){
      GUILayout.BeginHorizontal();
      GUILayout.BeginVertical();
      {
        // Player Selector
        scrollPosition = GUILayout.BeginScrollView (scrollPosition);
        GameObject[] players = CameraManager.GetPlayers();
        GameObject[] newPlayers = new GameObject[players.Length + 1];
        Array.Copy(players, 0, newPlayers, 1, players.Length);
        newPlayers[0] = CameraManager.GetMachine().gameObject;

        this.Target = Array.FindIndex(newPlayers, i => i.transform == camera.TrackingTarget);
        int NowTarget = GUILayout.SelectionGrid(this.Target, Array.ConvertAll(newPlayers, i => i.name.Equals("Machine_P(Clone)") ? "Machine Loading..." : i.name), 1, this.style["select"]);
        if (NowTarget != Target)
        {
          this.Target = NowTarget;
          camera.TrackingTarget = newPlayers[this.Target].transform;
        }
        GUILayout.BeginVertical(this.style["fill"], GUILayout.MinHeight(0), GUILayout.MinWidth(0));
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.EndScrollView();

        GUILayout.Space(3);
        // TrackingType
        GUILayout.BeginVertical(this.style["fill"], GUILayout.MinHeight(0), GUILayout.MinWidth(0));
        GUILayout.Label("Tracking Type", this.style["label"]);
        GUILayout.BeginHorizontal(this.style["frame-margin"]);
        camera.TrackingType = GUILayout.SelectionGrid(camera.TrackingType, Array.ConvertAll(camera.trackers.ToArray(), i => i.Name()), camera.trackers.Count, this.style["select"]);
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();

        GUILayout.Space(3);
        GUILayout.BeginHorizontal();
        // Camera Switch
        GUILayout.BeginVertical(this.style["fill"], GUILayout.MinHeight(0), GUILayout.MinWidth(0));
        GUILayout.Label("Script Camera", this.style["label"]);
        GUILayout.BeginHorizontal(this.style["frame-margin"]);
        bool NawCamera = !Convert.ToBoolean(GUILayout.SelectionGrid(Convert.ToInt32(!camera.Enabled()), new string[] { "ON", "OFF" }, 2, this.style["select"]));
        if(NawCamera != camera.Enabled()){
          if(NawCamera){
            this.camera.CreateCamera();
          } else {
            this.camera.DestroyCamera();
          }
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();

        GUILayout.Space(3);
        // SmoothMove
        GUILayout.BeginVertical(this.style["fill"], GUILayout.MinHeight(0), GUILayout.MinWidth(0));
        GUILayout.Label("Smooth Move", this.style["label"]);
        GUILayout.BeginHorizontal(this.style["frame-margin"]);
        camera.SmoothMove = !Convert.ToBoolean(GUILayout.SelectionGrid(Convert.ToInt32(!camera.SmoothMove), new string[] { "ON", "OFF" }, 2, this.style["select"]));
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();

        GUILayout.EndHorizontal();
      }
      GUILayout.EndVertical();
      GUILayout.Space(3);
      GUILayout.BeginVertical(GUILayout.Width(200));
      {
        if(this.Offset.Vector3 != this.camera.CameraOffset)
          this.Offset.Vector3 = this.camera.CameraOffset;

        this.Angle.Mount(this.style);
        GUILayout.Space(3);
        this.Offset.Mount(this.style);
        GUILayout.Space(3);
        this.OffsetAngle.Mount(this.style);

        if(this.Angle.Vector3 != this.camera.CameraAngle)
          this.camera.CameraAngle = this.Angle.Vector3;
        if(this.Offset.Vector3 != this.camera.CameraOffset)
          this.camera.CameraOffset = this.Offset.Vector3;
        if(this.OffsetAngle.Vector3 != this.camera.CameraOffsetAngle)
          this.camera.CameraOffsetAngle = this.OffsetAngle.Vector3;
      }
      GUILayout.EndVertical();
      GUILayout.Space(3);
      GUILayout.BeginVertical(GUILayout.Width(200));
      {
        if(this.MoveSpeed.Value != this.camera.CameraMoveSpeed)
          this.MoveSpeed.Value = this.camera.CameraMoveSpeed;
        if(this.FieldOfView.Value != this.camera.CameraFoV)
          this.FieldOfView.Value = this.camera.CameraFoV;
        if(this.StalkingDistance.Value != this.camera.StalkingDistance)
          this.StalkingDistance.Value = this.camera.StalkingDistance;
        if(this.TrackingResponse.Value != this.camera.TrackingResponse)
          this.TrackingResponse.Value = this.camera.TrackingResponse;

        this.MoveSpeed.Mount(this.style);
        GUILayout.Space(3);
        this.StalkingDistance.Mount(this.style);
        GUILayout.Space(3);
        this.FieldOfView.Mount(this.style);
        GUILayout.Space(3);
        this.TrackingResponse.Mount(this.style);

        if(this.MoveSpeed.Value != this.camera.CameraMoveSpeed)
          this.camera.CameraMoveSpeed = this.MoveSpeed.Value;
        if(this.FieldOfView.Value != this.camera.CameraFoV)
          this.camera.CameraFoV= this.FieldOfView.Value;
        if(this.StalkingDistance.Value != this.camera.StalkingDistance)
          this.camera.StalkingDistance= this.StalkingDistance.Value;
        if(this.TrackingResponse.Value != this.camera.TrackingResponse)
          this.camera.TrackingResponse= this.TrackingResponse.Value;

          
      }
      GUILayout.EndVertical();
      
      GUILayout.EndHorizontal();
    }
  }
}