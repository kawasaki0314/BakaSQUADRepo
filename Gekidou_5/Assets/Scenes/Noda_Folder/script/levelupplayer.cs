using UnityEngine;

public class levelupplayer : MonoBehaviour
{
    public int currentlevel = 1; //現在のレベル
    public int currentexp = 0; // 初期経験値
    public int maxexp = 100; //必要経験値

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Addexperience(int amount)
    {
        //レベルアップ
        currentexp += amount;
        Debug.Log($"{amount}の経験値を獲得！現在の経験値:{currentexp}/{maxexp}");

        while(currentexp >= maxexp)
        {
            LevelUp();
        }
    }

    // Update is called once per frame
    public void LevelUp()
    {
        currentexp -= maxexp; //余った経験値は持ち越す
        currentlevel++;

        //次のレベルへの必要経験値を増やす
        maxexp = Mathf.RoundToInt(maxexp * 1.05f);

        Debug.Log($"レベルアップ！現在のレベル：{currentlevel}次の必要経験値：{maxexp}");
    }
}
