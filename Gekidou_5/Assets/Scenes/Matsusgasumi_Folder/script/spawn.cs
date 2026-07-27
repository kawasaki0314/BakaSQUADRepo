using UnityEngine;

public class spawn : MonoBehaviour
{
    [Header("隠し敵の設定")]
    [SerializeField] GameObject chickenPrefab;
    [Range(0f, 100f)]
    [SerializeField] float chickenSpawnRate = 20.0f;

    [Header("プレイヤーから離す設定")]
    [SerializeField] float distanceFromPlayer = 5.0f; // プレイヤーから何メートル離すか

    public static spawn Instance;

    //一試合に1回だけにするためのフラグ
    private bool rareItemSpawned = false;
    private bool chickenSpawned = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        //まだ発生していない場合のみ抽選
        if (!rareItemSpawned)
        {
            float spawnChance = Random.Range(0.0f, 100.0f);
            if (spawnChance <= 5.0f)
            {
                Debug.Log("レアアイテムがスポーンしました！");
                rareItemSpawned = true; // 一度発生したら二度と発生しない
            }
        }
    }

    void Update()
    {

    }

    public void OnEnemyDefeated(bool isRare, Vector3 deadPosition)
    {
        Debug.Log("敵が倒された位置: " + deadPosition);

        //まだ出現していない場合のみ抽選
        if (!chickenSpawned)
        {
            float chickenChance = Random.Range(0.0f, 100.0f);
            if (chickenChance <= chickenSpawnRate)
            {
                Vector3 spawnPos = GetPositionAwayFromPlayer(deadPosition);
                Instantiate(chickenPrefab, spawnPos, Quaternion.identity);
                Debug.Log("隠し敵(chicken)が出現しました！");
                chickenSpawned = true; // 一度出現したら以降は抽選しない
            }
        }
    }

    // プレイヤーから離れた座標を計算する
    Vector3 GetPositionAwayFromPlayer(Vector3 basePos)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogWarning("PlayerタグのオブジェクトがByt見つかりません！そのままの座標で出現させます。");
            return basePos;
        }

        // プレイヤー→倒された場所 の方向を計算
        Vector3 directionFromPlayer = (basePos - player.transform.position).normalized;

        // その方向にさらにdistanceFromPlayer分だけ進んだ座標を返す
        Vector3 awayPos = basePos + directionFromPlayer * distanceFromPlayer;

        return awayPos;
    }
}