using UnityEngine;
public class AIHoming : MonoBehaviour
{

    Transform playerTr;//プレイヤーのTransform

    [SerializeField] float speed = 3f;  //敵の動くスピード

    //=== 追加===
    [Header("Enemy Status")]
    [SerializeField] int maxHP = 3; //敵の最大HP
    [SerializeField] int attackPower = 1;　//敵の攻撃力

    int currentHP;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //===【修正】ここで、現在のHPを最大HPと同じ値に初期化します。
        currentHP = maxHP;

        //1. 最初は「GameObject playerObj」と書いて、箱(変数)を用意してプレイヤーを探す
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        //2. 小文字の「playerObj」がちゃんと見つかったか確認
        if (playerObj != null)
        {
            playerTr = playerObj.transform;
        }
        else
        {
            Debug.LogError("タグ'Player'が見つかりません。インスペクターで設定を確認してください。");
        }
    }

    // Update is called once per frame
    private void Update()
    {
        //プレイヤーが見つかっていない、または距離が0.1f未満なら処理しない
        if (playerTr == null || Vector2.Distance(transform.position, playerTr.position) < 0.1f)
            return;

        //プレイヤーに向けて進む
        transform.position = Vector2.MoveTowards(
            transform.position,
            playerTr.position,//Vector3は自動的にVector2として計算されます
            speed * Time.deltaTime);

    }


    //ダメージを受ける関数
    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        //HPが0以下なら死亡
        if (currentHP <= 0)
        {
            Die();
        }
    }

    //死亡処理
    void Die()
    {
        Debug.Log("敵を倒した!");

        Destroy(gameObject);
    }

    //プレイヤーに当たった時
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("プレイヤーに当たりました!");

            //プレイヤー側のコンポーネントを取得
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

            //コンポーネントがちゃんと取得できていればダメージを与える
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackPower);
            }

            //敵を消す
            Destroy(gameObject);
        }
    }

}