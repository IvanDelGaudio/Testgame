using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    #region Public Variables
    public Text timerText;
    public float elapsedTime;
    public List<GameObject> timers = new List<GameObject>();
    #endregion
    #region Private Variables
    private int seconds;
    private int minutes;
    private int resume = 1;
    #endregion
    #region Lifecycle
    private void Update()
    {
        if (Input.GetKeyDown("a"))
        {StopTimer();}
        if (Input.GetKeyDown("r"))
        { ResumeTimer();}
        elapsedTime += Time.deltaTime*resume;
        seconds = Mathf.FloorToInt(elapsedTime % 60);
        minutes = Mathf.FloorToInt(elapsedTime / 60);
        timerText.text = string.Format("{0:00}:{1:00}",minutes,seconds);
    }
    #endregion
    #region Public Methods
    public void StopTimer()
    {
        resume = 0 ;
    }
    public void ResumeTimer()
    {
        resume = 1;
    }
    #endregion
    #region Private Methods
    #endregion
}
