using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EggEntry
{
    public int id;
    public Vector3 eggPos;
}
public class EggBasketManager : MonoBehaviour
{

    [SerializeField] protected GameObject eggPrefab;
    [SerializeField] protected GameObject basketObj;
    [SerializeField] protected GameObject studyDoneUI;
    [Header("Optional Params")]
    [SerializeField] protected loggingManager studyLogger; // Nullable for training child
    [SerializeField] protected GameObject playerHeadAnchor; // Nullable for training child

    protected Vector3 originalBasketPos;
    protected List<EggEntry> eggEntries = new List<EggEntry>();


    protected virtual void Awake()
    {
        InitializeEggs();
        HideUIandBasket();
    }

    protected virtual void InitializeEggs()
    {
        List<Vector3> eggPositions = new List<Vector3>
        {
            new Vector3(0.645f, 0.2f, 3.245f),

        };

        for (int i = 0; i < eggPositions.Count; i++)
        {
            EggEntry entry = new EggEntry();
            entry.id = i;
            entry.eggPos = eggPositions[i];
            eggEntries.Add(entry);
        }
    }

    protected virtual void HideUIandBasket()
    {
        studyDoneUI.transform.position = new Vector3(0, 400, 0); // start the done UI up and out of the way until we need it
        
        originalBasketPos = basketObj.transform.position;
        basketObj.transform.position = new Vector3(0, 400, 0);
    }

    public void startEggRoutineFromUI(GameObject uiGO)
    {
        spawnNextEgg();
        basketObj.transform.position = originalBasketPos;

        uiGO.SetActive(false);
    }

    protected virtual void spawnNextEgg()
    {

        GameObject egg = Instantiate(eggPrefab, eggEntries[0].eggPos, Quaternion.identity);

        egg.GetComponent<EggLogic>().SetManager(this); // this makes sure we are explicit about what the script is referencing since instanciating stuff can sometimes make this weird

        if (studyLogger != null)
        {
            studyLogger.StartSegmentLogging(eggEntries[0].id);
        }
    }

    public void onEggCompletion(float delay)
    {
        StartCoroutine(CompleteAfterDelayRoutine(delay));
    }

    private IEnumerator CompleteAfterDelayRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        _onEggCompletion();
    }

    protected virtual void _onEggCompletion()
    {
        if (eggEntries.Count == 0)
        {
            Debug.LogWarning("No egg/basket pairs left to spawn.", this);
            return;
        }


        eggEntries.RemoveAt(0);

        if (studyLogger != null)
        {
            studyLogger.StopSegmentLogging();
        }


        if (eggEntries.Count > 0)
        {
            spawnNextEgg();
        }
        else if (eggEntries.Count == 0)
        {
            LoadStudyDoneUI();
        }
    }

    protected virtual void LoadStudyDoneUI()
    {
        if (studyDoneUI != null)
        {
            studyDoneUI.transform.position = playerHeadAnchor.transform.TransformPoint(new Vector3(0, 0, 1)); //place ui right in front of players head relative to where they are 
            studyDoneUI.transform.rotation = playerHeadAnchor.transform.rotation;
            studyDoneUI.transform.SetParent(playerHeadAnchor.transform);
        }
        else
        {
            Debug.LogError("There is no study done ui obj assigned to EggBasketManager.");
        }
        
    }

    
}
