using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Models;
using UIScripts;
using UnityEngine;

public sealed class GameInitializer : MonoBehaviour
{
    [SerializeField] private GameObject controller;
    [SerializeField] private GameObject cardHand;
    [SerializeField] private GameObject cardEventHandManagerParent;
    [SerializeField] private GameObject enemySpawner;
    [SerializeField] private Player player;
    [SerializeField] private GameObject notifications;
    
    
    public static bool FinishedInitializing;

    private void Awake()
    {
        FinishedInitializing = false;
        LoadCards();
        LoadEnemyAbilities();
        LoadEnemies();
        
                
        Game.UnlockableCards = Game.AllCards.Where(e => e.InDeck == false).ToList();
        Game.UnlockedCards = new List<Card>();
        
        var deck = Game.PlayerDeck = controller.AddComponent<Deck>();
        deck.Initialize();

        Game.Notification = notifications;

        var cardHandEventManager = Game.CardHandEventManager = cardEventHandManagerParent.GetComponent<CardHandEventManager>();
        //cardHandEventManager.Initialize();
        Game.Spawner = enemySpawner.GetComponent<EnemySpawner>();
        Game.GameLogic = controller.GetComponent<GameLogic>();
        Game.Player = player;
        
        Game.CurrentCombatSystem = new CombatSystem();
        FinishedInitializing = true;
    }

    private static void LoadCards()
    {
        var xml = (TextAsset) Resources.Load("CardList", typeof(TextAsset));
        var serializer = new XmlSerializer(typeof(CardList));
        using (TextReader reader = new StringReader(xml.text))
        {
            var result = (CardList) serializer.Deserialize(reader);
            Game.AllCards = result.Cards;
        }
    }

    private static void LoadEnemyAbilities()
    {
        var xml = (TextAsset) Resources.Load("EnemyAbilityList", typeof(TextAsset));
        var serializer = new XmlSerializer(typeof(EnemyAbilityList));
        using (TextReader reader = new StringReader(xml.text))
        {
            var result = (EnemyAbilityList) serializer.Deserialize(reader);
            Game.AllAbilities = result.Abilities;
        }
    }

    private static void LoadEnemies()
    {
        var enemies = new List<Enemy>();
        var xml = (TextAsset) Resources.Load("EnemyList", typeof(TextAsset));
        using (XmlReader reader = XmlReader.Create(new StringReader(xml.text)))
        {
            string name = string.Empty,
                ability = String.Empty;
            int health = 0,
                chance = 0;
            bool isBoss =false;
            while (reader.Read())
            {
                if (reader.NodeType != XmlNodeType.EndElement)
                {
                    switch (reader.Name)
                    {
                        case "Name":
                            name = reader.ReadInnerXml();
                            break;
                        case "Health":
                            health = Int32.Parse(reader.ReadInnerXml());
                            break;
                        case "Ability":
                            ability = reader.ReadInnerXml();
                            break;
                        case "Chance":
                            chance = Int32.Parse(reader.ReadInnerXml());
                            break;
                        case "IsBoss":
                            isBoss =  bool.Parse(reader.ReadInnerXml());
                            break;
                        case "Enemy":
                            if (name != String.Empty)
                            {
                                Enemy enemy = enemies.FirstOrDefault(e => e.Name == name);
                                if (enemy == null)
                                {
                                    enemy = new Enemy()
                                    {
                                        Name = name,
                                        Health = health,
                                        EnemyAbilities = new Dictionary<EnemyAbility, int>(),
                                        IsBoss = isBoss
                                    };
                                    EnemyAbility ab = Game.AllAbilities.First(e => e.Name == ability);
                                    enemy.EnemyAbilities.Add(ab, chance);
                                    enemies.Add(enemy);
                                }
                                else
                                {
                                    enemy.EnemyAbilities.Add(Game.AllAbilities.First(e => e.Name == ability), chance);
                                }
                            }

                            break;
                    }
                }
            }
        }
        Game.EnemyPool = enemies.Where(e=> e.IsBoss == false).ToList();
    }
}