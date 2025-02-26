using UnityEngine;
using System.Collections;

public class Orc : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public gameplay knight;
    private Rigidbody2D rb;
    private Animator animator; // Reference to the animator

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float attackRange = 2f;
    private bool isAttacking = false;

    private bool isDamaged = false;

    public AudioSource orcAudioSource;
    public AudioClip orcAttackSound;
    public AudioClip orcDeathSound;

    public CapsuleCollider2D orcCollider;  // Parent's collider for attacks

    private bool playerInRange = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        orcCollider = GetComponent<CapsuleCollider2D>();

        // Find player if not assigned
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
            Debug.Log($"Found player at position: {player?.position}");
        }
    }

    private void Update()
    {
        if (player == null)
        {
            Debug.LogWarning("No player reference!");
            return;
        }

        // Don't process attack/movement logic if damaged
        if (isDamaged)
        {
            // Stop any current movement and attack
            rb.velocity = Vector2.zero;
            isAttacking = false;
            animator.SetBool("IsAttacking", false);
            animator.SetBool("IsWalking", false);
            return;
        }

        // Calculate distance to player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // If in attack range and not already attacking
        if ((distanceToPlayer <= attackRange) && !isAttacking)
        {
            StartAttack();
        }

        // If not attacking, continue moving
        if (!isAttacking)
        {
            MoveTowardsPlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
        float direction = Mathf.Sign(player.position.x - transform.position.x);
        rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);

        // Set walking animation
        animator.SetBool("IsWalking", true);
        animator.SetBool("IsAttacking", false);

        // Flip sprite based on direction
        transform.localScale = new Vector3(
            Mathf.Abs(transform.localScale.x) * direction,
            transform.localScale.y,
            transform.localScale.z
        );
    }

    public void StartAttack()
    {
        isAttacking = true;
        rb.velocity = Vector2.zero; // Stop moving
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsAttacking", true);
    }

    // Called by Animation Event at the end of attack animation
    public void OnAttackAnimationEnd()
    {
        isAttacking = false;
        animator.SetBool("IsAttacking", false);
        animator.SetBool("IsWalking", true);
    }

    // Optional: Visualize attack range in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void TakeDamage(int damage)
    {
        Debug.Log($"Orc took {damage} damage!");
        isDamaged = true;
        isAttacking = false;
        animator.SetBool("IsAttacking", false);
        animator.SetBool("IsWalking", false);

        // Set the hurt animation to play for 2 seconds
        // StartCoroutine(PlayHurtAnimation());

        animator.SetTrigger("Damaged");
        ResetAfterDamage();
    }

    // private IEnumerator PlayHurtAnimation()
    // {
    //     animator.SetTrigger("Damaged");
    //     yield return new WaitForSeconds(2f);
    //     ResetAfterDamage();
    // }

    // Now called by coroutine instead of animation event
    public void ResetAfterDamage()
    {
        isDamaged = false;
        animator.SetBool("IsWalking", true);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            knight = other.GetComponent<gameplay>();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    // Called by Animation Event during attack animation
    public void StartDamageCoroutine()
    {
        if (knight != null && playerInRange && !knight.invincible)
        {
            StartCoroutine(knight.Damaged());
        }
    }

    public void PlayAttackSound()
    {
        orcAudioSource.PlayOneShot(orcAttackSound);
    }

    public void PlayDeathSound()
    {
        orcAudioSource.PlayOneShot(orcDeathSound);
    }
}
