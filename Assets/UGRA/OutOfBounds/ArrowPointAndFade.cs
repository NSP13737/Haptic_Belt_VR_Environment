using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPointAndFade: MonoBehaviour
{

    [SerializeField]
    private Transform centerTarget;
    [SerializeField]
    private Material myMaterial;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(centerTarget);
        Color color = myMaterial.color;
        color.a = Mathf.PingPong(Time.time, 1f);
        myMaterial.color = color;
    }
}
