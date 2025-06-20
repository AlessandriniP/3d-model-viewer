using System.Collections.Generic;
using UnityEngine;
using UnityGLTF;

public class GltfImporterService : Singleton<GltfImporterService>
{
  [SerializeField] private ObjectsController _objectsController;
  [SerializeField] private GameObject _gltfObject;
  [SerializeField] private Transform _objectParent;

  private List<GameObject> _objects = new();

  public void ImportGltfModelsFrom(Dictionary<string, string> files)
  {
    _objects.Clear();

    foreach (var file in files)
    {
      var obj = Instantiate(_gltfObject, _objectParent);
      obj.name = file.Value;

      var gltfComponent = obj.GetComponent<GLTFComponent>();
      gltfComponent.GLTFUri = file.Key;
      gltfComponent.Load();

      _objects.Add(obj);
    }

    _objectsController.ProcessGltfObjects(_objects.ToArray());
  }
}
