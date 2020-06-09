using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    public Camera cam;
    [SerializeField] protected NavMeshAgent agent;
    protected Animator animator;
    protected Rigidbody rb;
    protected Collider col;
    bool isAttack;
    protected bool onGround = false;
    public UnityEvent landingEvent;
   
    void Start()
    {
        agent = GetComponentInChildren<NavMeshAgent>();
        animator = GetComponent<Animator>();
       
        col = GetComponent<CapsuleCollider>();     
        isAttack = false;
        


    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Attack();
  
   
    }

    public void Move()
    {

        if (!agent.enabled)
            return;
        if (  Input.GetMouseButtonDown(0))
        {
            RaycastHit target;
            Ray targetRay = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(targetRay, out target))
            {
                if (isAttack == true)
                {
                    animator.SetBool("isAttack", false);
                    isAttack = false;
                    agent.isStopped = false;
                }
                agent.SetDestination(target.point);
            }

        }
        if (agent.isOnNavMesh && agent.remainingDistance > agent.stoppingDistance)
        {
            animator.SetFloat("Speed", 1f);
            this.NotifyCamWhenMoving();
        }
        else
        {
            animator.SetFloat("Speed", 0f);
        }


    }

    public void NotifyCamWhenMoving()
    {
        cam.GetComponent<CameraController>().FollowPlayer();
    }
    public void Rotate()
    {
        transform.forward = (agent.nextPosition - transform.position).normalized;
        transform.LookAt(rb.position + transform.forward);
    }

    public void Attack()
    {
        if (Input.GetKey(KeyCode.Space) && isAttack == false)
        {
            if (agent.enabled)
                agent.isStopped = true;
            animator.SetBool("isAttack", true);
            animator.SetFloat("Speed", 0f);
            isAttack = true;
        }
    }


            
 }


