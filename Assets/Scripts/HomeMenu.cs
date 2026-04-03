using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // QUAN TRỌNG: Dùng cho Canvas Button

public class HomeMenu : MonoBehaviour
{
    public AudioSource[] audioSources;
    
    [Header("UI References")]
    public Button playButton;
    public Button exitButton;

    private void Start()
    {
        // Đăng ký sự kiện trực tiếp bằng code (hoặc kéo thả trong Inspector đều được)
        if (playButton != null) playButton.onClick.AddListener(OnPlayClicked);
        if (exitButton != null) exitButton.onClick.AddListener(OnExitClicked);
    }

    private void OnPlayClicked()
    {
        PlayClickSound();
        Debug.Log("Button Play Clicked - Preparing to load Intro...");
        StartCoroutine(LoadSceneAfterDelay(1f));
    }

    private void OnExitClicked()
    {
        PlayClickSound();
        Debug.Log("Button Exit Clicked");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private void PlayClickSound()
    {
        // Kiểm tra an toàn để không bị lỗi đỏ nếu quên kéo Audio
        if (audioSources != null && audioSources.Length > 0 && audioSources[0] != null)
        {
            audioSources[0].Play();
        }
    }

    private IEnumerator LoadSceneAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        SceneManager.LoadScene("Intro");
    }
}