using BBUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidance : MonoBehaviour
{

    public Collider detection_area;
    public Rigidbody rigid;

    public Police LaPolice;
    public BehaviorExecutor executor;

    public float avoidanceWeight = 8;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponentInParent<Rigidbody>();
    }



    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Devert positions");

        Vector3 distance = transform.position - other.transform.position;


        float forwardProjection = Vector3.Dot(transform.forward, distance);

        rigid.velocity += (-transform.right * forwardProjection) * avoidanceWeight;



    }

    private void Update()
    {
        if (LaPolice.sees_pellet == true && executor.blackboard.boolParams[0] == false) /* If the "police" isnt looking at the treasure, go steal it */
        {
            // Set isSeen to true
            executor.blackboard.boolParams[1] = true;
        }
        else
        {
            executor.blackboard.boolParams[1] = false;
        }
    }

}
