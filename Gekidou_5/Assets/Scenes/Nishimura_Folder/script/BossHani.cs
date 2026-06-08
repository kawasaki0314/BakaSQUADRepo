using UnityEditor.Build;
using UnityEngine;

public class BossHani : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionStay2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            levelupplayer player = other.gameObject.GetComponent<levelupplayer>();
            player.hp -= 10;
            Debug.Log("10ダメージ受けた");
        }
    }
}
