using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordRepository : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The Text Asset with the words")]
    private TextAsset _wordList;

    List<string> _words;

    private void Awake()
    {
        _words = new List<string>(_wordList.text.Split(new char[] {',',' ','\n','\r'},System.StringSplitOptions.RemoveEmptyEntries));
    }

    public string GetRandomWord()
    {
        return _words[Random.Range(0, _words.Count)];
    }

    public bool CheckWordExist(string word)
    {
        return _words.Contains(word);
    }
}
