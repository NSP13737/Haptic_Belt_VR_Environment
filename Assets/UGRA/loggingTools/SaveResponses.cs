using TMPro;
using UnityEngine;

public class SaveResponses : MonoBehaviour
{
    public TMP_InputField input1;
    public TMP_InputField input2;
    public logTest logger;

    public void SaveToLog()
    {
        string t1 = input1.text;
        string t2 = input2.text;

        logger.WriteLog("responses.txt", "Input 1: " + t1);
        logger.WriteLog("responses.txt", "Input 2: " + t2);

        Debug.Log("Saved!");
    }
}
