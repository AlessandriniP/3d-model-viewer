using System.Runtime.InteropServices;
using UnityEngine;
using UnityGLTF;

public class WebCommunicatorService : Singleton<WebCommunicatorService>
{
  [SerializeField] private FileFetchingService _fileFetchingService;
  [SerializeField] private ObjectsController _objectsController;
  [SerializeField] private CameraController _cameraController;

  [DllImport("__Internal")]
  private static extern void CanShowPreviousObject(string param, int value);
  [DllImport("__Internal")]
  private static extern void CanShowNextObject(string param, int value);
  [DllImport("__Internal")]
  private static extern void ObjectDescription(string param, string value1, string value2);

  private string _modelsPath;
  private string _modelOverviewJson;

  private void Start()
  {
    _objectsController.CanGoPrevious += SendCanGoPreviousState;
    _objectsController.CanGoNext += SendCanGoNextState;

    _fileFetchingService.JsonFetchedFromWeb += FetchCurrentObjectDescription;
  }

  public void OnShowPreviousObject()
  {
    _objectsController.ShowPreviousObject();

#if UNITY_WEBGL && !UNITY_EDITOR
     FetchCurrentObjectDescription();
#endif
  }

  public void OnShowNextObject()
  {
    _objectsController.ShowNextObject();

#if UNITY_WEBGL && !UNITY_EDITOR
     FetchCurrentObjectDescription();
#endif
  }

  public void OnResetView()
  {
    _cameraController.ResetView();
  }

  public void OnSendModelsPath(string modelsPath)
  {
    _modelsPath = modelsPath;

    FetchModelsFromWeb();
  }

  public void OnSendModelOverviewJson(string modelOverviewJson)
  {
    _modelOverviewJson = modelOverviewJson;

    FetchModelsFromWeb();
  }

  private async void FetchModelsFromWeb()
  {
    if (_modelsPath != null && _modelOverviewJson != null)
    {
      await _fileFetchingService.FetchJsonFromWebAsync(_modelOverviewJson, _modelsPath);
    }
  }

  private void FetchCurrentObjectDescription()
  {
    var currentObject = _objectsController.CurrentObject; // TODO: wrong object

    ObjectDescription("ObjectDescription",
      currentObject?.GetComponent<GLTFComponent>().GLTFUri, // TODO: GLTFComponent does not exist
      currentObject?.name);

    if (!currentObject)
    {
      Debug.LogWarning("No objects found.");
    }
  }

  private void SendCanGoPreviousState(bool state)
  {
#if UNITY_WEBGL && !UNITY_EDITOR
     CanShowPreviousObject("CanGoPrevious", state ? 1 : 0);
#endif
  }

  private void SendCanGoNextState(bool state)
  {
#if UNITY_WEBGL && !UNITY_EDITOR
     CanShowNextObject("CanGoNext", state ? 1 : 0);
#endif
  }
}
