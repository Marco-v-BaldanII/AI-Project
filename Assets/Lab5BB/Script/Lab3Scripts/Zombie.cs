using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public Camera frustum;
    public LayerMask mask;


    private Rigidbody rigid;

    public ZombieSpawner blackboard;

    public Wander wander_component;


    private float slowDownRadius = 3.0f;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        frustum.enabled = false;
    }

    void Update()
    {
        bool detect = false;

        Collider[] colliders = Physics.OverlapSphere(transform.position, frustum.farClipPlane, mask);
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(frustum);
        foreach (Collider col in colliders)
        {
            if (col.gameObject != gameObject && GeometryUtility.TestPlanesAABB(planes, col.bounds))
            {
                RaycastHit hit;
                Ray ray = new Ray();
                ray.origin = transform.position;
                ray.direction = (col.transform.position - transform.position).normalized;
                ray.origin = ray.GetPoint(frustum.nearClipPlane);




                if (Physics.Raycast(ray, out hit, frustum.farClipPlane, mask))
                {
                    if (hit.collider.gameObject.CompareTag("Captain")) 
                    {
                        detect = true;
                        // Seek
                        Arrive(hit.collider.transform.position);
                        

                        Debug.DrawLine(ray.origin, hit.collider.transform.position);
                        

                        StartCoroutine("FaceTarget");

                        // notify all other zombies
                        blackboard?.CallHorde();

                    }

                }
            }

        }

        if (!detect)
        {
            wander_component.Move();
        }

    }

    bool rotating = false;

    public void DetectCaptain(GameObject player)
    {
        StartCoroutine("FaceTarget");
    }

    private IEnumerator FaceTarget() /* Slowly turn to face target */
    {   if (!rotating)
        {

            rotating = true;
            for (int i = 0; i < 50; ++i)
            {

                Quaternion targetRotation = Quaternion.LookRotation(( PikminManager.instance.transform.position - transform.position), Vector3.up);

                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 400 * Time.deltaTime);
                yield return null;
            }
            rotating = false;
        }
        yield return null;
    }

    private void Arrive(Vector3 colliderPos)
    {
        Vector3 direction = (colliderPos - transform.position).normalized;


        rigid.velocity = new Vector3(direction.x, Mathf.Clamp(rigid.velocity.y, -2.0f, 0.01f), direction.z) * 5;

        // ARRIVE
        var distance = Vector3.Distance(colliderPos, transform.position);

        if (distance < slowDownRadius) /*Slow down when get closer than "slowDownRadius" */
        {
            rigid.velocity *= distance / slowDownRadius;
            if (distance < slowDownRadius / 2)
            {
                rigid.velocity = Vector3.zero;
            }
        }
    }

}
