using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooseScript : MonoBehaviour
{
    public GameObject husky;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private Animator anim;

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
         anim = GetComponent<Animator>();
         anim.enabled = false;
    }

    private float attackTimer = 0;
    private bool attack = false;
    // Update is called once per frame
    void FixedUpdate()
    {
        checkLife();
        if (attack && attackTimer <= 0) {
            if (husky.transform.position.x > transform.position.x) {
                moveDirection = 1;
                spriteRenderer.flipX = true;
            } else {
                moveDirection = -1;
                spriteRenderer.flipX = false;
            }
            attackTimer = 2;
        } else {
            attackTimer -= Time.deltaTime;
        }
        float distance = (husky.transform.position.x - transform.position.x);
        if (distance < 0) {
            distance = -distance;
        }
        float speed = regular;
        if (distance >=5 && distance < 20) {
            attack = true;
        } else if (distance < 5) {
            spriteRenderer.color = Color.red;
            speed = faster;
        } 
        if (distance >= 5) 
        {
            speed = regular;
        }
        transform.Translate(new Vector2(speed * moveDirection * Time.deltaTime,0));
    }

    private void checkLife() {
        if (alive == false) {
            Debug.Log("death");
            Destroy(gameObject);
        }
    }
}
