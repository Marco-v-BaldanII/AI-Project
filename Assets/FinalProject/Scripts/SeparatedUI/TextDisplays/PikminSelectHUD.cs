using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PikminSelectHUD : MonoBehaviour
{
    private List<PikminInfoContainer> pikIcons;
    private List<Vector3>  iconPositions;

    private Vector3 smolSize;
    private Vector3 beegSize;

    private int index = 0;
    void Awake()
    {
        PikminInfoContainer[] piks = GetComponentsInChildren<PikminInfoContainer>();
        pikIcons = new List<PikminInfoContainer>();
        iconPositions = new List<Vector3>();
        for(int i = 0; i < piks.Length; ++i)
        {
            pikIcons.Add(piks[i]);
            iconPositions.Add(piks[i].transform.position);
            pikIcons[i].index = i;
        }
        smolSize = new Vector3(1, 1, 1);
        beegSize = new Vector3(1.5f, 1.5f, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            index++;

            foreach (PikminInfoContainer pik in pikIcons)
            {
                pik.index++;
                pik.transform.DOMove(iconPositions[pik.index % pikIcons.Count] , 0.4f);


                if (pik.index % pikIcons.Count == 1)
                {
                    pik.transform.DOScale(beegSize, 0.4f);
                }
                else
                {
                    pik.transform.DOScale(smolSize, 0.4f);
                }
            }


        }
    }
}
