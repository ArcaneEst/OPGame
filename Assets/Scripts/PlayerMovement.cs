using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    private Animator animator;
    
    private bool grounded;
    private float beforeAttackCooldown = 0.5f;
    private float cooldownTimerBeforeAttack = 0;

    [SerializeField] private float speed;
    [SerializeField] private float jumpSpeed;
    public float attackRecoil = 3;

    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;
    private float attackCooldown = 0.3f;
    private float cooldownTimerForAttack = Mathf.Infinity;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    { 
        var horizontal = Input.GetAxis("Horizontal");
        
        body.velocity = new Vector2(horizontal * speed, body.velocity.y);
        
        // Flip player when moving to different directions horizontally
        if (horizontal > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontal < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);
        
        if (Input.GetKey(KeyCode.Space))
            SpacePressed();
        if (!grounded)
            cooldownTimerBeforeAttack += Time.deltaTime;
        
        cooldownTimerForAttack += Time.deltaTime;
        
        animator.SetBool("run", horizontal != 0);
        animator.SetBool("grounded", grounded);
    }

    private void SpacePressed()
    {
        if (grounded)
        {
            Jump();
        }
        else if (cooldownTimerBeforeAttack >= beforeAttackCooldown)
        {
            Attack();
        }
    }

    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, jumpSpeed);
        grounded = false;
        
        animator.SetTrigger("jump");
    }
    
    private void Attack()
    {
        if (cooldownTimerForAttack >= attackCooldown)
        {
            cooldownTimerForAttack = 0;
            var fireballIndex = FindFireball();
            fireballs[fireballIndex].transform.position = firePoint.position;
            fireballs[fireballIndex].GetComponent<Projectile>().Shoot();
            body.velocity = new Vector2(body.velocity.x, attackRecoil);
        }   
    }

    private int FindFireball()
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
        }
    }
}
    