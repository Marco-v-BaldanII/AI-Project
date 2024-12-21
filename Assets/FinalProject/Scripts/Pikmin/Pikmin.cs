using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pikmin : MonoBehaviour
{

	public float speed = 200;

    private float minSpeed = 1.5f * 3;
    private float maxSpedd = 3.0f * 3;

	public Rigidbody rigid;
	public Animator animator;
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

    public PikColor myColor;

	public GameObject followPos;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        

    }

    private void Start()
    {
        followPos = PikminManager.instance.GetFollowPosition();
    }




}
