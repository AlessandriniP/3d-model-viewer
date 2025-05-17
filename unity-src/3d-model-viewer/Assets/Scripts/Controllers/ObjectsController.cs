using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsController : MonoBehaviour
{
  [SerializeField] private Transform _next;
  [SerializeField] private Transform _current;
  [SerializeField] private Transform _previous;
  [SerializeField] private GameObject[] _allObjects; // going to be removed after gltf import

  private readonly Stack<GameObject> _nextObjects = new();
  private readonly Stack<GameObject> _previousObjects = new();
  private GameObject _currentObject;
  private bool _canGoNext;
  private bool _canGoPrevious;

  private enum ObjectHistory
  {
    Next,
    Previous
  }

  private void Awake()
  {
    foreach (var obj in _allObjects)
    {
      var gameObject = Instantiate(obj, Vector3.zero, Quaternion.identity, transform);
      gameObject.SetActive(false);
      _nextObjects.Push(gameObject);
    }

    ActivateObject(_nextObjects, _previousObjects, ObjectHistory.Next);
  }

  [Button]
  public void ShowNextObject()
  {
    ActivateObject(_nextObjects, _previousObjects, ObjectHistory.Next);
  }

  [Button]
  public void ShowPreviousObject()
  {
    ActivateObject(_previousObjects, _nextObjects, ObjectHistory.Previous);
  }

  private void ActivateObject(Stack<GameObject> fromStack, Stack<GameObject> toStack, ObjectHistory history)
  {
    if (fromStack.Count > 0)
    {
      if (_currentObject)
      {
        toStack.Push(_currentObject);
      }

      var obj = fromStack.Pop();
      obj.SetActive(true);
      _currentObject?.SetActive(false);
      _currentObject = obj;
    }

    CanGoNextOrPrevious(fromStack, toStack, history);
  }

  private void CanGoNextOrPrevious(Stack<GameObject> fromStack, Stack<GameObject> toStack, ObjectHistory history)
  {
    if (history == ObjectHistory.Next)
    {
      _canGoNext = fromStack.Count > 0;
      _canGoPrevious = toStack.Count > 0;
    }
    else
    {
      _canGoPrevious = fromStack.Count > 0;
      _canGoNext = toStack.Count > 0;
    }

    // TODO: call javascript function to set _canGoNext and _canGoPrevious
  }
}
