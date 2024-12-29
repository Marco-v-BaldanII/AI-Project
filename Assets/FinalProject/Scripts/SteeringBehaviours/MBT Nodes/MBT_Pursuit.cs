using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using MBT;

[AddComponentMenu("")]
[MBTNode(name = "Pursuit")]
public class MBT_Pursuit : Leaf
{
	
	public Rigidbody ghostRb;
	public float slowDownRadius;
	
	public Vector3 targetPos;
	
	private Blackboard board;
	

	void Awake()
    {
	    board = GetComponent<Blackboard>();
    }


	public override NodeResult Execute()
	{
    	
		targetPos = board.GetVariable<TransformVariable>("target").Value.position;
    	
	    Vector3 direction = (targetPos - ghostRb.transform.position).normalized;


	    ghostRb.velocity = new Vector3(direction.x, Mathf.Clamp(ghostRb.velocity.y, -2.0f, 0.01f), direction.z) * 5;

	    // ARRIVE
	    var distance = Vector3.Distance(targetPos, ghostRb.transform.position);

	    if (distance < slowDownRadius) /*Slow down when get closer than "slowDownRadius" */
	    {
		    ghostRb.velocity *= distance / slowDownRadius;
		    if (distance < slowDownRadius / 2)
		    {
			    ghostRb.velocity = Vector3.zero;
				return NodeResult.failure; /* this way atk can execute */
			    // it should go into atk state
			    
		    }
	    }
	    
	    return NodeResult.success;
    }
}
