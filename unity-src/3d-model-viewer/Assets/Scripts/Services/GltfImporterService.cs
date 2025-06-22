using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityGLTF;
using UnityGLTF.Loader;

public class GltfImporterService : Singleton<GltfImporterService>
{
  [SerializeField] private ObjectsController _objectsController;
  [SerializeField] private Transform _objectParent;

  private const string _gltfExtension = ".glb";

  private List<GameObject> _objects = new();

  public async Task ImportGltfModelsFrom(Dictionary<string, string> files)
  {
    _objects.Clear();

    foreach (var file in files)
    {
      var obj = await ImportGltfModel(file.Value, file.Key);

      _objects.Add(obj);
    }

    _objectsController.ProcessGltfObjects(_objects.ToArray());
  }

  private async Task<GameObject> ImportGltfModel(string uriDir, string filename)
  {
    var importOpt = new ImportOptions
    {
      DataLoader = new UnityWebRequestLoader(uriDir)
    };

    var import = new GLTFSceneImporter(filename + _gltfExtension, importOpt);

    await import.LoadSceneAsync();

    var sceneObj = import.LastLoadedScene;
    sceneObj.SetActive(false);
    sceneObj.transform.SetParent(_objectParent);
    sceneObj.transform.Rotate(transform.up, 180f);
    sceneObj.name = filename;

    return sceneObj;
  }
}
