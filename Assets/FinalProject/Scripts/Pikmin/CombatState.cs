using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using MBT;

public class CombatState : IState
{
	public Pikmin myPikmin;
	private MonoBehaviourTree tree;
	private Blackboard board;

	
	public CombatState(Pikmin pikmin)
	{
		myPikmin = pikmin;
		tree = myPikmin.GetComponent<MonoBehaviourTree>();
		board = myPikmin.GetComponent<Blackboard>();
	}


    public override void Enter() 
	{
		TransformVariable trs = board.GetVariable<TransformVariable>("target");
		trs.Value = myPikmin.targetObject.transform;
	}

    public override void Exit()
	{
	}


    public override void Process()
	{

	}

	public override void PhysicsProcess()
	{
		tree.Tick();
	}
	
	public override void OnAreaStay(Collider collision)
	{
		if (collision.tag == "Captain") /* touched by whistle or captain = rejoin group */
		{
			CallTransition(State.IN_SQUAD, this);
		}
		if(collision.tag == "Pellet")
		{
			CallTransition(State.IDLE, this);
		}
	}
	

}