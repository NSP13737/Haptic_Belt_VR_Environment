/* using System;
using System.IO;
using UnityEngine;

public class logTest : MonoBehaviour
{
    private string logFolder;

    // Start is called before the first frame update
    void Start()
    {
        logFolder = Application.persistentDataPath + "/StudyLogs/";

        if (!Directory.Exists(logFolder))
        {
            Directory.CreateDirectory(logFolder);
        }

        WriteLog("testing01.txt", "howdy");
    }

    // Update is called once per frame
    public void WriteLog(string fileName, string content)
    {
        string filePath = Path.Combine(logFolder, fileName);

        //append content to file
        using (StreamWriter writer = new StreamWriter(filePath, true))
        {
            writer.WriteLine(content);
        }
    }
}
*/

using System;
using System.IO;
using UnityEngine;

public class logTest : MonoBehaviour
{
    private string logFolder;
    private string logFileName = "logDump";
    private float logStartTime;
    private int entryNum;

    void Start()
    {
        logFolder = Application.persistentDataPath + "/StudyLogs/";

        if (!Directory.Exists(logFolder))
        {
            Directory.CreateDirectory(logFolder);
        }

        //WriteLog("testing01.txt", "howdy");
    }

    public void setLogFileName(string logFileName)
    {
        this.logFileName = logFileName;
    }

    public void startLogging(int entryNum)
    {
        this.entryNum = entryNum;
        logStartTime = 0;
        startHeadCollisionTracker();
    }
    public void stopLogging()
    {
        float eggToBasketTime = Time.time - logStartTime;
        int numCollisions = stopHeadCollisionTracker();
        string content = $"{entryNum},{eggToBasketTime},{numCollisions}, {pathLength}";
        WriteLog(content);
    }
    private void startHeadCollisionTracker()
    {
        ;
    }
    private int stopHeadCollisionTracker()
    {
        return -1;
    }

    public void WriteLog(string content)
    {
        string filePath = Path.Combine(logFolder, logFileName);

        using (StreamWriter writer = new StreamWriter(filePath, true))
        {
            writer.WriteLine(content);
        }
    }
}
