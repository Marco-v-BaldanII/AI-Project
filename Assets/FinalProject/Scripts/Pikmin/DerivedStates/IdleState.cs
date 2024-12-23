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
    
	public  void IState.OnAreaExit(Collider collision){}
	
	public  void IState.OnBodyEnter(Collider collison){}
	public  void IState.OnBodyStay(Collider collison) {}
    
	public void IState.OnAreaEnter(Collider collision)
	{
		CheckIfGrabObject(collision);
	}

	public void IState.OnAreaStay(Collider collision)
	{
		CheckIfGrabObject(collision);
	}
	
	private void CheckIfGrabObject(Collider collision)
	{
		if (collision.tag == "Pellet")
		{
			myPikmin.targetObject = collision.gameObject;
			CallTransition(StateType.GRAB, this);
		}
	}
	¨
	public void IState.CallTransition(StateType new_state_type, IState prev_){}
}
