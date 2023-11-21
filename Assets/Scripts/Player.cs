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
    float boostSpeed = 1000;
    float speed = 5000;
    float posX;
    float posZ;

    float inputHorizontal;
    float inputVertical;
    
    bool isStop = false;
    bool isStep = false;

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
        swingJump = 1,
        swingUp,
        swingSide,
        swingDown,
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
            rb.velocity = Vector3.zero;
            animator.SetInteger("State", (int)State.stay);
            return;
        }
        else
        {
             PlayerMove();
            PlayerAttack();
        }
    }

    void PlayerMove()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");

        rb.velocity = new Vector3(inputHorizontal, 0.0f, inputVertical) * speed * Time.deltaTime;

        if (rb.velocity.x == 0)
        {
            animator.SetInteger("State", (int)State.stay);
        }

        if (isStep)
        {
            rb.AddForce(new Vector3(posX, rb.velocity.y, posZ) * boostSpeed * Time.deltaTime, ForceMode.VelocityChange);
        }

        if (rb.velocity.z > 0.1f)
        {
            animator.SetInteger("State", (int)State.forward);
            if (Input.GetKey(KeyCode.LeftShift) && !isStep)
            {
                animator.SetInteger("State", (int)State.stepFront);
                posZ = 5;
                Debug.Log("A");
            }
        }
        else if (rb.velocity.z < -0.1f)
        {
            animator.SetInteger("State", (int)State.back);
            if (Input.GetKey(KeyCode.LeftShift) && !isStep)
            {
                animator.SetInteger("State", (int)State.stepBack);
                posZ = -5;
            }
        }

        if (rb.velocity.x > 0.1f)
        {
            animator.SetInteger("State", (int)State.right);
            if (Input.GetKey(KeyCode.LeftShift) && !isStep)
            {
                animator.SetInteger("State", (int)State.stepRight);
                posX = 5;
            }
        }
        else if (rb.velocity.x < -0.1f)
        {
            animator.SetInteger("State", (int)State.left);
            if (Input.GetKey(KeyCode.LeftShift) && !isStep)
            {
                animator.SetInteger("State", (int)State.stepLeft);
                posX = -5;
            }
        }

        if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A))
        {
            animator.SetInteger("State", (int)State.stay);
            if (Input.GetKey(KeyCode.W))
            {
                animator.SetInteger("State", (int)State.forward);
                posZ = 2.5f;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                animator.SetInteger("State", (int)State.back);
                posZ = 2.5f;
            }
        }
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
        {
            animator.SetInteger("State", (int)State.stay);
            if (Input.GetKey(KeyCode.D))
            {
                animator.SetInteger("State", (int)State.right);
                posX = 2.5f;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                animator.SetInteger("State", (int)State.left);
                posX = 2.5f;
            }
        }
    }

    void PlayerAttack()
    {
        float strongAttackTimer = 0;
        float strongAttackCount = 5;

        float weakAttackTimer = 0;
        float weakAttackCount = 2;

        bool isWeakAttack = false;
        bool isStrongAttack = false;

        if (Input.GetMouseButtonDown(0) && !isStrongAttack)
        {
            animator.SetInteger("AttackState", (int)AttackState.swingJump);
            isStrongAttack = true;
            isStop = true;
        }
        else if(Input.GetMouseButtonDown(1) && !isStrongAttack)
        {
            animator.SetInteger("AttackState", (int)AttackState.swingUp);
            isStrongAttack = true;
            isStop = true;
        }

        if(Input.GetKeyDown(KeyCode.E) && !isWeakAttack)
        {
            animator.SetInteger("AttackState", (int)AttackState.swingSide);
            isWeakAttack = true;
            isStop = true;
        }
        else if(Input.GetKeyDown(KeyCode.F) && !isWeakAttack)
        {
            animator.SetInteger("AttackState", (int)AttackState.swingDown);
            isWeakAttack = true;
            isStop = true;
        }


        if(isStrongAttack)
        {
            strongAttackTimer += Time.deltaTime;
            if(strongAttackTimer > strongAttackCount)
            {
                strongAttackTimer = 0;
                isStrongAttack = false;
            }
        }

        if(isWeakAttack)
        {
            weakAttackTimer += Time.deltaTime;
            if(weakAttackTimer > weakAttackCount)
            {
                weakAttackTimer = 0;
                isWeakAttack = false;
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
        posX = 0;
        posZ = 0;
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
}
