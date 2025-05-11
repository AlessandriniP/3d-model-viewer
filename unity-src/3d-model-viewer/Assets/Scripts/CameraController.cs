using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class CameraController : MonoBehaviour
{
  [SerializeField] private Transform _cameraPivot;
  [SerializeField] private Transform _defaultCamPose;

  private const float _xRotationRange = 89f;
  private const float _panSpeed = 5f;
  private const float _resetViewSpeed = 0.2f;
  private const float _scrollZoomSpeed = 100f;
  private const float _mouseZoomSpeed = 50f;
  private const float _minZoomDist = 1f;
  private const float _maxZoomDist = 200f;

  private bool _resettingView;

  private void Awake()
  {
    transform.SetPositionAndRotation(_defaultCamPose.position, _defaultCamPose.rotation);
  }

  private void Start()
  {
    InputActionService.Instance.ScrollWheel += OnScrollWheel;
    InputActionService.Instance.MoveMouse += OnMoveMouse;
  }

  private void Update()
  {
    transform.LookAt(_cameraPivot);
  }

  [Button]
  public void ResetView()
  {
    // create a sequence to group the tweens
    var resetViewSequence = DOTween.Sequence();

    // add the pivot movement to the sequence
    resetViewSequence.Join(_cameraPivot.DOMove(Vector3.zero, _resetViewSpeed));

    // add the camera position and rotation to the sequence
    resetViewSequence.Join(transform.DOMove(_defaultCamPose.position, _resetViewSpeed));
    resetViewSequence.Join(transform.DORotateQuaternion(_defaultCamPose.rotation, _resetViewSpeed));

    // set callbacks for the sequence
    resetViewSequence.OnStart(() =>
    {
      _resettingView = true;
    }).OnComplete(() =>
    {
      _resettingView = false;
    });

    resetViewSequence.Play();
  }

  private void OnScrollWheel(Vector2 direction)
  {
    if (_resettingView)
    {
      return;
    }

    ZoomCamera(direction, _scrollZoomSpeed);
  }

  private void OnMoveMouse(Vector2 axes)
  {
    if (_resettingView)
    {
      return;
    }

    if (InputActionService.Instance.LeftDrag)
    {
      OrbitAroundTarget(axes);
    }

    if (InputActionService.Instance.MiddleDrag)
    {
      PanCamera(axes);
    }

    if (InputActionService.Instance.RightDrag)
    {
      ZoomCamera(axes, _mouseZoomSpeed);
    }
  }

  private void OrbitAroundTarget(Vector2 axes)
  {
    // calculate the current direction of the camera relative to the target
    var direction = transform.position - _cameraPivot.position;
    var currentXAngle = Vector3.SignedAngle(Vector3.up, direction, transform.right);

    // calculate the vertical rotation and clamp it
    var verticalRotation = -axes.y;
    var newXAngle = Mathf.Clamp(currentXAngle + verticalRotation, -_xRotationRange - 90f, _xRotationRange - 90f);

    // calculate the horizontal rotation
    var horizontalRotation = axes.x;

    // set the new position of the camera based on the limited angles
    var horizontalRotationQuat = Quaternion.AngleAxis(horizontalRotation, Vector3.up);
    var verticalRotationQuat = Quaternion.AngleAxis(newXAngle - currentXAngle, transform.right);

    // combine the rotations
    transform.position = horizontalRotationQuat * verticalRotationQuat * direction + _cameraPivot.position;

    // set the z-rotation to 0 to prevent tilting
    var eulerAngles = transform.eulerAngles;
    eulerAngles.z = 0f;
    transform.eulerAngles = eulerAngles;
  }

  private void PanCamera(Vector2 axes)
  {
    // calculate the movement in the local xy-plane of the camera
    var moveRight = axes.x * _panSpeed * Time.deltaTime * -transform.right;
    var moveUp = axes.y * _panSpeed * Time.deltaTime * -transform.up;

    // combine the movements
    var movement = moveRight + moveUp;

    // apply the movement
    transform.position += movement;
    _cameraPivot.position += movement;
  }

  private void ZoomCamera(Vector2 axes, float zoomSpeed)
  {
    // calculate the direction from the camera to the target object
    var directionToTarget = (_cameraPivot.position - transform.position).normalized;

    // calculate the new position based on the scroll direction
    var zoomAmount = axes.y * zoomSpeed * Time.deltaTime;
    var newPosition = transform.position + directionToTarget * zoomAmount;

    // limit the distance between the camera and the target object
    var distanceToTarget = Vector3.Distance(newPosition, _cameraPivot.position);

    if (distanceToTarget >= _minZoomDist && distanceToTarget <= _maxZoomDist)
    {
      transform.position = newPosition;
    }
  }
}
