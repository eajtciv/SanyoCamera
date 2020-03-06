using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using SanyoCamera.Camera;

namespace SanyoCamera.Camera.Tracker
{
  public class FreeTracker : ITracker
  {
    public string Name() => "Free";
    
    public void Update(CameraManager manager) {
      if(manager.Setting)
        return;
      Vector3 euler = manager.CameraTransform.rotation.eulerAngles;
      double forword = euler.y * Math.PI / 180;
      double side = (euler.y - 90) * Math.PI / 180;
      Vector3 moveTarget = Vector3.zero;
      if (Input.GetKey(KeyCode.W))
        moveTarget += new Vector3((float)Math.Sin(forword), 0, (float)Math.Cos(forword));
      if (Input.GetKey(KeyCode.S))
        moveTarget -= new Vector3((float)Math.Sin(forword), 0, (float)Math.Cos(forword));
      if (Input.GetKey(KeyCode.A))
        moveTarget += new Vector3((float)Math.Sin(side), 0, (float)Math.Cos(side));
      if (Input.GetKey(KeyCode.D))
        moveTarget -= new Vector3((float)Math.Sin(side), 0, (float)Math.Cos(side));
      if (Input.GetKey(KeyCode.Space))
        moveTarget += Vector3.up;
      if (Input.GetKey(KeyCode.LeftShift))
        moveTarget += Vector3.down;
      if(Input.GetKey(manager.config.Key["CAMERA_HIGHMOVE_KEY"]))
        moveTarget *= 2;
      manager.SetSmoothblePosition(moveTarget * manager.CameraMoveSpeed);

      Vector3 angleTarget = Vector3.zero;
      angleTarget += new Vector3(0, Input.GetAxis("MouseX"), 0);
      angleTarget += new Vector3(-Input.GetAxis("MouseY"), 0, 0);
      if(Input.GetKey(manager.config.Key["CAMERA_HIGHMOVE_KEY"]))
        angleTarget *= 2;
      manager.SetSmoothbleRotation(angleTarget * manager.CameraMoveSpeed);
    }
  }
}