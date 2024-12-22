using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
	public Pikmin myPikmin;
	
	public IdleState(Pikmin pikmin)
	{
		myPikmin = pikmin;
	}
	
	
    void Enter()
    {

    }

    void Exit()
    {

    }

    void Process()
    {

    }

    void PhysicsProcess()
    {

    }
    
	public override void OnAreaEnter(Collider collision)
	{
		CheckIfGrabObject(collision);
	}

	public override void OnAreaStay(Collider collision)
	{
		CheckIfGrabObject(collision);
	}
	
	private void CheckIfGrabObject(Collider collision)
	{
		if (collision.tag == "Pellet")
		{
			myPikmin.targetObject = collision.gameObject;
			CallTransition(State.GRAB, this);
		}
	}

}
