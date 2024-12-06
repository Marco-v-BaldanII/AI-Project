using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Hide : MonoBehaviour, FSM_State
{


    public Collider[] hiding_spots;

    private Collider current_spot;

    private Rigidbody rigid;

    public GameObject chaser;

    private FSM state_machine;

    private NavMeshAgent naveMesh;

    Vector3 destination;

    float speed = 2;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        state_machine = GetComponent<FSM>();
        naveMesh = GetComponent<NavMeshAgent>();
    }

    public void OnEnter()
    {
        float distance = 1000;

        foreach (Collider spot in hiding_spots)
        {
            float currentDist = Vector3.Distance(transform.position, spot.transform.position);
            if (currentDist < distance)
            {
                distance = currentDist;
                current_spot = spot;
            }

        }


        destination = chaser.transform.TransformDirection( current_spot.transform.position - chaser.transform.position);

        /* Calculate the direction from the hiding spot to the chaser */
        Vector3 directionToChaser = chaser.transform.position - current_spot.transform.position;
        Vector3 hidingDirection = directionToChaser.normalized;

        // Calculate the destination behind the hiding spot
        float distanceBehindSpot = 3.0f; 
        destination = current_spot.transform.position - hidingDirection * distanceBehindSpot;

        destination.y = current_spot.transform.position.y;



    }
    public void Move() /* Custom seek method according to Craig Reynold's vehicle model */
    {

        Vector3 direction = destination - transform.position;

        Vector3 desired_velocity = Vector3.Normalize(direction) * speed;

        Vector3 steeringForce = desired_velocity - rigid.velocity;

        rigid.velocity += steeringForce * 25 * Time.deltaTime; /* Create smooth turn towards next destination */

        rigid.velocity += (avoidance_force *  Time.deltaTime);

        //rigid.velocity = Vector3.ClampMagnitude(rigid.velocity, 40);

        Quaternion targetRotation = Quaternion.LookRotation(rigid.velocity, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 50 * Time.deltaTime);

        Debug.DrawLine(transform.position, destination);

        if(Vector3.Distance(transform.position, destination) < 1.5f)
        {
            state_machine.ChangeState(state_machine.Stay);
        }


        //if (Vector3.Distance(transform.position, target.transform.position) > grab_distance)
        //{
        //    state_machine.ChangeState(state_machine.Thievery);
        //}


    }

    public void OnExit()
    {

    }

    bool detect_obstacle = false;

    Vector3 avoidance_force;

    private void OnTriggerStay(Collider other)
    {
        detect_obstacle = true;

        Debug.Log("Devert positions");

        Vector3 distance = transform.position - other.transform.position;


        float forwardProjection = Vector3.Dot(transform.forward, distance);

        avoidance_force = (-transform.right * forwardProjection) * 2 ;

        avoidance_force /= (distance.magnitude * 120);



    }

    private void OnTriggerExit(Collider other)
    {
        avoidance_force = Vector3.zero;
        detect_obstacle = false;
    }

}
