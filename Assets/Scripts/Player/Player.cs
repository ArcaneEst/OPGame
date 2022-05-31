using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;


public class Player : MonoBehaviour
{
    public static event Action OnPlayerDeath;
    
    private Rigidbody2D player;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;
    
    [SerializeField] private RandomClip fireballClip;
    private AudioSource fireballSound;

    private const float Speed = 10;
    private const float JumpHeight = 9;
    private const float AttackRecoil = 3;

    private const float CooldownBetweenFireballs = 0.3f;
    private const float CooldownBeforeAttack = 0.5f;
    private const float CooldownTakeDamage = 2f;
    [SerializeField] private int MaxNumberOfFireballs = 8;

    private float cooldownTimerForTakeDamage = Mathf.Infinity;
    private float lastY;
    private float cooldownTimerForAttack = Mathf.Infinity;
    private float cooldownTimerBeforeAttack = 0;

    [SerializeField] private int hp = 1;
    public int CurrentHp => hp;
    private bool grounded;
    private enum Direction
    {
        Rigth, Left
    }

    private Direction lastDirection = Direction.Rigth;
    private int currentNumberOfFireballs;
    public int CurrentNumberOfFirebolls => currentNumberOfFireballs;
    
    private void Awake()
    {
        player = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        fireballSound = GetComponent<AudioSource>();
        
        currentNumberOfFireballs = MaxNumberOfFireballs;
    }

    private void Update()
    { 
        MovePlayer();

        UpdateTimers();
        
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

    private void UpdateTimers()
    {
        if (!grounded)
            cooldownTimerBeforeAttack += Time.deltaTime;
        
        cooldownTimerForAttack += Time.deltaTime;
        cooldownTimerForTakeDamage += Time.deltaTime;
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

    private void ShakeCamera(float duration, float power)
    {
        Camera.Shake(duration, power);
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

            ShakeCamera(0.2f, 0.1f);

            fireballSound.pitch = Random.Range(fireballClip.Pitch.min, fireballClip.Pitch.max);
            fireballSound.volume = Random.Range(fireballClip.Volume.min, fireballClip.Volume.max);
            fireballSound.PlayOneShot(fireballClip.Clip);
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
            col.gameObject.GetComponent<Enemy>()?.PlayAttackAnimation(TakeDamage);
        }
    }

    private void TakeDamage()
    {
        if (cooldownTimerForTakeDamage < CooldownTakeDamage)
            return;
        cooldownTimerForTakeDamage = 0;
        
        ShakeCamera(1, 1);
        
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
    
    private IEnumerator Deactivate(float delay) {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
        OnPlayerDeath?.Invoke();
    }
}
