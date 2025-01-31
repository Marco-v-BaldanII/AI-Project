﻿using System.Collections;
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

	public string targetVarName = "target";
	
	private Blackboard board;
	
	public float speed = 5f;
	

	void Awake()
    {
	    board = GetComponent<Blackboard>();
    }


	public override NodeResult Execute()
	{
    	
		if (board.GetVariable<TransformVariable>(targetVarName).Value != null){
				targetPos = board.GetVariable<TransformVariable>(targetVarName).Value.position;
	    	
		    Vector3 direction = (targetPos - ghostRb.transform.position).normalized;
			
			//Add somethinf to retrieve the radius from the target
		
	
			ghostRb.velocity = new Vector3(direction.x, Mathf.Clamp(ghostRb.velocity.y, -2.0f, 0.01f), direction.z) * speed;
	
		    // ARRIVE
		    var distance = Vector3.Distance(targetPos, transform.position);
	
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
		}
	    return NodeResult.success;
    }
}
