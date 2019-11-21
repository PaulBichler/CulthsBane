using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace UIScripts
{
    public class CardMouseEvents : MonoBehaviour
    {
        public float hoverZoomFactor = 2f;
        private Vector2 initialScale;
        private bool isSelected = false;

        public float moveUpFactor = 0f;
        private Vector3 initialPosition;
        private CardHandEventManager handManager;
        [HideInInspector]
        public Models.Card card;

        // Start is called before the first frame update
        void Start()
        {
            initialScale = transform.localScale;
            initialPosition = transform.position;
            handManager = GetComponentInParent<CardHandEventManager>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnMouseEnter()
        {
            if (!handManager.isCardSelected)
            {
                highlightCard();
            }
        }

        private void OnMouseExit()
        {
            if (!handManager.isCardSelected)
            {
                unhighlightCard();
            }
        }

        private void OnMouseDown()
        {
            if (handManager.isCardSelected)
            {
                if (handManager.selectedCardPrefab == this)
                {
                    unhighlightCard();
                    handManager.isCardSelected = false;
                    handManager.selectedCard = null;
                    handManager.selectedCardPrefab = null;
                }
            }
            else
            {
                handManager.isCardSelected = true;
                handManager.selectedCard = card;
                handManager.selectedCardPrefab = this;
            }
        }

        public void highlightCard()
        {
            transform.localScale *= hoverZoomFactor;
            foreach (Transform child in transform)
            {
                child.GetComponent<MeshRenderer>().sortingOrder = 1;
            }
            GetComponent<SpriteRenderer>().sortingOrder = 1;


            transform.position += new Vector3(0, moveUpFactor, 0);
        }

        public void unhighlightCard()
        {
            transform.localScale = initialScale;
            foreach (Transform child in transform)
            {
                child.GetComponent<MeshRenderer>().sortingOrder = 0;
            }
            GetComponent<SpriteRenderer>().sortingOrder = 0;

            transform.position = initialPosition;
        }
    }
}
