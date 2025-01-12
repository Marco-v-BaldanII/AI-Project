using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;
using UnityEngine.AI;

[AddComponentMenu("")]
[MBTNode(name = "Notify")]
public class MBT_NotifyPartners : Leaf
{
    public Blackboard myBoard;

    public Blackboard[] boards;

    public float PursuitDistance = 1000f;

    // Start is called before the first frame update
    public override void OnEnter()
    {
        Transform target = myBoard.GetVariable<TransformVariable>("target").Value;
        PursuitDistance = 100f;

        for (int i = 0; i < boards.Length; ++i) {
            if (boards[i] != null)
            {

                TransformVariable tV = boards[i].GetVariable<TransformVariable>("target");
                GameObject dumple = boards[i].gameObject;
                if (Vector3.Distance(dumple.transform.position, target.position) < PursuitDistance)
                {
                    //Notify other dumples
                    tV.Value = target;
                }
            }
            
        }
    }


    public override NodeResult Execute()
    {




        
        return NodeResult.failure;
    }
}
