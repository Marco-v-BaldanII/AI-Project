using Codice.Client.Common;
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
    
	public Collider home;

    private void Awake()
    {
	    //home = GetComponentInParent<Collider>();
        rigid = GetComponent<Rigidbody>();
    }
    
	public override void OnEnter()
	{
		base.OnEnter();
		//angle = 0;
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


	    int i = 0;
	    // keeps the wander point inside home area
	    //while (home.bounds.Contains(endPoint) == false)
	    //{
	    //	angle += 5f;
	    //	projection.x = Mathf.Cos(angle);
		//    projection.z = Mathf.Sin(angle);
		//    projection *= wanderRadius;
	    //	endPoint = wander_shpere.transform.position + projection;
	    //	i++;
	    //	if(i > 600){
        //        break;
        //    }
	    //}


        Debug.DrawLine(wander_shpere.transform.position, endPoint, Color.red);

        var speed = (endPoint - transform.position).normalized * movementSpeed;
        rigid.velocity = new Vector3(speed.x, rigid.velocity.y, speed.z);

        Quaternion targetRotation = Quaternion.LookRotation(rigid.velocity, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 50 * UnityEngine.Time.deltaTime);

        return NodeResult.success;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyHome")
        {
            int sum = 0;
            int rand = Random.Range(0, 2);
            if(rand == 0)
            {
                sum = 170;
            }
            else
            {
                sum = -170;
            }
            angle += sum;
        }
    }

}
