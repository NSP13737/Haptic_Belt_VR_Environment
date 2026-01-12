using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class TwoSliderLogger : MonoBehaviour
{
    [Header("Assign your two Slider components here")]
    public Slider slider1;
    public Slider slider2;

    [Header("Log file name (saved in Application.persistentDataPath)")]
    public string fileName = "slider_log.csv";

    [Header("Log every time a slider changes")]
    public bool logOnValueChanged = true;

    private string _path;

    // Store listeners so RemoveListener works correctly
    private UnityEngine.Events.UnityAction<float> _slider1Listener;
    private UnityEngine.Events.UnityAction<float> _slider2Listener;

    private void Awake()
    {
        _path = Path.Combine(Application.persistentDataPath, fileName);

        // Print paths so you can find the file on disk
        Debug.Log("PERSISTENT PATH: " + Application.persistentDataPath);
        Debug.Log("LOG PATH: " + _path);

        EnsureHeader();

        if (logOnValueChanged)
        {
            _slider1Listener = _ => SaveLog();
            _slider2Listener = _ => SaveLog();

            if (slider1 != null) slider1.onValueChanged.AddListener(_slider1Listener);
            if (slider2 != null) slider2.onValueChanged.AddListener(_slider2Listener);
        }
    }

    private void OnDestroy()
    {
        if (slider1 != null && _slider1Listener != null)
            slider1.onValueChanged.RemoveListener(_slider1Listener);

        if (slider2 != null && _slider2Listener != null)
            slider2.onValueChanged.RemoveListener(_slider2Listener);
    }

    private void EnsureHeader()
    {
        if (!File.Exists(_path))
        {
            File.AppendAllText(_path, "timestamp,slider1,slider2\n");
        }
    }

    // Call this from a button, or it will be called automatically on slider changes (if enabled).
    public void SaveLog()
    {
        if (slider1 == null || slider2 == null)
        {
            Debug.LogWarning("TwoSliderLogger: Assign slider1 and slider2 in the Inspector.");
            return;
        }

        string ts = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        float v1 = slider1.value;
        float v2 = slider2.value;

        string line = $"{ts},{v1},{v2}\n";
        File.AppendAllText(_path, line);

        // Optional: confirm each write
        // Debug.Log("WROTE LINE: " + line.Trim());
    }

    // Optional: call this from a UI Button to print the path again
    public void PrintLogPath()
    {
        Debug.Log("LOG PATH: " + _path);
    }
}
