using UnityEngine;
using System;
using System.IO;
namespace SanyoCamera.Camera
{
  public class ScreenCaptureUtil
  {
    public static void Capture(string fileName, int width, int height, UnityEngine.Camera baseCamera = null, bool safeMode = false)
    {
      // create RenderTexture
      RenderTexture renderTexture = new RenderTexture(width, height, 24, RenderTextureFormat.Default);
      renderTexture.Create();
      RenderTexture currentRT = RenderTexture.active;
      // rendering
      string[] whitelist = new string[] { (baseCamera ?? UnityEngine.Camera.main).name, "SkyBoxCamera" };
      foreach (UnityEngine.Camera c in UnityEngine.Camera.allCameras)
      {
        if (Array.Exists(whitelist, i => (i.Equals(c.name))) && c.targetTexture == null && c.enabled && c.gameObject.activeSelf)
        {
          UnityEngine.Camera camera = c;
          if (safeMode)
          {
            GameObject go = UnityEngine.Object.Instantiate(camera.gameObject);
            camera = go.GetComponent<UnityEngine.Camera>();
            camera.fieldOfView = c.fieldOfView;
            camera.orthographic = c.orthographic;
            camera.nearClipPlane = c.nearClipPlane;
            camera.farClipPlane = c.farClipPlane;
            camera.clearFlags = c.clearFlags;
          }
          RenderTexture currentTargetTexture = camera.targetTexture;
          camera.targetTexture = renderTexture;
          camera.Render();
          camera.targetTexture = currentTargetTexture;
          camera.Render();
          if (safeMode)
            GameObject.DestroyImmediate(camera.gameObject);
        }
      }
      // to Texture2D
      Texture2D texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
      RenderTexture.active = renderTexture;
      texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
      texture2D.Apply();
      RenderTexture.active = currentRT;
      renderTexture.DiscardContents();
      renderTexture.Release();
      // save PNG
      byte[] bytes = texture2D.EncodeToPNG();

      UnityEngine.Object.Destroy(renderTexture);
      UnityEngine.Object.Destroy(texture2D);

      File.WriteAllBytes(fileName, bytes);
    }
  }
}