using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Letter : MonoBehaviour
{
    readonly int _animatorResetTrigger = Animator.StringToHash("Reset");
    readonly int _animatorShakeTrigger = Animator.StringToHash("Shake");
    readonly int _animatorStateParameter = Animator.StringToHash("State");

    private Animator _animator;
    private Text _text;

    public char? Entry { get; private set; } = null;

    public LetterState LetterState { get; private set; } = LetterState.Unknown;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _text = GetComponentInChildren<Text>();
    }

    private void Start()
    {
        _text.text = null;
    }

    public void EnterLetter(char c)
    {
        Entry = c;
        _text.text = c.ToString().ToUpper();
    }

    public void DeleteLetter()
    {
        Entry = null;
        _text.text = null;
        _animator.SetTrigger(_animatorResetTrigger);
    }

    public void Shake()
    {
        _animator.SetTrigger(_animatorShakeTrigger);
    }

    public void SetState(LetterState letterState)
    {
        LetterState= letterState;
        _animator.SetInteger(_animatorStateParameter, (int)letterState);
    }

    public void Clear()
    {
        LetterState = LetterState.Unknown;
        _animator.SetInteger(_animatorStateParameter, -1);
        _animator.SetTrigger(_animatorResetTrigger);
        Entry = null;
        _text.text = null;
    }
}
