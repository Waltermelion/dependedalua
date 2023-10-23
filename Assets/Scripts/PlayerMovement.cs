using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour {

    [SerializeField] private NetworkVariable<int> speed = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [SerializeField] private NetworkVariable<int> jumpingPower = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


    private Transform groundCheck;
    private Transform wallCheck;


    private int extraJumps;

    private bool isFacingRight;
    private bool canWallSlide;

    float horizontal;

    Rigidbody2D rb;



    private void Start() {
        Components();
    }


    private void Update() {
        if (!IsOwner) return;
        Inputs();
    }
    private void FixedUpdate() {

        if (IsWalled() && canWallSlide) {
            rb.velocity = new Vector2(horizontal, rb.velocity.y * .1f);
        } else {
            rb.velocity = new Vector2(horizontal * speed.Value, rb.velocity.y);
        }
    }
    private void Components() {
        rb = GetComponent<Rigidbody2D>();
        groundCheck = gameObject.transform.GetChild(1).GetComponent<Transform>();
        wallCheck = gameObject.transform.GetChild(2).GetComponent<Transform>();
    }

    private void Inputs() {
        
        FlipController();

        horizontal = Input.GetAxisRaw("Horizontal");


        if (IsGrounded() || IsWalled()) {
            extraJumps = 2;
        }
        if (Input.GetKeyDown(KeyCode.Space) && extraJumps > 0) {
            rb.velocity = Vector2.up * jumpingPower.Value;
            extraJumps--;
        } else if (Input.GetKeyDown(KeyCode.Space) && extraJumps == 0 && IsGrounded()) {
            rb.velocity = Vector2.up * jumpingPower.Value;
        }

        if (!IsGrounded() && rb.velocity.y < 0) {
            canWallSlide = true;
        }

    }

    private bool IsGrounded() {
        int layerMask = 1 << 8;
        return Physics2D.OverlapCircle(groundCheck.position, .1f, layerMask);
    }

    private bool IsWalled() {
        int layerMask = 1 << 8;
        return Physics2D.Raycast(wallCheck.position, Vector2.right, .1f, layerMask);
    }

    private void Flip() {

        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    private void FlipController() {

        if (IsGrounded() && IsWalled()) {
            if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f) {
                Flip();
            }
        }

        if (isFacingRight && rb.velocity.x < 0f || !isFacingRight && rb.velocity.x > 0f) {
            Flip();
        }
    }

}
