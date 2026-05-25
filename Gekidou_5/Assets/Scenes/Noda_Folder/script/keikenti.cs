using UnityEngine;

public class keikenti : MonoBehaviour
{
    [Header("設定")]
    [SerializeField] private int expAmount = 20;//このアイテムでもらえる経験値
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))//Playerのタグがあるか
        {
            //今のオブジェクトからコンポーネントを取得
            levelupplayer levelupplayer = other.GetComponent<levelupplayer>();

            if(levelupplayer != null)
            {
                //経験値を獲得
                levelupplayer.Addexperience(expAmount);

                //アイテムを消滅させる
                Destroy(gameObject);
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
