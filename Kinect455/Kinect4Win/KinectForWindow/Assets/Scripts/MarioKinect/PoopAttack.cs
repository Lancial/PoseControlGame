using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoopAttack : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        pooplife = 1f;
    }
    private bool IsGrounded;
    public float pooplife = 3f;
    // Update is called once per frame
    void Update()
    {
        if (IsGrounded) {
            pooplife -= Time.deltaTime;
        }
        if (pooplife <= 0) {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            IsGrounded = true;
        }
    }
}
