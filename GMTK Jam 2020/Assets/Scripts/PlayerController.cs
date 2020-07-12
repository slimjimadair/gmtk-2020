using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Components
    Rigidbody2D rb;

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

    Vector3 playerStart = new Vector3(0f, -2.25f, 0f);
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
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        // Set Base Velocity
        float hVel = fixedSideInput * runSpeed;
        float vVel = rb.velocity.y;

        // ABILITIES
        // Sprint - apply a speed multiplier
        if (fixedShiftInput)
        {
            hVel *= sprintMultiplier;
        }

        // Jump - add vertical velocity if on ground
        if (fixedUpInput && isGrounded && jumpCount > 0)
        {
            vVel = jumpForce;
            jumpCount -= 1;
        }

        // Air Jump - add vertical velocity if in air
        if (fixedUpInput && !isGrounded && airJumpCount > 0)
        {
            vVel = jumpForce;
            airJumpCount -= 1;
        }
        fixedUpInput = false;

        // Dash - teleport in movement direction
        if (fixedSpaceInput && dashCount > 0)
        {
            rb.MovePosition(new Vector2(rb.position.x + (fixedSideInput * dashDistance), rb.position.y));
            fixedSpaceInput = false;
            dashCount -= 1;
        }

        // Update Velocity
        rb.velocity = new Vector2(hVel, vVel);

        // Update UI
        int[] uiCounts = new int[] { jumpCount, airJumpCount, sprintCount, dashCount };
        abilityUI.GetComponent<AbilityUI>().UpdateUI(uiCounts);

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

}
