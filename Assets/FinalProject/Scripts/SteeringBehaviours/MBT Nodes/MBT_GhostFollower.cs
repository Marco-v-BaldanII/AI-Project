using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using MBT;

[AddComponentMenu("")]
[MBTNode(name = "FollowGhost")]
public class MBT_GhostFollower : Leaf
{
	public GameObject ghost;
	private NavMeshAgent agent;
	

	public override void OnEnter()
	{
		agent = GetComponent<NavMeshAgent>();
	}


	public override NodeResult Execute()
	{
		agent.destination = ghost.transform.position;
		/*Returns fail cause this is meant to be used in selector */
		return NodeResult.failure;
	}
}
