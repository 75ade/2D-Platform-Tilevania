using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] float rollSpeed = 5f;
    [SerializeField] float rollDuration = 0.7f;
    [SerializeField] Vector2 deathKick = new Vector2(10f, 20f);
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject arrow;
    [SerializeField] Transform gun;

    [SerializeField] float shootingArrowAnimationDuration = 0.5f;
    [SerializeField] float rollCoolDown = 2f;

    Rigidbody2D myRigidbody;
    Vector2 moveInput;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    float gravityScaleAtStart;
    BoxCollider2D myFeetCollider;
    bool isAlive = true;

    bool isRolling = false;
    bool canRoll = true;
    float rollDirection;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        gravityScaleAtStart = myRigidbody.gravityScale;
        myFeetCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (!isAlive) { return; }
        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }

    void OnMove(InputValue value)
    {
        if(!isAlive) { return; }
        
        moveInput = value.Get<Vector2>();
        Debug.Log("Detect OnMove");
    }

    void OnJump(InputValue value)
    {
        if(!isAlive) { return; }

        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))   return;

        if (value.isPressed)
        {
            myRigidbody.linearVelocity += new Vector2(0f, jumpSpeed);
        }
    }


    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, myRigidbody.linearVelocity.y);
        // Debug.Log($"LinearVelocity.y: {myRigidbody.linearVelocity.y} , {myRigidbody.linearVelocityY}");
        myRigidbody.linearVelocity = playerVelocity;

        bool hasHorizontalSpeed = Mathf.Abs(myRigidbody.linearVelocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", hasHorizontalSpeed);
    }

    void FlipSprite()
    {
        bool hasHorizontalSpeed = Mathf.Abs(myRigidbody.linearVelocity.x) > Mathf.Epsilon;
        
        if (hasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.linearVelocity.x), 1f);
        }
    }
    void ClimbLadder()
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            myRigidbody.gravityScale = gravityScaleAtStart;
            myAnimator.SetBool("isClimbing", false);
            return;
        }
        
        Vector2 climbVelocity = new Vector2(myRigidbody.linearVelocity.x, moveInput.y * climbSpeed);
        myRigidbody.linearVelocity = climbVelocity;
        myRigidbody.gravityScale = 0f;

        bool hasVerticalSpeed = Mathf.Abs(myRigidbody.linearVelocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("isClimbing", hasVerticalSpeed);
    }

    void OnAttack()
    {
        if (!isAlive)   { return; }
        // Instantiate(bullet, gun.position, transform.rotation);

        StartCoroutine(ShootingArrow());
    }

    IEnumerator ShootingArrow()
    {
        myAnimator.SetBool("isShooting", true);
        Instantiate(arrow, gun.position, Quaternion.identity);

        yield return new WaitForSeconds(shootingArrowAnimationDuration);

        myAnimator.SetBool("isShooting", false);
    }

    void Die()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")) ||
            myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")))
        {
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            myRigidbody.linearVelocity = deathKick;
            FindAnyObjectByType<GameSession>().ProcessPlayerDeath();
        }
    }

    void OnSprint()
    {
        if (!isAlive)   return;

        if (canRoll && !isRolling)
        {
            StartCoroutine(Rolling());
        }
    }

    IEnumerator Rolling()
    {
        isRolling = true;
        canRoll = false;
        rollDirection = transform.localScale.x;
        myAnimator.SetTrigger("Rolling");

        float elapsed = 0f;
        
        while (elapsed < rollDuration)
        {
            myRigidbody.linearVelocity = new Vector2(rollDirection * rollSpeed, myRigidbody.linearVelocity.y);
            
            elapsed += Time.deltaTime;

            yield return null;
        }

        isRolling = false;
        
        yield return new WaitForSeconds(rollCoolDown);

        canRoll = true;
    }
}
