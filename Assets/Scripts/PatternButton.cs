using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PatternButton : MonoBehaviour, IPointerDownHandler
{
    public Color NormalColor;
    public Color SelectedColor;
    public bool isSelected;
    public Image image;
    public TextMeshProUGUI symbol;
    public int Row;
    public int Column;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isSelected)
        {
            image.color = NormalColor;
            symbol.text = "-";
            isSelected = false;
        }
        else
        {
            image.color = SelectedColor;
            symbol.text = "O";
            isSelected = true;

        }
    }

    public void EnableButton()
    {
        image.color = SelectedColor;
        symbol.text = "O";
        isSelected = true;
    }
}
