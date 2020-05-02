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

      if(!manager.Setting && Input.GetKey(KeyCode.Mouse0)){
        Quaternion quaternion = manager.CameraTransform.rotation;
        if(manager.TrackingTarget != null)
          quaternion =  manager.TrackingTarget.rotation * Quaternion.Euler(manager.CameraAngle);
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

        manager.CameraOffset += Quaternion.Euler(manager.CameraAngle) * (moveTarget / 4);

        Vector3 angleTarget = Vector3.zero;
        angleTarget += new Vector3(0, Input.GetAxis("MouseX"), 0);
        angleTarget += new Vector3(-Input.GetAxis("MouseY"), 0, 0);
        if(Input.GetKey(manager.config.Key["CAMERA_HIGHMOVE_KEY"]))
          angleTarget *= 2;
        manager.CameraAngle += angleTarget;
      }

      if(manager.TrackingTarget == null)
        return;
        
      manager.CameraTransform.position = manager.TrackingTarget.position + (manager.TrackingTarget.rotation * Quaternion.Euler(manager.CameraOffsetAngle)) * manager.CameraOffset;
      manager.CameraTransform.rotation = manager.TrackingTarget.rotation * Quaternion.Euler(manager.CameraAngle);
    }
  }
}