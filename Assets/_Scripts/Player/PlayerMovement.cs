using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;

    public float speed = 12f;
    public float jumpForce = 3f;

    //sprint
    public float sprintMultiplier;
    private bool isRunning;
    //Stamina
    public float maxStamina, stamina;
    public float staminaDelay, staminaDelayCounter;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

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

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        rb.AddForce(move * speed * Time.deltaTime, ForceMode.VelocityChange);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(new Vector3(0, jumpForce));
        }

        //Sprint
        if (Input.GetKeyDown(KeyCode.LeftShift) && x + z != 0 && staminaDelayCounter <= 0 && !isRunning)
        {
            speed *= sprintMultiplier;
            isRunning = true;
        }
        if (isRunning && x + z == 0)
        {
            speed /= sprintMultiplier;
            isRunning = false;
        }
        if (isRunning && stamina <= 0)
        {
            speed /= sprintMultiplier;
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