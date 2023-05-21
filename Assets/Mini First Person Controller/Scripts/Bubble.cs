using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    SphereCollider collider;
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RadiusKill()
    {
        Collider[] hitColliders = Physics.OverlapSphere(collider.center, collider.radius);
        foreach (var hitCollider in hitColliders)
        {
            //Debug.Log(collider.center);
            //Debug.Log(collider.radius);
            //Debug.Log(hitCollider.gameObject.name);
            if(hitCollider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                hitCollider.gameObject.GetComponent<EnemyController>().Die();
            }
        }
        Destroy(gameObject);
    }
}
