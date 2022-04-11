using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Linq;

public class Player : MonoBehaviour
{
    public List<Card> allCards;
    Queue<Card> deck;
    public Card[] hand;
    public Button[] visibleHand;
    public Button[] field;
    public Card currentCard;
    public Sprite UISprite;
    public TMP_Text deckSize;
    public int hp, sp;

    private void Start() 
    {
        deck = new Queue<Card>();
        hand = new Card[5];
        hp = 10;
        sp = 1;

        for(int i = 0; i < 20; i++)
            deck.Enqueue(allCards[UnityEngine.Random.Range(0,allCards.Count)]);

        for(int i = 0; i < 5; i++)
            hand[i] = deck.Dequeue();
    }

    private void Update() 
    {
        for(int i = 0; i < 5; i++){
            if(hand[i] == null){
                if(deck.Count > 0)
                    hand[i] = deck.Dequeue();
                else    
                    visibleHand[i].gameObject.SetActive(false);
            }
            else
            {
                visibleHand[i].image.sprite = hand[i].portrait;
                visibleHand[i].gameObject.SetActive(true);
            }
        }
        deckSize.text = deck.Count.ToString();
    }

    public void PlayCard(Button button)
    {
        var index = Array.IndexOf(visibleHand, button);
        currentCard = hand[index];
        hand[index] = null;
    }

    public void MoveToField(Button button) 
    {
        var index = Array.IndexOf(field, button);
        if(field[index].image.sprite == UISprite)
        {
            field[index].image.sprite = currentCard.portrait;
            currentCard = null;
        }
    }
}