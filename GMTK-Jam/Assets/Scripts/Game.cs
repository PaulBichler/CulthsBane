using System;
using System.Collections.Generic;
using Models;
using UIScripts;
using UnityEngine;

public static class Game
{
    public static CombatSystem CurrentCombatSystem;
    public static List<Card> AllCards;
    public static List<Card> UnlockedCards;
    public static List<Card> UnlockableCards;
    public static Deck PlayerDeck;
    public static List<EnemyAbility> AllAbilities;
    public static List<Enemy> EnemyPool;
    public static Player Player;
    public static DeckDisplay DeckDisplay;
    public static GameLogic GameLogic;
    public static CardHandEventManager CardHandEventManager;
    public static EnemySpawner Spawner;
    public static GameObject Notification;
}