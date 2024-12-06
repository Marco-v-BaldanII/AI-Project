using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BBUnity.Actions
{
    /// <summary>
    /// wander.
    /// </summary>
    [Action("GameObject/BB_Wander")]
    //[Help("Rotates the transform so the forward vector of the game object points at target's current position")]
    public class BB_Wander : GOAction
    {
        ///<value>Input Target game object Parameter.</value>
        [InParam("target")]
        [Help("Target game object")]


        public float wanderRadius = 3;
        public float wanderDistance = 1;
        public float wanderJitter;

        public float movementSpeed = 2;

        [InParam("rigid")]
        private Rigidbody rigid;

        [InParam("wander_shpere")]
        public GameObject wander_shpere;

        [InParam("transform")]
        public Transform transform;

        public float angle = 0;

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
            Move();
            return TaskStatus.RUNNING;

            //if (target == null)
            //    return TaskStatus.FAILED;
            //Vector3 lookPos = targetTransform.position;
            //gameObject.transform.LookAt(lookPos);
            //return TaskStatus.COMPLETED;
        }

        public void Move() /* Custom wander method according to Craig Reynold's vehicle model */
        {

            float angleOffset = Random.Range(-0.10f, 0.10f);

            Vector3 projection = new Vector3();

            projection.x = Mathf.Cos(angle);
            projection.z = Mathf.Sin(angle);
            // these unit circle values work for the origin (0,0)

            projection *= wanderRadius;

            angle += angleOffset;

            // Adding the sphere's pos shifts the vector to start at it
            Vector3 endPoint = wander_shpere.transform.position + projection;

            Debug.DrawLine(wander_shpere.transform.position, endPoint, Color.red);

            var speed = (endPoint - transform.position).normalized * movementSpeed;
            rigid.velocity = new Vector3(speed.x, rigid.velocity.y, speed.z);

            Quaternion targetRotation = Quaternion.LookRotation(rigid.velocity, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 50 * Time.deltaTime);
        }
    }
}
