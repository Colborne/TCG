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
        Evolve,
        Bomb
    }

    public enum Phase
    {
        Beginning,
        During,
        End
    }
    public string title;
    public int SPR;
    public Ability ability; 
    public Phase phase;
    public Sprite portrait;
    public int cardPosition;
    public Card evolution;
    public GameObject effect;

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
            case Ability.Bomb:
                player.hp -= SPR;
                player.field[cardPosition] = null;
                player.visibleField[cardPosition].image.sprite = player.UISprite;
                break;
            case Ability.Damage:
                Damage(player, target);
                break;
            case Ability.Heal:
                Instantiate(effect, transform.position, Quaternion.identity);
                player.hp += SPR;
                break;
            case Ability.Summoning:
                Instantiate(effect, transform.position, Quaternion.identity);
                player.sp += SPR;
                break;
            case Ability.Duplicate:
                for(int i = 0; i < 5; i++)
                {
                    if(player.field[i] == null)
                    {
                        player.field[i] = this;
                        player.visibleField[i].image.sprite = this.portrait;
                        return;
                    }
                }
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

    public void Damage(Player player, Player target)
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
                int damage = 0;
                RectTransform rect = player.visibleField[cardPosition].GetComponent<RectTransform>();
                var attack = Instantiate(effect, Vector3.zero, rect.rotation);
                attack.GetComponent<Projectile>().destination = new Vector3(target.visibleField[i].GetComponent<RectTransform>().localPosition.x,
                    target.visibleField[i].GetComponent<RectTransform>().localPosition.y,
                    -50);

                if(i == cardPosition - 1)
                    damage = Mathf.Max(0, leftDamage - target.field[i].SPR);
                else if(i == cardPosition)
                    damage = Mathf.Max(0, mainDamage - target.field[i].SPR);
                else if(i == cardPosition + 1)
                    damage = Mathf.Max(0, rightDamage - target.field[i].SPR);

                if(damage > 0)
                {
                    target.hp -= damage;
                    target.field[i] = null;
                    target.visibleField[i].image.sprite = target.UISprite;
                }

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