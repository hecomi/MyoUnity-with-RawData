using UnityEngine;
using System;
using System.Linq;

public class RssiAndEmgGUI : MonoBehaviour 
{
    public GameObject myo;
    private ThalmicMyo thalmicMyo_;
    private ThalmicMyo thalmicMyo {
        get { return thalmicMyo_ ?? (thalmicMyo_ = myo.GetComponent<ThalmicMyo>()); }
    }

    private float averageEmg = 0f;

    void Start()
    {
        thalmicMyo.SetStreamEmg(Thalmic.Myo.StreamEmgType.Enabled);
    }

    void Update()
    {
        thalmicMyo.RequestRssi();

        if (thalmicMyo.emg != null && thalmicMyo.emg.Length > 0) {
            var currentEmgAve = (float)thalmicMyo.emg.Average(x => Mathf.Abs(x));
            averageEmg += (currentEmgAve - averageEmg) * 0.1f;
        }
    }

    void OnGUI()
    {
        if (thalmicMyo.emg != null && thalmicMyo.emg.Length > 0) {
            GUI.Label(new Rect (12, Screen.height - 100, Screen.width, 100),
                "RSSI: " + thalmicMyo.rssi + "\n" +
                "EMG:  " + string.Join(", ", Array.ConvertAll<sbyte, string>(thalmicMyo.emg, x => x.ToString())) + "\n" +
                "   => Average: " + Mathf.Floor(averageEmg)
            );
        }
    }
}
