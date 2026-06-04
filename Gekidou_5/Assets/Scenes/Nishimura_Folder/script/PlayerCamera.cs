using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    public GameObject player;
    Vector3 prePlayerPos;



    // Use this for initialization
    void Start()
    {

    
    }

    void Update()
    {
        if (player.transform.position != prePlayerPos)
        {
            transform.position = new Vector3(player.transform.position.x + 0, player.transform.position.y + 0, -10);
            prePlayerPos = player.transform.position;
        }
    }
}
