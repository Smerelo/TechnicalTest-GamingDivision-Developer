using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    public class PatternEditor : MonoBehaviour
    {
        public GameObject PatternPrefab;
        public GameObject PatternsParent;
        public SaveManager SaveManager;
        [HideInInspector]
        public List<Pattern> Patterns = new List<Pattern>();



        public void AddPatternFromSaveFile(List<List<bool>> matchPattern)
        {
            Patterns.Add(Instantiate(PatternPrefab, Vector3.zero, 
                quaternion.identity, PatternsParent.transform).GetComponent<Pattern>());
            var p = Patterns[^1];
            for (int i = 0; i < matchPattern.Count; i++)
            {
                for (int j = 0; j < matchPattern[i].Count; j++)
                {
                    if (matchPattern[i][j])
                    {
                        p.PatternButtons.Find(x => x.Row == i && x.Column == j).EnableButton();
                    }
                }
            }
        }
        
        public void AddPattern()
        {
            Patterns.Add(Instantiate(PatternPrefab, Vector3.zero, 
                quaternion.identity, PatternsParent.transform).GetComponent<Pattern>()); 
        }

        public void AddPatternsToSaveManager()
        {
            SaveManager.matchPatternCollection.MatchPatterns.Clear();
            foreach (Pattern pattern in Patterns)
            {
                SaveManager.AddMatchPattern(pattern.PatternButtons);
            }
        }

        public void RemovePattern()
        { 
            var temp = Patterns[^1];
            Patterns.RemoveAt(Patterns.Count - 1);
            Destroy(temp.gameObject);
        }
    }
}