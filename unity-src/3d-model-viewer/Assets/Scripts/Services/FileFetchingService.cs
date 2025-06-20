using System;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class FileFetchingService : Singleton<FileFetchingService>
{
  [field: SerializeField] public string BaseFetchingUrl { get; private set; }
  [field: SerializeField] public string ModelJsonFileName { get; private set; }
  [field: SerializeField] public string ModelBaseFolderPath { get; private set; }

  private void Start()
  {
    StartCoroutine(FetchJson(fetchLocal: true));
  }

  private IEnumerator FetchJson(bool fetchLocal)
  {
    var fileText = String.Empty;

    if (fetchLocal)
    {
      var filePath = Path.Combine(BaseFetchingUrl, ModelJsonFileName);

      if (!File.Exists(filePath))
      {
        Debug.LogError($"Local file not found: {filePath}");

        yield break;
      }

      fileText = File.ReadAllText(filePath);
    }
    else
    {
      using var webRequest = UnityWebRequest.Get($"{BaseFetchingUrl}{ModelJsonFileName}");

      yield return webRequest.SendWebRequest();

      if (webRequest.result == UnityWebRequest.Result.Success)
      {
        fileText = webRequest.downloadHandler.text;
      }
      else
      {
        Debug.LogError($"Error while fetching the remote file: " +
          $"{BaseFetchingUrl}{ModelJsonFileName} - {webRequest.error}");

        yield break;
      }
    }

    if (string.IsNullOrEmpty(fileText))
    {
      yield break;
    }

    var json = JsonUtility.FromJson<ModelFileNames>(fileText);

    if (json?.model_names == null)
    {
      yield break;
    }

    var files = json.model_names
        .ToDictionary(
            name => $"{BaseFetchingUrl}{ModelBaseFolderPath}{name}.glb",
            name => name
        );

    GltfImporterService.Instance.ImportGltfModelsFrom(files);
  }
}
