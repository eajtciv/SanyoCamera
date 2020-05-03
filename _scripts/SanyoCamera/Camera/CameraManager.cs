using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SanyoCamera.Camera.Tracker;


namespace SanyoCamera.Camera
{
  public class CameraManager
  {
    public ConfigManager config { private set; get; }

    public CameraManager(){} //don't call

    public CameraManager(ConfigManager config){
      this.config = config;
      this.trackers.Add(new LockTracker());
      this.trackers.Add(new FixedTracker());
      this.trackers.Add(new StalkingTracker());
      this.trackers.Add(new FreeTracker());
    }

    public GameObject RootObject { set; get; }
    public UnityEngine.Camera MainCamera { private set; get; }
    public UnityEngine.Camera Camera { private set; get; }
    public GameObject CameraObject { private set; get; }
    public Transform CameraTransform { private set; get; }

    private float MainCameraFOV;

    public int TrackingType { set; get; }
    public List<ITracker> trackers = new List<ITracker>();

    public Transform TrackingTarget { set; get; }
    public Vector3 CameraOffset { set; get; }
    public Vector3 CameraOffsetAngle { set; get; }
    public Vector3 CameraAngle { set; get; }
    public bool SmoothMove { set; get; }
    public float CameraFoV { set; get; }
    public float StalkingDistance { set; get; } = 50;
    public float CameraMoveSpeed { set; get; } = 1;
    public float TrackingResponse { set; get; } = 1;
    public float CameraNearClip { set; get; }
    public float CameraForClip { set; get; }

    public bool Setting { set; get; }

    private Vector3 SmoothedMove, SmoothMoveVelocity;
    private Vector3 SmoothedAngle, SmoothAngleVelocity;
    private CameraDestoryer cameraDestoryer;

    public void SetSmoothblePosition(Vector3 moveTarget){
      if(SmoothMove)
        this.CameraTransform.position += (SmoothedMove = Vector3.SmoothDamp(SmoothedMove, moveTarget, ref SmoothMoveVelocity, 0.5f));
      else
        this.CameraTransform.position += moveTarget;
    }

    public void SetSmoothbleRotation(Vector3 angleTarget){

      Vector3 euler = this.CameraTransform.rotation.eulerAngles;
      if(SmoothMove)
        this.CameraTransform.rotation = Quaternion.Euler(euler + (SmoothedAngle = Vector3.SmoothDamp(SmoothedAngle, angleTarget, ref SmoothAngleVelocity, 0.5f)));
      else
        this.CameraTransform.rotation = Quaternion.Euler(euler + angleTarget);
    }

    public void CreateCamera(){
      this.MainCamera = UnityEngine.Camera.main;
      this.MainCameraFOV = this.MainCamera.fieldOfView;
      if(this.CameraFoV == 0)
        this.CameraFoV = this.MainCameraFOV;
      if(this.CameraNearClip == 0)
        this.CameraNearClip = this.MainCamera.nearClipPlane;
      if(this.CameraForClip == 0)
        this.CameraForClip = this.MainCamera.farClipPlane;
      this.MainCamera.enabled = false;
      // free camera on
      this.MainCamera.GetComponent<RideCameraController>().ONGNOMCJBGE = true;
      // copy camera object
      this.CameraObject = UnityEngine.Object.Instantiate(this.MainCamera.gameObject);
      this.CameraObject.name = "Camera";
      this.Camera = this.CameraObject.GetComponent<UnityEngine.Camera>();
      this.Camera.enabled = true;
      this.CameraTransform = this.CameraObject.transform;
      if(this.RootObject != null)
        this.CameraTransform.SetParent(this.RootObject.transform);
      this.CameraObject.transform.position = this.MainCamera.transform.position;
      // delete components
      UnityEngine.AudioListener audioLis = this.CameraObject.GetComponent<UnityEngine.AudioListener>();
      if (audioLis != null) UnityEngine.Object.DestroyImmediate(audioLis);
      audioLis = null;

      UnityEngine.GUILayer guilayer = this.CameraObject.GetComponent<UnityEngine.GUILayer>();
      if (guilayer != null) UnityEngine.Object.DestroyImmediate(guilayer);
      guilayer = null;

      RideCameraController rideCamCntrl = this.CameraObject.GetComponent<RideCameraController>();
      rideCamCntrl.ONGNOMCJBGE = true;
      if (rideCamCntrl != null) UnityEngine.Object.DestroyImmediate(rideCamCntrl);
      rideCamCntrl = null;

      Skybox skybox = this.CameraObject.GetComponent<Skybox>();
      if (skybox != null) UnityEngine.Object.DestroyImmediate(skybox);
      skybox = null;

      this.cameraDestoryer = CameraDestoryer.Create(this);
    }
    
    public void DestroyCamera() {
      if(this.MainCamera != null){
        this.MainCamera.enabled = true;
        this.MainCamera.fieldOfView = this.MainCameraFOV;
        this.MainCamera.GetComponent<RideCameraController>().ONGNOMCJBGE = false;
      }
      UnityEngine.Object.DestroyImmediate(this.CameraObject);
      this.Camera = null;
      this.CameraObject = null;
      if(cameraDestoryer != null)
        this.cameraDestoryer.Harmless();
    }

    public bool Enabled() => (this.CameraObject != null);

    public void Update() {
      if(this.Camera != null){
        this.CameraFoV += -(Input.GetAxis("MouseW") * 10f);
        this.CameraFoV = Math.Max(0.01f, Math.Min(179f, this.CameraFoV));
        this.Camera.fieldOfView -= (this.Camera.fieldOfView - this.CameraFoV) / 10;
        this.Camera.nearClipPlane = this.CameraNearClip;
        this.Camera.farClipPlane = this.CameraForClip;

        this.MainCamera.transform.position = this.CameraTransform.position;
        this.MainCamera.transform.rotation = this.CameraTransform.rotation;
      }

      if(this.CameraTransform != null){
        this.trackers[TrackingType].Update(this);
      }
    }

    public static Vector3 Vector3DiffSmooth(Vector3 vec1, Vector3 vec2, float v) {
      return new Vector3(
      vec1.x - DiffAngle(vec2.x, vec1.x) / v,
      vec1.y - DiffAngle(vec2.y, vec1.y) / v,
      vec1.z - DiffAngle(vec2.z, vec1.z) / v
      );
    }

    // Get own machine object
    public static Transform GetMachine() => Array.Find(SceneManager.GetActiveScene().GetRootGameObjects(), i => (i.transform.childCount > 0 && (i.transform.GetChild(0).name.StartsWith(i.name) || i.transform.GetChild(0).name.Equals("Limb0"))))?.transform?.GetChild(0);
    // Get player root objects
    public static GameObject[] GetPlayers() => Array.FindAll(SceneManager.GetActiveScene().GetRootGameObjects(), i => (i.transform.Find("model_1") != null));
    // Degree and radian conversion
    public static double ToDeg(double rad) => (rad / Math.PI * 180);
    public static double ToRad(double deg) => (deg * Math.PI / 180);
    // Angle difference
    public static float DiffAngle(float a, float b)
    {
      float c = b - a;
      c -= (float)Math.Floor(c / 360f) * 360f;
      c -= (c > 180f ? 360f : 0);
      return c;
    }
  }
}