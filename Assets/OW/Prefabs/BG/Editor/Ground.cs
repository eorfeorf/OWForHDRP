using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Ground : MonoBehaviour
{
    private void Update()
    {
        var transforms = GetComponentsInChildren<Transform>();

        foreach (var t in transforms)
        {
            if (t.parent == null)
            {
                // 一番親なので無視.
                continue;
            }
            
            if (!t.hasChanged)
            {
                continue;
            }
            
            ForceAdjustPositionY(t);   
        }
    }

    void ForceAdjustPositionY(Transform t)
    {
         
        var scaleY = t.localScale.y;
        var pos = t.position;
        pos.y = scaleY / 2f;
        t.position = pos;
    }
}
