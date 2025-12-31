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
        Instantiate(eggPrefab, entries[0].eggPos, Quaternion.identity);
        Instantiate(basketPrefab, entries[0].basketPos, Quaternion.identity);
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
        onEggBasketCompletion();

    }

    
    public void onEggBasketCompletion()
    {
        if (entries.Count > 0)
        {
            Debug.Log("Removing!" + "Blah");
            entries.RemoveAt(0);
            spawnNextEggBasketPair();
        }
        else
        {
            Debug.LogWarning(entries);
            Debug.LogWarning("You are trying to spawn a new egg basket pair, but there are none left!", this);
        }
        
    }

    
}
