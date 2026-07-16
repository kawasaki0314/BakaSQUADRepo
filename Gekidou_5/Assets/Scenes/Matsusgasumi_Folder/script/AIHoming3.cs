using Unity.VisualScripting;
using UnityEngine;
public class AIHoming3 : MonoBehaviour
{
    Transform playerTr;//プレイヤーのTransform
    [SerializeField] float speed = 2f;  //敵の動くスピード

    [Header("Enemy Status")]
    public int attackPower = 2;　//敵の攻撃力
    public float attackInterval = 1f;//攻撃のインターバル（1秒に1回）
    public float attackTimer = 0f;
        private Rigidbody2D rb;
    Enemyanims enemyanims;


    //遠距離攻撃（弾を発射する）の設定
    [Header("遠距離攻撃の設定")]
    [SerializeField] GameObject enemyBulletPrefab; //発射する弾のプレハブ
    [SerializeField] float shotInteval = 2f;      //何秒に１回弾を撃つか
    [SerializeField] float bulletSpeed = 5f;      //弾の飛ぶ速度
    float shotTimer = 0f;　　　　　　　　　　　　 //弾を撃つためのタイマー

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyanims = GetComponent<Enemyanims>(); 
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
                if (enemyanims != null)
                {
                    enemyanims.animState = Enemyanims.AnimState.shot;
                }
                else
                {
                    enemyanims = GetComponent<Enemyanims>();
                }
                shotTimer = 0f;  //タイマーリセット
            }
        }
    }

    //毎フレームのタイマー更新は　Update で行う
    private void FixedUpdate()
    {
        if (playerTr == null) return;

        // 【追加】衝突で発生した余計な吹っ飛び速度（慣性）をここで毎フレームリセットする
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        // プレイヤーに向けて進む（元の処理のまま）
        transform.position = Vector2.MoveTowards(transform.position,
            playerTr.position,
            speed * Time.fixedDeltaTime);
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
        if (rb != null)
        {
            rb.linearVelocity = direction * bulletSpeed;
            Debug.Log($"弾を発射！方向:{direction} 速度{rb.linearVelocity}");
        }
        else
        {
            Debug.LogWarning("生成した弾に　Rigidbody2D がついていません！弾が飛びません");
        }
    }


    //死亡処理
   public  void Die()
    {
        // EnemySpawnクラスのInstance（自分自身）を直接呼ぶ
        // ※もしクラス名が EnemySpeawn なら、ここも EnemySpeawn に合わせる
        Debug.Log("敵を倒した!スポナーに補充を頼みます。");
        
        //画面内からプレイヤーのレベルスクリプトを探して、経験値を手渡す
        levelupplayer playerlevel = FindFirstObjectByType<levelupplayer>();
        if(playerlevel != null)
        {
            playerlevel.Addexperience(10);//敵を1体倒したら経験値「10」手に入れる設定
            Debug.Log("プレイヤーに経験値を10あたえました！");
        }
        else
        {
            //もしもこのエラーが出てたら、プレイヤーに「PlayerLevel」スクリプトが付いているか確認してください
            Debug.LogWarning("PlayerLevelスクリプトは見つかりません！経験値が加算さrませんでした。");
        }
        
        //Instance(シングルトン)を使ってスポナーに報告
        //EnemySpawnのInstance(さっきAwakeで作ったやつ)を直接呼ぶ
        if (EnemySpawn3.Instance != null)
        {
            //倒された場所を伝えて補充してもらうう
            EnemySpawn3.Instance.OnEnemyDefeated(false, transform.position);
            // Debug.Log("補充依頼しました");
        }
        else
        {
            //もしこれが出たら、Spawner側のAwakeが動いていない証拠です
            Debug.LogError("EnemySpawn3のInstanceが見つかりません！Spawnerオブジェクトにスクリプトを付け直してください");
        }

        //じぶんおｗ消去する処理は、すべての報告や処理が「終わったら最後」に1回だけ書くのが鉄則！
        Destroy(gameObject);
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