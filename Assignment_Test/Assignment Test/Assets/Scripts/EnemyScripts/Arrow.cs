using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

    public enemyArcher owner;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
			player.ApplyDamage(owner.GetComponent<EnemyStats>().damage.GetValue(), 0);
            Destroy(gameObject);
        }
        if(other.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == "Wall") {
			Destroy (gameObject);
		}
	}
}
