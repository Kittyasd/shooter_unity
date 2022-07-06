using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform groundCheck;
    public float speed = 10f;
    public float speedImpulse = 100f;
    public float distanceImpulse = 40f;
    public float cooldownImpulse = 1f;
    public float gravity = -9.8f;
    public float jumpHeight = 3f;
    public LayerMask groundMask;
    public float groundDistance = 0.5f;
    Vector3 velocity;
    bool checkImpulse = true;
    bool inputController = true;
    bool isGrounded;
    Vector3 moveImpulse;

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

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        if(Input.GetKeyDown(KeyCode.Mouse0) && checkImpulse)
            {
                StartCoroutine(Impulse());
            }

        if (inputController)
        {
            Vector3 move = transform.right * x + transform.forward * z;
            controller.Move(move * speed * Time.deltaTime);
        }
        else
        {
            controller.Move(moveImpulse * speedImpulse * Time.deltaTime);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    IEnumerator Impulse()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        checkImpulse = false;
        inputController = false;
        float durationImpulse = distanceImpulse/speedImpulse;
        moveImpulse = transform.right * x + transform.forward * z;
        yield return new WaitForSeconds(durationImpulse);
        inputController = true;
        yield return new WaitForSeconds(cooldownImpulse);
        checkImpulse = true;
    }
}
