using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public enemyLight light;
    public enemyHeavy heavy;
    public enemyArcher archer;
    public GameObject player;
    public float spawnTime = 5.0f;
    public int detectionDistance;

    // Update is called once per frame
    void Update () {

        player = GameObject.FindWithTag("Player");
        if ((player.transform.position - transform.position).magnitude <= 20) spawnTime -= Time.deltaTime;
        else if(spawnTime <= 0)
        {
            int temp = Random.Range(0, 2);
            spawnTime = 5.0f; 
            spawn(temp);
        } 
	}

    void spawn(int type)
    {

    }
}
