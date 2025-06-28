using System.Collections.Generic;
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
  private Dictionary<string, string> _modelURIs;

  private void Start()
  {
    _objectsController.CanGoPrevious += SendCanGoPreviousState;
    _objectsController.CanGoNext += SendCanGoNextState;

    _fileFetchingService.ModelsJsonFetched += OnModelsJsonFetched;
  }

  public void OnShowPreviousObject()
  {
    _objectsController.ShowPreviousObject();
  }

  public void OnShowNextObject()
  {
    _objectsController.ShowNextObject();
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
     FetchCurrentObjectDescription();
#endif
  }

  private void OnModelsJsonFetched(Dictionary<string, string> modelURIs)
  {
    _modelURIs = modelURIs;
  }

  private void FetchCurrentObjectDescription()
  {
    var currentObject = _objectsController.CurrentObject;
    var objectName = currentObject.name;

    _modelURIs.TryGetValue(objectName, out var modelURI);

    if (objectName == null || modelURI == null)
    {
      Debug.LogError("Wrong object description.");
    }
    else
    {
      ObjectDescription("ObjectDescription", modelURI, objectName);
    }

    if (!currentObject)
    {
      Debug.LogWarning("No objects found.");
    }
  }
}
