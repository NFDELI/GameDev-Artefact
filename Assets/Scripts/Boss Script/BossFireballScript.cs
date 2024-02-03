using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BossFireballScript : MonoBehaviour
{
    [SerializeField]
    public float speed = 1;
    public float defaultSpeed = 1.7f;
    public float superSpeed = 2f;
    [SerializeField]
    public float defaultDamage = 10f;
    public float superDamage = 5f;
    public float damage = 10f;
    [SerializeField]
    public int defaultHitCount = 1;
    public int hitCount = 1;
    public int superHitCount = 1;
    private int forceDirection = -1;
    public float stunDuration = 0.25f;
    public float superStunDuration = 999f;

    public bool isSuper = false;
    public bool isFire = false;

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

        if (isSuper)
        {
            // Change into Super Fireball animation.
            animator.SetBool("FireballTravel", false);
            animator.SetBool("SuperFireballTravel", true);
            damage = superDamage;
            speed = superSpeed;
            hitCount = superHitCount;

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
        else if (isFire)
        {
            // Change into Burning Fireball animation.
        }
        else
        {
            damage = defaultDamage;
            speed = defaultSpeed;
            hitCount = defaultHitCount;

            // Spawn Regular Fireball.
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

    }

    public void FireballHit()
    {
        hitCount--;
        if(isSuper && hitCount <= 0)
        {
            animator.SetTrigger("SuperFireballHit");
        }

        animator.SetBool("FireballTravel", false);
        animator.SetTrigger("FireballHit");
        boxCollider.enabled = false;

        if(hitCount > 0)
        {
            ReTriggerCollisionBoxWithDelay(0.15f);
        }
    }

    public void ReTriggerCollisionBoxWithDelay(float delay)
    {
        Invoke("ReTriggerCollisionBox", delay);
    }

    public void ReTriggerCollisionBox()
    {
        boxCollider.enabled = true;
    }

    public void FireballReset()
    {
        isSpawned = false;
        speed = defaultSpeed;
        rb.position = standbyPosition;
        animator.SetBool("FireballTravel", true);
        animator.ResetTrigger("FireballHit");
        boxCollider.enabled = true;

        // Reset Flags.
        isSuper = false;
        isFire = false; 
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
            speed = 0.1f;
            FireballHit();
        }
        if (collision.tag == "PlayerFireball")
        {
            // Fireball cancels out with player fireball.
            FireballHit();
        }
    }
}