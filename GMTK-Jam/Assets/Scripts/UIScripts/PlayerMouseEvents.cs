using System.Collections;
using System.Collections.Generic;
using UIScripts;
using UnityEngine;

public class PlayerMouseEvents : MonoBehaviour
{
    private Player playerScript;
    private CardHandEventManager handManager;
    public float highlightScaleFactor = 1.5f;
    private Vector3 initialScale;

    // Start is called before the first frame update
    void Start()
    {
        playerScript = GetComponent<Player>();
        handManager = FindObjectOfType<CardHandEventManager>();
        initialScale = transform.localScale;
    }

    private void OnMouseEnter()
    {
        if(handManager.isCardSelected && handManager.selectedCard.Targets == -1)
        {
            highlightPlayer();
        }
    }

    private void OnMouseExit()
    {
        unhighlightPlayer();
    }

    private void OnMouseDown()
    {
        if (handManager.isCardSelected && handManager.selectedCard.Targets == -1)
        {
            playerScript.ReceiveHealing(handManager.selectedCard.Heal);
            playerScript.shield += handManager.selectedCard.SelfBlock;
            playerScript.ReceiveDamageDebuff(-1 * handManager.selectedCard.DamageBuff);

            Game.CurrentCombatSystem.CurrentCombat.EndTurn();
        }
    }

    private void highlightPlayer()
    {
        transform.localScale *= highlightScaleFactor;
    }

    private void unhighlightPlayer()
    {
        transform.localScale = initialScale;
    }
}
