using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour {
    public static InputManager Instance;

    public event EventHandler OnJumpPressed;
    public event EventHandler OnJumpReleased;
    public event EventHandler OnDashPressed;
    public event EventHandler OnFirePreformed;
    public event EventHandler OnFireReleased;
    public event EventHandler OnUseCardPressed;
    public event EventHandler OnLockPressed;
    public event EventHandler OnLockReleased;
    public event EventHandler OnCardDeckOpened;

    private PlayerInputs playerInputs;

    private void Awake() {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;

        playerInputs = new PlayerInputs();
        playerInputs.Player.Enable();

        playerInputs.Player.Jump.performed += Jump_performed;
        playerInputs.Player.Jump.canceled += Jump_canceled;
        playerInputs.Player.Dash.performed += Dash_performed;
        playerInputs.Player.Fire.performed += Fire_performed;
        playerInputs.Player.Fire.canceled += Fire_canceled;
        playerInputs.Player.UseCard.performed += UseCard_performed;
        playerInputs.Player.Lock.performed += Lock_performed;
        playerInputs.Player.Lock.canceled += Lock_canceled;
        playerInputs.Player.OpenCardDeck.performed += OpenCardDeck_performed;
    }

    private void OpenCardDeck_performed(InputAction.CallbackContext obj) => OnCardDeckOpened?.Invoke(this, EventArgs.Empty);
    private void Lock_canceled(InputAction.CallbackContext obj) => OnLockReleased?.Invoke(this, EventArgs.Empty);
    private void Lock_performed(InputAction.CallbackContext obj) => OnLockPressed?.Invoke(this, EventArgs.Empty);
    private void UseCard_performed(InputAction.CallbackContext obj) => OnUseCardPressed?.Invoke(this, EventArgs.Empty);
    private void Fire_canceled(InputAction.CallbackContext obj) => OnFireReleased?.Invoke(this, EventArgs.Empty);
    private void Fire_performed(InputAction.CallbackContext obj) => OnFirePreformed?.Invoke(this, EventArgs.Empty);
    private void Dash_performed(InputAction.CallbackContext obj) => OnDashPressed?.Invoke(this, EventArgs.Empty);
    private void Jump_canceled(InputAction.CallbackContext obj) => OnJumpReleased?.Invoke(this, EventArgs.Empty);
    private void Jump_performed(InputAction.CallbackContext obj) => OnJumpPressed?.Invoke(this, EventArgs.Empty);

    private void OnDestroy() {
        playerInputs.Player.Jump.performed -= Jump_performed;
        playerInputs.Player.Jump.canceled -= Jump_canceled;
        playerInputs.Player.Dash.performed -= Dash_performed;
        playerInputs.Player.Fire.performed -= Fire_performed;
        playerInputs.Player.Fire.canceled -= Fire_canceled;
        playerInputs.Player.UseCard.performed -= UseCard_performed;
        playerInputs.Player.Lock.performed -= Lock_performed;
        playerInputs.Player.Lock.canceled -= Lock_canceled;

        playerInputs.Player.Disable();
    }

    public float GetHorizontalMovement() => playerInputs.Player.HorizontalMove.ReadValue<float>();
    public float GetVerticalMovement() => playerInputs.Player.VerticalMove.ReadValue<float>();
    public float GetScrollCardAxis() => playerInputs.Player.ScrollCards.ReadValue<float>();
    

}
