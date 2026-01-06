using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EggBasketEntry
{
    public int id;
    public Vector3 eggPos;
    public Vector3 basketPos;
}
public class EggBasketManager : MonoBehaviour
{

    [SerializeField] private Event nextEggBasketPair;
    [SerializeField] private GameObject eggPrefab;
    [SerializeField] private GameObject basketPrefab;
    [SerializeField] private loggingManager studyLogger;
    [SerializeField] private GameObject studyDoneUI;
    [SerializeField] private GameObject playerHeadAnchor;

    public List<EggBasketEntry> eggBasketPairEntries = new List<EggBasketEntry>();


    

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

        List<Vector3> basketPositions = new List<Vector3>
        {
            new Vector3(0.287f, 0.224f, 5.896f),
            new Vector3(1.77f, 0.224f, 3.14f),
            new Vector3(4.1067f, 0.224f, 2.216f),
            new Vector3(0.4864f, 0.224f, 1.8336f),
            new Vector3(2.076f, 0.224f, 0.3377f),
        };

        for (int i = 0; i < eggPositions.Count; i++)
        {
            EggBasketEntry entry = new EggBasketEntry();
            entry.id = i;
            entry.eggPos = eggPositions[i];
            entry.basketPos = basketPositions[i];
            eggBasketPairEntries.Add(entry);
        }

        studyDoneUI.transform.position = new Vector3(0, 400, 0); // start the done UI up and out of the way until we need it


    }

    public void startEggBasketFromUI(GameObject uiGO)
    {
        spawnNextEggBasketPair();
        uiGO.SetActive(false);
    }

    private void spawnNextEggBasketPair()
    {

        GameObject egg = Instantiate(eggPrefab, eggBasketPairEntries[0].eggPos, Quaternion.identity);
        GameObject basket = Instantiate(basketPrefab, eggBasketPairEntries[0].basketPos, Quaternion.identity);
        basket.transform.Rotate(new Vector3(-90, 0, 0)); //scuffed way of making sure basket is right side up :)

        egg.GetComponent<EggLogic>().SetManager(this); // this makes sure we are explicit about what the script is referencing since instanciating stuff can sometimes make this weird

        //TODO: Start logging
        studyLogger.StartSegmentLogging(eggBasketPairEntries[0].id);
    }

    public void onEggBasketCompletion(float delay)
    {
        StartCoroutine(CompleteAfterDelayRoutine(delay));
    }

    private IEnumerator CompleteAfterDelayRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        _onEggBasketCompletion();
    }

    private void _onEggBasketCompletion()
    {
        if (eggBasketPairEntries.Count == 0)
        {
            Debug.LogWarning("No egg/basket pairs left to spawn.", this);
            return;
        }


        eggBasketPairEntries.RemoveAt(0);
        studyLogger.StopSegmentLogging();


        if (eggBasketPairEntries.Count > 0)
        {
            spawnNextEggBasketPair();
        }
        else if (eggBasketPairEntries.Count == 0)
        {
            LoadStudyDoneUI();
        }
    }

    private void LoadStudyDoneUI()
    {
        studyDoneUI.transform.position = playerHeadAnchor.transform.TransformPoint(new Vector3(0, 0, 1)); //place ui right in front of players head relative to where they are 
        studyDoneUI.transform.rotation = playerHeadAnchor.transform.rotation;
        studyDoneUI.transform.SetParent(playerHeadAnchor.transform);
        Debug.LogError("LOAD STUDY STUFF DONE");
    }

    
}
