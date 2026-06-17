using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI targetText;
    [SerializeField] private string[] gameOverMessages = new string[10];
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DisplayRandomMessage();
    }

    public void DisplayRandomMessage()
    {
        //エラー防止
        if(gameOverMessages == null || gameOverMessages.Length == 0 || targetText == null)
        {
            Debug.Log("何か設定ミスってない？");
            return;
        }

        //0から配列の要素数－1までのランダムな数字を取得
        int randomIndex = Random.Range(0, gameOverMessages.Length);

        //選ばれた文章をUIに反映
        targetText.text = gameOverMessages[randomIndex];

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
