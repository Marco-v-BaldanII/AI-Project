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

    public override void OnBodyStay(Collider collison)
    {
		CheckIfGrabObject(collison);
    }

    private void CheckIfGrabObject(Collider collision)
	{
		if (collision.tag == "Pellet")
		{
			myPikmin.targetObject = collision.gameObject;
			
			//GrabAgent agent = myPikmin.GetComponent<GrabAgent>();
			//agent.Target = collision.gameObject.transform.position;
			
			EpisodeManager.Instance.pellet = collision.gameObject;
			
			CallTransition(State.GRAB, this);
		}
		else if (collision.tag == "Enemy")
		{
			myPikmin.targetObject = collision.gameObject;
			CallTransition(State.COMBAT, this);
		}
		else if (collision.tag == "Captain") /* touched by whistle or captain = rejoin group */
		{
			CallTransition(State.IN_SQUAD, this);
		}
	}

}
