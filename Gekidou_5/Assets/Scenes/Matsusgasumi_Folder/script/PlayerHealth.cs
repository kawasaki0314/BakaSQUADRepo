
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Player HP setings")]
    [SerializeField] int maxHP = 100;//プレイヤーの最大HP
    int currentHP;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //ゲーム開始時にHPを最大値にリセット
        currentHP = maxHP;
    }

    //ここに「public」をつけることで、敵(AiHoming)から呼び出せるようになります!
    public void TakeDamage(int damage)
    {
        //ダメージ分だけHPを減らす
        currentHP -= damage;
        Debug.Log($"プレイヤーが{damage}のダメージを受けた! 残りHP:{currentHP}");

        //HPが0以下になったら死亡処理
        if (currentHP <= 0)
        {
            PlayerDie();
        }
    }

    //プレイヤーの死亡処理
    void PlayerDie()
    {
        Debug.Log("プレイヤーは倒された...");

        //とりあえずプレイヤーの動きを止める、または非表示にするなどの処理
        gameObject.SetActive(false);

        //ここに「ゲームオーバー画面を表示する」などの処理を後から追加できます
    }

    //(おまけ)現在のHPを確認・利用したいとき用の関数
    public int GetcurrentHP()
    {
        return currentHP;
    }

}
