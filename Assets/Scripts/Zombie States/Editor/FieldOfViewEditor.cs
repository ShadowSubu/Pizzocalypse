using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        FieldOfView FOV = (FieldOfView)target;  
        Handles.color = Color.white;
        Handles.DrawWireArc(FOV.transform.position, Vector3.up, Vector3.forward, 360, FOV.ViewRadius);
        Vector3 viewAngleA = FOV.DirectionFromAngle(-FOV.ViewAngle / 2, false);
        Vector3 viewAngleB = FOV.DirectionFromAngle(FOV.ViewAngle / 2, false);
        Handles.DrawLine(FOV.transform.position, FOV.transform.position + viewAngleA * FOV.ViewRadius );
        Handles.DrawLine(FOV.transform.position, FOV.transform.position + viewAngleB * FOV.ViewRadius );

        Handles.color = Color.red;
        foreach (Transform visibleTarget in FOV.visibleTargets)
        {
            Handles.DrawLine(FOV.transform.position, visibleTarget.position);
        }
    }
}


        
 