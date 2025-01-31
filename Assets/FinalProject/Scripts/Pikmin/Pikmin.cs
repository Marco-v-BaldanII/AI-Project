﻿using DG.Tweening;
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
	
	public float debugSpeed = 0;
	
	[Header("Grabing State fields")]
	
	public GameObject targetObject;
	
	public TypeAmount brothersAmount; // the amount of other pikmin from my same type

    public bool grabbing = false;

    public AudioClip throwSfx;
    public AudioClip heySfx;
    public AudioSource source;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();

    }

    private void Start()
    {
        if(PikminManager.instance) followPos = PikminManager.instance.GetFollowPosition();
    }

    private void Update()
    {

        //((rigid.velocity = Vector3.right * 20;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyBite")
        {
            // Death
            rigid.velocity = Vector3.zero; /* connect the destroyu method to the OnComplete via a lmba function */
            transform.DOMove(other.gameObject.transform.position + new Vector3(0,0.4f,0), 0.5f).OnComplete((/*no parameters*/) => Destroy(this.gameObject));
            transform.DORotate(transform.eulerAngles + new Vector3(115, 0, 0), 0.48f);
            transform.DOScale(transform.localScale * 0.3f, 0.48f);
        }
    }

    private void OnDestroy()
    {
        if (PikminManager.instance && PikminManager.instance.units.Contains(this))
        {
            PikminManager.instance.units.Remove(this);


        }
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
	    ROCK,
	    NONE
    }


public class TypeAmount
{
	public TypeAmount(){amount = 0;}
	
	public int amount = 0;
    
	public static TypeAmount operator ++(TypeAmount obj)
	{
		obj.amount++;
		return obj;
	}

	public static TypeAmount operator --(TypeAmount obj)
	{
		obj.amount--;
		return obj;
	}


	public override string ToString()
	{
		return amount.ToString();
	}
}