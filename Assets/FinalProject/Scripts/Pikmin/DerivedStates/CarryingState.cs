﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryingState : IState
{
	public Pikmin myPikmin;
	
	public CarryingState(Pikmin pikmin)
	{
		myPikmin = pikmin;
	}
	
	
	void Enter()
	{
		myPikmin.source.clip = myPikmin.heySfx;
		myPikmin.source.Play();
	}

	public override void Exit()
	{
        GrabAgent agent = myPikmin.GetComponent<GrabAgent>();
        if (agent) agent.enabled = false;
    }

	void Process()
	{
		Quaternion targetRotation = Quaternion.LookRotation(myPikmin.rigid.velocity.normalized, Vector3.up);
		myPikmin.transform.rotation = Quaternion.Slerp(myPikmin.transform.rotation, targetRotation, Time.deltaTime * 10f);
	}

	void PhysicsProcess()
	{

	}
	
	public override void OnAreaStay(Collider collision)
	{
		if (collision.tag == "Captain" && collision.gameObject != PikminManager.instance.gameObject) /* touched by whistle or captain = rejoin group */
		{
			CallTransition(State.IN_SQUAD, this);
		}
	}
}