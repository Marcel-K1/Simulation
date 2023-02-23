/*********************************************************************************************
* Project: Simulation
* File   : Animation Controller
* Date   : 25.05.2022
* Author : Marcel Klein
*
* Controls the animations and sounds for the player.
* 
* History:
*    09.02.2022    MK    Created
*    25.05.2022    MK    Updated for new project Simulation
*********************************************************************************************/


using UnityEngine;


[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerController))]
public class AnimationController : MonoBehaviour
{
    #region Variables
    private string idleParameterName = "Idle";
    private string runningParameterName = "Running";
    private string jumpingParameterName = "Jumping";

    private int idleHash = 0;
    private int runningHash = 0;
    private int jumpingHash = 0;

    [Header("Required Components")]
    [SerializeField]
    private AudioSource walkingSource = null;
    [SerializeField]
    private AudioSource jumpingSource = null;
    [SerializeField]
    private AudioClip walkingClip = null;
    [SerializeField]
    private AudioClip jumpingClip = null;

    private CharacterController characterController = null;
    private Animator playerAnimator = null;
    private PlayerController playerController = null;
    private bool alreadyPlayedWalking = false;

    #endregion

    #region Methods
    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        playerController = GetComponent<PlayerController>();

        //walkingSource = gameObject.AddComponent<AudioSource>();
        //jumpingSource = gameObject.AddComponent<AudioSource>();

        idleHash = Animator.StringToHash(idleParameterName);
        runningHash = Animator.StringToHash(runningParameterName);
        jumpingHash = Animator.StringToHash(jumpingParameterName);

        //walkingSource.clip = walkingClip;
        //jumpingSource.clip = jumpingClip;

        //walkingSource.playOnAwake = false;
        //walkingSource.loop = true;
        //jumpingSource.playOnAwake = false;
        //jumpingSource.loop = false;

    }
    private void Update()
    {
        //Running animation and sound
        if (playerController.MoveInput.sqrMagnitude > 0)
        {
            playerAnimator.SetBool(idleHash, false);
            playerAnimator.SetBool(runningHash, true);
            //if (InputManager.Instance.Jumped == true && characterController.isGrounded == false)
            //{
            //    if (characterController.isGrounded == true)
            //    {
            //        if (!alreadyPlayedWalking)
            //        {
            //            walkingSource.Play();
            //        }
            //        else
            //        {
            //            walkingSource.UnPause();
            //        }
            //    }
            //    else if (characterController.isGrounded == false)
            //    {
            //        walkingSource.Pause();
            //        alreadyPlayedWalking = true;
            //    }
            //}
            //else if (InputManager.Instance.Jumped == false && characterController.isGrounded == false)
            //{
            //    if (characterController.isGrounded == true)
            //    {
            //        if (!alreadyPlayedWalking)
            //        {
            //            walkingSource.Play();
            //        }
            //        else
            //        {
            //            walkingSource.UnPause();
            //        }
            //    }
            //}
            //else
            //{
            //    if (walkingSource.isPlaying == false)
            //    {
            //        if (!alreadyPlayedWalking)
            //        {
            //            walkingSource.Play();
            //        }
            //        else
            //        {
            //            walkingSource.UnPause();
            //        }
            //    }
            //}
        }
        if (playerController.MoveInput.sqrMagnitude == 0)
        {
            playerAnimator.SetBool(runningHash, false);
            playerAnimator.SetBool(idleHash, true);
            //if (walkingSource.isPlaying == true)
            //{
            //    walkingSource.Pause();
            //    alreadyPlayedWalking = true;
            //}
        }

        //Jumping animation
        if (characterController.isGrounded)
        {
            playerAnimator.SetBool(jumpingHash, false);
        }
        if (!characterController.isGrounded)
        {
            playerAnimator.SetBool(jumpingHash, true);
            playerAnimator.SetBool(runningHash, false);
            playerAnimator.SetBool(idleHash, false);
        }
    }

    //Jumping sound
    //public void OnSoundPlaying()
    //{
    //    jumpingSource.PlayOneShot(jumpingClip);
    //}
    //private void OnEnable()
    //{
    //    if (InputManager.Instance != null)
    //    {
    //        InputManager.Instance.JumpSoundEvent += OnSoundPlaying;
    //    }
    //}
    //private void OnDisable()
    //{
    //    if (InputManager.Instance != null)
    //    {
    //        InputManager.Instance.JumpSoundEvent -= OnSoundPlaying;
    //    }
    //}

    #endregion
}
