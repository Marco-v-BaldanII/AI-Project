using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cmaera : MonoBehaviour
{

    public GameObject target;
    public GameObject cameraPos ;


    private Vector3 offset;

    bool rotating = false;

    Quaternion bufferRotation;

    // Start is called before the first frame update
    void Start()
    {
        bufferRotation = transform.rotation;
        offset = transform.position - target.transform.position;

    }

    // Update is called once per frame
    void Update()
    {

        transform.position = Vector3.Lerp(transform.position, target.transform.position + offset, 5 * Time.deltaTime);



        if (Input.GetKeyDown(KeyCode.Space))
        {
            MoveCameraBehind();
        }

    }

    private void MoveCameraBehind()
    {


        transform.position = cameraPos.transform.position;

        Quaternion desiredRotation = Quaternion.LookRotation(target.transform.position, transform.up);

        //transform.forward = target.transform.forward;

        //transform.rotation = bufferRotation;

        transform.forward = ( target.transform.position - transform.position ).normalized;
        transform.eulerAngles = new Vector3(22.437f, transform.eulerAngles.y, transform.eulerAngles.z);

        offset =  transform.position - target.transform.position ; /* Update offset */


    }

    public Space GetCoordinateSystem()
    {
        return Space.Self;
    }

}
