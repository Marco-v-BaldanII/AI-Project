using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{

    [SerializeField] State[] stateTypes;

	
	private Dictionary<State, IState> states;
	
	public State startingState ;
	
	private IState currentState;
    
	public Pikmin pikmin;

    // Start is called before the first frame update
    void Start()
    {
		states = new Dictionary<State, IState>();

        foreach (var state in stateTypes)
        {
        	IState new_state = null;
        	
            switch (state)
            {
            case State.GRAB:
            	new_state = new GrabState(pikmin);
            	break;
            case State.IDLE:
	            new_state = new IdleState(pikmin);
	            break;
            case State.IN_SQUAD:
	            new_state = new InSquadState(pikmin) ;
	            break;
	            
            	
            }
            
	        states[state] = new_state;
        }
        
	    if ( states[startingState] != null )
	    {
	    	currentState = states[startingState];
	    	states[startingState].Enter();
	    }
    }

    // Update is called once per frame
    void Update()
    {

	    currentState?.Process();
	    
    }
    
	void FixedUpdate()
	{
		currentState?.PhysicsProcess();
	}
	
	private void OnChildTransitionEvent(State new_state_type, IState prev_state )
	{
		if (currentState != prev_state) {return;}

		prev_state.Exit();
		states[new_state_type]?.Enter();
		currentState = states[new_state_type];
		
	}
}
