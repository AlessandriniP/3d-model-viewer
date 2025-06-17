using UnityEngine;

public class WebCommunicatorService : Singleton<WebCommunicatorService>
{
  [SerializeField] private ObjectsController _objectsController;
  [SerializeField] private CameraController _cameraController;

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
