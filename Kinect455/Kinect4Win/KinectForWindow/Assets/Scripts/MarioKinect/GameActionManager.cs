using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAction;
public class GameActionManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public Action gameAction = Action.STAND;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(gameAction == Action.STAND)
        {

        } else if(gameAction == Action.RUN)
        {

        } else if(gameAction == Action.JUMP_LEFT)
        {

        }
    }
}
