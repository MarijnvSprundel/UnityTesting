using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ExplosionChecker(transform.position, 5);
        
    }

    void ExplosionChecker(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        foreach(var hitCollider in hitColliders)
        {
            if (hitCollider.GetComponent<Rigidbody>())
            {
                hitCollider.GetComponent<Rigidbody>().velocity = (hitCollider.GetComponent<Rigidbody>().position - transform.position) * 10;
            }

        }
    }
}
