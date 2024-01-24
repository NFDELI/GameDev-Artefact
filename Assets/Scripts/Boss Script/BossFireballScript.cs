using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BossFireballScript : MonoBehaviour
{
    [SerializeField]
    public float speed = 1;
    public float defaultSpeed;
    [SerializeField]
    public float damage = 10f;
    [SerializeField]
    public int hitCount = 1;
    private int forceDirection = -1;
    public float stunDuration = 0.25f;

    [SerializeField]
    public Vector2 knockbackForce;

    private Vector2 standbyPosition;

    public Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private BoxCollider2D boxCollider;

    [SerializeField]
    private PlayerStateManager playerStateManager;

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

        standbyPosition = new Vector2(0, -3);
        speed = defaultSpeed;
    }

    void Update()
    {
        if (isSpawned)
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
            boxCollider.offset = new Vector2(-0.2f, -1.28f);
        }
        else
        {
            forceDirection = -1;
            boxCollider.offset = new Vector2(0.2f, -1.28f);
        }
    }

    public void FireballHit()
    {
        //playerStateManager.AttackHitProperty(10f, );
        animator.SetBool("FireballTravel", false);
        animator.SetTrigger("FireballHit");
        boxCollider.enabled = false;
        speed = 0.1f;
    }

    public void FireballReset()
    {
        isSpawned = false;
        speed = defaultSpeed;
        rb.position = standbyPosition;
        animator.SetBool("FireballTravel", true);
        animator.ResetTrigger("FireballHit");
        boxCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            // Reset Position.
            FireballReset();
        }
        if (collision.tag == "Player")
        {
            // Fireball hits the Player.
            FireballHit();
        }
        if (collision.tag == "PlayerFireball")
        {
            // Fireball cancels out with player fireball.
            FireballHit();
        }
    }
}