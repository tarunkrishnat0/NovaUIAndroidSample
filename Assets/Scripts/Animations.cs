using Nova;
using System;
using UnityEngine;

public struct ScaleAnimations : IAnimation
{
    public float StartValue;
    public float EndValue;
    public Transform TransformToScale;

    public void Update(float percentDone)
    {
        float scale;
        if (percentDone < 0.5f)
        {
            scale = Mathf.Lerp(StartValue, EndValue, percentDone * 2);
        } else
        {
            scale = Mathf.Lerp(EndValue, StartValue, (percentDone - 0.5f)*2);
        }
        
        TransformToScale.localScale = new Vector3(scale, scale, 1);
    }
}
