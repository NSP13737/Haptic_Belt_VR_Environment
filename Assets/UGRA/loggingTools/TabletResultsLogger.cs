using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TabletResultsLogger : MonoBehaviour
{
    [Header("UI References")]
    public Slider smallSlider;
    public TMP_Text sliderParamVal;

    [Header("Optional: display fields to log too")]
    public TMP_Text button1ParamVal;
    public TMP_Text button2ParamVal;
    public TMP_Text button3ParamVal;

    [Header("File Settings")]
    public string fileName = "tablet_log.csv";

    public void SaveLog()
    {
        // Only log when toggle is ON (prevents double logging)
        var toggle = GetComponentInChildren<Toggle>();
        if (toggle != null && !toggle.isOn)
            return;

        string path = Path.Combine(Application.persistentDataPath, fileName);

        string ts = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

        float sliderValue = smallSlider ? smallSlider.value : float.NaN;
        string sliderText = sliderParamVal ? Sanitize(sliderParamVal.text) : "";

        string b1Text = button1ParamVal ? Sanitize(button1ParamVal.text) : "";
        string b2Text = button2ParamVal ? Sanitize(button2ParamVal.text) : "";
        string b3Text = button3ParamVal ? Sanitize(button3ParamVal.text) : "";

        if (!File.Exists(path))
        {
            File.AppendAllText(path,
                "timestamp,sliderValue,sliderParamVal,btn1ParamVal,btn2ParamVal,btn3ParamVal\n");
        }

        string line = $"{ts},{sliderValue},{sliderText},{b1Text},{b2Text},{b3Text}\n";
        File.AppendAllText(path, line);

        Debug.Log($"[TabletResultsLogger] Logged to: {path}");
    }

    private string Sanitize(string s)
    {
        if (string.IsNullOrEmpty(s)) return "";
        s = s.Replace("\"", "\"\"");
        if (s.Contains(",") || s.Contains("\n") || s.Contains("\r"))
            s = $"\"{s}\"";
        return s;
    }
}
