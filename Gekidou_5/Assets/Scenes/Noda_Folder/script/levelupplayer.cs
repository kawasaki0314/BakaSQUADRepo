using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class levelupplayer : MonoBehaviour
{
    public int currentlevel = 1; //現在のレベル
    public int currentexp = 0; // 初期経験値
    public int maxexp = 100; //必要経験値

    [SerializeField]private int expPerKeyPress = 10;  //1会押すたびにもらえる経験値の量
    [SerializeField]private Slider expSlider;         //InspectorでSliderをドラッグ&ドロップ
    [SerializeField]private TextMeshProUGUI levelText;//InspectorでTMPをドラッグ&ドロップ

    private void start()
    {
        UpdateUI();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))//スペースキーを押した時検知する
        {
            Addexperience(expPerKeyPress);
        }
    }
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

        UpdateUI();
    }

    // Update is called once per frame
    public void LevelUp()
    {
        currentexp -= maxexp; //余った経験値は持ち越す
        currentlevel++; //レベルアップ

        //次のレベルへの必要経験値を増やす
        maxexp = Mathf.RoundToInt(maxexp * 1.48f);

        UpdateUI();

        Debug.Log($"レベルアップ！現在のレベル：{currentlevel}次の必要経験値：{maxexp}");
    }

    private void UpdateUI()
    {
        //Sliderの更新
        if(expSlider != null)
        {
            expSlider.maxValue = maxexp; //最大値を現在の必要経験値に合わせる
            expSlider.value = currentexp;//現在のゲージ量を合わせる
        }

        if(levelText != null)
        {
            levelText.text = "Level:" + currentlevel; //レベルの表示
        }
    }
}
