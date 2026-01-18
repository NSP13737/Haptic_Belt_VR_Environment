using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class loggingManager : MonoBehaviour
{
    // -------------------------
    // ORIGINAL functionality
    // -------------------------
    private string logFolder;
    private string logFileName = "logDump.log";
    private float logStartTime;
    private int entryNum;

    public HeadTrackerTools headTracker;

    // -------------------------
    // Added: Participant + 2 sliders
    // -------------------------
    [Header("Participant / Questionnaire (assign in Inspector)")]
    public Slider participantIdSlider;   // slider1
    public Slider questionSlider2;       // slider2

    [Header("TMP label lookup")]
    public string labelChildName = "Label";

    [Header("Participant file naming")]
    public string filePrefix = "logID_";
    public int fileNameDecimals = 0; // 0 => logID_7.log, 2 => logID_7p00.log

    [Header("Questionnaire behavior")]
    public bool overwriteQuestionnaireOnSubmit = false;

    // -------------------------
    // Always initialize early
    // -------------------------
    private void Awake()
    {
        logFolder = Path.Combine(Application.persistentDataPath, "StudyLogs");
        try
        {
            Directory.CreateDirectory(logFolder);
        }
        catch (Exception e)
        {
            Debug.LogError("loggingManager Awake: Failed to create log folder: " + logFolder + "\n" + e);
        }

        Debug.Log("PERSISTENT PATH: " + Application.persistentDataPath);
        Debug.Log("LOG FOLDER: " + logFolder);
        Debug.Log("INITIAL LOG FILE: " + Path.Combine(logFolder, logFileName));
    }

    void Start()
    {
        // keep Start in case you had other Start-time behavior later
        // (folder already created in Awake)
    }

    // -------------------------
    // ORIGINAL API (kept)
    // -------------------------
    public void SetLogFileName(string logFileName)
    {
        this.logFileName = logFileName;
        Debug.Log("SetLogFileName -> " + Path.Combine(logFolder, this.logFileName));
        EnsureFileHasHeaders(Path.Combine(logFolder, this.logFileName));
    }

    public void StartSegmentLogging(int entryNum)
    {
        this.entryNum = entryNum;
        logStartTime = Time.time;

        if (headTracker == null)
            headTracker = FindObjectOfType<HeadTrackerTools>();

        if (headTracker != null)
        {
            headTracker.StartHeadCollisionTracker();
            headTracker.BeginDistanceTracking();
        }
        else
        {
            Debug.LogWarning("loggingManager: HeadTrackerTools not found in this scene. Will log collisions=0, distance=0.");
        }
    }

    public void StopSegmentLogging()
    {
        float eggToBasketTime = Time.time - logStartTime;

        int numCollisions = 0;
        float distanceTraveled = 0f;

        if (headTracker == null)
            headTracker = FindObjectOfType<HeadTrackerTools>();

        if (headTracker != null)
        {
            numCollisions = headTracker.StopHeadCollisionTracker();
            distanceTraveled = headTracker.EndDistanceTracking();
        }

        // ORIGINAL content format preserved
        string content = $"{entryNum},{eggToBasketTime},{numCollisions},{distanceTraveled}";
        WriteLog(content);
    }

    public void WriteLog(string content)
    {
        string filePath = Path.Combine(logFolder, logFileName);

        try
        {
            // if this is the first write ever, ensure file exists + headers
            EnsureFileHasHeaders(filePath);

            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(content);
            }

            Debug.Log("WriteLog OK -> " + filePath);
        }
        catch (Exception e)
        {
            Debug.LogError("WriteLog ERROR -> " + filePath + "\n" + e);
        }
    }

    // -------------------------
    // NEW: Hook this to your Toggle OnValueChanged(bool)
    // -------------------------
    public void SubmitParticipantAndQuestions(bool isOn)
    {
        Debug.Log("SubmitParticipantAndQuestions fired. isOn=" + isOn + " | manager=" + gameObject.name);
        //if (!isOn) return;

        if (participantIdSlider == null || questionSlider2 == null)
        {
            Debug.LogWarning("SubmitParticipantAndQuestions: sliders not assigned in Inspector.");
            return;
        }

        try
        {
            Directory.CreateDirectory(logFolder);

            // Set participant-specific file name
            logFileName = MakeSafeFileNameFromParticipantSlider(participantIdSlider.value);
            string filePath = Path.Combine(logFolder, logFileName);

            Debug.Log("SUBMIT will write file -> " + filePath);

            // Force-create file immediately (so you can see it appear even before writing)
            ForceCreateFileIfMissing(filePath);

            // Ensure headers for new file
            EnsureFileHasHeaders(filePath);

            // Build questionnaire line
            string ts = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string label1 = GetSliderLabelTMP(participantIdSlider);
            string label2 = GetSliderLabelTMP(questionSlider2);

            string questionnaireLine =
                $"questionnaire,{ts},{Csv(label1)},{participantIdSlider.value},{Csv(label2)},{questionSlider2.value}";

            if (overwriteQuestionnaireOnSubmit)
            {
                // WARNING: overwrites entire file
                string content =
                    "section,columns\n" +
                    "questionnaire,timestamp,label1,value1,label2,value2\n" +
                    "segment,entryNum,eggToBasketTime,numCollisions,distanceTraveled\n" +
                    questionnaireLine + "\n";

                File.WriteAllText(filePath, content);
            }
            else
            {
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine(questionnaireLine);
                }
            }

            Debug.Log("SUBMIT write done. File exists? " + File.Exists(filePath));
        }
        catch (Exception e)
        {
            Debug.LogError("SubmitParticipantAndQuestions ERROR:\n" + e);
        }
    }

    // -------------------------
    // Helpers
    // -------------------------
    private void ForceCreateFileIfMissing(string filePath)
    {
        if (File.Exists(filePath)) return;

        // create empty file and close immediately
        using (FileStream fs = File.Create(filePath)) { }
        Debug.Log("Force-created new file -> " + filePath);
    }

    private void EnsureFileHasHeaders(string filePath)
    {
        if (File.Exists(filePath))
        {
            // If file exists but is empty, write headers
            try
            {
                var info = new FileInfo(filePath);
                if (info.Length > 0) return;
            }
            catch { /* ignore */ }
        }

        try
        {
            string header =
                "section,columns\n" +
                "questionnaire,timestamp,label1,value1,label2,value2\n" +
                "segment,entryNum,eggToBasketTime,numCollisions,distanceTraveled\n";

            File.WriteAllText(filePath, header);
            Debug.Log("Wrote headers -> " + filePath);
        }
        catch (Exception e)
        {
            Debug.LogError("EnsureFileHasHeaders ERROR -> " + filePath + "\n" + e);
        }
    }

    private string MakeSafeFileNameFromParticipantSlider(float value)
    {
        string formatted = value.ToString("F" + Mathf.Max(0, fileNameDecimals));

        formatted = formatted.Replace("-", "neg");
        formatted = formatted.Replace(".", "p");
        formatted = formatted.Replace(":", "_");
        formatted = formatted.Replace("/", "_");
        formatted = formatted.Replace("\\", "_");
        formatted = formatted.Replace(" ", "");

        return $"{filePrefix}{formatted}.log";
    }

    private string GetSliderLabelTMP(Slider slider)
    {
        // direct child
        Transform t = slider.transform.Find(labelChildName);
        if (t != null)
        {
            var tmp = t.GetComponent<TMP_Text>();
            if (tmp != null) return tmp.text;
        }

        // search anywhere under slider
        TMP_Text[] tmps = slider.GetComponentsInChildren<TMP_Text>(true);
        foreach (var tmp in tmps)
        {
            if (tmp.gameObject.name == labelChildName)
                return tmp.text;
        }

        return slider.gameObject.name;
    }

    private string Csv(string s)
    {
        if (string.IsNullOrEmpty(s)) return "";
        if (s.Contains(",") || s.Contains("\"") || s.Contains("\n"))
            return "\"" + s.Replace("\"", "\"\"") + "\"";
        return s;
    }
}
