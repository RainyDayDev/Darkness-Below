using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour {
	public Light light;
	float minFlicker = 0.1f;
	float maxFlicker = 1.0f;
	float changeTime;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (Random.value > 0.9) {
			if (light.enabled == true) {
				light.enabled = false;
			} else {
				light.enabled = true;
			}
		}
	}


}
