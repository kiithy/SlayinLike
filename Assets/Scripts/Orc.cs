using UnityEngine;
using System.Collections;

public class Orc : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    private Rigidbody2D rb;
    private Animator animator; // Reference to the animator

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float attackRange = 2f;
    private bool isAttacking = false;

    private bool isDamaged = false;

    private CapsuleCollider2D orcCollider;

    private void Awake()
    {
        orcCollider = GetComponent<CapsuleCollider2D>();
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

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
        if (isDamaged) return;

        // Calculate distance to player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // If in attack range and not already attacking
        if (distanceToPlayer <= attackRange && !isAttacking)
        {
            StartAttack();
        }
        // If not attacking, continue moving
        else if (!isAttacking)
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

    private void StartAttack()
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
        isAttacking = false;  // Stop any current attack
        animator.SetBool("IsAttacking", false);  // Reset attack animation
        animator.SetBool("IsWalking", false);    // Stop walking animation
        animator.SetTrigger("Damaged");

    }

    public void ResetAfterDamage()
    {
        isDamaged = false;
        animator.SetBool("IsWalking", true);
        // Attacks will happen naturally again
    }
}
