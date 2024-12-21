using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatState : IState
{
	public Pikmin myPikmin;
	
	public CombatState(Pikmin pikmin)
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

}