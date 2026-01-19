using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class Training_EggBasketManager : EggBasketManager
{

    private Vector3 originalStudyDoneUIPos;
    [SerializeField] private GameObject grassPortal;

    protected override void Awake() //set pos of training done UI since we want it in fixed location
    {
        originalStudyDoneUIPos = studyDoneUI.transform.position;
        if (grassPortal != null)
        {
            grassPortal.SetActive(false);
        }
        else
        {
            Debug.LogError("You need to attach the grass portal to the training basket manager");
        }

            base.Awake();

    }

    protected override void InitializeEggs() // we want diff egg spawns for training scene
    {
        List<Vector3> eggPositions = new List<Vector3>
        {
            new Vector3(1.16700006f,0.209000006f,2.63599992f),
            new Vector3(2.52399993f,0.209000006f,2.27900004f),
            new Vector3(2.52399993f,0.209000006f,3.86299992f),
            new Vector3(1.19599998f,0.209000006f,4.50899982f),
            new Vector3(3.61400008f,0.209000006f,4.50899982f),
        };

        for (int i = 0; i < eggPositions.Count; i++)
        {
            EggEntry entry = new EggEntry();
            entry.id = i;
            entry.eggPos = eggPositions[i];
            eggEntries.Add(entry);
        }
    }

    protected override void LoadStudyDoneUI() // we want UI to be in fixed pos, not attached to HMD
    {
        if (studyDoneUI != null)
        {
            studyDoneUI.transform.position = originalStudyDoneUIPos;
        }
        else
        {
            Debug.LogError("There is no study done ui obj assigned to EggBasketManager.");
        }
        if (grassPortal != null)
        {
            grassPortal.SetActive(true);
        }
        else
        {
            Debug.LogError("There is no study grassPortal obj assigned to EggBasketManager.");
        }

    }


}
