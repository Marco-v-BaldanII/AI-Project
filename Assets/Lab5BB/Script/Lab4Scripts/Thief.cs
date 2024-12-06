using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thief : MonoBehaviour , FSM_State
{

    public Police LaPolice;

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

        Vector3 direction = target.transform.position - transform.position;

        Vector3 desired_velocity = Vector3.Normalize( direction) * speed;

        Vector3 steeringForce = desired_velocity - rigid.velocity;

        rigid.velocity += steeringForce * 50 * Time.deltaTime; /* Create smooth turn towards next destination */
        rigid.velocity = Vector3.ClampMagnitude(rigid.velocity, 20);

        Quaternion targetRotation = Quaternion.LookRotation(rigid.velocity, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 50 * Time.deltaTime);

        if ( Vector3.Distance(transform.position , target.transform.position) < grab_distance)
        {
            GrabPellet();
            state_machine.ChangeState(state_machine.Hide);
        }

        Debug.Log(Vector3.Distance(transform.position, target.transform.position));

    }

    public void GrabPellet()
    {
        target.transform.SetParent(transform);
        state_machine.has_pellet = true;
    }

    public void OnExit()
    {

    }

}
