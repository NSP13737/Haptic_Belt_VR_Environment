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
