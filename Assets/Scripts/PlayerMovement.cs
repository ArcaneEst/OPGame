using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D player;
    private Animator animator;
    
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;

    private const float Speed = 10;
    private const float JumpHeight = 10;
    private const float AttackRecoil = 3;
    
    private const float CooldownBetweenFireballs = 0.3f;
    private const float CooldownBeforeAttack = 0.5f;
    private const int MaxNumberOfFireballs = 4;
    
    private bool grounded;
    private int currentNumberOfFireballs = 0;
    private float cooldownTimerBeforeAttack = 0;
    private float cooldownTimerForAttack = Mathf.Infinity;

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
        fireballs[index].GetComponent<Projectile>().Shoot();
    }
    
    private int FindAvailableFireballIndex()
    {
        for (var i = 0; i < fireballs.Length; i++)
            if (!fireballs[i].activeInHierarchy)
                return i;
        return 0;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            cooldownTimerBeforeAttack = 0;
            grounded = true;
            currentNumberOfFireballs = MaxNumberOfFireballs;
        }
    }
}
    