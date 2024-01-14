using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballScript : MonoBehaviour
{
    [SerializeField]
    public float speed = 1;
    public float defaultSpeed;
    [SerializeField]
    public float damage = 1;
    [SerializeField]
    public int hitCount = 1;
    private int forceDirection = -1;
    public float stunDuration = 0.25f;

    private Vector2 standbyPosition;
    private Vector2 knockbackForce;

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
        speed = defaultSpeed;
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

    public void FireballReset()
    {
        isSpawned = false;
        speed = defaultSpeed;
        rb.position = standbyPosition;
        animator.SetBool("FireballTravel", true);
        animator.ResetTrigger("FireballHit");
    }
    public void FireballHit()
    {
        animator.SetBool("FireballTravel", false);
        animator.SetTrigger("FireballHit");
        speed = 0.1f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Wall")
        {
            // Reset Position.
            FireballReset();
        }
        if(collision.tag == "Boss")
        {
            // Fireball hits the boss.
            FireballHit();
        }
        if(collision.tag == "BossFireball")
        {
            // Fireballs cancel out one another.
            FireballHit();
        }
    }
}
