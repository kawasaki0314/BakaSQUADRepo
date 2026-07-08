using System;
using UnityEngine;


public class ItemZentai : MonoBehaviour
{

    [SerializeField] AudioSource itemDelete;
    Collision col;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      
    }

    //Playerに当たったら自分を消す
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {


            //  Debug.Log("ItemZentai: OnTriggerEnter2D");
            
            if (collision.gameObject.name == "HeelItem")
            {
                //col(gameObject);
            }
            else if (collision.gameObject.name == "SpeedUp")
            {
                
            }
            else if (collision.gameObject.name == "AttackUp")
            {

                
            }

            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
     

    }
}