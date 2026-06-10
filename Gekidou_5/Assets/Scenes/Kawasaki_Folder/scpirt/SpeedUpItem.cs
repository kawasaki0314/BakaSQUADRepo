using Unity.VisualScripting;
using UnityEngine;

public class SpeedUpItem : MonoBehaviour
{
    [Header("Buff Settings")]
    public int speedValue = 2;
    public float duration = 10f; // 持続時間

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 触れた相手がプレイヤーの場合
        if(other.CompareTag("Player"))
        {
            PlayerMove playerMove = other.GetComponent<PlayerMove>();
            if(playerMove != null)
            {
                // PlayerMove側の受付窓口を呼び出す
                playerMove.StartSpeedUpBuff(speedValue, duration);

                Destroy(gameObject); // 君消す
            }
        }
    }

}
