using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    [Header("Level Settings")]
    public int currentLevel = 1; // 現在のレベル
    public int currentExp = 0; // 現在の経験値
    public int expToNextLevel = 10; // 次のレベルまでに必要な経験値(初期値は10)

    private PlayerAttack playerAttack;
    private PlayerMove playerMove;
//  private PlayerHp playerHp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // プレイヤーの他のスクリプトの取得
        playerAttack = GetComponent<PlayerAttack>();
        playerMove = GetComponent<PlayerMove>();
//      playerHp = Getconponent<PlayerHp>();
    }

    // 経験値を獲得する関数
    public void GainExp(int amount)
    {
        currentExp += amount;
        Debug.Log($"【レベルシステム】経験値を {amount} 獲得！ 現在の経験値: {currentExp}/{expToNextLevel}");

        if(currentExp >= expToNextLevel)
        {
            // 溜まった経験値を消費する
            currentExp -= expToNextLevel;
            currentLevel++;

            // 次のレベルに必要な経験値を増す
            expToNextLevel = Mathf.RoundToInt(expToNextLevel * 1.5f);

            Debug.Log($" レベルアップ！ 現在のレベル: {currentLevel}");

            // プレイヤーの基礎攻撃量力を1増やす
            if (playerAttack != null)
            {
                playerAttack.normalAttackPower += 1;
                playerAttack.orbitAttackPower += 1;
                playerAttack.bulletAttackPower += 1;
                Debug.Log($" プレイヤーの全攻撃力が 1 上がった！");
            }

            // プレイヤーのの移動速度を上げる
            if (playerMove != null && currentLevel % 5 == 0)
            {
                playerMove.playerSpeed += 0.5f;
                Debug.Log($" プレイヤーの移動速度が 1 上がった！");
            }
        }
    }
}
