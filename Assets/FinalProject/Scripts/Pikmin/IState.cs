using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState 
{
	// Events in C# can only be called from the class that owns them, other classes can only subscribe or unsubscribe
	public  void CallTransition(StateType new_state_type, IState prev_);
	
	public  void Enter();

	public  void Exit();

	public  void Process();

	public  void PhysicsProcess();
	
	public  void OnAreaEnter(Collider collision);
	public  void OnAreaStay(Collider collision);
	public  void OnAreaExit(Collider collision);
	
	// vale Marco , cuando vuelvas, que throw overridee OnBodyEnter, y que al chocar contra el suelo transicione a idle
	public  void OnBodyEnter(Collider collison);
	public  void OnBodyStay(Collider collison) ;

}
