using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stay : MonoBehaviour, FSM_State
{


    public Rigidbody rigid;

    public GameObject target;

    public float speed = 3;

    public float grab_distance = 3;

    private FSM state_machine;

    // Start is called before the first frame update
    void Start()
    {
        state_machine = GetComponent<FSM>();
        rigid = GetComponent<Rigidbody>();
    }

    public void OnEnter()
    {

    }

    public void Move() /* Custom seek method according to Craig Reynold's vehicle model */
    {

        
        rigid.velocity = Vector3.zero;  

    }



    public void OnExit()
    {

    }

}
