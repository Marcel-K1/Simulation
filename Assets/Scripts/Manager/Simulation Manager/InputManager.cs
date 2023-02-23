/*******************************************************************************
* Project: Simulation
* File   : InputManager
* Date   : 29.05.2022
* Author : Marcel Klein
*
* Takes input from the new input system and puts it into properties for the player controller to use.
* 
* History:
*    29.05.2022    MK    Created
*******************************************************************************/


using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputManager : MonoBehaviour
{
    #region Variables
    //Singleton
    private static InputManager instance;
    public static InputManager Instance
    {
        get
        {
            return instance;
        }
    }
    #endregion

    #region Methods

    //Input
    private InputActions playerControls = null;

    //Event (for communicating with animation controller)
    //private event Action jumpSoundEvent = null;

    //public event Action JumpSoundEvent { add { jumpSoundEvent += value; } remove { jumpSoundEvent -= value; } }

    //Auto-Properties
    public Vector2 Move { get; private set; }
    public Vector2 Look { get; private set; }
    public bool Jumped { get; private set; }
    public float Jump { get; private set; }
    public float Use { get; private set; }
    public float Exit { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        playerControls = new InputActions();

    }

    //Eventszuweisung
    private void OnEnable()
    {
        playerControls.Enable();
        playerControls.Player.Move.performed += context => Move = context.ReadValue<Vector2>();
        playerControls.Player.Move.canceled += context => Move = context.ReadValue<Vector2>();
        playerControls.Player.Look.performed += context => Look = context.ReadValue<Vector2>();
        playerControls.Player.Look.canceled += context => Look = context.ReadValue<Vector2>();
        //playerControls.Player.Jump.started += context =>
        //{
        //    jumpSoundEvent.Invoke();
        //};
        playerControls.Player.Jump.performed += context => 
        {
            Jump = 0; 
            Jumped = true;
        };
        playerControls.Player.Jump.canceled += context =>
        {
            Jump = 1;
            Jumped = false;
        };
        playerControls.Player.Use.performed += context => Use = context.ReadValue<float>();
        playerControls.Player.Use.canceled += context => Use = context.ReadValue<float>();
        playerControls.Player.Jump.started += context => Jump = context.ReadValue<float>();
        playerControls.Player.Jump.canceled += context => Jump = context.ReadValue<float>();
        playerControls.Player.Exit.performed += context => Exit = context.ReadValue<float>();
        playerControls.Player.Exit.canceled += context => Exit = context.ReadValue<float>();
    }
    private void OnDisable()
    {
        playerControls.Player.Move.canceled -= context => Move = context.ReadValue<Vector2>();
        playerControls.Player.Move.performed -= context => Move = context.ReadValue<Vector2>();
        playerControls.Player.Look.canceled -= context => Look = context.ReadValue<Vector2>();
        playerControls.Player.Look.performed -= context => Look = context.ReadValue<Vector2>();
        playerControls.Player.Jump.started -= context => {};
        playerControls.Player.Jump.canceled -= context => {};
        playerControls.Player.Jump.performed -= context => {};
        playerControls.Player.Use.canceled -= context => Use = context.ReadValue<float>();
        playerControls.Player.Use.performed -= context => Use = context.ReadValue<float>();
        playerControls.Player.Jump.started -= context => Jump = context.ReadValue<float>();
        playerControls.Player.Jump.canceled -= context => Jump = context.ReadValue<float>();
        playerControls.Player.Exit.canceled -= context => Exit = context.ReadValue<float>();
        playerControls.Player.Exit.performed -= context => Exit = context.ReadValue<float>();
        playerControls.Disable();
    }

    #endregion

    #region [!Just for research!]Alternative Input Management with events
    
    //Event
    //private event Action movingEvent = () => { };

    //Eventproperties
    //public event Action MovingEvent
    //{
    //    add { movingEvent += value; }
    //    remove { movingEvent -= value; }
    //}

    //Eventsubscriber
    //public void OnMove(InputAction.CallbackContext context)
    //{
    //    Move = context.ReadValue<Vector2>();
    //    //movingEvent.Invoke();
    //}

    //Zusätzlich dann im PlayerController :
    //private void OnEnable()
    //{
    //    inputManager.MovingEvent += MovePlayer;
    //}
    //private void OnDisable()
    //{
    //    inputManager.MovingEvent -= MovePlayer;
    //}

    #endregion
}
