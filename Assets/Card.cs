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
    public bool justPlayed = true;
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
                var heal = Instantiate(effect, player.visibleField[cardPosition].GetComponent<RectTransform>().localPosition, Quaternion.identity);
                heal.GetComponent<RectTransform>().SetParent(FindObjectOfType<Canvas>().transform);
                heal.GetComponent<RectTransform>().localPosition = new Vector3(
                    player.visibleField[cardPosition].GetComponent<RectTransform>().localPosition.x, 
                    player.visibleField[cardPosition].GetComponent<RectTransform>().localPosition.y, 
                    -50);
                heal.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
                player.hp += SPR;
                break;
            case Ability.Summoning:
                var sum = Instantiate(effect, player.visibleField[cardPosition].GetComponent<RectTransform>().localPosition, Quaternion.identity);
                sum.GetComponent<RectTransform>().SetParent(FindObjectOfType<Canvas>().transform);
                sum.GetComponent<RectTransform>().localPosition = new Vector3(
                    player.visibleField[cardPosition].GetComponent<RectTransform>().localPosition.x, 
                    player.visibleField[cardPosition].GetComponent<RectTransform>().localPosition.y, 
                    -50);
                sum.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
                player.sp += SPR;
                break;
            case Ability.Duplicate:
                for(int i = 0; i < 5; i++)
                {
                    if(player.field[i] == null)
                    {
                        var dupe = Instantiate(this);
                        player.field[i] = dupe;
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
        bool[] spread = new bool[3];

        if(attackPattern.Length == 3)
        {
            leftDamage = attackPattern[0];
            mainDamage = attackPattern[1];
            rightDamage = attackPattern[2];
            spread[0] = true; 
            spread[1] = true; 
            spread[2] = true;
        }
        else
            mainDamage = attackPattern[0];
            spread[0] = false; 
            spread[1] = true; 
            spread[2] = false;

        for(int i = 0; i < 5; i++)
        {
            if(target.field[i] != null)
            {
                int damage = 0;
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
                    AttackSetup(player,target,i);
                }
            }
            else
            {
                if(i == cardPosition - 1 && leftDamage > 0){
                    target.hp -= leftDamage;
                    AttackSetup(player,target,i);
                }
                else if(i == cardPosition && mainDamage > 0)
                {
                    target.hp -= mainDamage;
                    AttackSetup(player,target,i);
                }
                else if(i == cardPosition + 1 && rightDamage > 0){
                    target.hp -= rightDamage;
                    AttackSetup(player,target,i);
                }
            }
        }
    }

    void AttackSetup(Player player, Player target,int i) 
    {
        RectTransform rect = player.visibleField[cardPosition].GetComponent<RectTransform>();
        var attack = Instantiate(effect, rect.localPosition, rect.rotation);
        attack.GetComponent<RectTransform>().SetParent(FindObjectOfType<Canvas>().transform);
        attack.GetComponent<RectTransform>().localPosition = new Vector3(
            player.visibleField[cardPosition].GetComponent<RectTransform>().localPosition.x, 
            player.visibleField[cardPosition].GetComponent<RectTransform>().localPosition.y, 
            -50);
        attack.GetComponent<Projectile>().destination = new Vector3(
            target.visibleField[i].GetComponent<RectTransform>().localPosition.x,
            target.visibleField[i].GetComponent<RectTransform>().localPosition.y,
            -50);
        attack.GetComponent<RectTransform>().localScale = new Vector3(1,1,1); 
    }
}