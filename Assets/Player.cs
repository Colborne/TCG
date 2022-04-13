using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Linq;

public class Player : MonoBehaviour
{
    public List<GameObject> allCards;
    Queue<Card> deck;
    public Card[] hand;
    public Card[] field;
    public Button[] visibleHand;
    public Button[] visibleField;
    public Card currentCard;
    public Sprite UISprite;
    public TMP_Text deckSize;
    public TMP_Text spSize;
    public TMP_Text hpSize;
    public TMP_Text currentTurn;
    public Image tempCard;
    public int hp, sp;

    private void Start() 
    {
        deck = new Queue<Card>();
        hand = new Card[3];
        field = new Card[5];
        hp = 10;
        sp = 1;

        for(int i = 0; i < 20; i++)
            deck.Enqueue(allCards[UnityEngine.Random.Range(0,allCards.Count)].GetComponent<Card>());

        for(int i = 0; i < hand.Length; i++)
            hand[i] = deck.Dequeue();
    }

    private void Update() 
    {
        for(int i = 0; i < hand.Length; i++){
            if(hand[i] == null)
            {
                visibleHand[i].gameObject.SetActive(false);
            }
            else
            {
                visibleHand[i].image.sprite = hand[i].portrait;
                visibleHand[i].gameObject.SetActive(true);
            }
        }
        deckSize.text = deck.Count.ToString();
        spSize.text = "SP: " + sp.ToString();
        hpSize.text = "HP: " + hp.ToString();
        CheckMoving();
    }

    public void PlayCard(Button button)
    {
        if(FindObjectOfType<TurnManager>().currentPlayer == this)
        {
            var index = Array.IndexOf(visibleHand, button);

            if(sp >= hand[index].SPR)
            {
                currentCard = hand[index];
                hand[index] = null;
            }
        }
    }

    public void MoveToField(Button button) 
    {
        var index = Array.IndexOf(visibleField, button);
        if(visibleField[index].image.sprite == UISprite)
        {
            sp -= currentCard.SPR;
            field[index] = currentCard;
            field[index].cardPosition = index;
            visibleField[index].image.sprite = currentCard.portrait;
            currentCard = null;
        }
    }

    public void CheckMoving()
    {
        if(currentCard != null)
        {
            tempCard.transform.position = Input.mousePosition;
            tempCard.sprite = currentCard.portrait;
            tempCard.enabled = true;
        }
        else
            tempCard.enabled = false; 
    }

    public void Draw() 
    {
        if(deck.Count > 0)
        {
            for(int i = 0; i < hand.Length; i++)
            {
                if(hand[i] == null)
                {
                    hand[i] = deck.Dequeue();
                    return;
                }
            }

            if(hand.Length < 5)
            {
                Array.Resize(ref hand, hand.Length + 1);
                hand[hand.Length - 1] = deck.Dequeue();
                return;
            }
        }
    }
}