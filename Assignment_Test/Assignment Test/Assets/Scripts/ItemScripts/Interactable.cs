using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Interactable : MonoBehaviour {

    public float radius = 0.5f;

    public Transform player;
    public bool canInteract;
    public Player knight;

    private void Start()
    {
        //player = FindObjectOfType<Player>().transform;
        //knight = FindObjectOfType<Player>();
    }

    public virtual void Interact() {
        Debug.Log("Interacting");
    }

    private void Update()
    {
        if (player == null)
        {
            player = FindObjectOfType<Player>().transform;
        }
        if (knight == null)
        {
            knight = FindObjectOfType<Player>();
        }
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= radius && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Interacting");
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
