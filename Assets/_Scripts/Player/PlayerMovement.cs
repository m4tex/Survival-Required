using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody controller;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    //sprint
    public float sprintSpeedMultiplier;
    private bool isRunning;
    //Stamina
    public float maxStamina, stamina;
    public float staminaDelay, staminaDelayCounter;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;

    //Stops movement when set to false
    static bool isPaused;

    //player GameObject singleton
    public static PlayerMovement instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            print("Multiple player instances occured. Deleting the old one...");
            Destroy(instance.gameObject);
            instance = this;
        }
    }
    private void Update()
    {
        if (!isPaused)
        {
            WalkingAndJumping();
            Crouching();
        }
    }

    public static void DisableControls(bool toState) => isPaused = toState;

    void WalkingAndJumping()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.AddForce(move * speed);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        controller.AddForce(velocity);

        //Sprint
        if (Input.GetKeyDown(KeyCode.LeftShift) && x + z != 0 && staminaDelayCounter <= 0 && !isRunning)
        {
            speed *= sprintSpeedMultiplier;
            isRunning = true;
        }
        if (isRunning && x + z == 0)
        {
            speed /= sprintSpeedMultiplier;
            isRunning = false;
        }
        if (isRunning && stamina <= 0)
        {
            speed /= sprintSpeedMultiplier;
            isRunning = false;
            staminaDelayCounter = staminaDelay;
        }

        //Stamina
        if (isRunning)
            stamina -= Time.deltaTime;
        if (stamina < maxStamina && !isRunning)
            stamina += Time.deltaTime / 2;
        if (staminaDelayCounter > 0)
            staminaDelayCounter -= Time.deltaTime;
    }

    void Crouching()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
            Crouch();
        if (Input.GetKeyUp(KeyCode.LeftControl))
            StandUp();
    }

    private void Crouch()
    {
        print("crouching");
    }

    private void StandUp()
    {
        print("standing up");
    }
}