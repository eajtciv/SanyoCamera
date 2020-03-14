using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SanyoCamera.GUI
{
  public interface IGuiTab
  {
    string Name();
    void Mount();
    void OnTabEnter();
    void OnTabExit();
  }
}