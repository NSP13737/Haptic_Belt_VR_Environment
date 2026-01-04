using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainingUI_Manager : MonoBehaviour
{


    [SerializeField] GameObject confirmationGO;
    [SerializeField] BeltDistanceCaster beltDistanceCaster;
    [SerializeField] UDP_Manager udpManager;

    // Start is called before the first frame update
    void Start()
    {
        confirmationGO.SetActive(false);
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
}
