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
	
	
	void IState.Enter()
	{

	}

	void IState.Exit()
	{

	}

	void IState.Process()
	{

	}

	void IState.PhysicsProcess()
	{

	}
    
	public  void IState.OnAreaEnter(Collider collision){}
	public  void IState.OnAreaStay(Collider collision){}
	public  void IState.OnAreaExit(Collider collision){}
	
	public  void IState.OnBodyEnter(Collider collison){}
	public  void IState.OnBodyStay(Collider collison) {}
	
	public  void CallTransition(StateType new_state_type, IState prev_){}
	
	
}