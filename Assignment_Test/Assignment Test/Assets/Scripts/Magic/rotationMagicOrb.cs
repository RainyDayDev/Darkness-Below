using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotationMagicOrb : MonoBehaviour {
    public float rotation_speed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0f,0f,Time.deltaTime*rotation_speed);

    }
}
