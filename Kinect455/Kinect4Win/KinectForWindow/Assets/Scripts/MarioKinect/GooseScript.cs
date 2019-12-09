using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooseScript : MonoBehaviour
{
    public GameObject husky;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private DetectWall wall;

    public float regular = 5f;
    public float faster = 7f;
    private float moveDirection = -1f;

    public bool alive = true;
    private bool attacking;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        wall = GetComponentInChildren<DetectWall>();
    }

    private float attackTimer = 0;
    private bool attack = false;
    // Update is called once per frame
    void FixedUpdate()
    {
        checkLife();
        float distance = (husky.transform.position.x - transform.position.x);
        if (distance < 0) {
            distance = -distance;
        }
        if (wall.flip) {
            flipBird();
            //wall.flip = false;
        }

        float speed = regular;
        transform.Translate(new Vector2(speed * moveDirection * Time.deltaTime,0));
    }

    private void checkLife() {
        if (alive == false) {
            Debug.Log("death");
            Destroy(gameObject);
        }
    }

    private void flipBird() {
        Debug.Log("flipping");
        moveDirection *= -1;
        spriteRenderer.flipX = !spriteRenderer.flipX ;
    }
}
