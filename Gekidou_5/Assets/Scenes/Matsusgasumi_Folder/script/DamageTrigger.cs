using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    private AIHoming aiHoming;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        aiHoming = GetComponentInParent<AIHoming>();
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
                playerHealth.TakeDamage(aiHoming.attackPower);
                // 当たった瞬間にタイマーをリセットして、次の継続ダメージまでの時間を正確にする
                aiHoming.attackTimer = 0f;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // インターバル以上の時間が経っていたら攻撃
            if (aiHoming.attackTimer >= aiHoming.attackInterval)
            {
                PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(aiHoming.attackPower);
                    Debug.Log("継続ダメージを与えました！");

                    // タイマーリセット
                    aiHoming.attackTimer = 0f;
                }
            }
        }
    }



    //プレイヤーが離れたらタイマーをリセット（スペルを修正しました）
     void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            aiHoming.attackTimer = 0f;
        }
    }
   
}
