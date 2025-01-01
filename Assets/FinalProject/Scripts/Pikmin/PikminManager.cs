using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;


public class PikminManager : MonoBehaviour
{
    public static PikminManager instance;

    public GameObject[] PikminPrefs;
    private Rigidbody rb;
    public GameObject floor;
    public GameObject[] followPositions;
    public GameObject forwardObject;
    public int numPikmin;
    public float speedMultiplier; /* make pikmin go slower when i'm not walking */
    public  List<Pikmin> units;

	[SerializeField]private Whistle pikminCursor;

    [Header("Boid Wights")]

    public int wSeparation = 10;
    public int wAlignment = 10;
    public int wCohesion = 10;
    public int wCaptain = 10;

    private Animator animator;

	public int pikminInSquad = 0;
	
	
	private TypeAmount yellowAmount = new TypeAmount();
	private TypeAmount purpleAmount = new TypeAmount();
    private TypeAmount whiteAmount = new TypeAmount();

    // Start is called before the first frame update
    void Start()
    {

        animator = GetComponentInChildren<Animator>();
        units = new List<Pikmin>();

        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;


        Spawn(numPikmin);

    }


    void OnDrawGizmos()
    {
        // Draw a blue line representing the forward direction in world space
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 5);
    }

    public Tuple<int,int,int> ReturnWeights() /* Usef for the pikmins fetching the flocking weights */
    {
        Tuple<int, int, int> tuple = new Tuple<int,int,int>(wAlignment, wCohesion, wSeparation);

        return tuple;

    }

    public GameObject GetFollowPosition() /* The pikmins follow at random one of theese positions */
    {
        return followPositions[UnityEngine.Random.Range(0,2)];
    }

    public float GetSpeedMultipliyer()
    {
        return speedMultiplier;
    }
    
	public Vector3 GetCursorPosition()
	{
		return pikminCursor.transform.position;
	}
	
	public void DestroyAll()
	{
		for(int i = 0; i < units.Count; ++i)
		{
			Destroy(units[i].gameObject);
		}
		
		units.Clear();
		
	}
	
	// specify position else, spawned around captain character
	public void Spawn(int amount)
	{
		
		for (int i = 0; i < amount; ++i)
		{
			Vector3 random_vec = new Vector3(UnityEngine.Random.Range(10, -10), 0, UnityEngine.Random.Range(5, -5));

			units.Add ( Instantiate(
				PikminPrefs[ UnityEngine.Random.Range(0, PikminPrefs.Length) ],
				transform.position + random_vec, Quaternion.identity, floor.transform)
				.GetComponent<Pikmin>() );
		        
			Debug.Log("pikmin spawned");
			
			switch (units[i].myColor)
			{
			case PikColor.PURPLE:
			
					units[i].brothersAmount = purpleAmount;
				break;
			case PikColor.YELLOW:
                    units[i].brothersAmount = yellowAmount;
                    break;
			case PikColor.WHITE:
                    units[i].brothersAmount = whiteAmount;
                    break;
			}
			
		}
	}
	
	
	public void Spawn(int amount, Vector3 position)
	{
		
		for (int i = 0; i < amount; ++i)
		{
			Vector3 random_vec = new Vector3(UnityEngine.Random.Range(10, -10), 0, UnityEngine.Random.Range(5, -5));

			units.Add ( Instantiate(
				PikminPrefs[ UnityEngine.Random.Range(0, PikminPrefs.Length) ],
			position + random_vec, Quaternion.identity, floor.transform)
				.GetComponent<Pikmin>() );
		        
			Debug.Log("pikmin spawned");
		}
	}
	
	public void ThrowAt(Vector3 position)
	{
		float x_offset = UnityEngine.Random.Range(1.5f,3f);
		float z_offset = UnityEngine.Random.Range(1.5f, 3f);
		
		int negative = UnityEngine.Random.Range(0,1);
		int negative2 = UnityEngine.Random.Range(0,1);
		
		if(negative == 0){x_offset *= -1;}
		if(negative2 == 0){z_offset*=-1;}
		
		position.x += x_offset; position.z += z_offset;
		
		pikminCursor.transform.position = position;
		
		pikminCursor.Throw();
		
	}
	
	public TypeAmount GetTypeAmount(PikColor color)
	{
		switch (color)
		{
		case PikColor.PURPLE:
			return purpleAmount;
		case PikColor.WHITE:
			return whiteAmount;
		case PikColor.YELLOW:
			return yellowAmount;
		}
		
		return null;
	}


}
