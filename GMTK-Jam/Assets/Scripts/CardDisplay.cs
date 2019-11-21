using Models;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardDisplay : MonoBehaviour, IPointerClickHandler
{
    public Card cardBehaviour;

    public Image cardImage;


    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(cardBehaviour.Name);
    }
}