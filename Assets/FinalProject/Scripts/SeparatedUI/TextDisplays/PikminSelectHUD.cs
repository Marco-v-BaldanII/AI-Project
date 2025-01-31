﻿using DG.Tweening;
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

    public PikColor currentColor { get; private set; }

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

    private void Start()
    {
        foreach (PikminInfoContainer pik in pikIcons)
        {


            if (pik.index % pikIcons.Count == 1)
            {
                pik.showText = true;
                pik.label.enabled = true;
                pik.transform.DOScale(beegSize, 0.4f);
                currentColor = pik.color;
                PikminManager.instance.selectedColor = currentColor;
            }
            else
            {
                pik.label.enabled = false;
                pik.showText = false;
                pik.transform.DOScale(smolSize, 0.4f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
	    if (Input.GetMouseButtonDown(2))
        {
            index++;

            foreach (PikminInfoContainer pik in pikIcons)
            {
                pik.index++;
                pik.transform.DOMove(iconPositions[pik.index % pikIcons.Count] , 0.4f);


                if (pik.index % pikIcons.Count == 1)
                {
                	pik.showText = true;
                	pik.label.enabled = true;
                    pik.transform.DOScale(beegSize, 0.4f);
	                currentColor = pik.color;
	                PikminManager.instance.selectedColor = currentColor;
                }
                else
                {
                	pik.label.enabled = false;
                	pik.showText = false;
                    pik.transform.DOScale(smolSize, 0.4f);
                }
            }


        }
    }
}
