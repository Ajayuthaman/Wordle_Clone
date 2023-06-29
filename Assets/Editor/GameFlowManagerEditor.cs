using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

[CustomEditor(typeof(GameFlowManager))]
public class GameFlowManagerEditor : Editor
{
    string _spoiler = null;

    GameFlowManager _gameFlowManager = null;

    private void OnEnable()
    {
        _gameFlowManager = (GameFlowManager)target;
        _gameFlowManager.Restarted += OnRestarted;
    }

    private void OnDisable()
    {
        _gameFlowManager.Restarted -= OnRestarted;
        _gameFlowManager = null;
    }

    private void OnRestarted()
    {
        _spoiler = null;
        Repaint();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (Application.isPlaying)
        {
            GUILayout.Space(20f);

            if (string.IsNullOrEmpty(_spoiler))
            {
                if (GUILayout.Button("Show Word"))
                {
                    _spoiler = _gameFlowManager.GetWord();
                }

            }
            else
            {
                GUILayout.Label(_spoiler, EditorStyles.boldLabel);
            }
        }
    }
}
