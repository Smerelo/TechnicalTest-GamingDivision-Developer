using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace DefaultNamespace
{
    public class SaveManager : MonoBehaviour
    {
        private string filePath;
        public MatchPatternCollection matchPatternCollection;
        public PatternEditor PatternEditor;
        void Awake()
        {
            filePath = Application.dataPath + "/MatchPatterns.json";
#if UNITY_EDITOR
            filePath = Application.persistentDataPath + "/MatchPatterns.json";

#endif
            
            
            LoadData();
        
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                SaveData();
            }
        }

        void OnApplicationQuit()
        {
            SaveData();
        }
        public void AddMatchPattern(List<PatternButton> patternButtons)
        {
            List<List<bool>> boolList = new List<List<bool>>(3);
            for (int i = 0; i < 3; i++)
            {
                List<bool> innerList = new List<bool>(5);
                for (int j = 0; j < 5; j++)
                {
                    innerList.Add(false);
                }
                boolList.Add(innerList);
            }
            for (int i = 0; i < patternButtons.Count; i++)
            {
                if (patternButtons[i].isSelected)
                {
                    boolList[patternButtons[i].Row][patternButtons[i].Column] = true;
                }
            }
            matchPatternCollection.MatchPatterns.Add(new MatchPattern(boolList));
        }
        
        public void LoadData()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                matchPatternCollection = JsonUtility.FromJson<MatchPatternCollection>(json);

                foreach (MatchPattern matchPattern in matchPatternCollection.MatchPatterns)
                {
                    var convertedPattern = new List<List<bool>>();
                    foreach (BoolRow row in matchPattern.Pattern)
                    {
                        convertedPattern.Add(row.Row);
                    }

                    PatternEditor.AddPatternFromSaveFile(convertedPattern);
                }
            }
            else
            {
                matchPatternCollection = new MatchPatternCollection();
                Debug.LogWarning("No save file found. Initialized new collection.");
            }
        }
        
        public void SaveData()
        {
        
            string json = JsonUtility.ToJson(matchPatternCollection, true);
            File.WriteAllText(filePath, json);
        }
        
    }
}