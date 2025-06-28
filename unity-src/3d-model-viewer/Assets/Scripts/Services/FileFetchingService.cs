using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class FileFetchingService : Singleton<FileFetchingService>
{
  public event Action<Dictionary<string, string>> ModelsJsonFetched;

  [field: SerializeField] public string LocalBaseFetchingUri { get; private set; }
  [field: SerializeField] public string LocalModelOverviewJsonName { get; private set; }

  [SerializeField] private bool _fetchLocal = false;

  private async void Start()
  {
    if (_fetchLocal)
    {
      await FetchJsonAndImportAsync(
          Path.Combine(LocalBaseFetchingUri, LocalModelOverviewJsonName),
          LocalBaseFetchingUri,
          isLocal: true
      );
    }
  }

  public async Task FetchJsonFromWebAsync(string modelsJson, string modelsBaseUri)
  {
    if (_fetchLocal)
    {
      return;
    }

    await FetchJsonAndImportAsync(modelsJson, modelsBaseUri, isLocal: false);
  }

  private async Task FetchJsonAndImportAsync(string jsonSource, string baseUri, bool isLocal)
  {
    string fileText = isLocal
        ? ReadLocalFile(jsonSource)
        : jsonSource;

    if (string.IsNullOrEmpty(fileText))
    {
      return;
    }

    var json = JsonUtility.FromJson<ModelFileNames>(fileText);

    if (json?.model_names == null)
    {
      Debug.LogError("Models JSON has wrong format.");
      return;
    }

    var files = json.model_names.ToDictionary(
        name => name,
        name => baseUri + name + FileExtensions.Gltf
    );

    ModelsJsonFetched?.Invoke(files);

    await GltfImporterService.Instance.ImportGltfModelsFrom(files, baseUri);
  }

  private string ReadLocalFile(string filePath)
  {
    if (!File.Exists(filePath))
    {
      Debug.LogError($"Local file not found: {filePath}");
      return null;
    }
    return File.ReadAllText(filePath);
  }
}
