using UnityEngine;
using UnityEngine.SceneManagement;
public class BGM : MonoBehaviour
{
    private static BGM instance;
    private AudioSource audioSource;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "startscene")
        {
            // タイトルに戻ったら再生
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else if (scene.name == "BattleScene" || scene.name == "Clear" || scene.name == "GameOver") 
        {
            // クリアやゲームオーバーでは停止
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
