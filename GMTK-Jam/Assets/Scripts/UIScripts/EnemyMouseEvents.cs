using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;

namespace UIScripts
{
    public class EnemyMouseEvents : MonoBehaviour
    {
        public GameObject popUpPrefab;
        [HideInInspector]
        public int enemyIndex;
        private Camera cameraRef;
        private bool showPopUp = false;
        [HideInInspector]
        public GameObject popUpRef;
        private CardHandEventManager handManager;
        private Player playerScript;
        public float targetedZoomFactor = 1.5f;
        private Vector3 initialScale;
        private TextMeshProUGUI notification;

        // Start is called before the first frame update
        void Start()
        {
            cameraRef = Camera.main;
            handManager = FindObjectOfType<CardHandEventManager>();
            playerScript = FindObjectOfType<Player>();
            initialScale = transform.localScale;
            notification = GameObject.Find("Notifications").GetComponent<TextMeshProUGUI>();
        }

        // Update is called once per frame
        void Update()
        {
            if (showPopUp)
            {
                Vector3 mousePos = cameraRef.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;
                popUpRef.transform.position = mousePos;
            }
        }

        private void OnMouseEnter()
        {
            Vector3 mousePos = cameraRef.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            popUpRef = Instantiate(popUpPrefab, mousePos, transform.rotation);
            showPopUp = true;

            if (handManager.isCardSelected)
            {
                //Debug.Log("card selected");
                if (handManager.selectedCard.Targets != -1)
                {
                    //Debug.Log("mousedown 2");
                    if (handManager.selectedCard.Aoe)
                    {
                        foreach (EnemyBehavior enemy in Game.CurrentCombatSystem.Enemies)
                        {
                            enemy.GetComponent<EnemyMouseEvents>().highlightEnemy();
                        }
                    }
                    else
                    {
                        for (int i = 0; i < handManager.selectedCard.Targets; i++)
                        {
                            if (enemyIndex + i < Game.CurrentCombatSystem.Enemies.Count)
                            {
                                Game.CurrentCombatSystem.Enemies[enemyIndex + i].GetComponent<EnemyMouseEvents>().highlightEnemy();
                            }
                        }
                    }
                } else
                {
                    notification.text = "Wrong Target!";
                }
            }
        }

        private void OnMouseExit()
        {
            showPopUp = false;
            Destroy(popUpRef);

            foreach (EnemyBehavior enemy in Game.CurrentCombatSystem.Enemies)
            {
                enemy.GetComponent<EnemyMouseEvents>().unhighlightEnemy();
            }
            notification.text = "";
        }

        private void OnMouseDown()
        {
            if(handManager.isCardSelected && handManager.selectedCard.Targets != -1)
            {
                if (handManager.selectedCard.Aoe)
                {
                    if(handManager.selectedCard.Dot)
                    {
                        foreach(EnemyBehavior enemy in Game.CurrentCombatSystem.Enemies)
                        {
                            enemy.ReceiveDot((int)handManager.selectedCard.DotDamage, (int)handManager.selectedCard.Time);
                        }
                    } else
                    {
                        foreach (EnemyBehavior enemy in Game.CurrentCombatSystem.Enemies)
                        {
                            enemy.GetComponentInChildren<TextMeshPro>().text = enemy.ReceiveDamage(handManager.selectedCard.Damage, handManager.selectedCard.ArmorPiercing).ToString();
                            enemy.GetComponentInChildren<MeshRenderer>().sortingOrder = 1;
                            //DisplayDamage(enemy.ReceiveDamage(handManager.selectedCard.Damage, handManager.selectedCard.ArmorPiercing), enemy);
                            StartCoroutine(ClearDamage(enemy));
                        }
                    }

                    if(handManager.selectedCard.DamageReduction > 0)
                    {
                        foreach (EnemyBehavior enemy in Game.CurrentCombatSystem.Enemies)
                        {
                            enemy.ReceiveDamageDebuff(handManager.selectedCard.DamageReduction);
                        }
                    }

                    doPlayerStuff();
                    deselectCard();
                } else
                {
                    List<Interfaces.IDamageable> targets = new List<Interfaces.IDamageable>();

                    for(int i = 0; i < handManager.selectedCard.Targets; i++)
                    {
                        if(enemyIndex + i < Game.CurrentCombatSystem.Enemies.Count)
                        {
                            targets.Add(Game.CurrentCombatSystem.Enemies[enemyIndex + i]);
                        }
                    }

                    if (handManager.selectedCard.Dot)
                    {
                        foreach (EnemyBehavior enemy in targets)
                        {
                            enemy.ReceiveDot((int)handManager.selectedCard.DotDamage, (int)handManager.selectedCard.Time);
                        }
                    }
                    else
                    {
                        foreach (EnemyBehavior enemy in targets)
                        {
                            enemy.GetComponentInChildren<TextMeshPro>().text = enemy.ReceiveDamage(handManager.selectedCard.Damage, handManager.selectedCard.ArmorPiercing).ToString();
                            enemy.GetComponentInChildren<MeshRenderer>().sortingOrder = 1;
                            //DisplayDamage(enemy.ReceiveDamage(handManager.selectedCard.Damage, handManager.selectedCard.ArmorPiercing), enemy);
                            StartCoroutine(ClearDamage(enemy));
                        }
                    }

                    if (handManager.selectedCard.DamageReduction > 0)
                    {
                        foreach (EnemyBehavior enemy in targets)
                        {
                            enemy.ReceiveDamageDebuff(handManager.selectedCard.DamageReduction);
                        }
                    }

                    doPlayerStuff();
                    deselectCard();
                }
                Game.CurrentCombatSystem.CurrentCombat.EndTurn();
            }
        }

        private void highlightEnemy()
        {
            transform.localScale *= targetedZoomFactor;
        }

        private void unhighlightEnemy()
        {
            transform.localScale = initialScale;
        }

        private void deselectCard()
        {
            handManager.selectedCardPrefab.unhighlightCard();
            handManager.selectedCardPrefab = null;
            handManager.selectedCard = null;
            handManager.isCardSelected = false;
        }

        private void doPlayerStuff()
        {
            if (playerScript)
            {
                playerScript.ReceiveHealing(handManager.selectedCard.Heal);
                playerScript.ReceiveDamageDebuff(-1 * handManager.selectedCard.DamageBuff);
                playerScript.shield += handManager.selectedCard.SelfBlock;
            }
        }
        IEnumerator ClearDamage(EnemyBehavior enemy)
        {
            yield return new WaitForSecondsRealtime(1);
            enemy.GetComponentInChildren<TextMeshPro>().text = "";
        }
    }
}
