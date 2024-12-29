using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostFollower : MonoBehaviour
{
	public GameObject ghost;
	private NavMeshAgent agent;
	

	void Awake()
    {
	    agent = GetComponent<NavMeshAgent>();
    }


	void FixedUpdate()
    {
	    agent.destination = ghost.transform.position;
    }
}
