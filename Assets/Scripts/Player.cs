using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] protected float runSpeed = 2.5f;
    [SerializeField] protected float walkSpeed = 2.5f;
    [SerializeField] protected float jumpAmount = 2f;
    [SerializeField] protected Vector2 deathKick = new Vector2(10f, 10f);

    [SerializeField] protected ParticleSystem bloodEffect;
    [SerializeField] protected ParticleSystem bloodEffect2;
    [SerializeField] protected ParticleSystem bloodEffect3;
    [SerializeField] protected AudioClip backgroundMusic;
    [SerializeField] protected AudioClip deathMusic;

    protected bool isRunning = false;
    protected bool isSitting = false;
    protected bool isStanding = false;
    public bool isAlive = true;

    protected Rigidbody2D myRigidbody2D;
    protected CapsuleCollider2D myBodyCollider2D;
    protected BoxCollider2D myFeetCollider2D;
    protected Animator myAnimator;
    protected EnemyMovement enemyMovement;

    protected virtual void Awake()
    {
        enemyMovement = FindObjectOfType<EnemyMovement>();
    }

    protected virtual void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider2D = GetComponent<CapsuleCollider2D>();
        myFeetCollider2D = GetComponent<BoxCollider2D>();
        // GetComponent<AudioSource>().PlayOneShot(backgroundMusic);
    }

    protected virtual void Update()
    {
        if (!isAlive) return;
        HandleDeath();
    }

    protected void HandleDeath()
    {
        if (myBodyCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemy")))
        {
myBodyCollider2D.enabled = false;
// myFeetCollider2D.enabled = false;

            if(!isSitting){
                // Log the value of deathKick to confirm
                Debug.Log("Applying deathKick force: " + deathKick);

                Debug.Log("Velocity before applying deathKick: " + myRigidbody2D.velocity);
                myRigidbody2D.AddForce(deathKick, ForceMode2D.Impulse);
                Debug.Log("Velocity after applying deathKick: " + myRigidbody2D.velocity);

            }


            isAlive = false;
            myAnimator.SetTrigger("Dying");
            bloodEffect.Play();
            enemyMovement.DisableMovement();
            myRigidbody2D.drag = 5f; // Gradually reduce velocity using linear drag


        }
    }

}
