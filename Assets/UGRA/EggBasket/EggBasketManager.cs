using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private logTest studyLogger;

    public List<EggBasketEntry> eggBasketPairEntries = new List<EggBasketEntry>();
    public float logStartTime;


    private void spawnNextEggBasketPair()
    {
        
        GameObject egg = Instantiate(eggPrefab, eggBasketPairEntries[0].eggPos, Quaternion.identity);
        GameObject basket = Instantiate(basketPrefab, eggBasketPairEntries[0].basketPos, Quaternion.identity);
        basket.transform.Rotate(new Vector3(-90, 0, 0)); //scuffed way of making sure basket is right side up :)

        egg.GetComponent<EggLogic>().SetManager(this);

        //TODO: Start logging
        studyLogger.startSegmentLogging(eggBasketPairEntries[0].id);
    }

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

        //start egg basket routine
        spawnNextEggBasketPair();

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
        //TODO: Stop logging (use id num of entry for logging)
        if (eggBasketPairEntries.Count == 0)
        {
            Debug.LogWarning("No egg/basket pairs left to spawn.", this);
            return;
        }


        eggBasketPairEntries.RemoveAt(0);
        studyLogger.stopSegmentLogging();


        if (eggBasketPairEntries.Count > 0)
        {
            spawnNextEggBasketPair();
        }
    }

    
}
