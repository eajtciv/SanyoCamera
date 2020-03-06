using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using SanyoCamera.Camera;

namespace SanyoCamera.Camera.Tracker
{
  public class LockTracker : ITracker
  {
    public string Name() => "Lock";
    
    public void Update(CameraManager manager) {
      Vector3 position = manager.CameraTransform.position;
      Vector3 euler = manager.CameraTransform.rotation.eulerAngles;

      // move
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

      if(manager.TrackingTarget == null)
        return;
        
      Vector3 target = manager.TrackingTarget.position + Quaternion.Euler(manager.CameraOffsetAngle) * (manager.TrackingTarget.rotation * manager.CameraOffset);

      // trucking
      double a = CameraManager.ToDeg(Math.Atan2(position.x - target.x, position.z - target.z));
      double b = CameraManager.ToDeg(Math.Atan2(position.y - target.y, Math.Sqrt(Math.Pow(position.x - target.x, 2) + Math.Pow(position.z - target.z, 2))));
      Vector3 angleTarget = new Vector3((float)b, (float)a - 180, 0) + manager.CameraAngle;
      angleTarget = CameraManager.Vector3DiffSmooth(euler, angleTarget, Math.Max(manager.TrackingResponse, 1));
      manager.CameraTransform.rotation = Quaternion.Euler(angleTarget);
    }
  }
}