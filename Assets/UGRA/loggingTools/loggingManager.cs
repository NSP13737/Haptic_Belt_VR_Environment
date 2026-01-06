/* using System;
using System.IO;
using UnityEngine;

public class loggingManager : MonoBehaviour
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

public class loggingManager : MonoBehaviour
{
    private string logFolder;
    private string logFileName = "logDump.log";
    private float logStartTime;
    private int entryNum;

    public HeadTrackerTools headTracker;

    void Start()
    {
        logFolder = Application.persistentDataPath + "/StudyLogs/";

        if (!Directory.Exists(logFolder))
        {
            Directory.CreateDirectory(logFolder);
        }

        //WriteLog("testing01.txt", "howdy");
    }

    public void SetLogFileName(string logFileName)
    {
        this.logFileName = logFileName;
    }

    public void StartSegmentLogging(int entryNum)
    {
        this.entryNum = entryNum;
        logStartTime = Time.time;
        headTracker.StartHeadCollisionTracker();
        headTracker.BeginDistanceTracking();
    }
    public void StopSegmentLogging()
    {
        float eggToBasketTime = Time.time - logStartTime;
        int numCollisions = headTracker.StopHeadCollisionTracker();
        float distanceTraveled = headTracker.EndDistanceTracking();
        string content = $"{entryNum},{eggToBasketTime},{numCollisions},{distanceTraveled}";
        WriteLog(content);
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
