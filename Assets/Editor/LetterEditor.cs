using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Letter))]
public class LetterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (Application.isPlaying)
        {
            GUILayout.Space(20f);

            if (GUILayout.Button("Enter Letter"))
            {
                ((Letter)target).EnterLetter('C');
            }

            if (GUILayout.Button("Delete Letter"))
            {
                ((Letter)target).DeleteLetter();
            }

            if (GUILayout.Button("Shake"))
            {
                ((Letter)target).Shake();
            }

            if (GUILayout.Button("Currect"))
            {
                ((Letter)target).SetState(LetterState.Correct);
            }
            if (GUILayout.Button("Inurrect"))
            {
                ((Letter)target).SetState(LetterState.Incorrect);
            }

            if (GUILayout.Button("Wrong"))
            {
                ((Letter)target).SetState(LetterState.WrongLocation);
            }
        }
    }
}
