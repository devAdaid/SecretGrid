using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class NicknameGenerator : MonoBehaviour
{
    [SerializeField]
    private TextAsset nounsEn;
    [SerializeField]
    private TextAsset adjectivesEn;
    [SerializeField]
    private TextAsset nounsKo;
    [SerializeField]
    private TextAsset adjectivesKo;
    [SerializeField]
    private TextMeshProUGUI nickname;

    private readonly List<string> nouns = new List<string>();
    private readonly List<string> adjectives = new List<string>();

    private void Start()
    {
        var nounStrings = nounsEn.text;
        var adjectiveStrings = adjectivesEn.text;
        if (Application.systemLanguage == SystemLanguage.Korean)
        {
            nounStrings = nounsKo.text;
            adjectiveStrings = adjectivesKo.text;
        }
        
        using var srNoun = new StringReader(nounStrings);
        while (srNoun.ReadLine() is { } line)
        {
            nouns.Add(line.Trim());
        }
            
        using var srAdj = new StringReader(adjectiveStrings);
        while (srAdj.ReadLine() is { } line)
        {
            adjectives.Add(line.Trim());
        }
    }

    public void OnGenerate()
    {
        var adj = adjectives[Random.Range(0, adjectives.Count)];
        adj = adj[0].ToString().ToUpper() + adj[1..];
        var noun = nouns[Random.Range(0, nouns.Count)];
        nickname.text = $"{adj} {noun}";
    }
}
