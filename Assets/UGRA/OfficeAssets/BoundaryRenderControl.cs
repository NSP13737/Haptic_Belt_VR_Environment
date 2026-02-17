using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryRenderControl : MonoBehaviour
{

    private List<GameObject> allBoundaryGOs = new List<GameObject>();

    [SerializeField] private bool renderBoundaries = false;
    //[SerializeField] private BeltDistanceCaster beltRayCaster;
    
    // Start is called before the first frame update
    void Start()
    {
        //beltRayCaster.beltRayVisibility = false;

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
            //beltRayCaster.beltRayVisibility = false;
            foreach (GameObject go in allBoundaryGOs)
            {
                go.GetComponent<Renderer>().enabled = false;
            }
        }
        else
        {
            //beltRayCaster.beltRayVisibility = true;
            foreach (GameObject go in allBoundaryGOs)
            {
                go.GetComponent<Renderer>().enabled = true;
            }
        }
    }

    public void changeRenderBool(bool render) { renderBoundaries = render; }
}
