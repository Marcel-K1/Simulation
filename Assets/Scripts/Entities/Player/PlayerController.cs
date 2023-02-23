/*********************************************************************************************
* Project: Simulation
* File   : PlayerController
* Date   : 25.05.2022
* Author : Marcel Klein
*
* Controls the player take the input properties from the input manager and uses them for movement and interaction methods.
* 
* History:
*    09.02.2022    MK    Created
*    25.05.2022    MK    Updated for new project Simulation
*********************************************************************************************/


using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    #region Variables

    private CharacterController playerController = null;
    private Camera playerCamera = null;
    private Vector2 currentDirectionVelocity = Vector2.zero;    
    private Vector2 currentDirection = Vector2.zero;
    private Vector2 currentMouseDelta = Vector2.zero;
    private Vector2 currentMouseDeltaVelocity = Vector2.zero;
    private Vector3 upVector = Vector3.zero;
    private Vector3 movementVelocity = Vector3.zero;
    private float cameraPitch = 0.0f;
    private float velocityY = 0.0f;
    private float cameraPitchClampUp = 90f;
    private float cameraPitchClampDown = -90f;
    private float jumpMultiplyer = -1.5f;
    private bool groundedPlayer = false;
    private bool terraintToSteep = false;
    private Vector2 moveInput = Vector2.zero;
    private bool lockCursor = true;

    [Header("Important Player Values")]
    [SerializeField]
    private float jumpHeight = 2f;
    [SerializeField]
    private float playerSpeed = 10.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField, Range(0f, 0.5f)]
    private float moveSmoothTime = 0.5f;
    [SerializeField]
    [Range(0.1f, 0.3f)]
    private float mouseSmoothTime = 0.2f;
    [SerializeField]
    [Range(0.2f, 1.0f)]
    private float mouseSensetivity = 0.2f;

    [Header("List For Audio Zones")]
    [SerializeField]
    private List<AudioSource> audioSources = new List<AudioSource>();

#endregion

    #region Properties

    public Vector2 MoveInput { get { return moveInput; } } 
    public bool LockCursor { get { return lockCursor; } set { lockCursor = value; } }

    #endregion

    #region Methods

    private void Awake()
    {
        playerController = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();
    }
    public void Update()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    private void FixedUpdate()
    {
        UpdateGroundPlayer();
        UpdateJumpPlayer();
        UpdateMouseLook();
        UpdateMovement();
        UpdateExitInput();
    }

    //Checking for collisions with trigger zones and MazePlane
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Pregenerated Mesh Trigger":
                GameEvents.instance.MeshGeneratorTriggerEnter();
                break;
            case "PCG Noise":
                GameEvents.instance.PCGNoiseEnter();
                break;
            case "PCG Simple":
                GameEvents.instance.PCGSimpleEnter();
                break;
            case "Procedural Mesh Start":
                GameEvents.instance.ProceduralMeshStartEnter();
                break;
            case "Procedural Mesh Stop":
                GameEvents.instance.ProceduralMeshStopEnter();
                break;
            default:
                break;
        }

        if (other.tag == "MazePlane")
        {
            gameObject.GetComponentInChildren<Light>().intensity = 60f;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "MazePlane")
        {
            gameObject.GetComponentInChildren<Light>().intensity = 0f;
        }
    }

    //Checking for steepness and AI collision
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Steepness
        if ((Vector3.Angle(Vector3.up, hit.normal) > playerController.slopeLimit))
        {
            terraintToSteep = true;
        }
        else
        {
            terraintToSteep = false;
        }

        if (hit.collider.tag == "MazePlane")
        {
            gameObject.GetComponentInChildren<Light>().intensity = 60f;
        }
        else
        {
            gameObject.GetComponentInChildren<Light>().intensity = 0f;
        }

    }
    //Respawn
    ////Respawn
    //public void OnTriggerEnter(Collider _other)
    //{
    //    switch (_other.gameObject.tag)
    //    {
    //        case "Water":
    //            //Checking for currently playing audio source and pausing it
    //            for (int i = 0; i < audioSources.Count; i++)
    //            {
    //                switch (audioSources[i].name)
    //                {
    //                    case "Audio Zone Center":
    //                        if (audioSources[i].isPlaying)
    //                        {
    //                            audioSources[i].Pause();
    //                        }
    //                        break;
    //                    case "Audio Zone Forrest":
    //                        if (audioSources[i].isPlaying)
    //                        {
    //                            audioSources[i].Pause();
    //                        }
    //                        break;
    //                    case "Audio Zone Swamp":
    //                        if (audioSources[i].isPlaying)
    //                        {
    //                            audioSources[i].Pause();
    //                        }
    //                        break;
    //                    case "Audio Zone Canyon":
    //                        if (audioSources[i].isPlaying)
    //                        {
    //                            audioSources[i].Pause();
    //                        }
    //                        break;
    //                    case "Audio Zone Sea":
    //                        if (audioSources[i].isPlaying)
    //                        {
    //                            audioSources[i].Pause();
    //                        }
    //                        break;
    //                    case "Audio Zone Mountains":
    //                        if (audioSources[i].isPlaying)
    //                        {
    //                            audioSources[i].Pause();
    //                        }
    //                        break;
    //                    default:
    //                        break;
    //                }
    //            }
    //            playerController.enabled = false;
    //            transform.position = LevelManager.Instance.FindRandomCharSP().position; 
    //            playerController.enabled = true;
    //            break;
    //        default:
    //            break;
    //    }
    //}

    //Physic methods
    private void UpdateGroundPlayer()
    {
        groundedPlayer = playerController.isGrounded;
        if (groundedPlayer && upVector.y < 0)
        {
            upVector.y = -0.1f;
        }
    }
    private void UpdateJumpPlayer()
    {

        if (InputManager.Instance.Jumped && groundedPlayer)
        {
            if (!terraintToSteep)
            {
                upVector.y += Mathf.Sqrt(jumpHeight * jumpMultiplyer * gravityValue);
            }
            else if (terraintToSteep)
            {
                upVector.y = 0;
            }
            else
            {
                upVector.y += Mathf.Sqrt(jumpHeight * jumpMultiplyer * gravityValue);
            }
        }
        upVector.y += gravityValue * Time.deltaTime;
        //playerRB.MovePosition(upVector * Time.deltaTime);
        playerController.Move(upVector * Time.deltaTime);

    }
    private void UpdateMouseLook()
    {
        Vector2 lookInput = InputManager.Instance.Look;
        Vector2 targetMouseDelta = new Vector2(lookInput.x, lookInput.y);
        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);
        cameraPitch -= currentMouseDelta.y * mouseSensetivity;
        cameraPitch = Mathf.Clamp(cameraPitch, cameraPitchClampDown, cameraPitchClampUp);
        playerCamera.transform.localEulerAngles = Vector3.right * cameraPitch;
        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensetivity);   
    }
    private void UpdateMovement()
    {
        moveInput = InputManager.Instance.Move;
        Vector2 targetDirection = new Vector2(moveInput.x, moveInput.y).normalized;
        currentDirection = Vector2.SmoothDamp(currentDirection, targetDirection, ref currentDirectionVelocity, moveSmoothTime);
        if (playerController.isGrounded)
        {
            velocityY = 0f;
            velocityY += gravityValue * Time.deltaTime;
        }
        movementVelocity = (transform.forward * currentDirection.y + transform.right * currentDirection.x) * playerSpeed + Vector3.up * velocityY;
        playerController.Move(movementVelocity * Time.deltaTime);
        //playerRB.MovePosition(movementVelocity * Time.deltaTime);
    }

    private void UpdateExitInput()
    {
        if (InputManager.Instance.Exit == 1)
        {
            Quit();
        }
    }

    private void Quit()
    {
#if UNITY_STANDALONE
        Application.Quit();
#endif
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    #endregion
}
