using UnityEngine;
using UnityEngine.SceneManagement;

public class go : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onGo()
    {
        SceneManager.LoadScene("Battle_Scene");
    }
}
