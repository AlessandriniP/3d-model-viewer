using UnityEngine;

public class CameraController : MonoBehaviour
{
  private void Awake()
  {
    InputActionService.Instance.ScrollWheel += OnScrollWheel;
    InputActionService.Instance.MoveMouse += OnMoveMouse;
  }

  private void OnScrollWheel(Vector2 direction)
  {
    Debug.Log(direction);
  }

  private void OnMoveMouse(Vector2 axes)
  {
    Debug.Log(axes);
  }
}
