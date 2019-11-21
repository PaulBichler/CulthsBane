using System;
using System.Collections.Generic;
using System.Linq;
using Models;
using UnityEngine;
using Random = UnityEngine.Random;

public class Deck : MonoBehaviour
{
    public int exhaustionStart = 10;
    public int exhaustionBeginAmount = 1;
    public float exhaustionMultiplier = 2;
    
    private List<Card> _deckCards;
    private List<Card> _cards;
    
    public void Initialize()
    {
        Game.PlayerDeck = this;
        
        this._deckCards = Game.AllCards.Where(e => e.InDeck == true).ToList();
        this._deckCards.AddRange(Game.UnlockedCards);
        foreach (var card in this._deckCards.FindAll(e => e.Copies > 1))
        {
            for (int i = 0; i < card.Copies - 1; i++)
            {
                this._deckCards.Add(card);
            }
        }
        this._cards = new List<Card>(this._deckCards);
    }
    
    
    public Card[] DrawHand()
    {
        Card[] cards =
        {
            _cards[Random.Range(1, _cards.Count - 1)],
            _cards[Random.Range(1, _cards.Count - 1)],
            _cards[Random.Range(1, _cards.Count - 1)]
        };
        foreach (var card in cards)
        {
            if (card.Copies != -1)
            {
                _cards.Remove(card);
            }
        }
        if (Game.CurrentCombatSystem.TurnNumber >= exhaustionStart)
        {
            Exhaustion();
        }
        
        return cards;
    }

    private void Exhaustion()
    {
        for (int i = 0; i < Math.Round(exhaustionStart * exhaustionMultiplier); i++)
        {
            this._cards.Add(Game.AllCards[0]);
        }
    }
}