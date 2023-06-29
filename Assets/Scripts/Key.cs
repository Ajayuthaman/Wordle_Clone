using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]

public class Key : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Key code representing keys")]
    KeyCode _keyCode = KeyCode.None;

    [Serializable]
    public struct LetterStateColor
    {
        public LetterState LetterState;
        public  Color Color;
    }

    [SerializeField]
    [Tooltip("Color per state")]
    LetterStateColor[] _letterStateColors = null;

    private Image _image;

    Color _startingColor = Color.gray;

    public Action<KeyCode> Pressed;

    public KeyCode KeyCode { get { return _keyCode; } }

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnButtonClick);
        Text text = GetComponentInChildren<Text>();

        if(text && string.IsNullOrEmpty(text.text))
            text.text = _keyCode.ToString();

        _image = GetComponent<Image>();
        _startingColor = _image.color;
    }


    public void OnButtonClick()
    {
        Pressed?.Invoke(_keyCode);
    }

    public void SetState(LetterState letterState)
    {
        foreach (LetterStateColor letterStateColor in _letterStateColors)
        {
            if (letterStateColor.LetterState == letterState)
            {
                _image.color = letterStateColor.Color;
                break;
            }
        }
    }

    public void RestState()
    {
        _image.color = _startingColor; 
    }

}
