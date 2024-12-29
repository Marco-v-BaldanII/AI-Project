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
	

    // Start is called before the first frame update
	void Awake()
    {
	    animator = GetComponent<Animator>();
    }

	public override void  OnEnter()
	{
		animator.SetTrigger(attck_trigger);
	}

	public override NodeResult Execute()
	{
    	
    	
		AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

		// Check if the animation is complete
		if (stateInfo.IsName(attck_trigger) && stateInfo.normalizedTime >= 0.95f)
		{
			Debug.Log("Animation Finished!");
			
			return NodeResult.success;
		}
    	
	    return NodeResult.running;
    }
}
