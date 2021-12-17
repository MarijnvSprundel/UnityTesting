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
    public Vector3 moveDirection = Vector3.zero;
    private Camera cam;
    private GameObject mapCamPos;
    private CharacterController controller;
    private Vector3 impactForce = Vector3.zero;
    private GameObject bombPrefab;
    private UIController ui;
    private float respawnTime = 10F;
    private float originalRespawnTime;
    private bool isDead = false;
    private GameObject capsule;
    public int health = 100;
    private int oldHealth;

    private void Start()
    {
        oldHealth = health;
        cam = Camera.main;
        mapCamPos = GameObject.Find("MapCamPos");
        capsule = transform.Find("Capsule").gameObject;
        bombPrefab = (GameObject) Resources.Load("Prefabs/Bomb", typeof(GameObject));
        controller = GetComponent<CharacterController>();
        ui = GameObject.Find("UI").GetComponent<UIController>();
        originalRespawnTime = respawnTime;
    }
    void Update()
    {
        ui.UpdateHealth(health);
        if (health <= 0)
        {
            StartCoroutine(GetComponent<PlayerController>().PlayerDeath());
        }
        
        if (controller.isGrounded && !isDead)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = cam.transform.TransformDirection(moveDirection);
            moveDirection *= speed;

            if (Input.GetButton("Jump"))
                moveDirection.y = jumpSpeed;
        }

        moveDirection.y -= gravity * Time.deltaTime;
        if (impactForce.magnitude > 0.2) moveDirection += impactForce;
        if (controller.enabled) controller.Move(moveDirection * Time.deltaTime);

            
            

        impactForce /= 200F;
        
        if (Input.GetKeyDown("e"))
        {
            GameObject bomb = Instantiate(bombPrefab, cam.transform.position + cam.transform.forward, Quaternion.identity);
            bomb.GetComponent<Rigidbody>().velocity = (cam.transform.forward * 20);
        }
        
        if (transform.position.y < -20 && !isDead) StartCoroutine(PlayerDeath(true));

        if (isDead)
        {
            originalRespawnTime -= Time.deltaTime;
            ui.UpdateCountdownTimer((int)originalRespawnTime);
            if (originalRespawnTime < 0) Respawn();
        }
        
    }
    



    public void AddImpact(Vector3 force)
    {
        impactForce += force;
    }

    public IEnumerator PlayerDeath(bool instant = false)
    {
        health = oldHealth;
        isDead = true;
        ui.Death();
        capsule.GetComponent<MeshRenderer>().enabled = false;

        if (!instant) yield return new WaitForSeconds(5);
        print($"Died! {instant}");
        impactForce = Vector3.zero;
        moveDirection = Vector3.zero;

        controller.enabled = false;
        transform.position = mapCamPos.transform.position;
        cam.transform.rotation = mapCamPos.transform.rotation;
    }

    private void Respawn()
    {
        
        isDead = false;
        originalRespawnTime = respawnTime;
        ui.Respawn();
        transform.position = new Vector3(0, 2, 0);
        capsule.GetComponent<MeshRenderer>().enabled = false;
        controller.enabled = true;
    }
    
    
}
