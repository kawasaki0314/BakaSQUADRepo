using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    [Header("HP Settongs")]
    [SerializeField] int maxHp = 5;
    public int currentHp = 5;

    private AIHoming aiHoming;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //ゲーム開始時にHPを満タンにする
        currentHp = maxHp;

        //同じオブジェクトについているAIHoming スクリプトを取得する
        aiHoming = GetComponent<AIHoming>();

        if (aiHoming == null)
        {
            Debug.LogError("AIHomingスクリプトが同じオブジェクトに見つかりません");

        }
    }

    //外部（プレイヤーの弾など）からダメージを受け取る関数
    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        Debug.Log($"{gameObject.name}に{damage}のダメージ！残りHP:{currentHp}");

        //HPが0以下になったら
        if (currentHp <= 0)
        {
            if (aiHoming != null)
            {
                // AIHoming側の死亡処理（スポナーへの報告など）を呼び出す
               // aiHoming.Die();
            }
            else
            {
                // 万が一AIHomingがなくても、自分自身を消去してバグを防ぐ
                Destroy(gameObject);
            }
        }
    }
}
    