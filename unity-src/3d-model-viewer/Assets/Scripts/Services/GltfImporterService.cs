using System;
using UnityEngine;
using UnityGLTF;

public class GltfImporterService : Singleton<GltfImporterService>
{
  public event Action<GameObject[]> GltfObjectsImported;

  [SerializeField] private GameObject _gltfObject;
  [SerializeField] private Transform _objectParent;

  public void ImportGltfModelsFrom(string[] filePaths)
  {
    var objectCount = filePaths.Length;
    var objects = new GameObject[objectCount];

    for (var i = 0; i < objectCount; i++)
    {
      var obj = Instantiate(_gltfObject, _objectParent);
      var gltfComponent = obj.GetComponent<GLTFComponent>();

      gltfComponent.GLTFUri = filePaths[i];
      gltfComponent.Load();

      objects[i] = obj;
    }

    GltfObjectsImported?.Invoke(objects);
  }
}
