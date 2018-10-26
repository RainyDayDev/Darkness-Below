using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingPickup : MonoBehaviour {
	public int value = 0;
	Vector3 startPos;
	float time;
	// Use this for initialization
	void Start () {
		startPos = transform.position;
	}

	// Update is called once per frame
	void Update () {
		/*
        time += Time.deltaTime;
		Vector3 y = new Vector3 (0.0f, 1.0f, 0.0f);
		Vector3 x = new Vector3 (1.0f, 0.0f, 0.0f);
		if (time <= 0.1f) {
			transform.Translate (y * Time.deltaTime * 4);
			transform.Translate (-x * Time.deltaTime * 4);
		}
		if (time > 0.1f && transform.position.y > startPos.y) {
			transform.Translate (-y * Time.deltaTime * 4);
		}
		*/
	}
}
