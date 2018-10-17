using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class death_letters : MonoBehaviour {
	float count = 4;
	// Use this for initialization
	void Start () {
		
	}
	void Awake(){
		
	}
	// Update is called once per frame
	void Update () {
		



		if(count<0){
			Destroy (gameObject);
		}
		count -= Time.deltaTime;
		
	}
}
