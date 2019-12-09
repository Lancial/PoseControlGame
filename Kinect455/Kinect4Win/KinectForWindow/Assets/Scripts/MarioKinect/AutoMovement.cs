using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMovement : MonoBehaviour
{
    public GameObject poo;
    public GameObject husky;
    public float poopSpeed;
    public float speed = 5f;
    public float range = 10f;

    private bool isRightDirection;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private float timeElapsed;
    private Vector3 startPos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPos = GetComponent<Transform>().position;
    }

    private float poopTimer = 0;
    // Update is called once per frame
    void Update()
    {
        Vector3 pos = GetComponent<Transform>().position;
        if (GetComponent<Transform>().position.x < startPos.x - range) {
            GetComponent<Transform>().position = new Vector3(startPos.x - range, pos.y, pos.z);
            flipBird();
        } 
        if (GetComponent<Transform>().position.x > startPos.x + range) {
            GetComponent<Transform>().position = new Vector3(startPos.x + range, pos.y, pos.z);
            flipBird();
        }

        if (poopTimer <= 0) {
            poop();
            poopTimer = 2;
        }
        poopTimer -= Time.deltaTime;
        rb.velocity = new Vector2(speed, rb.velocity.y);
    }

    private void flipBird() {
        speed *= -1;
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
    }

    private void poop()
    {
        Debug.Log("pooping");
        Vector3 bird = GetComponent<Transform>().position;
        Vector3 drop = new Vector3(bird.x, bird.y - 1, bird.z);
        GameObject clone = (GameObject)Instantiate (poo, drop, Quaternion.identity);
        clone.GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, -1 * poopSpeed);
    }
    
}
