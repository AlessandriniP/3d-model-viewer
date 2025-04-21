using UnityEngine;

public class CameraController : MonoBehaviour
{
  [SerializeField] private Transform _lookAtTransform;

  private const float _scrollZoomSpeed = 5f;
  private const float _maxZoomDist = 10f;
  private const float _minZoomDist = 2f;
  private const float _xRotationRange = 89f;

  private void Start()
  {
    InputActionService.Instance.ScrollWheel += OnScrollWheel;
    InputActionService.Instance.MoveMouse += OnMoveMouse;
  }

  private void Update()
  {
    transform.LookAt(_lookAtTransform);
  }

  private void OnScrollWheel(Vector2 direction)
  {
    Debug.Log(direction);
  }

  private void OnMoveMouse(Vector2 axes)
  {
    if (InputActionService.Instance.LeftDrag)
    {
      OrbitAroundTarget(axes);
    }
  }

  private void OrbitAroundTarget(Vector2 axes)
  {
    // calculate the current direction of the camera relative to the target
    var direction = transform.position - _lookAtTransform.position;
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
    transform.position = horizontalRotationQuat * verticalRotationQuat * direction + _lookAtTransform.position;

    // set the z-rotation to 0 to prevent tilting
    var eulerAngles = transform.eulerAngles;
    eulerAngles.z = 0f;
    transform.eulerAngles = eulerAngles;
  }
}
