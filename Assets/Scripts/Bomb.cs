using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float force;
    public bool affectsPlayers;
    
    public void Explode(float radius)
    {
        Vector3 position = transform.position;
        Collider[] hitColliders = Physics.OverlapSphere(position, radius);
        foreach(var hitCollider in hitColliders)
        {
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
        Destroy(gameObject);
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


}
