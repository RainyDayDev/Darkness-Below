using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingSpawn : MonoBehaviour {
	public Ring ring;
	// Use this for initialization
	void Start () {
		Ring spawned = Instantiate(ring, transform.position, transform.rotation);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
