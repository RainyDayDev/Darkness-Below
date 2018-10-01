using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour {

    public Rigidbody2D bulletPrefab;

    public float bulletSpeed = 3;

    //Fires the bow
    public void fireAt(Vector3 place)
    {
        transform.up = place;

        var newBullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        newBullet.velocity = transform.up * bulletSpeed;
		newBullet.transform.Rotate (new Vector3 (180, 0, 0));
        newBullet.GetComponent<Arrow>().owner = GetComponentInParent<enemyArcher>();
    }
}
