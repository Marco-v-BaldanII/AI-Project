using System.Collections;
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
	
	public override void OnAreaStay(Collider collision)
	{
		if (collision.tag == "Captain" && collision.gameObject != PikminManager.instance.gameObject) /* touched by whistle or captain = rejoin group */
		{
			CallTransition(State.IN_SQUAD, this);
		}
	}
}