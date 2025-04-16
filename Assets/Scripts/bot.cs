using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bot : MonoBehaviour
{
    [SerializeField] private GameObject checkPoint;
    public LayerMask tilemapLayer;
    [SerializeField] private float rayLength = 0.9f;
    [SerializeField] private float moveSpeed = 2f;

    [SerializeField] private float attackRayLength = 1.5f;
    [SerializeField] private float attackRate = 1f;
    private bool isAttacking = false;
    private bool isTurning = false;
    public float flipHoldTime;

    private bool isFacingRight = true;
    private float direction;

    private Animator anim;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        direction = UnityEngine.Random.Range(0, 2) * 2 - 1;
        isFacingRight = direction == 1;
        if (!isFacingRight) flip();
    }

    void Update()
    {
        if (!isTurning) MoveBot();
        checkObstacles();
        UpdateRayDirection();
        AttackRaycast();
    }

    private void MoveBot()
    {
        if (!isAttacking && !isTurning)
        {
            rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);
            anim.SetFloat("move", Mathf.Abs(direction));
        }
    }

    private void flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 size = transform.localScale;
        size.x *= -1;
        transform.localScale = size;
    }

    private void checkObstacles()
    {
        if (isTurning) return;

        Transform checkPointPosition = checkPoint.transform;
        Vector2 checkX = new Vector2(checkPointPosition.position.x, checkPointPosition.position.y);
        Vector2 rayDirection = isFacingRight ? Vector2.right : Vector2.left;

        RaycastHit2D hit = Physics2D.Raycast(checkX, rayDirection, rayLength, tilemapLayer);
        Debug.DrawRay(checkX, rayDirection * rayLength, Color.red);

        if (hit.collider != null && hit.collider.GetComponent<Tilemap>() != null)
        {
            StartCoroutine(TurnIdle());
        }
    }

    private IEnumerator TurnIdle()
    {
        isTurning = true;
        anim.SetBool("Idle", true);
        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(flipHoldTime);

        direction *= -1;
        flip();

        anim.SetBool("Idle", false);
        isTurning = false;
    }

    private void UpdateRayDirection()
    {
        isFacingRight = direction > 0;
    }

    private void AttackRaycast()
    {
        if (isAttacking) return;

        Vector2 origin = transform.position;
        Vector2 directionRay = isFacingRight ? Vector2.right : Vector2.left;

        RaycastHit2D hitTile = Physics2D.Raycast(origin, directionRay, attackRayLength, tilemapLayer);
        RaycastHit2D hitPlayer = Physics2D.Raycast(origin, directionRay, attackRayLength, LayerMask.GetMask("Player"));

        Debug.DrawRay(origin, directionRay * attackRayLength, Color.blue);

        if (hitPlayer.collider != null && hitPlayer.collider.CompareTag("Player"))
        {
            if (hitTile.collider == null || hitTile.distance > hitPlayer.distance)
            {
                
                GameController gameController = FindObjectOfType<GameController>();
                if (gameController != null)
                {
                    gameController.TakeDamage(1);
                    StartCoroutine(AttackCooldown());
                }
            }
        }
    }

    private IEnumerator AttackCooldown()
    {
        isAttacking = true;
        anim.SetBool("Attacking", true);
        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(attackRate);

        isAttacking = false;
        anim.SetBool("Attacking", false);
    }
}
