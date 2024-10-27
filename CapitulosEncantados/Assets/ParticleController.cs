using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
   [SerializeField] private ParticleSystem movementParticle;

   [Range(0, 10)] 
   [SerializeField] private int occurAfterVelocity;

   [Range(0, 0.2f)] [SerializeField] private float dustFormationPeriod;

   [SerializeField] private Rigidbody2D playerRb;
   
   float counter;

   bool isOnGround;
   
   [SerializeField] private ParticleSystem fallParticle;
   
   

   private void Update()
   {
      counter += Time.deltaTime;

      if (isOnGround && Mathf.Abs(playerRb.velocity.x) > occurAfterVelocity)
      {
         if (counter > dustFormationPeriod)
         {
            movementParticle.Play();
            counter = 0;
         }
      }
   }

   private void OnTriggerEnter2D(Collider2D collision)
   {
      if (collision.CompareTag("chao"))
      {
         fallParticle.Play();
         isOnGround = true;
      }
   }
   
   private void OnTriggerExit2D(Collider2D collision)
   {
      if (collision.CompareTag("chao"))
      {
         isOnGround = false;
      }
   }
}
