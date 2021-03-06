using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{

    public CharacterController controller;
    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;

    private bool isCrouching;
    private Vector3 originalCenter;
    private float originalHeight;
    private float originalMoveSpeed;

    void Start () {
        transform.tag = "Player";
        controller = GetComponent<CharacterController>();   
        originalCenter = controller.center;
        originalHeight = controller.height;
        originalMoveSpeed = speed;  
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y += Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if(Input.GetButtonDown("Crouch")) {
            controller.height = 1f;
            controller.center = new Vector3(0f, -0.5f, 0f);
            speed = 3f;
            isCrouching = true;
        }

        if(!Input.GetButton("Crouch") && isCrouching) {
            controller.height = originalHeight;
            controller.center = originalCenter;
            speed = originalMoveSpeed;
            isCrouching = false;
        }

        if(!Input.GetButton("Crouch") && isCrouching) {
            Vector3 point0 = transform.position + originalCenter - new Vector3(0.0f, originalHeight, 0.0f);           
            Vector3 point1 = transform.position + originalCenter + new Vector3(0.0f, originalHeight, 0.0f);
            if (Physics.OverlapCapsule(point0, point1, controller.radius).Length == 0) {
                controller.height = originalHeight;
                controller.center = originalCenter;
                speed = originalMoveSpeed;
                isCrouching = false;
            }
        }   
        
    }
}