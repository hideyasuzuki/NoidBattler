using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using UnityEditor.TextCore.Text;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] Slider hp;
    private Rigidbody rb;

    Vector3 movingDirection;
    float speed = 20;
    Vector3 movingVelocity;

    float boostSpeed = 250;

    bool isStop = false;
    bool isStep = false;

    int jumpCount = 0;

    Animator animator;

    public enum State
    {
        stay,
        forward,
        right,
        back,
        left,
        stepFront,
        stepRight,
        stepBack,
        stepLeft,
    }

    public enum AttackState
    {
        swingSide = 1,
        swingDown,
        swingJump,
        swingUp,
    }


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isStop)
        {
            movingVelocity = Vector3.zero;
            animator.SetInteger("State", (int)State.stay);
            return;
        }
        else
        {
            PlayerMove();
            PlayerAttack();
            Jump();
        }
    }

    void FixedUpdate()
    {
        if (isStep)
        {
            rb.AddForce(new Vector3(movingVelocity.x, 0, movingVelocity.z) * boostSpeed * Time.deltaTime, ForceMode.VelocityChange);
        }
        else
        {
            rb.velocity = new Vector3(movingVelocity.x, rb.velocity.y, movingVelocity.z);
        }
    }

    void Jump()
    {
        float jumpPower = 50f; // ÉWÉÉÉìÉvÇÃçÇÇ≥

        if (Input.GetKeyDown(KeyCode.Space) && jumpCount <= 1)
        {
            rb.AddForce(transform.up * jumpPower, ForceMode.Impulse);
            jumpCount++;
        }
    }

    void PlayerMove()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        movingDirection = new Vector3(x, 0, z);

        if (rb.velocity.x == 0)
        {
            animator.SetInteger("State", (int)State.stay);
        }

        if (rb.velocity.z > 0.1f)
        {
            animator.SetInteger("State", (int)State.forward);
            if (Input.GetKey(KeyCode.LeftShift) && !isStep)
            {
                animator.SetInteger("State", (int)State.stepFront);
            }
        }
        else if (rb.velocity.z < -0.1f)
        {
            animator.SetInteger("State", (int)State.back);
            if (Input.GetKey(KeyCode.LeftShift) && !isStep)
            {
                animator.SetInteger("State", (int)State.stepBack);
            }
        }

        if (rb.velocity.x > 0.1f)
        {
            animator.SetInteger("State", (int)State.right);
            if (Input.GetKey(KeyCode.LeftShift) && !isStep)
            {
                animator.SetInteger("State", (int)State.stepRight);
            }
        }
        else if (rb.velocity.x < -0.1f)
        {
            animator.SetInteger("State", (int)State.left);
            if (Input.GetKey(KeyCode.LeftShift) && !isStep)
            {
                animator.SetInteger("State", (int)State.stepLeft);
            }
        }

        if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A))
        {
            animator.SetInteger("State", (int)State.stay);
            if (Input.GetKey(KeyCode.W))
            {
                animator.SetInteger("State", (int)State.forward);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                animator.SetInteger("State", (int)State.back);
            }
        }
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
        {
            animator.SetInteger("State", (int)State.stay);
            if (Input.GetKey(KeyCode.D))
            {
                animator.SetInteger("State", (int)State.right);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                animator.SetInteger("State", (int)State.left);
            }
        }

        movingDirection.Normalize();//éŒÇﬂÇÃãóó£Ç™í∑Ç≠Ç»ÇÈÇÃÇñhÇ¨Ç‹Ç∑
        movingVelocity = movingDirection * speed;
    }

    void PlayerAttack()
    {
        float weakAttackTimer = 0;
        float weakAttackCount = 2;

        float strongAttackTimer = 0;
        float strongAttackCount = 5;

        bool isWeakAttack = false;
        bool isStrongAttack = false;

        if (Input.GetMouseButtonDown(0) && !isStrongAttack)
        {
            isStop = true;
            isStrongAttack = true;
            animator.SetInteger("AttackState", (int)AttackState.swingSide);
        }
        else if (Input.GetMouseButtonDown(1) && !isStrongAttack)
        {
            isStop = true;
            isStrongAttack = true;
            animator.SetInteger("AttackState", (int)AttackState.swingDown);
        }


        if (Input.GetKeyDown(KeyCode.E) && !isWeakAttack)
        {
            isStop = true;
            isWeakAttack = true;
            animator.SetInteger("AttackState", (int)AttackState.swingUp);
        }
        else if (Input.GetKeyDown(KeyCode.F) && !isWeakAttack)
        {
            isStop = true;
            isWeakAttack = true;
            animator.SetInteger("AttackState", (int)AttackState.swingJump);
        }


        if (isWeakAttack)
        {
            weakAttackTimer += Time.deltaTime;
            if (weakAttackTimer > weakAttackCount)
            {
                weakAttackTimer = 0;
                isWeakAttack = false;
            }
        }


        if (isStrongAttack)
        {
            strongAttackTimer += Time.deltaTime;
            if(strongAttackTimer > strongAttackCount)
            {
                strongAttackTimer = 0;
                isStrongAttack = false;
            }
        }
    }

    void StepEvent()
    {
        isStep = true;
    }

    void StepEndEvent()
    {
        isStep = false;
    }


    void IsStopEvent()
    {
        isStop = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Weapon")
        {
            int damage = 1;
            hp.value -= damage * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ground")
        {
            jumpCount = 0;
        }
    }
}
