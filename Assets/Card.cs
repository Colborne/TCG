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
        Swap
    }
    public string title;
    public int SPR;
    public Ability ability; 
    public Sprite portrait;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = portrait;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
