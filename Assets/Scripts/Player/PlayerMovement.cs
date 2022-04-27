using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D player;
    private Animator animator;
    
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;

    private const float Speed = 10;
    private const float JumpHeight = 9;
    private const float AttackRecoil = 3;
    
    private const float CooldownBetweenFireballs = 0.3f;
    private const float CooldownBeforeAttack = 0.5f;
    private const float CooldownTakeDamage = 2f;
    private const int MaxNumberOfFireballs = 4;

    private float cooldownTimerForTakeDamage = Mathf.Infinity;
    private float cooldownTimerForAttack = Mathf.Infinity;
    private float cooldownTimerBeforeAttack = 0;
    
    private int hp = 3;
    private bool grounded;
    private int currentNumberOfFireballs;

    private void Awake()
    {
        player = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

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
        
        animator.SetBool("grounded", grounded);
    }

    private void MovePlayer()
    {
        var horizontal = Input.GetAxis("Horizontal");
        
        player.velocity = new Vector2(horizontal * Speed, player.velocity.y);
        
        // Flip player when moving to different directions horizontally
        if (horizontal > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontal < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);
        
        animator.SetBool("run", horizontal != 0);
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
        
        animator.SetTrigger("jump");
    }
    
    private void Attack()
    {
        if (cooldownTimerForAttack >= CooldownBetweenFireballs)
        {
            cooldownTimerForAttack = 0;
            currentNumberOfFireballs -= 1;
            SendFireball();
            player.velocity = new Vector2(player.velocity.x, AttackRecoil);
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
        if (col.gameObject.CompareTag("Head"))
        {
            Jump();
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("Mine") || col.gameObject.CompareTag("Breakable"))
        {
            cooldownTimerBeforeAttack = 0;
            grounded = true;
            currentNumberOfFireballs = MaxNumberOfFireballs;
        }
        if (col.gameObject.CompareTag("Mine") || col.gameObject.CompareTag("Turtle") || col.gameObject.CompareTag("Ball"))
        {
            TakeDamage();
        }
    }

    private void TakeDamage()
    {
        if (cooldownTimerForTakeDamage < CooldownTakeDamage)
            return;
        cooldownTimerForTakeDamage = 0;
        
        hp -= 1;
        Debug.Log(hp);
        if (hp == 0)
        {
            Deactivate();
        }
    }
    
    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
    