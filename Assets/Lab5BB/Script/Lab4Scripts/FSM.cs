using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FSM : MonoBehaviour
{

    private WaitForSeconds wait = new WaitForSeconds( 1.0f / 20.0f);

    public delegate IEnumerator State(); /* Define a type of delegate for a ienumerator with no parameters */
    [SerializeField] private State current_state;

    public FSM_State wander_component;
    public FSM_State thief_component;
    public FSM_State hide_component;
    public FSM_State stay_component;

    public Police LaPolice;

    public bool has_pellet = false;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        wander_component = GetComponent<Wander>();
        thief_component = GetComponent<Thief>();
        hide_component = GetComponent<Hide>();
        stay_component = GetComponent<Stay>();

        current_state = Wander; // Wander is assigned to the current_state

        while (enabled)
        {
            yield return StartCoroutine( current_state());

            if (!has_pellet)
            {

                if (LaPolice.sees_pellet == false) /* If the "police" isnt looking at the treasure, go steal it */
                {
                    if (current_state != Thievery)
                    {
                        OnExit();
                    }
                    ChangeState(Thievery);
                }
                else
                {
                    if (current_state != Wander)
                    {
                        OnExit();
                    }
                    ChangeState(Wander);
                }
            }


        }
        yield return null;
    }
    public void ChangeState(State newState)
    {
        // Exit previous state
        if (current_state != null)
        {
            OnExit();
        }

        current_state = newState;

        // Enter new state
        OnEnter();

    }

    private void OnExit()
    {
        if (current_state == Wander)
        {
            wander_component?.OnExit();
        }
        else if (current_state == Thievery)
        {
            thief_component?.OnExit();
        }
        else if (current_state == Stay)
        {
            stay_component?.OnExit();
        }
    }

    private void OnEnter()
    {
        if (current_state == Wander)
        {
            wander_component?.OnEnter();
        }
        else if (current_state == Thievery)
        {
            thief_component?.OnEnter();
        }
        else if (current_state == Hide)
        {
            hide_component?.OnEnter();
        }
    }

    private IEnumerator Wander()
    {
            wander_component?.Move();

            yield return wait;

    }

    public IEnumerator Thievery()
    {
        thief_component?.Move();

        yield return null;
    }

    public IEnumerator Stay()
    {

        stay_component?.Move(); 

        yield return null;
    }

    public IEnumerator Hide()
    {
        hide_component?.Move();

        yield return null;
    }

}
