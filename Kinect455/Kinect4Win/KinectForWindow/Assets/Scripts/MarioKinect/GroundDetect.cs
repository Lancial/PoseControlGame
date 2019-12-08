using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetect : MonoBehaviour
{
    public bool IsGrounded { get; private set; }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Enemy"))
        {
            IsGrounded = true;
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // something happen when jump on enemy
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Enemy"))
        {
            IsGrounded = false;
        }
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if(collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Enemy"))
    //    {
    //        IsGrounded = true; 
    //    }
    //    if(collision.gameObject.CompareTag("Enemy"))
    //    {
    //        // something happen when jump on enemy
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Enemy"))
    //    {
    //        IsGrounded = false;
    //    }
    //}
}
