using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {

    public float radius = 0.5f;

    public Transform player;
    bool canInteract;

    private void Start()
    {
        player = FindObjectOfType<Player>().transform;
    }

    public virtual void Interact() {
        Debug.Log("Interacting");
    }

    private void Update()
    {

        float distance = Vector3.Distance(player.position, transform.position);
        if (distance <= radius && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
            canInteract = true;
        }
        else if (distance > radius && canInteract)
        {
            canInteract = false;
        }
        

        
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }


}
