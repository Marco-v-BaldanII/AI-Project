using BBUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;

public class ObstacleAvoidance : MonoBehaviour
{

    public Collider detection_area;
    public Rigidbody rigid;

    public MBTExecutor executor;


    public float avoidanceWeight = 8;

    private MBT_Wander wanderer;

    float time = 2f;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponentInParent<Rigidbody>();
        executor = GetComponentInParent<MBTExecutor>();
        wanderer = GetComponent<MBT_Wander>();
        int u = 0;
    }



    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Devert positions");
        if (other.tag == "ground")
        {

            Vector3 distance = transform.position - other.transform.position;


            float forwardProjection = Vector3.Dot(transform.forward, distance);

            rigid.velocity += (-transform.right * forwardProjection) * avoidanceWeight;
            time = 0.5f;
        }


    }

    private void Update()
    {
        Vector3 buff = rigid.velocity; buff.y = 0;
        if (buff != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(buff.normalized, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2f);
            //transform.rotation = targetRotation;
        }

        time -= Time.deltaTime;
        if (time <= 0)
        {
            if (executor) executor.enabled = true;
        }
        else
        {
            if (executor )
            {
                wanderer.angle += 1;
                executor.enabled = false;
            }
        }
    }

}
