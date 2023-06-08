using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCounter : MonoBehaviour
{
    private float timeStart;

    [SerializeField] private Text timeText;

    void Start()
    {
        timeText.text = timeStart.ToString("F2");
    }

    void Update()
    {
        timeStart += Time.deltaTime;
        int timeInt = (int)timeStart;
        timeText.text = "Time: " + timeInt.ToString();

    }
}
