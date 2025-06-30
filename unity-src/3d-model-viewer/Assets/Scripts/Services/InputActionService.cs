using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputActionService : Singleton<InputActionService>
{
  public event Action<Vector2> ScrollWheel;
  public event Action<Vector2> MoveMouse;
  public event Action ShowPreviousObject;
  public event Action ShowNextObject;
  public event Action ResetView;

  public bool RightDrag { get; private set; }
  public bool LeftDrag { get; private set; }
  public bool MiddleDrag { get; private set; }

  [SerializeField] private InputActionReference _rightDragAction;
  [SerializeField] private InputActionReference _leftDragAction;
  [SerializeField] private InputActionReference _middleDragAction;
  [SerializeField] private InputActionReference _scrollWheelAction;
  [SerializeField] private InputActionReference _moveMouseAction;
  [SerializeField] private InputActionReference _showPreviousObjectAction;
  [SerializeField] private InputActionReference _showNextObjectAction;
  [SerializeField] private InputActionReference _resetViewAction;

  protected override void Awake()
  {
    base.Awake();

    _rightDragAction.action.performed += OnRightDragStarted;
    _rightDragAction.action.canceled += OnRightDragStopped;

    _leftDragAction.action.performed += OnLeftDragStarted;
    _leftDragAction.action.canceled += OnLeftDragStopped;

    _middleDragAction.action.performed += OnMiddleDragStarted;
    _middleDragAction.action.canceled += OnMiddleDragStopped;

    _scrollWheelAction.action.performed += OnScrollWheel;

    _moveMouseAction.action.performed += OnMoveMouse;

    _showPreviousObjectAction.action.performed += OnShowPreviousObject;
    _showNextObjectAction.action.performed += OnShowNextObject;
    _resetViewAction.action.performed += OnResetView;
  }

  private void OnRightDragStarted(InputAction.CallbackContext ctx)
  {
    RightDrag = true;
  }

  private void OnRightDragStopped(InputAction.CallbackContext ctx)
  {
    RightDrag = false;
  }

  private void OnLeftDragStarted(InputAction.CallbackContext ctx)
  {
    LeftDrag = true;
  }

  private void OnLeftDragStopped(InputAction.CallbackContext ctx)
  {
    LeftDrag = false;
  }

  private void OnMiddleDragStarted(InputAction.CallbackContext ctx)
  {
    MiddleDrag = true;
  }

  private void OnMiddleDragStopped(InputAction.CallbackContext ctx)
  {
    MiddleDrag = false;
  }

  private void OnScrollWheel(InputAction.CallbackContext ctx)
  {
    ScrollWheel?.Invoke(ctx.ReadValue<Vector2>());
  }

  private void OnMoveMouse(InputAction.CallbackContext ctx)
  {
    MoveMouse?.Invoke(ctx.ReadValue<Vector2>());
  }

  private void OnShowPreviousObject(InputAction.CallbackContext ctx)
  {
    ShowPreviousObject?.Invoke();
  }

  private void OnShowNextObject(InputAction.CallbackContext ctx)
  {
    ShowNextObject?.Invoke();
  }

  private void OnResetView(InputAction.CallbackContext ctx)
  {
    ResetView?.Invoke();
  }
}
