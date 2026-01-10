using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class Training_EggBasketManager : EggBasketManager
{

    private Vector3 originalStudyDoneUIPos;

    protected override void Awake() //set pos of training done UI since we want it in fixed location
    {
        originalStudyDoneUIPos = studyDoneUI.transform.position;
        
        base.Awake();

    }

    protected override void InitializeEggs() // we want diff egg spawns for training scene
    {
        List<Vector3> eggPositions = new List<Vector3>
        {
            new Vector3(0.645f, 0.2f, 3.245f),
            new Vector3(3.973f, 0.2f, 0.2567f),
            new Vector3(0.4777f, 0.2f, 0.4034f),
            new Vector3(4.0678f, 0.2f, 5.68f),
            new Vector3(0.821f, 0.2f, 5.177f),
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

    }


}
