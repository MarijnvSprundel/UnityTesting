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
    private void Start()
    {
        StartCoroutine(ExampleCoroutine()); 
    }

    IEnumerator ExampleCoroutine()
    {
        switch (type)
        {
            case "cluster":
                yield return new WaitForSeconds(3F);
                ClusterExplode(10);
                break;
            default:
                yield return new WaitForSeconds(2F);
                Explode(10);
                break;
        }
    }

    public void Explode(float radius)
    {
        if (!explodedOthers && type == "")
        {
            Vector3 position = transform.position;
            Collider[] hitColliders = Physics.OverlapSphere(position, radius);
            foreach(var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Bomb") && hitCollider.gameObject != gameObject)
                {
                    explodedOthers = true;
                    hitCollider.GetComponent<Bomb>().Explode(10);
                }
            
                if (hitCollider.GetComponent<Rigidbody>())
                {
                    Vector3 hitPos = hitCollider.GetComponent<Rigidbody>().position;
                    float distance = Distance(hitPos, position);
                    hitCollider.GetComponent<Rigidbody>().velocity = ((radius - distance) * (hitCollider.GetComponent<Rigidbody>().position - position)) * (force * 0.1F);
                }
                else if (hitCollider.GetComponent(typeof(PlayerController)) && affectsPlayers)
                {
                    PlayerController playerController = (PlayerController) hitCollider.GetComponent(typeof(PlayerController));
                    Vector3 hitPos = playerController.transform.position;
                    float distance = Distance(hitPos, position);
                    playerController.AddImpact(((radius - distance) * (hitCollider.GetComponent(typeof(PlayerController)).transform.position - position)) * (force * 0.1F));
                }
            }
            
            ExplodeEffect(position);
        }
        
    }

    public void ClusterExplode(int amount)
    {
        Vector3 position = transform.position;
        GameObject bombPrefab = (GameObject)Resources.Load("Prefabs/Bomb", typeof(GameObject));
        
        for (int i = 0; i < amount; i++){
            GameObject bomb = Instantiate(bombPrefab, position + Vector3.up, Quaternion.identity);
            bomb.GetComponent<Rigidbody>().velocity = RandomAngle() * 10;
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
    Vector3 RandomAngle()
    {
        Vector3 pos = new Vector3((UnityEngine.Random.value * 2) - 1, (UnityEngine.Random.value * 2) - 1, (UnityEngine.Random.value * 2) - 1);
        return pos;
    }

    private void ExplodeEffect(Vector3 position)
    {
        GameObject go = Instantiate((GameObject)Resources.Load("Prefabs/Explosion", typeof(GameObject)), position, Quaternion.identity);
        go.GetComponent<ParticleSystem>().Play();
        Destroy(gameObject);
        Destroy(go, 2f);
    }

}
