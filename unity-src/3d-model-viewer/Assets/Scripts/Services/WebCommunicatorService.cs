using UnityEngine;
using System.Runtime.InteropServices;

public class WebCommunicatorService : Singleton<WebCommunicatorService>
{
  [SerializeField] private ObjectsController _objectsController;
  [SerializeField] private CameraController _cameraController;

  [DllImport("__Internal")]
  private static extern void SendMessageToWeb(string message);

  private void Start()
  {
#if UNITY_WEBGL && !UNITY_EDITOR
    SendMessageToWeb("Hello from Unity WebGL!");
#endif
  }

  public void OnShowNextObject()
  {
    //_objectsController.ShowNextObject();
    Debug.Log("Next object requested.");
  }

  public void OnShowPreviousObject()
  {
    //_objectsController.ShowPreviousObject();
    Debug.Log("Previous object requested.");
  }

  public void OnResetView()
  {
    //_cameraController.ResetView();
    Debug.Log("Reset camera view.");
  }
}
