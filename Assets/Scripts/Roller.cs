using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Roller : MonoBehaviour
{
    public RectTransform strip; 
    public float spinSpeed = 300f; 
    public float symbolHeight = 189.1903f; 
    public float symbolSpacing = 30f; 
    public List<Image> rollerFigures;
    
    private float targetPositionY; 
    private Vector3 initalPos;
    private bool isSpinning = false;
    private float offset;
    void Start()
    {
        initalPos = strip.localPosition;
        offset = initalPos.y % 220;

    }
    
    public IEnumerator SpinForDuration(float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            strip.localPosition -= new Vector3(0, spinSpeed * Time.deltaTime, 0);
            if (strip.localPosition.y - symbolHeight - symbolSpacing <= -strip.rect.height / 2)
            {
                strip.localPosition = initalPos;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        SnapToNearestValidPosition();
    }
    private void SnapToNearestValidPosition()
    {
        float totalSymbolHeight = 220;
        float currentY = strip.localPosition.y;
        float nearestValidPositionY = Mathf.Round(currentY / totalSymbolHeight) * totalSymbolHeight + offset;
        targetPositionY = nearestValidPositionY;
        strip.transform.localPosition = new Vector3(strip.transform.localPosition.x, targetPositionY,
            strip.transform.localPosition.z);
        
        if (strip.localPosition.y - 220 <= -strip.rect.height / 2 || strip.localPosition.y > initalPos.y)
        {
            strip.localPosition = initalPos;
        }
        
    }

    public string GetFigureAt(int i)
    {
        int currentFigure = Mathf.Abs(Mathf.RoundToInt((strip.localPosition.y - initalPos.y) / 220));
        return rollerFigures[rollerFigures.Count - currentFigure + i - 3].sprite.name;
    }
}
