using UnityEngine;
using System.Collections.Generic;

public class GaolSpawnManager : MonoBehaviour
{
    [Header("生成するキャラクターのプレハブ")]
    public GameObject characterPrefab;

    [Header("画面内に維持するキャラクターの数")]
    [Range(1, 10)] public int targetCount = 3;

    [Header("キャラクター同士が重ならないためのスポーン間隔（秒）")]
    public float minSpawnDelay = 1.0f;

    [Header("キャラクターをスポーンさせるY座標")]
    public float spawnYPosition = 0f;

    private Camera mainCamera;
    private float screenLeftLimit;
    private float screenRightLimit;

    // 現在画面内にいるキャラクターたちを追跡するリスト
    private List<GameObject> activeCharacters = new List<GameObject>();
    private float lastSpawnTime;

    void Start()
    {
        Time.timeScale = 1.0f;

        mainCamera = Camera.main;
        CalculateScreenLimits();

        // 開始時に指定数（例：3体）を一気にスポーンさせる
        // ただし、一箇所に重ならないように少しずつX座標をズラして初期配置します
        for (int i = 0; i < targetCount; i++)
        {
            // 画面の左端から少し右にバラけさせて配置
            float startX = screenLeftLimit - (i * 2.0f);
            SpawnCharacter(startX);
        }
    }

    void CalculateScreenLimits()
    {
        // 画面の左端と右端のワールド座標（X座標）を計算
        screenLeftLimit = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x - 1.5f;
        screenRightLimit = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x + 1.5f;
    }

    // 指定したX座標にスポーンさせる
    void SpawnCharacter(float xPosition)
    {
        Vector3 spawnPosition = new Vector3(xPosition, spawnYPosition, 0f);
        GameObject newChar = Instantiate(characterPrefab, spawnPosition, Quaternion.identity);

        // リストに追加して管理対象にする
        activeCharacters.Add(newChar);
        lastSpawnTime = Time.time;
    }

    void Update()
    {
        // 1. 右端を超えたキャラクターの削除処理
        // リストの要素を削除しながらループするため、逆順（後ろから）ループします
        for (int i = activeCharacters.Count - 1; i >= 0; i--)
        {
            GameObject character = activeCharacters[i];

            // 念のため、nullチェック（エラー防止）
            if (character == null)
            {
                activeCharacters.RemoveAt(i);
                continue;
            }

            // 右端の限界を超えたらデスポーン
            if (character.transform.position.x > screenRightLimit)
            {
                activeCharacters.RemoveAt(i); // リストから除外
                Destroy(character);           // ゲーム内から削除
            }
        }

        // 2. 人数が足りなくなった場合の補充処理
        // 目標数より少なくなっていて、かつ最後のスポーンから一定時間経っている場合
        if (activeCharacters.Count < targetCount && Time.time - lastSpawnTime > minSpawnDelay)
        {
            // いつもの左端からスポーン
            SpawnCharacter(screenLeftLimit);
        }
    }
}