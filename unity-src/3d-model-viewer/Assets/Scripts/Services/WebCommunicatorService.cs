using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class WebCommunicatorService : Singleton<WebCommunicatorService>
{
  [SerializeField] private FileFetchingService _fileFetchingService;
  [SerializeField] private ObjectsController _objectsController;
  [SerializeField] private CameraController _cameraController;

  [DllImport("__Internal")]
  private static extern void ModelsFetched(string param);
  [DllImport("__Internal")]
  private static extern void CanShowPreviousObject(string param, int value);
  [DllImport("__Internal")]
  private static extern void CanShowNextObject(string param, int value);
  [DllImport("__Internal")]
  private static extern void ObjectDescription(string param, string value1, string value2);

  private string _modelsPath;
  private string _modelOverviewJson;
  private Dictionary<string, string> _modelURIs;

  protected override void Awake()
  {
    base.Awake();

    _fileFetchingService.ModelsJsonFetched += OnModelsJsonFetched;

    _objectsController.ObjectProcessed += FetchCurrentObjectDescription;
    _objectsController.CanGoPrevious += SendCanGoPreviousState;
    _objectsController.CanGoNext += SendCanGoNextState;
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

      SendFetchedModelsInfo();
    }
  }

  private void SendFetchedModelsInfo()
  {
#if UNITY_WEBGL && !UNITY_EDITOR
     ModelsFetched("ModelsFetched");
#endif
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

  private void FetchCurrentObjectDescription(GameObject currentObject)
  {
#if UNITY_WEBGL && !UNITY_EDITOR
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
#endif
  }

  private void OnModelsJsonFetched(Dictionary<string, string> modelURIs)
  {
    _modelURIs = modelURIs;
  }
}
