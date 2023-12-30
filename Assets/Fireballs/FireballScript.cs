using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballScript : MonoBehaviour
{
    [SerializeField]
    private float speed = 1;
    [SerializeField]
    private float damage = 1;
    [SerializeField]
    private int hitCount = 1;
    private int forceDirection = -1;

    private Vector2 standbyPosition;

    public Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private BoxCollider2D boxCollider;

    [SerializeField]
    private SpriteRenderer ownerSprite;

    public bool isSpawned = false;

    void Awake()
    {
        // Fireball is spawned.
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();

        standbyPosition = new Vector2(0, -2);
    }

    void Update()
    {
        if(isSpawned)
        {
            rb.MovePosition(rb.position + new Vector2(speed * forceDirection, 0) * Time.fixedDeltaTime);
        }
    }
    public void FireballSpawned()
    {
        isSpawned = true;
        spriteRenderer.flipX = ownerSprite.flipX;
        if (spriteRenderer.flipX)
        {
            forceDirection = 1;
        }
        else
        {
            forceDirection = -1;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Wall")
        {
            // Reset Position.
            isSpawned = false;
            rb.position = standbyPosition;
        }
    }
}
