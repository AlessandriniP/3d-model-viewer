using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class FileFetchingService : Singleton<FileFetchingService>
{
  public event Action<string[]> FetchFilePaths;

  [field: SerializeField] public string BaseFetchingUrl { get; private set; }
  [field: SerializeField] public string ModelBaseFolderPath { get; private set; }
  [field: SerializeField] public string ModelJsonFileName { get; private set; }

  protected override void Awake()
  {
    base.Awake();

    StartCoroutine(FetchJson());
  }

  private IEnumerator FetchJson()
  {
    using (UnityWebRequest webRequest = UnityWebRequest.Get(BaseFetchingUrl + ModelJsonFileName))
    {

      yield return webRequest.SendWebRequest();

      if (webRequest.result == UnityWebRequest.Result.Success)
      {
        var fileText = webRequest.downloadHandler.text;
        var json = JsonUtility.FromJson<ModelFileNames>(fileText);
        var modelNames = json.model_names;
        var filePaths = new string[modelNames.Length];

        for (var i = 0; i < modelNames.Length; i++)
        {
          filePaths[i] = $"{BaseFetchingUrl}{ModelBaseFolderPath}{modelNames[i]}.glb";
        }

        FetchFilePaths?.Invoke(filePaths);
      }
      else
      {
        Debug.LogError("Error fetching JSON from " + BaseFetchingUrl
          + ModelJsonFileName + ": " + webRequest.error);
      }
    }
  }
}
