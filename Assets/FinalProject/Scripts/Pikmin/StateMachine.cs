using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{

    [SerializeField] StateType[] stateTypes;

	
	private Dictionary<StateType, IState> states;
	
	public StateType startingState ;
	
	public IState currentState;
    
	public Pikmin pikmin;
	
	[SerializeField] StateType checkState;

    // Start is called before the first frame update
    void Start()
    {
		states = new Dictionary<StateType, IState>();

        foreach (var state in stateTypes)
        {
        	IState new_state = null;
        	
            switch (state)
            {
            case StateType.GRAB:
            	new_state = new DeprecatedGrabState(pikmin);
            	break;
            case StateType.IDLE:
	            new_state = new IdleState(pikmin);
	            break;
            case StateType.IN_SQUAD:
	            new_state = new InSquadState(pikmin) ;
	            break;
            case StateType.THROWN:
	            new_state = new ThrownState(pikmin);
	            break;
            case StateType.CARRYING:
	            new_state = new CarryingState(pikmin);
	            break;
	            
            	
            }
	        // each state should subscribe to the OnChild Transition
	        // therefore when Transitioned is meitted by any state, the exit() and enter() occur automatically
	        new_state.Transition += OnChildTransitionEvent;
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
	
	// Call sparingly from non-state classes , be careful
	public void OnChildTransitionEvent(StateType new_state_type )
	{
		if (new_state_type == StateType.GRAB)
		{
			
		}
		currentState.Exit();
		states[new_state_type]?.Enter();
		checkState = new_state_type;
		currentState = states[new_state_type];
		
	}
	
	
	private void OnChildTransitionEvent(StateType new_state_type, IState prev_state )
	{
		if (currentState != prev_state) {return;}

		prev_state.Exit();
		states[new_state_type]?.Enter();
		checkState = new_state_type;
		currentState = states[new_state_type];
		
	}

    private void OnTriggerEnter(Collider collision)
    {
        currentState?.OnAreaEnter(collision);
    }

    private void OnTriggerExit(Collider collision)
    {
        currentState?.OnAreaExit(collision);
    }

    private void OnTriggerStay(Collider collision)
    {
        currentState?.OnAreaStay(collision);
    }

    private void OnCollisionEnter(Collision collision)
    {
		currentState?.OnBodyEnter(collision.collider);
    }
    private void OnCollisionStay(Collision collision)
    {
        currentState?.OnBodyStay(collision.collider);
    }

    public StateType GetCurrentState()
	{
		foreach (var entry in states)
		{
			if (entry.Value == currentState)
			{
				return entry.Key;
			}
		}
		
		return StateType.IDLE;
	}

}
