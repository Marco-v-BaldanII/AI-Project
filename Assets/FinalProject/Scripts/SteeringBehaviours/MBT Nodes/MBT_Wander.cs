using MBT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[AddComponentMenu("")]
[MBTNode(name = "Wander")]
public class MBT_Wander : Leaf
{
    public float wanderRadius;
    public float wanderDistance;
    public float wanderJitter;


    public float movementSpeed = 5;

    private Rigidbody rigid;

    public GameObject wander_shpere;

    public float angle = 0;

    private void Awake()
    {

        rigid = GetComponent<Rigidbody>();
    }
    public override NodeResult Execute()
    {
        float angleOffset = Random.Range(-0.10f, 0.10f);

        Vector3 projection = new Vector3();

        projection.x = Mathf.Cos(angle);
        projection.z = Mathf.Sin(angle);
        // these unit circle values work from the origin (0,0)

        projection *= wanderRadius;

        angle += angleOffset;

        // Adding the sphere's pos shifts the vector to start at it
	    Vector3 endPoint = wander_shpere.transform.position + projection;


        Debug.DrawLine(wander_shpere.transform.position, endPoint, Color.red);

        var speed = (endPoint - transform.position).normalized * movementSpeed;
        rigid.velocity = new Vector3(speed.x, rigid.velocity.y, speed.z);

        Quaternion targetRotation = Quaternion.LookRotation(rigid.velocity, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 50 * Time.deltaTime);

        return NodeResult.success;

    }

}
