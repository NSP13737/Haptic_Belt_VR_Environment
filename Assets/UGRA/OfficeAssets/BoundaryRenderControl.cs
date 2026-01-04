using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryRenderControl : MonoBehaviour
{

    private List<GameObject> allBoundaryGOs = new List<GameObject>();
    private int combinedMask;

    [SerializeField] public bool renderBoundaries = false;
    
    // Start is called before the first frame update
    void Start()
    {

        GameObject[] allGOs = FindObjectsOfType<GameObject>();
        foreach (GameObject go in allGOs)
        {
            if ((go.layer == LayerMask.NameToLayer("Boundary")) || (go.layer == LayerMask.NameToLayer("realBoundary")))
            {
                allBoundaryGOs.Add(go);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!renderBoundaries)
        {
            foreach (GameObject go in allBoundaryGOs)
            {
                go.GetComponent<Renderer>().enabled = false;
            }
        }
        else
        {
            foreach (GameObject go in allBoundaryGOs)
            {
                go.GetComponent<Renderer>().enabled = true;
            }
        }
    }

    public void changeRenderBool(bool render) { renderBoundaries = render; }
}
