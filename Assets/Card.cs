using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public enum Ability
    {
        Draw,
        Damage,
        Heal,
        Summoning,
        Duplicate,
        Swap,
        Evolve
    }

    public enum Phase
    {
        Beginning,
        End
    }
    public string title;
    public int SPR;
    public Ability ability; 
    public Phase phase;
    public Sprite portrait;
    public int cardPosition;
    public Card evolution;

    public int[] attackPattern;
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = portrait;
    }

    public void UseAbility(Player player, Player target)
    {
        switch(ability)
        {
            case Ability.Draw:
                player.Draw();
                break;
            case Ability.Damage:
                Damage(target);
                break;
            case Ability.Heal:
                player.hp += SPR;
                break;
            case Ability.Summoning:
                player.sp += SPR;
                break;
            case Ability.Swap:
                if(target.field[cardPosition] != null)
                {
                    Card temp = target.field[cardPosition];
                    target.field[cardPosition] = player.field[cardPosition];
                    player.field[cardPosition] = temp;
                    
                    player.visibleField[cardPosition].image.sprite = player.field[cardPosition].portrait;
                    target.visibleField[cardPosition].image.sprite = target.field[cardPosition].portrait;
                }
                break;
            case Ability.Evolve:
                if(evolution != null)
                {
                    player.field[cardPosition] = evolution;
                    player.visibleField[cardPosition].image.sprite = evolution.portrait;
                    player.field[cardPosition].cardPosition = cardPosition;
                }
                break;
        }
    }

    public void Damage(Player target)
    {
        int leftDamage = 0;
        int mainDamage = 0;
        int rightDamage = 0;

        if(attackPattern.Length == 3)
        {
            leftDamage = attackPattern[0];
            mainDamage = attackPattern[1];
            rightDamage = attackPattern[2];
        }
        else
            mainDamage = attackPattern[0];

        for(int i = 0; i < 5; i++)
        {
            if(target.field[i] != null)
            {
                if(i == cardPosition - 1)
                    target.hp -= Mathf.Max(0, leftDamage - target.field[i].SPR);
                else if(i == cardPosition)
                    target.hp -= Mathf.Max(0, mainDamage - target.field[i].SPR);
                else if(i == cardPosition + 1)
                    target.hp -= Mathf.Max(0, rightDamage - target.field[i].SPR);
            }
            else
            {
                if(i == cardPosition - 1)
                    target.hp -= leftDamage;
                else if(i == cardPosition)
                    target.hp -= mainDamage;
                else if(i == cardPosition + 1)
                    target.hp -= rightDamage;
            }
        }
    }
}
