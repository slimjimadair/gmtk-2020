using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Components
    Rigidbody2D rb;
    Animator anim;

    // Settings
    public float runSpeed;
    public float sprintMultiplier;
    public float jumpForce;
    public float dashDistance;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask whatIsGround;

    public GameObject abilityUI;
    GameObject game;

    public AudioSource footstep1Audio;
    public AudioSource footstep2Audio;
    public AudioSource jumpAudio;
    public AudioSource dashAudio;
    int stepSoundInterval = 12;
    int stepSoundCount = 0;
    bool stepSound1 = true;

    public GameObject dustLeft;
    public GameObject dustRight;

    Vector3 playerStart;
    float deathFloor = -20f;

    // Input Trackers
    float sideInput;
    float fixedSideInput;
    bool upInput;
    bool fixedUpInput;
    bool downInput;
    bool fixedDownInput;
    bool spaceInput;
    bool fixedSpaceInput;
    bool shiftInput;
    bool fixedShiftInput;

    // Contexts
    bool isGrounded;
    bool isDangerous = false;
    bool facingRight = true;

    // Ability Limits
    int[] abilityCounts = new int[] { 0, 0, 0, 0 };
    int sprintCount;
    int jumpCount;
    int airJumpCount;
    int dashCount;

    private void Start()
    {
        // Get Elements
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerStart = transform.position;

        game = GameObject.FindGameObjectWithTag("GameController");
        Restart();
    }

    private void Update()
    {
        // Get Player Inputs
        sideInput = Input.GetAxisRaw("Horizontal");
        upInput = Input.GetKeyDown(KeyCode.UpArrow) || upInput;
        downInput = Input.GetKeyDown(KeyCode.DownArrow) || downInput;
        spaceInput = Input.GetKeyDown(KeyCode.Space) || spaceInput;
        shiftInput = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
    }

    private void FixedUpdate()
    {
        // Set Fixed Inputs
        fixedSideInput = sideInput;
        if (upInput) {
            fixedUpInput = true;
            upInput = false;
        }
        if (downInput) {
            fixedDownInput = true;
            downInput = false;
        }
        if (spaceInput) {
            fixedSpaceInput = true;
            spaceInput = false;
        }
        if (fixedShiftInput && !shiftInput) {
            sprintCount -= 1;
        }
        fixedShiftInput = (sprintCount > 0) ? shiftInput : false;

        // Get Context
        isGrounded = Physics2D.OverlapCapsule(groundCheck.position, new Vector2(groundCheckRadius, 1 + groundCheckRadius), CapsuleDirection2D.Horizontal, 0, whatIsGround);

        // Set Base Velocity
        float hVel = fixedSideInput * runSpeed;
        float vVel = rb.velocity.y;

        // ABILITIES
        // Sprint - apply a speed multiplier
        if (fixedShiftInput)
        {
            hVel *= sprintMultiplier;
        }
        isDangerous = fixedShiftInput;

        // Jump - add vertical velocity if on ground
        if (fixedUpInput && isGrounded && jumpCount > 0)
        {
            vVel = jumpForce;
            jumpCount -= 1;
            jumpAudio.Play();
        }

        // Air Jump - add vertical velocity if in air
        if (fixedUpInput && !isGrounded && airJumpCount > 0)
        {
            vVel = jumpForce;
            airJumpCount -= 1;
            jumpAudio.Play();
        }
        fixedUpInput = false;

        // Dash - teleport in movement direction
        if (fixedSpaceInput && dashCount > 0)
        {
            isDangerous = true;
            rb.MovePosition(new Vector2(rb.position.x + (fixedSideInput * dashDistance), rb.position.y));
            fixedSpaceInput = false;
            dashCount -= 1;
            dashAudio.Play();
            isDangerous = false;
        }

        // Update Velocity
        rb.velocity = new Vector2(hVel, vVel);

        // Set Animation
        if ((!facingRight && hVel > 0) || (facingRight && hVel <0))
        {
            Flip();
        }
        anim.SetFloat("Speed", Mathf.Abs(hVel));
        anim.SetBool("IsJumping", (!isGrounded && vVel > 0));
        anim.SetBool("IsFalling", (!isGrounded && vVel <= 0));

        // Update UI
        int[] uiCounts = new int[] { jumpCount, airJumpCount, sprintCount, dashCount };
        abilityUI.GetComponent<AbilityUI>().UpdateUI(uiCounts);

        // Footstep Sound
        if (isGrounded && Mathf.Abs(hVel) > 0 && stepSoundCount >= stepSoundInterval)
        {
            stepSoundCount = 0;
            if (stepSound1)
            {
                footstep1Audio.Play();
            } else {
                footstep2Audio.Play();
            }
            stepSound1 = !stepSound1;
        } else {
            stepSoundCount += 1;
        }

        // Dust Particles
        if (isGrounded && hVel > 0) {
            dustRight.SetActive(true);
            dustLeft.SetActive(false);
        } else if (isGrounded && hVel < 0) {
            dustLeft.SetActive(true);
            dustRight.SetActive(false);
        } else {
            dustLeft.SetActive(false);
            dustRight.SetActive(false);
        }

        // Check Death
        if (rb.position.y < deathFloor)
        {
            game.GetComponent<GameController>().Die();
        }
    }

    public void UpdateAbilities(int[] newAbilityCounts)
    {
        abilityCounts = newAbilityCounts;
        SetAbilities();
    }

    void SetAbilities()
    {
        // Reset Ability Counts
        jumpCount = abilityCounts[0];
        airJumpCount = abilityCounts[1];
        sprintCount = abilityCounts[2];
        dashCount = abilityCounts[3];
    }

    public void Restart()
    {
        // Reposition
        transform.position = playerStart;
        rb.velocity = Vector2.zero;

        // Reset Ability Counts
        SetAbilities();
        abilityUI.GetComponent<AbilityUI>().BuildUI(abilityCounts);
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    public bool IsDangerous()
    {
        return isDangerous;
    }

}
