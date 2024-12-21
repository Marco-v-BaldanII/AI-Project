using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Olimar : MonoBehaviour
{
    public Camera camera;
    public int speed = 20;
    public int rotateSpeed = 250;

    private PikminManager pik_manager;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        pik_manager = GetComponent<PikminManager>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
	{
        Vector2 input;
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");


        Vector3 cameraForward = camera.transform.forward;
        Vector3 cameraRight = camera.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;


        cameraForward.Normalize();
        cameraRight.Normalize();


        Vector3 direction = new Vector3(input.x, 0, input.y).normalized;

        transform.Translate(direction * speed * Time.deltaTime, camera.transform);

        direction = (cameraForward * input.y + cameraRight * input.x).normalized;



        // Debug.DrawLine(transform.position, transform.position + transform.forward  * 10);

        if (input != Vector2.zero)
        {
            pik_manager.speedMultiplier = 1;
            animator.SetBool("walking", true);

            Quaternion targetRotation = Quaternion.LookRotation(direction * speed, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);

        }
        else
        {
            pik_manager.speedMultiplier = 0.5f;
            animator.SetBool("walking", false);
        }
    }
}
