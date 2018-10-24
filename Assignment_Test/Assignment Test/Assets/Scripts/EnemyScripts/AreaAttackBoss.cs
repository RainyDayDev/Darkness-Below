using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaAttackBoss : MonoBehaviour {

    public float speed = 360;
    public GameObject owner;
    public float timer = 1.0f;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * speed);
        timer -= Time.deltaTime;
        if (timer <= 0) Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player enemy = other.GetComponent<Player>();
            enemy.ApplyDamage(40 * enemy.level/5);
            Destroy(gameObject);
        }
    }
}