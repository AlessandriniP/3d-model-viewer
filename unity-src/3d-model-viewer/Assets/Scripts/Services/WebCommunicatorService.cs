using System.Runtime.InteropServices;
using UnityEngine;
using UnityGLTF;

public class WebCommunicatorService : Singleton<WebCommunicatorService>
{
  [SerializeField] private ObjectsController _objectsController;
  [SerializeField] private CameraController _cameraController;

  [DllImport("__Internal")]
  private static extern void CanShowPreviousObject(string param, int value);
  [DllImport("__Internal")]
  private static extern void CanShowNextObject(string param, int value);
  [DllImport("__Internal")]
  private static extern void ObjectDescription(string param, string value1, string value2);

  private void Start()
  {
#if UNITY_WEBGL && !UNITY_EDITOR
    CanShowPreviousObject("CanGoPrevious", _objectsController.CanGoPrevious ? 1 : 0);
    CanShowNextObject("CanGoNext", _objectsController.CanGoNext ? 1 : 0);
    FetchCurrentObjectDescription();
#endif
  }

  public void OnShowPreviousObject()
  {
    _objectsController.ShowPreviousObject();

#if UNITY_WEBGL && !UNITY_EDITOR
     CanShowPreviousObject("CanGoPrevious", _objectsController.CanGoPrevious ? 1 : 0);
     FetchCurrentObjectDescription();
#endif
  }

  public void OnShowNextObject()
  {
    _objectsController.ShowNextObject();

#if UNITY_WEBGL && !UNITY_EDITOR
     CanShowNextObject("CanGoNext", _objectsController.CanGoNext ? 1 : 0);
     FetchCurrentObjectDescription();
#endif
  }

  public void OnResetView()
  {
    _cameraController.ResetView();
  }

  public void OnSendModelsPath(string modelsPath)
  {
    Debug.Log($"Models path sent: {modelsPath}");
  }

  public void OnSendModelOverviewJson(string modelOverviewJson)
  {
    Debug.Log($"Model overview JSON sent: {modelOverviewJson}");
  }

  private void FetchCurrentObjectDescription() // TODO: start fetching description after first object is loaded and current object is set instead of Start() 
  {
    var currentObject = _objectsController.CurrentObject;

    ObjectDescription("ObjectDescription",
      currentObject?.GetComponent<GLTFComponent>().GLTFUri,
      currentObject?.name);

    if (!currentObject)
    {
      Debug.LogWarning("No objects found.");
    }
  }
}
