using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace UIScripts
{
    public class CardHandEventManager : MonoBehaviour
    {
        [HideInInspector]
        public bool isCardSelected = false;
        public Models.Card selectedCard;
        public CardMouseEvents selectedCardPrefab;
        private List<Transform> cardSlots = new List<Transform>();
        private List<GameObject> currentHandInstances = new List<GameObject>();
        public GameObject cardPrefab;

        // Start is called before the first frame update
        void Start()
        {
            foreach (Transform child in transform) cardSlots.Add(child);
        }



        public void SpawnCards(Models.Card[] cards)
        {
            selectedCard = null;
            isCardSelected = false;

            foreach (Transform child in transform) cardSlots.Add(child);
            
            foreach (GameObject card in currentHandInstances)
            {
                Destroy(card);
            }

            int i = 0;
            foreach (Models.Card card in cards)
            {
                GameObject newCard = Instantiate(cardPrefab, cardSlots[i]);
                newCard.GetComponent<CardMouseEvents>().card = card;
                //newCard.transform.Find("Name").GetComponent<TextMeshPro>().text = card.Name;
                newCard.transform.Find("AttackValue").GetComponent<TextMeshPro>().text = card.Damage.ToString();
                newCard.transform.Find("DefenceValue").GetComponent<TextMeshPro>().text = card.SelfBlock.ToString();
                newCard.transform.Find("HealValue").GetComponent<TextMeshPro>().text = card.Heal.ToString();
                newCard.transform.GetComponent<SpriteRenderer>().sprite = Resources.Load(card.Name.Replace(" ", ""), typeof(Sprite)) as Sprite;

                string tags = "";
                if (card.BlockRate > 0) tags += "Block Rate: " + card.BlockRate + "\n";
                if (card.DamageBuff > 0) tags += "Damage Buff: " + card.DamageBuff + "\n";
                if (card.DamageReduction > 0) tags += "Damage Reduction: " + card.DamageReduction + "\n";
                if (card.DotDamage > 0) tags += "DOT: " + card.DotDamage + "\n";
                if (card.ArmorPiercing) tags += "Armor Piercing\n";
                if (card.Stun) tags += "Stun \n";
                if (card.ComboBreaker) tags += "Combo Breaker \n";
                if (card.Aoe) tags += "AOE \n";

                newCard.transform.Find("Tags").GetComponent<TextMeshPro>().text = tags;

                currentHandInstances.Add(newCard);
                i++;
            }
        }
    }
}