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

    [SerializeField] private Event nextEggBasketPair;
    [SerializeField] private GameObject eggPrefab;
    [SerializeField] private GameObject basketPrefab;
    [SerializeField] private loggingManager studyLogger;
    [SerializeField] private GameObject studyDoneUI;
    [SerializeField] private GameObject playerHeadAnchor;

    public List<EggEntry> eggEntries = new List<EggEntry>();


    

    private void Awake()
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

        studyDoneUI.transform.position = new Vector3(0, 400, 0); // start the done UI up and out of the way until we need it


    }

    public void startEggRoutineFromUI(GameObject uiGO)
    {
        spawnNextEgg();
        uiGO.SetActive(false);
    }

    private void spawnNextEgg()
    {

        GameObject egg = Instantiate(eggPrefab, eggEntries[0].eggPos, Quaternion.identity);

        egg.GetComponent<EggLogic>().SetManager(this); // this makes sure we are explicit about what the script is referencing since instanciating stuff can sometimes make this weird

        //TODO: Start logging
        studyLogger.StartSegmentLogging(eggEntries[0].id);
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

    private void _onEggCompletion()
    {
        if (eggEntries.Count == 0)
        {
            Debug.LogWarning("No egg/basket pairs left to spawn.", this);
            return;
        }


        eggEntries.RemoveAt(0);
        studyLogger.StopSegmentLogging();


        if (eggEntries.Count > 0)
        {
            spawnNextEgg();
        }
        else if (eggEntries.Count == 0)
        {
            LoadStudyDoneUI();
        }
    }

    private void LoadStudyDoneUI()
    {
        studyDoneUI.transform.position = playerHeadAnchor.transform.TransformPoint(new Vector3(0, 0, 1)); //place ui right in front of players head relative to where they are 
        studyDoneUI.transform.rotation = playerHeadAnchor.transform.rotation;
        studyDoneUI.transform.SetParent(playerHeadAnchor.transform);
    }

    
}
