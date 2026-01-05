using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainingUI_Manager : MonoBehaviour
{


    [SerializeField] GameObject confirmationGO;
    [SerializeField] BeltDistanceCaster beltDistanceCaster;
    [SerializeField] UDP_Manager udpManager;

    [Header("Below is for use with Min intensity UI only")]
    [SerializeField] GameObject boundaryVisibilityUI;


    // Start is called before the first frame update
    void Start()
    {
        confirmationGO.SetActive(false);
        boundaryVisibilityUI.transform.position = new Vector3(0, 1000, 0); // put this in space so that player doesn't see it (I'm choosing not to deactivate the object bc that was being finicky)
        beltDistanceCaster.disableRealtimeDistance(true); //disable realtime distance getting until we are done manually adjusting min detectable intensity
        
}

    // Update is called once per frame
    void Update()
    {
        
    }

    public void queueConfirmationButton(bool confirm)
    {
        if (confirm)
        {
            confirmationGO.SetActive(true);
        }
        else
        {
            confirmationGO.SetActive(false);
        }
    }

    public void moveToNextScene()
    {
        SceneManager.LoadScene("experimentalScene");
    }

    //only used by min intensity ui
    public void loadBoundaryVisUI()
    {

        boundaryVisibilityUI.transform.position = this.gameObject.transform.position;


        this.gameObject.SetActive(false); 

    }
}
