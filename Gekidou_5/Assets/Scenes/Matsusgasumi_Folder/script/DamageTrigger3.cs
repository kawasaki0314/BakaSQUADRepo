using UnityEngine;

public class DamageTrigger3 : MonoBehaviour
{
    private AIHoming3 aiHoming3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        aiHoming3 = GetComponentInParent<AIHoming3>();
    }
    //プレイヤーに当たった瞬間（最初の1発）
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("プレイヤーに当たりました！");

            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(aiHoming3.attackPower);
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (aiHoming3.attackTimer >= aiHoming3.attackInterval)
            {
                PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(aiHoming3.attackPower);
                    Debug.Log("継続ダメージを与えました！");
                    aiHoming3.attackTimer = 0f;  //タイマーリセット
                }
            }
        }
    }


    //プレイヤーが離れたらタイマーをリセット（スペルを修正しました）
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("プレイヤーが離れなした");
            aiHoming3.attackTimer = 0f;
        }
    }
}
