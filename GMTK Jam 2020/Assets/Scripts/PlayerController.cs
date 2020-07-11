﻿using System;
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


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
        fixedShiftInput = shiftInput;

        // Get Context
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        // Set Base Velocity
        float hVel = fixedSideInput * runSpeed;
        float vVel = rb.velocity.y;
        Debug.Log(hVel);

        // ABILITIES
        // Sprint - apply a speed multiplier
        if (shiftInput)
        {
            hVel *= sprintMultiplier;
        }

        // Jump - add vertical velocity if on ground
        if (fixedUpInput && isGrounded)
        {
            vVel = jumpForce;
            fixedUpInput = false;
        }

        // Air Jump - add vertical velocity if in air
        if (fixedUpInput && !isGrounded)
        {
            vVel = jumpForce;
            fixedUpInput = false;
        }

        // Dash - teleport in movement direction
        if (fixedSpaceInput)
        {
            rb.MovePosition(new Vector2(rb.position.x + (fixedSideInput * dashDistance), rb.position.y));
            fixedSpaceInput = false;
        }

        // Update Velocity
        rb.velocity = new Vector2(hVel, vVel);
    }
}