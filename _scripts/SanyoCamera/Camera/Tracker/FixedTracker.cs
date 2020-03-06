using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using SanyoCamera.Camera;

namespace SanyoCamera.Camera.Tracker
{
  public class FixedTracker : ITracker
  {
    public string Name() => "Fixed";
    
    public void Update(CameraManager manager) {
      if(manager.Setting){
        Vector3 moveTarget = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
          moveTarget += Vector3.forward;
        if (Input.GetKey(KeyCode.S))
          moveTarget += Vector3.back;
        if (Input.GetKey(KeyCode.A))
          moveTarget += Vector3.left;
        if (Input.GetKey(KeyCode.D))
          moveTarget += Vector3.right;
        if (Input.GetKey(KeyCode.Space))
          moveTarget += Vector3.up;
        if (Input.GetKey(KeyCode.LeftShift))
          moveTarget += Vector3.down;
      if(Input.GetKey(manager.config.Key["CAMERA_HIGHMOVE_KEY"]))
        moveTarget *= 2;
        manager.CameraOffset += moveTarget / 4;
      }

      if(manager.TrackingTarget == null)
        return;
        
      manager.CameraTransform.position = manager.TrackingTarget.position + (Quaternion.Euler(manager.TrackingTarget.eulerAngles + manager.CameraOffsetAngle) * manager.CameraOffset);
      manager.CameraTransform.rotation = Quaternion.Euler(manager.TrackingTarget.eulerAngles + manager.CameraOffsetAngle + manager.CameraAngle);
    }
  }
}