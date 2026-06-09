using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;

    private bool isPaused = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) //Pキーでポーズします
        {
            if (isPaused)
            {
                Debug.Log("Pキーが押されたので、Resume() を呼び出します");
                Resume();
            }
            else
            {
                Debug.Log("Pキーが押されたので、Pause() を呼び出します");
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        EventSystem.current.SetSelectedGameObject(null);
    }

}

