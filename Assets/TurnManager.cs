using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public Player playerOne, playerTwo;
    public bool startingTurn;
    void Update()
    {
        
    }

    void NewTurn()
    {
        playerOne.sp++;
    }
}
