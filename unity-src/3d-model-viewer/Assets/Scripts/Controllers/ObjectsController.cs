using DG.Tweening;
using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectsController : MonoBehaviour
{
  [SerializeField] private Transform _next;
  [SerializeField] private Transform _current;
  [SerializeField] private Transform _previous;
  [SerializeField] private GameObject[] _allObjects; // TODO: going to be removed after gltf import
  [SerializeField] private CameraController _cameraController;

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
    for (var i = 0; i < _allObjects.Length; i++)
    {
      var position = i == _allObjects.Length - 1 ? _current.position : _next.position;
      var active = i == _allObjects.Length - 1;
      var gameObject = Instantiate(_allObjects[i], position, Quaternion.identity, transform);

      gameObject.SetActive(active);
      _nextObjects.Push(gameObject);
    }

    ProcessObject(_nextObjects, _previousObjects, ObjectHistory.Next);
  }

  [EnableIf(nameof(_canGoNext))]
  [Button]
  public void ShowNextObject()
  {
    if (_cameraController.LockedView || !_canGoNext)
    {
      return;
    }

    MoveObjectIn(
    _nextObjects.First(), _current.position,
    _currentObject, _previous.position,
    _nextObjects, _previousObjects, ObjectHistory.Next
);
  }

  [EnableIf(nameof(_canGoPrevious))]
  [Button]
  public void ShowPreviousObject()
  {
    if (_cameraController.LockedView || !_canGoPrevious)
    {
      return;
    }

    MoveObjectIn(
    _previousObjects.First(), _current.position,
    _currentObject, _next.position,
    _previousObjects, _nextObjects, ObjectHistory.Previous
);
  }

  private void ProcessObject(Stack<GameObject> fromStack, Stack<GameObject> toStack, ObjectHistory history)
  {
    if (fromStack.Count > 0)
    {
      if (_currentObject)
      {
        toStack.Push(_currentObject);
      }

      var obj = fromStack.Pop();
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

  private void MoveObjectIn(
      GameObject moveInObject, Vector3 moveInTarget,
      GameObject moveOutObject, Vector3 moveOutTarget,
      Stack<GameObject> fromStack, Stack<GameObject> toStack, ObjectHistory history)
  {
    var moveSequence = DOTween.Sequence();

    moveSequence.Join(moveInObject.transform.DOMove(moveInTarget, _cameraController.ResetViewSpeed));
    moveSequence.Join(moveOutObject.transform.DOMove(moveOutTarget, _cameraController.ResetViewSpeed));

    moveSequence.OnStart(() =>
    {
      _cameraController.ResetView();
      moveInObject.SetActive(true);
    }).OnComplete(() =>
    {
      moveOutObject.SetActive(false);
      ProcessObject(fromStack, toStack, history);
    });

    moveSequence.Play();
  }
}
