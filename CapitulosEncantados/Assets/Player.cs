using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Speed; 
    public float Jumpforce;

    private Rigidbody2D rig;



    void Start()
    {
        rig = GetComponent<Rigidbody2D>();

    }

    void Update()
    {

        Move();
        Jump();

    }





    void Move()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
        transform.position += movement * Time.deltaTime * Speed;

    }

    void Jump()
    {
        if(Input.GetButtonDown("Jump"))
    {

        rig.AddForce(new Vector2(0f, Jumpforce), ForceMode2D.Impulse);
    }

    }

    
}


