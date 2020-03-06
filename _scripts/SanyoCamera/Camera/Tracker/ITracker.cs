using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using SanyoCamera.Camera;

namespace SanyoCamera.Camera.Tracker
{
  public interface ITracker
  {
    string Name();
    void Update(CameraManager manager);
  }
}