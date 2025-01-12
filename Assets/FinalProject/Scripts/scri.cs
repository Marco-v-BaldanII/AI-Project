using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class Police : MonoBehaviour
{
    private Camera frustum;
    public LayerMask mask;

    public bool sees_pellet = false;

    private void Awake()
    {
        frustum = GetComponentInChildren<Camera>();

        frustum.enabled = false;
    }
    private void Update()
    {
        // the thief can check this param to decide when to attempt a theivery 
        sees_pellet = false;


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
                ray.origin = transform.position;

                Debug.DrawRay(ray.origin, ray.direction, Color.yellow);

                if (col.gameObject.CompareTag("Pellet"))
                {
                    sees_pellet = true;
                }

                //if (Physics.Raycast(ray, out hit, 10000, mask))
                //{
                //    if (hit.collider.gameObject.CompareTag("Pellet"))
                //    {
                //        sees_pellet = true;

                //    }

                //}
            }

        }

    }


}
