using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using System.Linq; // Required for sorting and list manipulation
using System.Text.RegularExpressions; // Required for name parsing

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
        InitializeEggList();
        HideUIandBasket();
    }



protected virtual void InitializeEggList()
{
    // 1. Find all GameObjects in the scene (or use transform.Cast<Transform>() if they are children)
    var allObjects = FindObjectsOfType<GameObject>();

    // 2. Define a Regex pattern to match "user_egg" or "user_egg (123)"
    // ^ matches start, $ matches end. 
    // (?: \(\d+\))? optionally matches " (number)"
    Regex namePattern = new Regex(@"^user_egg(?:\s\((\d+)\))?$");

    // 3. Create a temporary list to hold the parsed data
    var foundEggs = new List<(int sortIndex, Vector3 position)>();

    foreach (Transform child in transform)
    {
        Match match = namePattern.Match(child.name);

        if (match.Success)
        {
            int index = 0;

            // Check if Group 1 (the number) exists.
            // If the name is just "user_egg", this group is empty, keeping index at 0.
            if (match.Groups[1].Success)
            {
                int.TryParse(match.Groups[1].Value, out index);
            }

            foundEggs.Add((index, child.position));
            child.gameObject.SetActive(false);
        }
    }
    // 4. Sort the list based on the extracted index
    // This ensures (1) comes before (3), even if (2) is missing.
    foundEggs.Sort((a, b) => a.sortIndex.CompareTo(b.sortIndex));

    // 5. Initialize the main list and populate it
    if (eggEntries == null) eggEntries = new List<EggEntry>();
    else eggEntries.Clear();

    for (int i = 0; i < foundEggs.Count; i++)
    {
        EggEntry entry = new EggEntry();
        
        // 'id' is set to the loop index (0, 1, 2...) ensuring a clean sequence 
        // regardless of gaps in the object naming (e.g. going from egg 1 to egg 3).
        entry.id = i; 
        entry.eggPos = foundEggs[i].position;
        
        eggEntries.Add(entry);
    }

    
    // Debug log to confirm it worked
    Debug.Log($"Initialized {eggEntries.Count} eggs from hierarchy.");
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
