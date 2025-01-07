using MBT;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class TerritorialAreaBinder : MonoBehaviour
{
    public Collider detection_area;
    public Rigidbody rigid;

    public MBTExecutor executor;


    public float avoidanceWeight = 8;
    private Blackboard board;
    private MBT_Wander wanderer;

    float time = 2f;

    public bool isInside = true;

    private bool _switch = false;

    TransformVariable target;

    private CancellationTokenSource _cancellationTokenSource;

    // Start is called before the first frame update
    void Start()
    {
        board = GetComponent<Blackboard>();
        rigid = GetComponentInParent<Rigidbody>();
        executor = GetComponentInParent<MBTExecutor>();
        wanderer = GetComponent<MBT_Wander>();
        target = board.GetVariable<TransformVariable>("target");

        _cancellationTokenSource = new CancellationTokenSource();

    }

    private async void Update()
    {
        if (!isInside && target.Value == null&& !_switch)
        {
            _switch = true;
            await Task.Delay(1500);
            _switch = false;
            if (!isInside && target.Value == null)
            {
                target.Value = detection_area.transform;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Devert positions");
        if (other.tag == "EnemyHome")
        {
            isInside = false;

            //target.Value = detection_area.transform;
        }
    }

    private async void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyHome")
        {
            isInside = true;
            await Task.Delay(1000);
            // Handle the object being destroyed during Delay
            if (this && isInside && target.Value == detection_area.transform)
            {
                target.Value = null;
            }
            else
            {
                return;
            }
        }
    }

}
