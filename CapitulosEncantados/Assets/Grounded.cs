using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grounded : MonoBehaviour
{
    private Character player; 

    void Start()
    {
       
        player = gameObject.transform.parent.gameObject.GetComponent<Character>();
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
       
        if (collider.gameObject.layer == 8)
        {
            player.isJumping = false;
        }
    }

    void OnCollisionExit2D(Collision2D collider)
    {
        
        if (collider.gameObject.layer == 8)
        {
            player.isJumping = true;
        }
    }
}
