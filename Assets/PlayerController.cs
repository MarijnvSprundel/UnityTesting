using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    // public float mass = 3.0F;
    private Vector3 moveDirection = Vector3.zero;
    private Camera cam;
    private CharacterController controller;
    private Vector3 impactForce = Vector3.zero;
    


    private void Start()
    {
        cam = Camera.main;
    }
    void Update()
    {
        controller = GetComponent<CharacterController>();
        if (controller.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = cam.transform.TransformDirection(moveDirection);
            moveDirection *= speed;

            if (Input.GetButton("Jump"))
                moveDirection.y = jumpSpeed;
        }

        moveDirection.y -= gravity * Time.deltaTime;
        
        if (impactForce.magnitude > 0.2) moveDirection += impactForce;
        controller.Move(moveDirection * Time.deltaTime);
        // impactForce = Vector3.Lerp(impactForce, Vector3.zero, 5000*Time.deltaTime);
        impactForce /= 200F;
    }
    
    public void AddImpact(Vector3 force)
    {
        impactForce += force;
    }
}
