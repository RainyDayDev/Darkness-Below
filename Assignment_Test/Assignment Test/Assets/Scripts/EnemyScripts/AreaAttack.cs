using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaAttack : MonoBehaviour {

    public int damage = 10;
    public float time_of_attack = 2.0f;
    public bool skeleton_direction;


    void TheStart(bool facingRight)
    {
        if (facingRight)
        {
            transform.Rotate(0, 0, -1);
        }
        skeleton_direction = facingRight;
    }

    // Update is called once per frame
    void Update()
    {
      
        if (time_of_attack <= 0){
            Destroy(gameObject);
        }
        else
        {
            time_of_attack -= Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player enemy = other.GetComponent<Player>();
            if (skeleton_direction)
            {
                enemy.ApplyDamage(damage + 3 * enemy.level, 1);
            }
            else
            {
                enemy.ApplyDamage(damage + 3 * enemy.level, 2);
            }
            Destroy(gameObject.GetComponent<EdgeCollider2D>());
        }
    }
}
