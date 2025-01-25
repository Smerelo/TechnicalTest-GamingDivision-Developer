using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SlotManager : MonoBehaviour
{
    public List<Roller> Rollers;
    public float delayBetweenRollers = 0.1f;
    public List<Slot> Connectors;
    public List<LineDrawer> LineDrawers;
    public List<WinningsTable> WinningsTables;
    public Button spinButton;
    public int startingCredits = 1000;
    private int credits = 0;
    public int spinCost = 10;
    public TextMeshProUGUI CreditsTMP;
    public PatternEditor PatternEditor;
    public GameObject winningsPanel;
    public TextMeshProUGUI winningsTMP;
    
    private bool isSpinning = false;
    private Slot[][] results = new Slot[3][];
    private Dictionary<string, WinningsTable> winningsDictionary = new Dictionary<string, WinningsTable>();
    private int roundWinnings = 0;
    
    
    private void Start()
    {
        for (int i = 0; i < results.Length; i++)
        {
            results[i] = new Slot[5];
        }

        for (int i = 0; i < results.Length; i++)
        {
            for (int j = 0; j < results[i].Length; j++)
            {
                results[i][j] = Connectors.Find(x => x.Coords == new Vector2(j, i));
            }
        }
        
        foreach (WinningsTable winningsTable in WinningsTables)
        {
            winningsDictionary.Add(winningsTable.Figure.name, winningsTable);
        }
        UpdateCredits(startingCredits);
    }

    public void OnClickSpin()
    {
        if (!isSpinning)
        {
            spinButton.interactable = false;
            isSpinning = true;
            UpdateCredits(-spinCost);
            StartCoroutine(StartRollers());
        }
    }

    private void UpdateCredits(int creditsToUpdate)
    {
        credits += creditsToUpdate;
        CreditsTMP.text = "CREDITS: " + credits;

    }
    
    
    private IEnumerator StartRollers()
    {
        float endTime = Random.Range(2f, 4f);

        for (int i = 0; i < Rollers.Count; i++)
        {
            StartCoroutine(Rollers[i].SpinForDuration(endTime));
            if (i != Rollers.Count - 1)
                yield return new WaitForSeconds(delayBetweenRollers);
        }

        yield return new WaitForSeconds(endTime);
        FillSlots();
        StartCoroutine(MatchFigures());

    }

    private void FillSlots()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                results[i][j].currentFigure = Rollers[j].GetFigureAt(i);
            }
        }

    }

    private IEnumerator MatchFigures()
    {
        int lineDrawersInUse = 0;
        for (int i = 0; i < results.Length; i++)
        {
            int matches = CheckLineMatches(results[i]);
            if (matches > 1)
            {
                LineDrawers[lineDrawersInUse].gameObject.SetActive(true);
                yield return StartCoroutine(LineDrawers[lineDrawersInUse].DrawMatchLine(results[i].Take(matches).ToList()));
                yield return StartCoroutine(CheckWinnings(results[i][0].currentFigure, matches));
                lineDrawersInUse++;
            }
        }

        if (PatternEditor.Patterns.Count > 0)
        {
            foreach (Pattern pattern in PatternEditor.Patterns)
            {
                List<Slot> slots = new List<Slot>();
                var selectedSlots = pattern.PatternButtons.FindAll(x => x.isSelected);
                if (selectedSlots.Count == 0)
                    continue;
                var figureToMatch = results[selectedSlots[0].Row][selectedSlots[0].Column].currentFigure;
                foreach (PatternButton slot in selectedSlots)
                {
                    if (results[slot.Row][slot.Column].currentFigure == figureToMatch)
                        slots.Add(results[slot.Row][slot.Column]);
                    else
                        break;
                }

                if (slots.Count == selectedSlots.Count)
                {
                    slots.Sort((a, b) =>
                    {
                        return a.Coords.x.CompareTo(b.Coords.x);              
                    });
                    LineDrawers[lineDrawersInUse].gameObject.SetActive(true);
                    yield return StartCoroutine(LineDrawers[lineDrawersInUse].DrawMatchLine(slots));
                    yield return StartCoroutine(CheckWinnings(figureToMatch, slots.Count));
                    lineDrawersInUse++;
                }
                
            }
        }
        foreach (LineDrawer lineDrawer in LineDrawers)
        {
            lineDrawer.gameObject.SetActive(false);
        }

        if (roundWinnings > 0)
        {
            winningsPanel.SetActive(true);
            winningsTMP.text = $"+{roundWinnings} CREDITS";
            yield return new WaitForSeconds(1f);
            winningsPanel.SetActive(false);
            roundWinnings = 0;
        }

        isSpinning = false;
        spinButton.interactable = true;
    }

    private IEnumerator CheckWinnings(string figure, int nbOfMatches)
    {
        if (nbOfMatches - 2 >  winningsDictionary[figure].Winnings.Length - 1)
        {
            nbOfMatches = 5;
        }
        roundWinnings += winningsDictionary[figure].Winnings[nbOfMatches - 2];
        UpdateCredits(winningsDictionary[figure].Winnings[nbOfMatches - 2]);
        yield return null;
    }
 
    
    private int CheckLineMatches(Slot[] resultSlots)
    {
        string figureToMatch = resultSlots[0].currentFigure;
        int matches = 1;
        for (int i = 1; i < resultSlots.Length; i++)
        {
            if (resultSlots[i].currentFigure != figureToMatch)
            {
                return matches;
            }
            matches++;
        }

        return matches;
    }
    
}
