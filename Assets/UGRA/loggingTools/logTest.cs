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

    private headTrackerTools headTracker;

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

    public void startSegmentLogging(int entryNum)
    {
        this.entryNum = entryNum;
        logStartTime = 0;
        headTracker.startHeadCollisionTracker();
        headTracker.BeginTrack();
    }
    public void stopSegmentLogging()
    {
        float eggToBasketTime = Time.time - logStartTime;
        int numCollisions = headTracker.stopHeadCollisionTracker();
        float distanceTraveled = headTracker.EndTrack();
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
