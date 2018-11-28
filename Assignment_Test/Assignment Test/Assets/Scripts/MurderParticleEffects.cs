using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MurderParticleEffects : MonoBehaviour {
    float timer;
	// Use this for initialization
	void Start () {
        timer = 1f;
	}
	
	// Update is called once per frame
	void Update () {
        if(timer<0){
            Destroy(gameObject);
        }else{
            timer -= Time.deltaTime;

        }
	}
}
