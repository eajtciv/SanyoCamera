using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using SanyoCamera.Camera;

namespace SanyoCamera.Camera.Tracker
{
  public class StalkingTracker : ITracker
  {
    public string Name() => "Stalking";
    
    public void Update(CameraManager manager) {
      if(manager.TrackingTarget == null)
        return;
      Vector3 position = manager.CameraTransform.position;
      Vector3 target = manager.TrackingTarget.position + Quaternion.Euler(manager.CameraOffsetAngle) * manager.CameraOffset;
      Vector3 euler = manager.CameraTransform.rotation.eulerAngles;
      double forword = CameraManager.ToRad(euler.y);
      double side = CameraManager.ToRad(euler.y - 90);


      if (Input.GetKey(KeyCode.W))
        manager.StalkingDistance -= 1;
      if (Input.GetKey(KeyCode.S))
        manager.StalkingDistance += 1;
      Vector3 moveTarget = Vector3.zero;
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

      // trucking
      double a = CameraManager.ToDeg(Math.Atan2(position.x - target.x, position.z - target.z));
      double b = CameraManager.ToDeg(Math.Atan2(position.y - target.y, Math.Sqrt(Math.Pow(position.x - target.x, 2) + Math.Pow(position.z - target.z, 2))));
      Vector3 angleTarget = new Vector3((float)b, (float)a - 180, 0);
      manager.CameraTransform.rotation = Quaternion.Euler(angleTarget + manager.CameraAngle);

      float distance = Vector3.Distance(target, position);
      float forwordDiff = -(manager.StalkingDistance - distance);

      position += new Vector3((float)(Math.Sin(forword) * forwordDiff), (float)-(b / 90) * forwordDiff, (float)(Math.Cos(forword) * forwordDiff));
      manager.CameraTransform.position = position;

    }
  }
}