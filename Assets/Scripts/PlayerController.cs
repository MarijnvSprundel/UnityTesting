using System;
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
    private GameObject bombPrefab;


    private void Start()
    {
        cam = Camera.main;
        bombPrefab = (GameObject) Resources.Load("Prefabs/Bomb", typeof(GameObject));
        controller = GetComponent<CharacterController>();
    }
    void Update()
    {
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
        
        
        // if (Input.GetMouseButtonDown(0))
        // {
        //     RaycastHit hit;
        //     Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        //     if (Physics.Raycast(ray, out hit))
        //     {
        //         if(hit.transform.gameObject.GetComponent(typeof(Bomb)))
        //         {
        //             Bomb bomb = (Bomb) hit.transform.gameObject.GetComponent(typeof(Bomb));
        //             bomb.Explode(10);
        //         }
        //     }
        // }
        if (Input.GetKeyDown("e"))
        {
            GameObject bomb = Instantiate(bombPrefab, cam.transform.position + cam.transform.forward, Quaternion.identity);
            bomb.GetComponent<Rigidbody>().velocity = (cam.transform.forward * 20);
            // bomb.GetComponent<Bomb>().type = "cluster";
            // bomb.GetComponent<Bomb>().isImpulse = true;
        }

        if (Input.GetKey("x"))
        {
            transform.position = new Vector3(0, 2, 0);
            moveDirection = Vector3.zero;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bomb"))
        {
            Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
        }
    }

    public void AddImpact(Vector3 force)
    {
        impactForce += force;
    }
    
    
}
