using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMovement : MonoBehaviour
{
    public float timeMax;
    public bool isRightDirection;

    public float speed = 5f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private float currentTimeMax;
    private float timeElapsed;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        GenerateNewRandomRange();
    }

    // Update is called once per frame
    void Update()
    {
        if(timeElapsed < currentTimeMax)
        {
            timeElapsed += Time.deltaTime;
        } else
        {
            GenerateNewRandomRange();
            timeElapsed = 0;
        }

        rb.velocity = new Vector2(speed, rb.velocity.y);
    }

    private void GenerateNewRandomRange()
    {
        currentTimeMax = Random.Range(0, timeMax);
        isRightDirection = !isRightDirection;
        speed *= isRightDirection ? 1 : -1;
        spriteRenderer.flipX = !isRightDirection;
    }
    
}
