using System;
using UnityEngine;
using SanyoCamera;

namespace SanyoCamera.Extension
{
  public interface IExtension
  {
    string Name();
    void Init(Core instance);
  }
}