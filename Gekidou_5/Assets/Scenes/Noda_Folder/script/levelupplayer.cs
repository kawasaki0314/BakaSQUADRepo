using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class levelupplayer : MonoBehaviour
{
    [Header("Level Settings")]
    public int currentlevel = 1; //現在のレベル
    public int currentexp = 0; // 初期経験値
    public int maxexp = 100; //必要経験値

    [Header("Hp Settings")]
    public Image healthImage; //体力表示
    public int maxHP; //最大体力
    public int hp; //体力

    [Header("Fade Settings")] // ★フェード用の設定を追加
    [SerializeField] private Image fadeImage; // 画面を覆う黒い画像
    [SerializeField] private float fadeDuration = 1.0f; // フェードアウトにかける時間（秒）

    [Header("Movement Limit Settings")]
    [SerializeField] private float maxPlayerSpeed = 3.0f;

    private PlayerAttack playerAttack;
    private PlayerMove playerMove;

    [SerializeField] private int expPerKeyPress = 10;  //1会押すたびにもらえる経験値の量
    [SerializeField] private Slider expSlider;         //InspectorでSliderをドラッグ&ドロップ
    [SerializeField] private TextMeshProUGUI levelText;//InspectorでTMPをドラッグ&ドロップ

    private bool isDead = false; // ★死亡二重処理防止フラグ

    void Start()
    {
        UpdateUI();
        hp = maxHP;

        // プレイヤーの他のスクリプトの取得
        playerAttack = GetComponent<PlayerAttack>();
        playerMove = GetComponent<PlayerMove>();

        // ★開始時はフェード用画像を透明にしておく（念のため）
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0;
            fadeImage.color = c;
            fadeImage.gameObject.SetActive(false); // 初期状態は非アクティブに
        }
    }

    private void Update()
    {
        // 死亡している場合は入力を受け付けない
        if (isDead) return;

        if (Input.GetKeyDown(KeyCode.Space))//スペースキーを押した時検知する
        {
            Addexperience(expPerKeyPress);
        }

        if (Input.GetKeyDown(KeyCode.Z))//Zキーでダメージを受け、体力ゲージが減少する
        {
            damage(10);
        }
        if (Input.GetKeyDown(KeyCode.X))//Xキーで回復し、体力ゲージの復元が可能
        {
            heal(10);
        }
    }

    public void Addexperience(int amount)
    {
        currentexp += amount;
        Debug.Log($"{amount}の経験値を獲得！現在の経験値:{currentexp}/{maxexp}");

        while (currentexp >= maxexp)
        {
            LevelUp();
        }

        UpdateUI();
    }

    public void LevelUp()
    {
        currentexp -= maxexp;
        currentlevel++;

        if (playerAttack != null)
        {
            playerAttack.normalAttackPower += 1;
            playerAttack.orbitAttackPower += 1;
            playerAttack.bulletAttackPower += 1;
            Debug.Log($" プレイヤーの全攻撃力が 1 上がった！");
        }

        if (playerMove != null && currentlevel % 5 == 0)
        {
            float targetSpeed = playerMove.playerSpeed + 0.5f;
            playerMove.playerSpeed = Mathf.Min(targetSpeed, maxPlayerSpeed);

            Debug.Log($" プレイヤーの移動速度が 1 上がった！");
        }

        maxexp = Mathf.RoundToInt(maxexp * 1.3f);
        UpdateUI();
        Debug.Log($"レベルアップ！現在のレベル：{currentlevel}次の必要経験値：{maxexp}");
    }

    private void UpdateUI()
    {
        if (expSlider != null)
        {
            expSlider.maxValue = maxexp;
            expSlider.value = currentexp;
        }

        if (levelText != null)
        {
            levelText.text = "Level:" + currentlevel;
        }
    }

    public void damage(int damage)
    {
        if (hp <= 0 || isDead) // ★死亡フラグもチェック
        {
            Debug.Log("やめて！もう城之内くんのライフポイントはゼロよ！");
            return;
        }

        hp -= damage;
        hp = Mathf.Clamp(hp, 0, maxHP);

        healthImage.fillAmount = (float)hp / maxHP;
        Debug.Log($"ダメージを{damage}受けた！現在のHP:{hp}/{maxHP}");

        if (hp <= 0)
        {
            // ★コルーチンを呼び出す
            StartCoroutine(PlayerDeathCoroutine());
        }
    }

    // ★コルーチンによる死亡・フェード処理
    private IEnumerator PlayerDeathCoroutine()
    {
        isDead = true; // 死亡フラグを立てる
        Debug.Log("城之内死す。デュエルスタンバイ！");

        // 移動スクリプトや攻撃スクリプトがあればここで止める
        if (playerMove != null) playerMove.enabled = false;
        if (playerAttack != null) playerAttack.enabled = false;

        // フェード用画像を表示する
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);

            float elapsedTime = 0f;
            Color color = fadeImage.color;

            // 時間をかけて透明度(a)を0から1にする
            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
                fadeImage.color = color;
                yield return null; // 1フレーム待つ
            }
        }
        else
        {
            // フェード用画像が設定されていない場合は一瞬だけ待つ（保険）
            yield return new WaitForSeconds(fadeDuration);
        }

        // 画面が真っ黒になった後にシーン遷移
        SceneManager.LoadScene("GameOver");

        // シーン遷移後にプレイヤーオブジェクトを消滅させる
        Destroy(gameObject);
    }

    public void heal(int heal)
    {
        if (hp >= maxHP || isDead) return;

        hp += heal;
        hp = Mathf.Clamp(hp, 0, maxHP);
        healthImage.fillAmount = (float)hp / maxHP;
        Debug.Log($"体力を{heal}回復した！現在のHP:{hp}/{maxHP}");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("HeelItem"))
        {
            Debug.Log("飯だ！うめぇ");
            heal(20);
            Destroy(other.gameObject);
        }
    }
}