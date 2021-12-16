using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Bomb : MonoBehaviour
{
    public float force;
    public bool affectsPlayers;
    private bool explodedOthers = false;
    public string type;
    public bool isImpulse;
    private bool impulsed = false;
    private void Start()
    {
        if (force == 0)
        {
            force = 10F;
        }

        if (!isImpulse)
        {
            StartCoroutine(ExampleCoroutine()); 
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isImpulse && !impulsed)
        {
            impulsed = true;
            switch (type)
            {
                case "cluster":
                    ClusterExplode(6);
                    break;
                default:
                    Explode();
                    break;
            }
        }
    }

    IEnumerator ExampleCoroutine()
    {
        switch (type)
        {
            case "cluster":
                yield return new WaitForSeconds(3F);
                ClusterExplode(6);
                break;
            default:
                yield return new WaitForSeconds(2F);
                Explode();
                break;
        }
    }

    public void Explode()
    {
        if (!explodedOthers && type == "")
        {
            Vector3 position = transform.position;
            Collider[] hitColliders = Physics.OverlapSphere(position, (float)Math.Max(force - Math.Min(force - 5, 2), 3));
            foreach(var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Bomb") && hitCollider.gameObject != gameObject)
                {
                    explodedOthers = true;
                    hitCollider.GetComponent<Bomb>().Explode();
                }
            
                if (hitCollider.GetComponent<Rigidbody>())
                {
                    Vector3 hitPos = hitCollider.GetComponent<Rigidbody>().position;
                    float distance = Distance(hitPos, position);
                    hitCollider.GetComponent<Rigidbody>().velocity = (float)(Math.Pow((Math.Min(force, 30F) - distance) / Math.Min(force, 30F) * 10, 4) * 0.001) * (hitCollider.GetComponent<Rigidbody>().position - position) * Math.Min(force * 2F, 1F);
                }
                else if (hitCollider.GetComponent(typeof(PlayerController)) && affectsPlayers)
                {
                    PlayerController playerController = (PlayerController) hitCollider.GetComponent(typeof(PlayerController));
                    Vector3 hitPos = playerController.transform.position;
                    float distance = Distance(hitPos, position);
                    playerController.AddImpact((float)(Math.Pow((Math.Min(force, 30F) - distance) / Math.Min(force, 30F) * 10, 4) * 0.001) * (hitCollider.GetComponent<CharacterController>().transform.position - position) * Math.Min(force * 0.1F, 1F));
                    int damage = (int) ((Math.Pow((Math.Min(force, 30F) - distance) / Math.Min(force, 30F) * 10, 4) * 0.001)) * 3;
                    hitCollider.GetComponent<PlayerMisc>().health -= damage;
                }
            }
            
            ExplodeEffect(position);
        }
        
    }

    public void ClusterExplode(int amount)
    {
        
        Vector3 position = transform.position;
        GameObject bombPrefab = (GameObject)Resources.Load("Prefabs/Bomb", typeof(GameObject));
        
        for (int i = 0; i < amount; i++)
        {
            Vector3 randomAngle = RandomVelocityUpwards();
            GameObject bomb = Instantiate(bombPrefab, position + Vector3.up, Quaternion.identity);
            bomb.GetComponent<Rigidbody>().velocity += RandomVelocityUpwards() * 3;
        }
        ExplodeEffect(position);
        
    }

    float Distance(Vector3 pos1, Vector3 pos2)
    {
        Vector3 difference = new Vector3(
            pos1.x - pos2.x,
            pos1.y - pos2.y,
            pos1.z - pos2.z
        );
        float distance = (float)Math.Sqrt(
                Math.Pow(difference.x, 2f) +
                Math.Pow(difference.y, 2f) +
                Math.Pow(difference.z, 2f));
        return distance;
    }
    Vector3 RandomVelocityUpwards()
    {
        Vector3 pos = new Vector3((UnityEngine.Random.value * 2) - 1, (UnityEngine.Random.value * 2), (UnityEngine.Random.value * 2) - 1);
        return pos;
    }

    private void ExplodeEffect(Vector3 position)
    {
        GameObject go = Instantiate((GameObject)Resources.Load("Prefabs/Explosion", typeof(GameObject)), position, Quaternion.identity);
        ParticleSystem ps = go.GetComponent<ParticleSystem>();
        var main = ps.main; 
        main.startSpeed = Math.Min(force, 30F) / 5;

        var burst = ps.emission.GetBurst(0);
        burst.count = force * 10;
        ps.emission.SetBurst(0, burst);
        // ps.limitVelocityOverLifetime.dampen = 
        ps.Play();
        Destroy(gameObject);
        Destroy(go, 2f);
    }

}
