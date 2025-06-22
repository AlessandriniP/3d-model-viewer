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
    _fileFetchingService.JsonFetchedFromWeb += FetchCurrentObjectDescription;

#if UNITY_WEBGL && !UNITY_EDITOR
    CanShowPreviousObject("CanGoPrevious", _objectsController.CanGoPrevious ? 1 : 0);
    CanShowNextObject("CanGoNext", _objectsController.CanGoNext ? 1 : 0);
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
