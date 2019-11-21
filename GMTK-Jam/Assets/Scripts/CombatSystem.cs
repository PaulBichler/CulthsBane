using System;
using System.Collections.Generic;
using Interfaces;
using Models;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class CombatSystem
{
    public Combat CurrentCombat;
    public int TurnNumber;
    public Card[] CurrentHand;
    public Queue<Combat> CombatHistory = new Queue<Combat>();
    public List<EnemyBehavior> Enemies = new List<EnemyBehavior>();
    
    #region Delegates & Events

    public delegate void OnReceiveDamage (int damageAmount, IDamageable target);
    public delegate void OnDrawNewHand(Card[] cards);
    
    
    public event OnReceiveDamage DamageReceived;
    public event OnDrawNewHand GetNewHand;

    #endregion

    public void ApplyDamageToTargets(int damage, IEnumerable<IDamageable> targets)
    {
        foreach (var target in targets)
        {
            target.ApplyDamage(damage, target);
        }
    }
    
    private void ReduceTargetHealth(int damage, IDamageable target)
    {
        target.ReceiveDamage(damage);
    }
    
    public CombatSystem()
    {
        DamageReceived += ReduceTargetHealth;
        GetNewHand += DisplayNewHand;
    }


    private static void DisplayNewHand(Card[] cards)
    {
        Game.CardHandEventManager.SpawnCards(cards);
    }

    public struct Combat
    {
        public EventHandler OnTurnCompleted;

        public List<Card> CardsInCombat;
        public List<int> Enemies;

        public bool IsPlayerTurn;
        
        public int TurnNumber;


        public void PlayCard (Card card)
        {

        }
        
        public void EndTurn()
        {
            OnTurnCompleted.Invoke(this, new CombatEventArgs(TurnNumber, IsPlayerTurn, Enemies, CardsInCombat));
        }
    }


    private static void NotifyUsedCard(Card card)
    {
        //Debug.Log(card.GetCardType());
    }
    
    private void NextTurn(object sender, EventArgs eventArgs)
    {
        Debug.Log(CurrentCombat);
        if (!CurrentCombat.IsPlayerTurn)
        {
            foreach (var enemy in Enemies)
            {
                enemy.PerformCombatAction();
            }
        }
        
        if (eventArgs is CombatEventArgs combatEventArgs)
        {
            //Debug.Log(combatEventArgs.TurnNumber);
        }
        
        CombatHistory.Enqueue(CurrentCombat);

        TurnNumber++;

        CurrentHand = Game.PlayerDeck.DrawHand();
        GetNewHand?.Invoke(CurrentHand);

        var lastCombat = CurrentCombat;
        
        CurrentCombat = new Combat
        {
            OnTurnCompleted = NextTurn,
            TurnNumber = TurnNumber,
            IsPlayerTurn = !lastCombat.IsPlayerTurn
        };
    }
    
    private class CombatEventArgs : EventArgs
    {
        public readonly int TurnNumber;
        private List<int> _enemies;
        private List<Card> _cards;

        private bool _wasPlayerTurn;
        

        public CombatEventArgs(int turnNumber, bool wasPlayerTurn, List<int> enemies, List<Card> cards)
        {
            TurnNumber = turnNumber;
            _wasPlayerTurn = wasPlayerTurn;
            _enemies = enemies;
            _cards = cards;
        }
    }

    public void StartCombat(bool playerStarts)
    {
        CurrentHand = Game.PlayerDeck.DrawHand();
        GetNewHand?.Invoke(CurrentHand);
        CombatHistory = new Queue<Combat>();
        TurnNumber = 1;
        
        CurrentCombat = new Combat
        {
            OnTurnCompleted = NextTurn,
            CardsInCombat = new List<Card>(),
            TurnNumber = TurnNumber,
            IsPlayerTurn = playerStarts
        };

        //Spawn Enemies
        int enemyCount = Random.Range(1, 5);
        int enemyPoolAmount = Game.EnemyPool.Count;
        for (var i = 0; i < enemyCount; i++)
        {
            var enemyToSpawn = Game.Spawner.SpawnEnemy(Game.EnemyPool[Random.Range(0, enemyPoolAmount)], i);
            this.Enemies.Add(enemyToSpawn);
        }




        //CurrentCombat.EndTurn();
    }
}