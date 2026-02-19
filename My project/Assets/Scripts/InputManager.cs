using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    // Declaring Singleton ------------------------
    private static InputManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one InputManager in the scene.");
        }
        instance = this;
    }
    public static InputManager GetInstance()
    {
        return instance;
    }

    // Input Variables ----------------------------
    private bool _continuePressed = false;

    // Input Functions ----------------------------
    public void ContinuePressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _continuePressed = true;
        }
        else if(context.canceled){
            _continuePressed = false;
        }
    }
}
