using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Models;
using UIScripts;
using TMPro;
using UnityEditor;

public class BossDrop : MonoBehaviour
{
    public GameObject card;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
        getBossDrop(Elements.Fire);
    }

    // Update is called once per frame
    /*void Update()
    {
        
    }*/
    
    

    public Card getBossDrop(Elements element)
    {
        Debug.Log("Create Temp List");
        List<Card> tempList = Game.UnlockableCards.FindAll(e => e.Element == element);

        System.Random rnd = new System.Random();
        Debug.Log(tempList.Count);
        int random = rnd.Next(tempList.Count);
        Card chosenCard = tempList[random];
        Debug.Log("get random Card" + chosenCard.Name);
        
        Game.UnlockableCards.Remove(chosenCard);
        Game.UnlockedCards.Add(chosenCard);
        
        Debug.Log("Add and remove");
        Vector3 pos = GameObject.FindWithTag("BossCardTag").transform.position;
        GameObject droppedCard = Instantiate(card, pos, Camera.main.transform.rotation);
        
        droppedCard.transform.Find("AttackValue").GetComponent<TextMeshPro>().text = chosenCard.Damage.ToString();
        droppedCard.transform.Find("DefenceValue").GetComponent<TextMeshPro>().text = chosenCard.SelfBlock.ToString();
        droppedCard.transform.Find("HealValue").GetComponent<TextMeshPro>().text = chosenCard.Heal.ToString();
        droppedCard.transform.GetComponent<SpriteRenderer>().sprite = Resources.Load(chosenCard.Name.Replace(" ",""),typeof(Sprite)) as Sprite;
        droppedCard.GetComponent<CardMouseEvents>().moveUpFactor = 0;
        
        string tags = "";
        if (chosenCard.BlockRate > 0) tags += "Block Rate: " + chosenCard.BlockRate + "\n";
        if (chosenCard.DamageBuff > 0) tags += "Damage Buff: " + chosenCard.DamageBuff + "\n";
        if (chosenCard.DamageReduction > 0) tags += "Damage Reduction: " + chosenCard.DamageReduction + "\n";
        if (chosenCard.DotDamage > 0) tags += "DOT: " + chosenCard.DotDamage + "\n";
        if (chosenCard.ArmorPiercing) tags += "Armor Piercing\n";
        if (chosenCard.Stun) tags += "Stun \n";
        if (chosenCard.ComboBreaker) tags += "Combo Breaker \n";
        if (chosenCard.Aoe) tags += "AOE \n";

        droppedCard.transform.Find("Tags").GetComponent<TextMeshPro>().text = tags;
        Debug.Log("Instanciated");

        droppedCard.AddComponent<BossCardDismiss>().bossCard = droppedCard;
        return chosenCard;
    }
}
