using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    Animator anim;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    public Transform cam;
    Rigidbody rigid;
    GameObject enemyCol;

    public float maxHealth = 100f;
    public float cur_Health = 0f;



    //public EnemyController enemyCont { get; set; }
    public GameObject attackGameObject;






    public float speed = 6f;
    void Start()

    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        attackGameObject = GetComponent<GameObject>();

        cur_Health = maxHealth;
        HealthBarSet();




    }


    void Update()
    {

        Attack();
        Movement();


    }

    void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {

            anim.SetTrigger("Attack");


        }





        if (Input.GetMouseButtonDown(1))
        {
            anim.SetTrigger("Attack2");
        }
    }

    void Movement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");


        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        if (direction.magnitude >= 0.1f)
        {
            anim.SetInteger("Condition", 1);
            float targetAngle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
        else { anim.SetInteger("Condition", 0); }
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            //enemy health -10
            //GetComponent<EnemyController>().TakeDamage(10);
            Debug.Log("Enemy hit");
        }
    }


    public void HealthBarSet()
    {
        float myHealth = cur_Health / maxHealth;
    }
}
