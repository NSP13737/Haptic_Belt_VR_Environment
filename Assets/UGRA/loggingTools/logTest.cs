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

    void Start()
    {
        logFolder = Application.persistentDataPath + "/StudyLogs/";

        if (!Directory.Exists(logFolder))
        {
            Directory.CreateDirectory(logFolder);
        }

        WriteLog("testing01.txt", "howdy");
    }

    public void WriteLog(string fileName, string content)
    {
        string filePath = Path.Combine(logFolder, fileName);

        using (StreamWriter writer = new StreamWriter(filePath, true))
        {
            writer.WriteLine(content);
        }
    }
}
