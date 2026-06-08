using Unity.VisualScripting;
using UnityEngine;
public class AIHoming3 : MonoBehaviour
{
    Transform playerTr;//プレイヤーのTransform
    [SerializeField] float speed = 2f;  //敵の動くスピード

    [Header("Enemy Status")]
    [SerializeField] int maxHP = 3; //敵の最大HP
    [SerializeField] int attackPower = 1;　//敵の攻撃力
    [SerializeField] float attackInterval = 1f;//攻撃のインターバル（1秒に1回）
    float attackTimer = 0f;


    //遠距離攻撃（弾を発射する）の設定
    [Header("遠距離攻撃の設定")]
    [SerializeField] GameObject enemyBulletPrefab; //発射する弾のプレハブ
    [SerializeField] float shotInteval = 2f;      //何秒に１回弾を撃つか
    [SerializeField] float bulletSpeed = 5f;      //弾の飛ぶ速度
    float shotTimer = 0f;　　　　　　　　　　　　 //弾を撃つためのタイマー

    int currentHP;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //現在のHPを最大HPと同じ値に初期化します。
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
    //物理移動は　Update　ではなく　FixedPdate で行うのがUnityの鉄則!
    private void Update()
    {
        //攻撃タイマーを常に進める（プレイヤーに触れる間だけカウントしたい場合は、下に 移動させてもOK)
        attackTimer += Time.deltaTime;

        //遠距離攻撃のタイマーを進めて、時間が来たら弾を撃つ
        if (playerTr != null)
        {
            shotTimer += Time.deltaTime;
            if (shotTimer >= shotInteval)
            {
                ShotBullet();
                shotTimer = 0f;  //タイマーリセット
            }
        }
    }

    //毎フレームのタイマー更新は　Update で行う
    private void FixedUpdate()
    {
        //プレイヤーが見つかっていないなら処理しない（距離制限は削除）
        if (playerTr == null) return;
        //プレイヤーに向けて進む

        transform.position = Vector2.MoveTowards(transform.position,
        playerTr.position,//Vector3は自動的にVector2として計算されます
        speed * Time.deltaTime);
    }

    //プレイヤーに向かって弾を飛ばす関数
    void ShotBullet()
    {
        //弾のプレハブがインスペクターで入っていないから処理をしない
        if (enemyBulletPrefab == null) return;

        //1.自分の位置に弾を生成する
        GameObject bullet = Instantiate(enemyBulletPrefab, transform.position, Quaternion.identity);

        //2.プレイヤーへの方向（ベクトル）を計算する
        Vector2 direction = (playerTr.position - transform.position).normalized;

        //3.弾のRigidbody2Dを取得して、プレイヤーの方向へ速度（速度ベクトル）を与える
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if(rb != null)
        {
            rb.linearVelocity = direction * bulletSpeed; 
        }
        else
        {
            Debug.LogWarning("生成した弾に　Rigidbody2D がついていません！弾が飛びません");
        }
    }

    //ダメージを受ける関数(ここがHP0で消滅するコアの部分です)
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        //  追加
        Debug.Log("敵の残りHP: " + currentHP);

        //HPが0以下なら死亡
        if (currentHP <= 0)
        {
            Die();
        }
    }

    //死亡処理
    void Die()
    {
        // EnemySpawnクラスのInstance（自分自身）を直接呼ぶ
        // ※もしクラス名が EnemySpeawn なら、ここも EnemySpeawn に合わせる
        Debug.Log("敵を倒した!スポナーに補充を頼みます。");
        /*
        //画面内からプレイヤーのレベルスクリプトを探して、経験値を手渡す
        PlayerLevel playerLevel = FindFirstObjectByType<PlayerLevel>();
        if(playerLevel != null)
        {
            playerLevel.GainExp(3);//敵を1体倒したら経験値「３」手に入れる設定
            Debug.Log("プレイヤーに経験値を３あたえました！");
        }
        else
        {
            //もしもこのエラーが出てたら、プレイヤーに「PlayerLevel」スクリプトが付いているか確認してください
            Debug.LogWarning("PlayerLevelスクリプトは見つかりません！経験値が加算さrませんでした。");
        }
        */
        //Instance(シングルトン)を使ってスポナーに報告
        //EnemySpawnのInstance(さっきAwakeで作ったやつ)を直接呼ぶ
        if (EnemySpawn.Instance != null)
        {
            //倒された場所を伝えて補充してもらうう
            EnemySpawn.Instance.OnEnemyDefeated(false, transform.position);
            // Debug.Log("補充依頼しました");
        }
        else
        {
            //もしこれが出たら、Spawner側のAwakeが動いていない証拠です
            Debug.LogError("スポナーのInstanceが見つかりません！Spawnerオブジェクトにスクリプトを付け直してください");
        }

        //じぶんおｗ消去する処理は、すべての報告や処理が「終わったら最後」に1回だけ書くのが鉄則！
        Destroy(gameObject);
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
                playerHealth.TakeDamage(attackPower);
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            attackTimer += Time.deltaTime;
        }

        if (attackTimer >= attackInterval)
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackPower);
                //タイマーリセット
                attackTimer = 0f;

            }
        }
    }

    //プレイヤーに触れ続けている間（2発目以降の継続ダメージ）
    private void OntriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            //インターバル以上の時間が経っていたら攻撃
            if (attackTimer >= attackInterval)
            {
                playerHealth.TakeDamage(attackPower);
                Debug.Log("継続ダメージを与えました！");

                //タイマーリセット
                attackTimer = 0f;
            }
        }
    }

    //プレイヤーが離れたらタイマーをリセット（スペルを修正しました）
    private void OnTriggerEt2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            attackTimer = 0f;
        }
    }
    //スポナーから一斉に消されるための関数
    public void Disapear()
    {
        Debug.Log("時間切れのため、一斉に消滅します。");

        //一斉に消える時は、個別に補充(OnEnemyDefeated)を呼ばす
        //そのまま自分自身を消去します(そうしないと消えた瞬間新しい敵が湧いてしまうため)
        Destroy(gameObject);
    }
}