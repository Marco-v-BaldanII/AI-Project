using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BBUnity.Actions
{
    /// <summary>
    /// hide.
    /// </summary>
    [Action("GameObject/BB_Hide")]
    //[Help("Rotates the transform so the forward vector of the game object points at target's current position")]
    public class BB_Hide : GOAction
    {

        [InParam("hiding_spots")]
        public HidingSpots hiding_spots;


        public Collider current_spot;


        [InParam("rigid")]
        private Rigidbody rigid;

        [InParam("chaser")]
        public Police chaser;

        [InParam("transform")]
        public Transform transform;

        Vector3 destination;

        float speed = 2;

        /// <summary>Initialization Method of LookAt.</summary>
        /// <remarks>Check if you have an objective gameObject associated with it.</remarks>
        public override void OnStart()
        {
            OnEnter();
            rigid.velocity = Vector3.zero;

            return;
        }

        /// <summary>Method of Update of LookAt.</summary>
        /// <remarks>In each Update Check the position of the target GameObject and rotate the vector where it points, the task fails
        /// if you have no objective Gameobject associated with it.</remarks>
        public override TaskStatus OnUpdate()
        {
            destination = chaser.transform.TransformDirection(current_spot.transform.position - chaser.transform.position);

            /* Calculate the direction from the hiding spot to the chaser */
            Vector3 directionToChaser = chaser.transform.position - current_spot.transform.position;
            Vector3 hidingDirection = directionToChaser.normalized;

            // Calculate the destination behind the hiding spot
            float distanceBehindSpot = 3.0f;
            destination = current_spot.transform.position - hidingDirection * distanceBehindSpot;

            destination.y = current_spot.transform.position.y;
            Move();

            Debug.Log("Hiding");

            if (Vector3.Distance(transform.position, destination) < 2.0f)
            {
                rigid.velocity = Vector3.zero;
            }


            return TaskStatus.RUNNING;

            //if (target == null)
            //    return TaskStatus.FAILED;
            //Vector3 lookPos = targetTransform.position;
            //gameObject.transform.LookAt(lookPos);
            //return TaskStatus.COMPLETED;
        }

        public void OnEnter()
        {

            if (!hiding_spots)
            {
                hiding_spots = GameObject.Find("HidinSpots").GetComponent<HidingSpots>() ;
            }
            if (!chaser)
            {
                chaser = GameObject.Find("PikminManager").GetComponent<Police>();
            }

            float distance = 1000;

            foreach (Collider spot in hiding_spots.colliders)
            {
                float currentDist = Vector3.Distance(transform.position, spot.transform.position);
                if (currentDist < distance)
                {
                    distance = currentDist;
                    current_spot = spot;
                }

            }


            destination = chaser.transform.TransformDirection(current_spot.transform.position - chaser.transform.position);

            /* Calculate the direction from the hiding spot to the chaser */
            Vector3 directionToChaser = chaser.transform.position - current_spot.transform.position;
            Vector3 hidingDirection = directionToChaser.normalized;

            // Calculate the destination behind the hiding spot
            float distanceBehindSpot = 3.0f;
            destination = current_spot.transform.position - hidingDirection * distanceBehindSpot;

            destination.y = current_spot.transform.position.y;



        }
        public void Move() /* Custom seek method according to Craig Reynold's vehicle model */
        {

            Vector3 direction = destination - transform.position;

            Vector3 desired_velocity = Vector3.Normalize(direction) * speed;

            Vector3 steeringForce = desired_velocity - rigid.velocity;

            rigid.velocity += steeringForce * 50 * Time.deltaTime; /* Create smooth turn towards next destination */

            //rigid.velocity += (avoidance_force * Time.deltaTime);

            //rigid.velocity = Vector3.ClampMagnitude(rigid.velocity, 40);

            Quaternion targetRotation = Quaternion.LookRotation(rigid.velocity, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 50 * Time.deltaTime);

            Debug.DrawLine(transform.position, destination);

            if (Vector3.Distance(transform.position, destination) < 1.5f)
            {
                // change some boolean or soemthing to remain in position
            }


            //if (Vector3.Distance(transform.position, target.transform.position) > grab_distance)
            //{
            //    state_machine.ChangeState(state_machine.Thievery);
            //}


        }

        //public void OnExit()
        //{

        //}

        //bool detect_obstacle = false;

        //Vector3 avoidance_force;

        //private void OnTriggerStay(Collider other)
        //{
        //    detect_obstacle = true;

        //    Debug.Log("Devert positions");

        //    Vector3 distance = transform.position - other.transform.position;


        //    float forwardProjection = Vector3.Dot(transform.forward, distance);

        //    avoidance_force = (-transform.right * forwardProjection) * 2;

        //    avoidance_force /= (distance.magnitude * 120);



        //}

        //private void OnTriggerExit(Collider other)
        //{
        //    avoidance_force = Vector3.zero;
        //    detect_obstacle = false;
        //}
    }
}
