using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Threading.Tasks;
using UnityEngine;

public class InSquadState : IState
{
	public Pikmin myPikmin;
	private Transform transform;
	private float speed = 200;

    private float minSpeed = 1.5f * 3;
    private float maxSpedd = 3.0f * 3;

    private Rigidbody rigid;
    private Animator animator;
    public enum PikColor
    {
        RED,
        YELLOW,
        BLUE,
        PURPLE,
        WHITE,
        PINK,
        ROCK
    }

    public PikColor myColor;

	private GameObject followPos;
	
	public InSquadState(Pikmin pikmin)
	{
		myPikmin = pikmin;
		transform = myPikmin.transform;
	}

	public void IState.Enter()
    {
	    rigid = myPikmin.rigid;
	    animator = myPikmin.animator;

	    //PikminManager.instance.units.Add(this);

	    RandomizeSpeed();
	    // maybe change to use a async method for this
	    //InvokeRepeating("RandomizeSpeed", 0, UnityEngine.Random.Range(1.8f, 2.5f));

        //InvokeRepeating("Movement", 0, UnityEngine.Random.Range(0.5f, 0.1f));

        

    }
    

    Vector3 vCaptain = Vector3.zero;
 
	public void IState.PhysicsProcess()
	{
		if (!myPikmin.followPos){ myPikmin.followPos = PikminManager.instance.GetFollowPosition();}

        Vector3 vCohesion = Vector3.zero;
        Vector3 vSeparation = Vector3.zero;
        float vAlingment = speed;

        var speedMultipliyer = PikminManager.instance.GetSpeedMultipliyer();

        if (Vector3.Distance(transform.position, PikminManager.instance.transform.position) > 2 + PikminManager.instance.numPikmin / 10)
        {
	        vCaptain = myPikmin.followPos.transform.position - transform.position;
            speedMultipliyer = 1;
        }
        else if (UnityEngine.Random.Range(0, 100) < 1)
        {
            Debug.Log("changin offset");
	        vCaptain = myPikmin.followPos.transform.position - transform.position;
            Vector3 captainOffset = new Vector3(UnityEngine.Random.Range(-0.1f, 0.1f), 0, UnityEngine.Random.Range(-0.1f, 0.1f));
            vCaptain += captainOffset;
            speed *= 0.4f;
        }

        foreach (Pikmin carrot in PikminManager.instance.units) /* Sumatorio */
        {
            if (carrot != myPikmin && carrot != null)
            {

                vCohesion += carrot.transform.position - transform.position;
                float divisor = Vector3.Distance(transform.position, carrot.transform.position) + 0.1f;
                vSeparation += (transform.position - carrot.transform.position) / divisor;

                vAlingment += carrot.speed;
            }
        }

        vCohesion /= PikminManager.instance.numPikmin;
        vAlingment /= PikminManager.instance.numPikmin;
        vSeparation /= PikminManager.instance.numPikmin;

        Tuple<int, int, int> weights = PikminManager.instance.ReturnWeights();

        // Calculation of the speed taking all the weights into account
        Vector3 vFlocking = ((weights.Item2 * vCohesion) + (weights.Item3 * vSeparation)) + (vCaptain * PikminManager.instance.wCaptain) * (weights.Item1 * vAlingment);


        vFlocking = vFlocking.normalized * speed * speedMultipliyer;


		myPikmin.rigid.velocity = new Vector3( vFlocking.x,  myPikmin.rigid.velocity.y, vFlocking.z);

		myPikmin.animator.SetFloat("velocity", Mathf.Abs(myPikmin.rigid.velocity.x) + Mathf.Abs(myPikmin.rigid.velocity.z) / 2.0f);



        transform.LookAt(PikminManager.instance.transform.position);
    }

    

	public async Task RandomizeSpeed()
    {
        // Gets called at different intervals for each pikmin
	    speed = UnityEngine.Random.Range(minSpeed, maxSpedd) * GetColorSpeedModifier() ;
	    await Task.Delay((int) (UnityEngine.Random.Range(1.8f, 2.5f) * 1000f));
    }


	/* TODO separate this into a different script */
    private float GetColorSpeedModifier()
    {
        // more pikmin to be discovered in the future
        switch (myColor)
        {
            case PikColor.RED:
                return 1;
            case PikColor.BLUE:
                return 1;
            case PikColor.YELLOW:
                return 1;
            case PikColor.PURPLE:
                return 0.6f;
            case PikColor.WHITE:
                return 1.4f;
            case PikColor.ROCK:
                return 0.8f;
            case PikColor.PINK:
                return 1.3f;
        }

        return 1;
    }
    

	void IState.Exit()
	{

	}

	void IState.Process()
	{

	}

    
	public  void IState.OnAreaEnter(Collider collision){}
	public  void IState.OnAreaStay(Collider collision){}
	public  void IState.OnAreaExit(Collider collision){}
	
	public  void IState.OnBodyEnter(Collider collison){}
	public  void IState.OnBodyStay(Collider collison) {}

	public  void IState.CallTransition(StateType new_state_type, IState prev_){}
}
