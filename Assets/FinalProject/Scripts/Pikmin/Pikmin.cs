using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pikmin : MonoBehaviour
{
	[Header("SquadState fields fields")]
	
	public float speed = 200;
    private float minSpeed = 1.5f * 3;
    private float maxSpedd = 3.0f * 3;
	public Rigidbody rigid;
	public Animator animator;
    public PikColor myColor;
	public GameObject followPos;
	
	
	[Header("Grabing State fields")]
	
	public GameObject targetObject;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        

    }

    private void Start()
    {
        followPos = PikminManager.instance.GetFollowPosition();
    }

    private void Update()
    {
        //((rigid.velocity = Vector3.right * 20;
    }


}

public enum PikColor
    {
	    RED,
	    YELLOW,
	    BLUE,
	    PURPLE,
	    WHITE,
	    PINK,
	    ROCK
    }
