using Nova;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateFPS : MonoBehaviour
{
    [SerializeField] private TextBlock _fpsText;
    
    private double deltaTime = 0;
    private int currentFPS = 0;
    private int averageFPS = 0;

    private int lastFrameIndex;
    private float[] frameDeltaTimeArray;

    private int bestFPS = 0, worstFPS = 0;

    private void Awake()
    {
        Application.targetFrameRate = Screen.currentResolution.refreshRate;

        frameDeltaTimeArray = new float[60];
        lastFrameIndex = 0;
    }

    private void Start()
    {
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
    }

    private void Update()
    {
        currentFPS = Mathf.RoundToInt(1f / Time.unscaledDeltaTime);

        frameDeltaTimeArray[lastFrameIndex] = Time.unscaledDeltaTime;
        lastFrameIndex = (lastFrameIndex + 1) % frameDeltaTimeArray.Length;
        averageFPS = Mathf.RoundToInt(GetFPS());

        if(Time.frameCount > 100)
        {
            if (worstFPS > currentFPS)
            {
                worstFPS = currentFPS;
            }

            if (bestFPS < currentFPS)
            {
                bestFPS = currentFPS;
            }
        }

        _fpsText.Text = $"cur : {currentFPS.ToString()}\navg : {averageFPS.ToString()}\n<color=#005500>{bestFPS.ToString()}</color>,<color=\"red\">{worstFPS.ToString()}</color>";

        if (Time.frameCount == 100)
        {
            worstFPS = int.MaxValue;
            bestFPS = 0;
        }
    }

    private float GetFPS()
    {
        float total = 0f;
        foreach (float deltatime in frameDeltaTimeArray)
        {
            total += deltatime;
        }
        return frameDeltaTimeArray.Length / total;
    }

    //private int _mRenderCount = 0;
    //private DateTime _mRenderTimer = DateTime.MinValue;

    //void OnRenderObject()
    //{
    //    ++_mRenderCount;

    //    if (_mRenderTimer < DateTime.Now)
    //    {
    //        _mRenderTimer = DateTime.Now + TimeSpan.FromSeconds(1);
    //        if (_fpsText)
    //        {
    //            _fpsText.Text = string.Format("FPS: {0}", _mRenderCount);
    //        }
    //        _mRenderCount = 0;
    //    }
    //}
}
