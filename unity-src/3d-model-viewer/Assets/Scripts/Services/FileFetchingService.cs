using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FileFetchingService : Singleton<FileFetchingService>
{
  public event Action<List<KeyValuePair<string, string>>> FetchFilePaths;

  [field: SerializeField] public string BaseFetchingUrl { get; private set; }
  [field: SerializeField] public string ModelBaseFolderName { get; private set; }
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
        var filePaths = new List<KeyValuePair<string, string>>();

        foreach (var modelName in modelNames)
        {
          var blendFile = $"{BaseFetchingUrl}{ModelBaseFolderName}{modelName}/{modelName}.blend";
          var glbFile = $"{BaseFetchingUrl}{ModelBaseFolderName}{modelName}/{modelName}.glb";

          filePaths.Add(new KeyValuePair<string, string>(blendFile, glbFile));
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
