using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateType
{
    GRAB,
    IDLE,
    COMBAT,
    CARRYING,
    IN_SQUAD,
    THROWN
}
// Rework this into a base state interface AND a NonML state base class
// then the stateMch will be able to store all of them through the interface,
// if they "is" Agent it will treat them in a different manner
public class CoreState 
{
	
	public delegate void TransitionEvent(StateType new_state_type, IState prev_state);

	public event TransitionEvent Transition;

	// Events in C# can only be called from the class that owns them, other classes can only subscribe or unsubscribe
	public virtual void CallTransition(StateType new_state_type, IState prev_) { Transition.Invoke(new_state_type, prev_); }
	
	public virtual void Enter() {}

	public virtual void Exit() {}

	public virtual void Process() {}

	public virtual void PhysicsProcess() {}
	
	public virtual void OnAreaEnter(Collider collision){}
	public virtual void OnAreaStay(Collider collision){}
	public virtual void OnAreaExit(Collider collision){}
	
	// vale Marco , cuando vuelvas, que throw overridee OnBodyEnter, y que al chocar contra el suelo transicione a idle
	public virtual void OnBodyEnter(Collider collison){}
	public virtual void OnBodyStay(Collider collison) {}

}
