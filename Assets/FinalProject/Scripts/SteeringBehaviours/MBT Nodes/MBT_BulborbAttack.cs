using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using MBT;

[AddComponentMenu("")]
[MBTNode(name = "Attack")]
public class MBT_BulborbAttack : Leaf
{
	
	private Animator animator;
	public string attck_trigger;
	private Blackboard board;
	
	public TransformVariable target;
	

    // Start is called before the first frame update
	void Awake()
    {
	    animator = GetComponent<Animator>();
	    board = GetComponent<Blackboard>();
    }

	public override void  OnEnter()
	{
		animator.SetTrigger(attck_trigger);
		target = board.GetVariable<TransformVariable>("target");
	}

	public override NodeResult Execute()
	{
		
		
		if (target != null && target.Value != null)
		{
			Quaternion targetRotation = Quaternion.LookRotation(target.Value.position - transform.position);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 8f);
		}
    	
		AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

		// Check if the animation is complete
		if (stateInfo.IsName(attck_trigger) && stateInfo.normalizedTime >= 0.95f)
		{
			Debug.Log("Animation Finished!");
			
			return NodeResult.success;
		}
        if (! stateInfo.IsName(attck_trigger))
		{
			animator.SetTrigger(attck_trigger);	
		}

        return NodeResult.running;
    }
}
