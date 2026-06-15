using UnityEngine;
using UnityEngine.SceneManagement; // シーン遷移に必要！

public class SceneChanger : MonoBehaviour
{
    // 遷移先のシーン名をインスペクターから設定できるようにする
    [SerializeField] private string nextSceneName;

    void Update()
    {
        // スペースキーが押されたか判定
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 設定したシーン名に遷移
            SceneManager.LoadScene(nextSceneName);
        }
    }
}