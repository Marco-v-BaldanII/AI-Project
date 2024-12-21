using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    GRAB,
    IDLE,
    COMBAT,
    CARRYING,
    IN_SQUAD,
    THROWN
}

public class IState 
{
	
	public virtual void Enter() {}

	public virtual void Exit() {}

	public virtual void Process() {}

	public virtual void PhysicsProcess() {}
}
