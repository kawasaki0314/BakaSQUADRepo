using UnityEngine;

public class SoundController : MonoBehaviour
{
    //このEnemyが鳴らす音のAudioSource
    public AudioSource audioSource;

    private static int currentPlayingCount = 0;
    private static readonly int maxSimultaneous = 5; // 同時再生の上限数（好きな値に調整）

    private bool isCountedIn = false;

    void Start()
    {
        TryPlay();
    }

    void TryPlay()
    {
        if (currentPlayingCount < maxSimultaneous)
        {
            currentPlayingCount++;
            isCountedIn = true;
            audioSource.Play();
        }
        // 上限を超えている場合は何もしない（鳴らさない）
    }

    void Update()
    {
        // 再生が終わったらカウントを戻す
        if (isCountedIn && !audioSource.isPlaying)
        {
            currentPlayingCount--;
            isCountedIn = false;
        }
    }

    // Enemyが再生中に破壊された場合もカウントを正しく戻す
    void OnDestroy()
    {
        if (isCountedIn)
        {
            currentPlayingCount--;
            isCountedIn = false;
        }
    }
}