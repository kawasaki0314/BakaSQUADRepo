using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;


public class levelupplayer : MonoBehaviour
{
    public int currentlevel = 1; //現在のレベル
    public int currentexp = 0; // 初期経験値
    public int maxexp = 100; //必要経験値

    public Image healthImage; //体力表示
    public int maxHP; //最大体力
    public int hp; //体力


    [SerializeField]private int expPerKeyPress = 10;  //1会押すたびにもらえる経験値の量
    [SerializeField]private Slider expSlider;         //InspectorでSliderをドラッグ&ドロップ
    [SerializeField]private TextMeshProUGUI levelText;//InspectorでTMPをドラッグ&ドロップ

    private void Start()
    {
        UpdateUI();
        hp = maxHP;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))//スペースキーを押した時検知する
        {
            Addexperience(expPerKeyPress);
        }

        if(Input.GetKeyDown(KeyCode.Z))//Zキーでダメージを受け、体力ゲージが減少する
        {
            damage(10);
        }
        if (Input.GetKeyDown(KeyCode.X))//Xキーで回復し、体力ゲージの復元が可能
        {
            heal(10);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Addexperience(int amount)
    {
        //レベルアップ
        currentexp += amount;
        Debug.Log($"{amount}の経験値を獲得！現在の経験値:{currentexp}/{maxexp}");//ログで確認可能

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

    public void damage(int damage) //ダメージの定義
    {
        if(hp <= 0)
        {
            Debug.Log("やめて！もう城之内くんのライフポイントはゼロよ！");//オーバーキル
            return;//これ以降の処理をスキップし、処理を終了させる
        }

        hp -= damage;
        // hpの値を 0 から maxHP の間に制限する
        hp = Mathf.Clamp(hp, 0, maxHP); 
        
        healthImage.fillAmount = (float)hp / maxHP;
        
        //体力がゼロの場合もこれは表示可能
        Debug.Log($"ダメージを{damage}受けた！現在のHP:{hp}/{maxHP}");//ダメージログの表示

    }

    public void heal(int heal) //体力回復の定義
    {
        if(hp >= 100)
        {
            Debug.Log("既に体力は全開だ！");//回復のしすぎ
            return;//処理のスキップをし、強制終了
        }

        hp += heal;
        // hpの値を 0 から maxHP の間に制限する
        hp = Mathf.Clamp(hp, 0, maxHP); 
        
        healthImage.fillAmount = (float)hp / maxHP;//UI上で表示

        Debug.Log($"体力を{heal}回復した！現在のHP:{hp}/{maxHP}");//回復ログの表示
    }
}
