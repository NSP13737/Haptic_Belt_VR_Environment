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
    private static string logFileName = "logDump.csv";
    private float logStartTime;
    private int entryNum;
    [SerializeField] private Study_Params_Manager paramsManager;

    public HeadTrackerTools headTracker;

    // -------------------------
    // Added: Participant + 2 sliders
    // -------------------------
    [Header("Participant / Questionnaire (assign in Inspector)")]
    public Slider participantIdSlider;   // slider1
    public Slider conditionSlider;       // slider2

    [Header("TMP label lookup")]
    public string labelChildName = "Label";

    [Header("Participant file naming")]
    public string filePrefix = "logID_";

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
    // NEW: Hook this to your Toggle OnValueChanged()
    // -------------------------
    public void SubmitParticipantAndQuestions(GameObject caller)
    {
        Debug.Log("SubmitParticipantAndQuestions fired. | manager=" + gameObject.name);

        if (participantIdSlider == null || conditionSlider == null)
        {
            Debug.LogError("SubmitParticipantAndQuestions: sliders not assigned in Inspector.");
            return;
        }

        try
        {

            // Set participant-specific file name
            logFileName = MakeSafeFileNameFromParticipantSlider(participantIdSlider.value);
            string filePath = Path.Combine(logFolder, logFileName);

            //Debug.Log("SUBMIT will write file -> " + filePath);

            // Force-create file immediately (so you can see it appear even before writing)
            ForceCreateFileIfMissing(filePath);


            string parID = GetSliderLabelTMP(participantIdSlider);
            string condition = GetSliderLabelTMP(conditionSlider);

            string questionnaireLine =
                $"{Csv(parID)},{participantIdSlider.value}\n" +
                $"{Csv(condition)},{conditionSlider.value}";

            //append info to file
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(questionnaireLine);
            }

            //create log headers for segment tracking
            WriteLog("segment,eggToBasketTime,numCollisions,distanceTraveled");

            //Debug.Log("SUBMIT write done. File exists? " + File.Exists(filePath));

            //disable the panel once we have submitted everything
            caller.transform.root.gameObject.SetActive(false);
            
            //Actually set the condition for the belt
            paramsManager.changeCondition(conditionSlider.value);


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
        //Debug.Log("Force-created new file -> " + filePath);
    }

    private void CreateLogHeaders(string filePath)
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
            string header = "segment,entryNum,eggToBasketTime,numCollisions,distanceTraveled\n";

            File.WriteAllText(filePath, header);
            //Debug.Log("Wrote headers -> " + filePath);
        }
        catch (Exception e)
        {
            Debug.LogError("EnsureFileHasHeaders ERROR -> " + filePath + "\n" + e);
        }
    }

    private string MakeSafeFileNameFromParticipantSlider(float value)
    {
        string formatted = value.ToString("F0");

        formatted = formatted.Replace("-", "neg");
        formatted = formatted.Replace(".", "p");
        formatted = formatted.Replace(":", "_");
        formatted = formatted.Replace("/", "_");
        formatted = formatted.Replace("\\", "_");
        formatted = formatted.Replace(" ", "");

        return $"{filePrefix}{formatted}.csv";
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
