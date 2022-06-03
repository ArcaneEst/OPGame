using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;


public class Player : MonoBehaviour
{
    public static event Action OnPlayerDeath;
    
    private Rigidbody2D player;
    private Animator animator;
    private CircleCollider2D circleCollider2D;
    
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
    private const int   MaxNumberOfFireballs = 8;

    private float cooldownTimerForTakeDamage = Mathf.Infinity;
    private float cooldownTimerForAttack = Mathf.Infinity;
    private float cooldownTimerBeforeAttack = 0;

    private bool grounded = false;

    private int hp = 1;

    public int CurrentNumberOfFireballs { get; private set; }
    
    private void Awake()
    {
        player = GetComponent<Rigidbody2D>();
        circleCollider2D = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
        fireballSound = GetComponent<AudioSource>();

        CurrentNumberOfFireballs = MaxNumberOfFireballs;
    }

    private void Update()
    {
        MovePlayer();
        
        if (Input.GetKey(KeyCode.Space))
            SpacePressed();
        
        UpdateTimers();
        
        var hit = Physics2D.BoxCast(new Vector2(player.position.x, player.position.y - 0.2f),
            new Vector2(circleCollider2D.bounds.size.x - 0.2f, circleCollider2D.bounds.size.y - 0.2f), 
            0, Vector2.down, 0.5f, 1 << 0);
        grounded = hit.collider is not null;
        // if (grounded)
        //     Debug.Log(hit.collider);
        
        animator.SetBool(AnimationBools.PlayerGrounded, grounded);
    }

    private void MovePlayer()
    {
        var horizontal = Input.GetAxis("Horizontal");

        player.velocity = new Vector2(horizontal * Speed, player.velocity.y);

        if (horizontal > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontal < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

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
            if (CurrentNumberOfFireballs > 0 && cooldownTimerForAttack >= CooldownBetweenFireballs)
            {
                cooldownTimerForAttack = 0;
                Attack();
            }
        }
    }

    private void Jump()
    {
        player.velocity = new Vector2(player.velocity.x, JumpHeight);
        
        animator.SetTrigger(AnimationTriggers.PlayerJump);
    }
    
    private void Attack()
    {
        CurrentNumberOfFireballs -= 1;
        SendFireball();
        player.velocity = new Vector2(player.velocity.x, AttackRecoil);
        
        ShakeCamera(0.2f, 0.1f);
        
        fireballSound.pitch = Random.Range(fireballClip.Pitch.min, fireballClip.Pitch.max);
        fireballSound.volume = Random.Range(fireballClip.Volume.min, fireballClip.Volume.max);
        fireballSound.PlayOneShot(fireballClip.Clip);
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
            CurrentNumberOfFireballs = MaxNumberOfFireballs;
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
        
        if (hp == 0)
        {
            Deactivate();
        }
    }

    private void Deactivate()
    {
        animator.SetTrigger(AnimationTriggers.PlayerDie);
        StartCoroutine(Deactivate(animator.GetAnimationLength(AnimationTriggers.PlayerDie)));
    }
    
    private IEnumerator Deactivate(float delay) {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
        OnPlayerDeath?.Invoke();
    }
    
    private void ShakeCamera(float duration, float power) => Camera.Shake(duration, power);
}
