using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace BBUnity.Actions
{
    /// <summary>
    /// thief.
    /// </summary>
    [Action("GameObject/BB_Thievery")]
    //[Help("Rotates the transform so the forward vector of the game object points at target's current position")]
    public class BB_Thievery : GOAction
    {
        ///<value>Input Target game object Parameter.</value>
        [InParam("executor")]
        //[Help("Target position where the game object will be moved")]
        public BehaviorExecutor executor;

        [InParam("LaPolice")]
        public Police LaPolice;

        [InParam("rigid")]
        public Rigidbody rigid;

        [InParam("thief_target")]
        public GameObject thief_target;

        [InParam("transform")]
        public Transform transform;

        [InParam("speed")]
        public float speed = 3;

        [InParam("grab_distance")]
        public float grab_distance = 3;


        /// <summary>Initialization Method of LookAt.</summary>
        /// <remarks>Check if you have an objective gameObject associated with it.</remarks>
        public override void OnStart()
        {

            

            return;
        }

        /// <summary>Method of Update of LookAt.</summary>
        /// <remarks>In each Update Check the position of the target GameObject and rotate the vector where it points, the task fails
        /// if you have no objective Gameobject associated with it.</remarks>
        public override TaskStatus OnUpdate()
        {

            return Move();

            //if (target == null)
            //    return TaskStatus.FAILED;
            //Vector3 lookPos = targetTransform.position;
            //gameObject.transform.LookAt(lookPos);
            //return TaskStatus.COMPLETED;
        }

        public TaskStatus Move() /* Custom seek method according to Craig Reynold's vehicle model */
        {

            Vector3 direction = thief_target.transform.position - transform.position;

            Vector3 desired_velocity = Vector3.Normalize(direction) * speed;

            Vector3 steeringForce = desired_velocity - rigid.velocity;

            rigid.velocity += steeringForce * 50 * Time.deltaTime; /* Create smooth turn towards next destination */
            rigid.velocity = Vector3.ClampMagnitude(rigid.velocity, 20);

            Quaternion targetRotation = Quaternion.LookRotation(rigid.velocity, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 100 * Time.deltaTime);

            if (Vector3.Distance(transform.position, thief_target.transform.position) < grab_distance)
            {
                GrabPellet();
                return TaskStatus.RUNNING;
            }

            return TaskStatus.RUNNING;

            Debug.Log(Vector3.Distance(transform.position, thief_target.transform.position));



        }

        public void GrabPellet()
        {
            thief_target.transform.SetParent(transform);
            // Change blackboard
            executor.blackboard.boolParams[0] = true; // Sets hasPellet to true
        }

    }
}
