using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public Player playerOne, playerTwo;
    public Player currentPlayer, target;
    public bool startingTurn;
    public int whichPlayer = 1;

    void Start()
    {
        startingTurn = false;
        currentPlayer = playerOne;
        target = playerTwo;
    }

    void Update()
    {
        if(startingTurn)
        {
            NewTurn();
            startingTurn = false;
        }
    }

    void NewTurn()
    {
        currentPlayer.currentTurn.enabled = true;
        target.currentTurn.enabled = false;
        currentPlayer.sp++;
        currentPlayer.Draw();

        for(int i = 0; i < currentPlayer.field.Length; i++)
        {
            if(currentPlayer.field[i] != null && currentPlayer.field[i].phase == Card.Phase.Beginning)
                currentPlayer.field[i].UseAbility(currentPlayer, target);
        }      
    }
    
    public void EndTurn()
    {
        for(int i = 0; i < currentPlayer.field.Length; i++)
        {
            if(currentPlayer.field[i] != null && currentPlayer.field[i].phase == Card.Phase.End)
                currentPlayer.field[i].UseAbility(currentPlayer, target);
        }      

        if(currentPlayer == playerOne)
        {
            currentPlayer = playerTwo;
            target = playerOne;
        }
        else
        {
            currentPlayer = playerOne;
            target = playerTwo;
        }
        
        startingTurn = true;
    }
}
