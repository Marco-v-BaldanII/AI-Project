using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabState : IState{
	public Pikmin myPikmin;
    public Vector3 velocity;
	public GrabState(Pikmin pikmin)
	{
		myPikmin = pikmin;
	}
	
    public override void Enter() 
    {
	    var direction= myPikmin.targetObject.transform.position - myPikmin.transform.position;
	    direction.Normalize();
	    velocity = direction * 5;
    }

    public override void Exit()
    {

    }

    public override void Process()
    {
        myPikmin.rigid.velocity = velocity;
    }

    public override void PhysicsProcess()
    {

    }

}
