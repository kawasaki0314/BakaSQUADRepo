using UnityEngine;

public class DamageTrigger2 : MonoBehaviour
{
    public AIHoming2 aiHoming2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        aiHoming2 = GetComponentInParent<AIHoming2>();
    }

    //プレイヤーに当たった瞬間（最初の1発）
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("プレイヤーに接触！");

            levelupplayer playerHealth = collision.GetComponent<levelupplayer>();
            if (playerHealth != null)
            {
                playerHealth.damage(aiHoming2.attackPower);
                aiHoming2.attackTimer = 0f;//接触週刊にタイマーリセット
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Upbateでも進めていますが、念のためここにもチャック
            if (aiHoming2.attackTimer >= aiHoming2.attackInterval)
            {
                levelupplayer playerHealth = collision.GetComponent<levelupplayer>();
                if (playerHealth != null)
                {
                    playerHealth.damage(aiHoming2.attackPower);
                    Debug.Log("継続ダメージを与えました！");
                    aiHoming2.attackTimer = 0f;
                }
            }
        }
    }
}
