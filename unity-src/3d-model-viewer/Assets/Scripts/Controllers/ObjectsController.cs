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
  [SerializeField] private CameraController _cameraController;

  private readonly Stack<GameObject> _nextObjects = new();
  private readonly Stack<GameObject> _previousObjects = new();

  private GameObject[] _allObjects;

  public GameObject CurrentObject { get; private set; }
  public bool CanGoPrevious { get; private set; }
  public bool CanGoNext { get; private set; }

  private enum ObjectHistory
  {
    Next,
    Previous
  }

  [EnableIf(nameof(CanGoNext))]
  [Button]
  public void ShowNextObject()
  {
    if (_cameraController.LockedView || !CanGoNext)
    {
      return;
    }

    MoveObjectIn(
    _nextObjects.First(), _current.position,
    CurrentObject, _previous.position,
    _nextObjects, _previousObjects, ObjectHistory.Next
);
  }

  [EnableIf(nameof(CanGoPrevious))]
  [Button]
  public void ShowPreviousObject()
  {
    if (_cameraController.LockedView || !CanGoPrevious)
    {
      return;
    }

    MoveObjectIn(
      _previousObjects.First(), _current.position,
      CurrentObject, _next.position,
      _previousObjects, _nextObjects, ObjectHistory.Previous
    );
  }

  public void ProcessGltfObjects(GameObject[] objects)
  {
    _allObjects = objects;

    for (var i = 0; i < _allObjects.Length; i++)
    {
      var position = i == _allObjects.Length - 1 ? _current.position : _next.position;
      var active = i == _allObjects.Length - 1;

      _allObjects[i].transform.position = position;
      _allObjects[i].SetActive(active);
      _nextObjects.Push(_allObjects[i]);
    }

    ProcessObject(_nextObjects, _previousObjects, ObjectHistory.Next);
  }

  private void ProcessObject(Stack<GameObject> fromStack, Stack<GameObject> toStack, ObjectHistory history)
  {
    if (fromStack.Count > 0)
    {
      if (CurrentObject)
      {
        toStack.Push(CurrentObject);
      }

      var obj = fromStack.Pop();
      CurrentObject = obj;
    }

    CanGoNextOrPrevious(fromStack, toStack, history);
  }

  private void CanGoNextOrPrevious(Stack<GameObject> fromStack, Stack<GameObject> toStack, ObjectHistory history)
  {
    if (history == ObjectHistory.Next)
    {
      CanGoNext = fromStack.Count > 0;
      CanGoPrevious = toStack.Count > 0;
    }
    else
    {
      CanGoPrevious = fromStack.Count > 0;
      CanGoNext = toStack.Count > 0;
    }
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
