using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class GameFlowManager : MonoBehaviour
{
    const int _wordLength = 5;

    [SerializeField]
    [Tooltip("The Word Repository")]
    WordRepository _wordRepository;

    [SerializeField]
    [Tooltip("Prefab for letter")]
    private Letter _letterPrefab;

    [SerializeField]
    [Tooltip("Amount of rows")]
    private int _amountOfRows = 6;

    [SerializeField]
    [Tooltip("Grid Parent")]
    GridLayoutGroup _grifLayout;

    [SerializeField]
    [Tooltip("Offset of letter ANimation")]
    private float letterAnimationOffsetTime = .5f;

    [SerializeField]
    [Tooltip("Keys on KeyBoard")]
    private Key[] _keys;

    List<Letter> _letters;

    private int _index = 0;
    private int _currentRow = 0;
    char?[] _guess = new char?[_wordLength];

    char[] _word = new char[_wordLength];

    public PuzzleState PuzzleState { get; private set; } = PuzzleState.InProgress;

    public Action Restarted;

    private void Awake()
    {
        SetupGrid();

        foreach(Key key in _keys)
        {
            key.Pressed += OnKeyPressed;
        }
    }

    private void Start()
    {
        SetWord();
    }

    private void Update()
    {
        if (Input.anyKeyDown)
            ParseInput(Input.inputString);

    }

    public void Restart()
    {
        PuzzleState = PuzzleState.InProgress;

        foreach(Letter letter in _letters)
        {
            letter.Clear();
        }

        foreach(Key key in _keys)
        {
            key.RestState();
        }

        _index = 0;
        _currentRow = 0;

        for(int i= 0; i < _wordLength; i++)
            _guess[i] = null;

        SetWord();

        Restarted?.Invoke();
    }

    void OnKeyPressed(KeyCode keyCode)
    {
        if(PuzzleState != PuzzleState.InProgress)
        {
            if (keyCode == KeyCode.Return)
                Restart();

            return;
        }
        if(keyCode == KeyCode.Return)
        {
            GuessWord();
        }
        else if(keyCode == KeyCode.Backspace || keyCode == KeyCode.Delete)
        {
            DeleteLetter();
        }
        else if(keyCode >= KeyCode.A && keyCode <= KeyCode.Z)
        {
            int index = keyCode - KeyCode.A;            
            EnterLetter((char)((int)'A' + index));
        }
    }

    public void ParseInput(string value)
    {
        if (PuzzleState != PuzzleState.InProgress)
        {
            foreach (char c in value)
            {
                if ((c == '\n') || (c == '\r')) //enter or return
                {
                    Restart();
                    return;
                }
            }

            return;
        }
            

        foreach(char c in value)
        {
            if(c == '\b') //backspace
            {
                DeleteLetter();
            }
            else if((c == '\n') || (c == '\r')) //enter or return
            {
                GuessWord();
            }
            else
            {
                EnterLetter(c);
            }
        }
    }

    public void SetupGrid()
    {
        if (_letters == null)
            _letters = new List<Letter>(); 

        for(int i = 0; i < _amountOfRows; i++)
        {
            for(int j = 0; j < _wordLength; j++)
            {
                Letter letter = Instantiate<Letter>(_letterPrefab);
                letter.transform.SetParent(_grifLayout.transform);
                _letters.Add(letter);
            }
        }

    }

    public void SetWord()
    {
        string word = _wordRepository.GetRandomWord();
        for (int i = 0; i < _word.Length; i++)
            _word[i] = word[i];
    }

    public string GetWord()
    {
        return new string(_word);
    }
    public void EnterLetter(char c)
    {
        if(_index < _wordLength)
        {
            c = char.ToUpper(c);

            _letters[(_currentRow * _wordLength) + _index].EnterLetter(c);

            _guess[_index] = c;
            _index++;
        }
    }

    public void DeleteLetter()
    {
        if(_index > 0)
        {
            _index--;
            _letters[(_currentRow * _wordLength) + _index].DeleteLetter();
            _guess[_index] = null;  
        }
    }

    void Shake()
    {
        for(int  i =0; i < _wordLength; i++)
        {
            _letters[(_currentRow * _wordLength) + i].Shake();
        }
    }

    public void GuessWord()
    {
        if(_index != _wordLength)
        {
            Shake();
        }
        else
        {
            StringBuilder word = new StringBuilder();

            for(int i=0;i<_wordLength; i++)
                word.Append(_guess[i].Value);

            if (_wordRepository.CheckWordExist(word.ToString()))
            {
                bool incorrect = false;

                for(int i = 0; i < _wordLength; i++)
                {
                    bool correct = _guess[i] == _word[i];

                    if (!correct)
                    {
                        incorrect = true;

                        bool letterExistsInWord = false;
                        
                        for(int j=0; j < _wordLength; j++)
                        {
                            letterExistsInWord = _guess[i] == _word[j];

                            if (letterExistsInWord)
                                break;
                        }

                        StartCoroutine(PlayeLetter(i * letterAnimationOffsetTime, (_currentRow * _wordLength) + i, letterExistsInWord ? LetterState.WrongLocation : LetterState.Incorrect));
                    }
                    else
                    {
                        StartCoroutine(PlayeLetter(i * letterAnimationOffsetTime, (_currentRow * _wordLength) + i,LetterState.Correct)); 

                    }
                }

                if (incorrect)
                {
                    _index = 0;
                    _currentRow++;

                    if(_currentRow>= _amountOfRows)
                    {
                        PuzzleState = PuzzleState.Failed;
                    }
                }
                else
                {
                    PuzzleState = PuzzleState.Complete;
                }
            }
        }
    }

    IEnumerator PlayeLetter(float offset,int index, LetterState letterState)
    {
        yield return new WaitForSeconds(offset);

        _letters[index].SetState(letterState);

        int indexOfChar = (int)_letters[index].Entry.Value - (int)'A';

        KeyCode keyCode = indexOfChar + KeyCode.A;

        foreach(Key key in _keys)
        {
            if(key.KeyCode == keyCode)
            {
                key.SetState(letterState);
                break;
            }
        }
    }
}
