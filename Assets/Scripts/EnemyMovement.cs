using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyMovement : MonoBehaviour
{

    [SerializeField] float movementSpeed = -10f;
    Rigidbody2D myRigidbody2D;
    CapsuleCollider2D myFeetCollider2D;
    BoxCollider2D myBodyCollider2D;

    Animator myAnimator;
    bool isWallHitted = false;
    // bool canMove = true;


    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myFeetCollider2D = GetComponent<CapsuleCollider2D>();
        myBodyCollider2D = GetComponent<BoxCollider2D>();

    }

    void Update()
    {


        if (myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            myRigidbody2D.velocity = new Vector2(movementSpeed, 0f);

        }

    }

    void OnTriggerExit2D(Collider2D other)
    {

        if (!isWallHitted && other.tag == "Ground")
        {

            FlipEnemyFacing();
        }



    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Wall")
        {

            isWallHitted = true;

            FlipEnemyFacing();
            isWallHitted = false;
        }
    }

    void FlipEnemyFacing()
    {

        float xVelocity = myRigidbody2D.velocity.x;

        // if (xVelocity > 0){
        //     transform.localScale = new Vector2(1f, 1f); // Facing right
        // }
        // else if (xVelocity < 0)
        //     transform.localScale = new Vector2(-1f, 1f); // Facing left


        movementSpeed = -xVelocity;
        transform.localScale = new Vector2(Mathf.Sign(myRigidbody2D.velocity.x), 1f);


    }
    public void DisableMovement()
    {
        movementSpeed = 0f;
        Debug.Log("Die Mortal Creature! ha ha ha");
        myAnimator.SetBool("isIdle", true);

    }
}
