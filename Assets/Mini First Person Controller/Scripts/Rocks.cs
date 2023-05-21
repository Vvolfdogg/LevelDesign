using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocks : MonoBehaviour
{
    [SerializeField] GameObject wall;
    BossControler boss;
    [SerializeField] GameObject bossGO;
    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.layer);
        if (other.gameObject.layer == LayerMask.NameToLayer("Boss"))
        {
            Destroy(wall);
            boss = other.transform.GetComponent<BossControler>();
            boss.speed = 0;
            boss.health -= 100;
            Invoke("SpeedChange", 3f);
        }
    }

    private void SpeedChange()
    {
        boss.speed = 3;
        if(boss.health <= 0) 
        {
            Destroy(bossGO);
        }
        Destroy(gameObject);
    }
}
