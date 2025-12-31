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

    public List<EggBasketEntry> entries = new List<EggBasketEntry>();


    private void spawnNextEggBasketPair()
    {
        
        GameObject egg = Instantiate(eggPrefab, entries[0].eggPos, Quaternion.identity);
        Instantiate(basketPrefab, entries[0].basketPos, Quaternion.identity);

        egg.GetComponent<EggLogic>().SetManager(this);
    }

    private void Awake()
    {
        List<Vector3> eggPositions = new List<Vector3>
        {
            new Vector3(0, 0, 0),
            new Vector3(0, 1, 0),
            new Vector3(0, 2, 0),
        };

        List<Vector3> basketPositions = new List<Vector3>
        {
            new Vector3(0, 0, 1),
            new Vector3(0, 1, 1),
            new Vector3(0, 2, 1),
        };

        for (int i = 0; i < eggPositions.Count; i++)
        {
            EggBasketEntry entry = new EggBasketEntry();
            entry.id = i;
            entry.eggPos = eggPositions[i];
            entry.basketPos = basketPositions[i];
            entries.Add(entry);
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
        if (entries.Count == 0)
        {
            Debug.LogWarning("No egg/basket pairs left to spawn.", this);
            return;
        }


        entries.RemoveAt(0);


        if (entries.Count > 0)
        {
            spawnNextEggBasketPair();
        }
    }

    
}
