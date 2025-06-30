using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class CameraController : MonoBehaviour
{
  [SerializeField] private Transform _cameraPivot;
  [SerializeField] private Transform _defaultCamPose;

  private const float _xRotationRange = 89f;
  private const float _panSpeed = 5f;
  private const float _scrollZoomSpeed = 150f;
  private const float _mouseZoomSpeed = 35f;
  private const float _minZoomDist = 1f;
  private const float _maxZoomDist = 200f;

  public float ResetViewSpeed { get; private set; } = 0.2f;
  public bool LockedView { get; set; }

  private void Awake()
  {
    transform.SetPositionAndRotation(_defaultCamPose.position, _defaultCamPose.rotation);
  }

  private void Start()
  {
    InputActionService.Instance.ScrollWheel += OnScrollWheel;
    InputActionService.Instance.MoveMouse += OnMoveMouse;
    InputActionService.Instance.ResetView += ResetView;
  }

  private void Update()
  {
    transform.LookAt(_cameraPivot);
  }

  [Button]
  public void ResetView()
  {
    if (LockedView)
    {
      return;
    }

    var resetViewSequence = DOTween.Sequence();

    resetViewSequence.Join(_cameraPivot.DOMove(Vector3.zero, ResetViewSpeed));

    resetViewSequence.Join(transform.DOMove(_defaultCamPose.position, ResetViewSpeed));
    resetViewSequence.Join(transform.DORotateQuaternion(_defaultCamPose.rotation, ResetViewSpeed));

    resetViewSequence.OnStart(() =>
    {
      LockedView = true;
    }).OnComplete(() =>
    {
      LockedView = false;
    });

    resetViewSequence.Play();
  }

  private void OnScrollWheel(Vector2 direction)
  {
    if (LockedView)
    {
      return;
    }

    ZoomCamera(direction, _scrollZoomSpeed);
  }

  private void OnMoveMouse(Vector2 axes)
  {
    if (LockedView)
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
    var direction = transform.position - _cameraPivot.position;
    var currentXAngle = Vector3.SignedAngle(Vector3.up, direction, transform.right);

    var verticalRotation = -axes.y;
    var newXAngle = Mathf.Clamp(currentXAngle + verticalRotation, -_xRotationRange - 90f, _xRotationRange - 90f);

    var horizontalRotation = axes.x;

    var horizontalRotationQuat = Quaternion.AngleAxis(horizontalRotation, Vector3.up);
    var verticalRotationQuat = Quaternion.AngleAxis(newXAngle - currentXAngle, transform.right);

    transform.position = horizontalRotationQuat * verticalRotationQuat * direction + _cameraPivot.position;

    var eulerAngles = transform.eulerAngles;
    eulerAngles.z = 0f;
    transform.eulerAngles = eulerAngles;
  }

  private void PanCamera(Vector2 axes)
  {
    var moveRight = axes.x * _panSpeed * Time.deltaTime * -transform.right;
    var moveUp = axes.y * _panSpeed * Time.deltaTime * -transform.up;

    var movement = moveRight + moveUp;

    transform.position += movement;
    _cameraPivot.position += movement;
  }

  private void ZoomCamera(Vector2 axes, float zoomSpeed)
  {
    var directionToTarget = (_cameraPivot.position - transform.position).normalized;
    var currentDistance = Vector3.Distance(transform.position, _cameraPivot.position);
    var zoomAmount = axes.y * zoomSpeed * Time.deltaTime;

    var targetDistance = currentDistance - zoomAmount;

    targetDistance = Mathf.Clamp(targetDistance, _minZoomDist, _maxZoomDist);

    var newPosition = _cameraPivot.position - directionToTarget * targetDistance;

    transform.position = newPosition;
  }
}
