using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{


    [SerializeField] private GameObject zombiePrefab;
    [SerializeField] private GameObject player;

    public int numZombies = 5;


    // Start is called before the first frame update
    void Start()
    {
        
        for (int i = 0; i < numZombies; ++i)
        {
            Instantiate(zombiePrefab, transform.position , Quaternion.identity , transform);

            var zombie = zombiePrefab.GetComponent<Zombie>();
            if(zombie) zombie.blackboard = this;
        }

    }

    
    public void CallHorde() /* Share the player's position with all zombies */
    {
        BroadcastMessage("DetectCaptain", player, SendMessageOptions.DontRequireReceiver);
    }

    public GameObject GetPlayer() { return player; }

}
