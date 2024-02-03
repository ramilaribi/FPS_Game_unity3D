using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovment : MonoBehaviour
{
    private CharacterController controller;

    public float speed = 12f; 
    public float gravity = -9.81f * 2;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    Vector3 velocity;

    bool isGrounded;
    bool isMoving;

    private Vector3 lastPosition = new Vector3(0f,0f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        // Resseting the Default velocity 
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //Getting the inputs 
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //creating the moving vector 
        Vector3 move = transform.right * x + transform.forward * z; //right red axis forward blue axis 

        //Actually moving the player
        controller.Move(move * speed * Time.deltaTime);

        //check if the player can jump 
        if (Input.GetButtonDown("Jump") && isGrounded) 
        {
            //Going up
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        //falling down 
        velocity.y += gravity * Time.deltaTime;

        //Executing the jump 
        controller.Move(velocity *  Time.deltaTime);

        if (lastPosition != gameObject.transform.position  && isGrounded == true) 
        {
            isMoving = true;
            //for later use
        }
        else
        {
            isMoving= false;
            //for later use
        }
        lastPosition = gameObject.transform.position;
    }
}
