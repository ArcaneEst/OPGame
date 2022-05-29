using System;
using System.Collections;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.TextCore;

public class Player : MonoBehaviour
{
    private Rigidbody2D player;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;

    private const float Speed = 10;
    private const float JumpHeight = 9;
    private const float AttackRecoil = 3;

    private const float CooldownBetweenFireballs = 0.3f;
    private const float CooldownBeforeAttack = 0.5f;
    private const float CooldownTakeDamage = 2f;
    private const int MaxNumberOfFireballs = 8;

    private float cooldownTimerForTakeDamage = Mathf.Infinity;
    private float lastY;
    private float cooldownTimerForAttack = Mathf.Infinity;
    private float cooldownTimerBeforeAttack = 0;

    private int hp = 3;
    private bool grounded;
    private enum Direction
    {
        Rigth, Left
    }

    private Direction lastDirection = Direction.Rigth;
    private int currentNumberOfFireballs;
    
    private void Awake()
    {
        player = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        currentNumberOfFireballs = MaxNumberOfFireballs;
    }

    private void Update()
    { 
        MovePlayer();

        if (Input.GetKey(KeyCode.Space))
            SpacePressed();
        
        if (!grounded)
            cooldownTimerBeforeAttack += Time.deltaTime;
        
        cooldownTimerForAttack += Time.deltaTime;
        cooldownTimerForTakeDamage += Time.deltaTime;
        
        animator.SetBool(AnimationBools.PlayerGrounded, grounded);
    }

    private void MovePlayer()
    {
        var horizontal = Input.GetAxis("Horizontal");

        player.velocity = new Vector2(horizontal * Speed, player.velocity.y);

        if (horizontal < -0.01f && lastDirection == Direction.Rigth)
        {
            spriteRenderer.flipX = true;
            lastDirection = Direction.Left;
        }
        else if (horizontal > 0.01f && lastDirection == Direction.Left)
        {
            spriteRenderer.flipX = false;
            lastDirection = Direction.Rigth;
        }

        animator.SetBool(AnimationBools.PlayerRun, horizontal != 0);
    }

    private void SpacePressed()
    {
        if (grounded)
        {
            Jump();
        }
        else if (cooldownTimerBeforeAttack >= CooldownBeforeAttack)
        {
            if (currentNumberOfFireballs > 0)
            {
                Attack();
            }
        }
    }

    private void Jump()
    {
        player.velocity = new Vector2(player.velocity.x, JumpHeight);
        grounded = false;
        
        animator.SetTrigger(AnimationTriggers.PlayerJump);
    }
    
    private void Attack()
    {
        if (cooldownTimerForAttack >= CooldownBetweenFireballs)
        {
            cooldownTimerForAttack = 0;
            currentNumberOfFireballs -= 1;
            SendFireball();
            player.velocity = new Vector2(player.velocity.x, AttackRecoil);

            Camera.Shake(0.2f, 0.1f);
            
            var audio = GetComponent(typeof(AudioSource)) as AudioSource;
            audio.Play();
        }
    }

    private void SendFireball()
    {
        var index = FindAvailableFireballIndex();
        fireballs[index].transform.position = firePoint.position;
        fireballs[index].GetComponent<Fireball>().Shoot();
    }
    
    private int FindAvailableFireballIndex()
    {
        for (var i = 0; i < fireballs.Length; i++)
            if (!fireballs[i].activeInHierarchy)
                return i;
        return 0;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag(Tags.Head))
        {
            Jump();
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag(Tags.Ground) || col.gameObject.CompareTag(Tags.Mushroom) || col.gameObject.CompareTag(Tags.Breakable))
        {
            cooldownTimerBeforeAttack = 0;
            grounded = true;
            currentNumberOfFireballs = MaxNumberOfFireballs;
        }
        if (col.gameObject.CompareTag(Tags.Mushroom) || col.gameObject.CompareTag(Tags.Goblin) || col.gameObject.CompareTag(Tags.Eye))
        {
            // TakeDamage();
            col.gameObject.GetComponent<IEnemy>()?.PlayAttackAnimation();
        }
    }

    public void TakeDamage()
    {
        if (cooldownTimerForTakeDamage < CooldownTakeDamage)
            return;
        cooldownTimerForTakeDamage = 0;
        
        Camera.Shake(1, 1f);
        
        hp -= 1;
        Debug.Log(hp);
        if (hp == 0)
        {
            Deactivate();
        }
    }

    private void Deactivate()
    {
        animator.SetTrigger(AnimationTriggers.PlayerDie);
        StartCoroutine(Deactivate(1.1f));
    }
    
    private IEnumerator Deactivate(float duration) {
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
    }
}
