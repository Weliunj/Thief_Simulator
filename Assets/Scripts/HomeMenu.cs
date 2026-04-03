using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro; // Nếu bạn dùng InputField của TextMeshPro

public class HomeMenu : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource[] audioSources;
    
    [Header("Main Menu UI")]
    public Button playButton;
    public Button settingsButton; // Nút để mở bảng cài đặt
    public Button exitButton;
    public GameObject mainMenuPanel; // Panel chứa 3 nút chính

    [Header("Settings UI")]
    public GameObject settingsPanel; // Panel chứa phần chỉnh độ phân giải
    public TMP_InputField widthInput;
    public TMP_InputField heightInput;
    public Button applyButton;
    public Button backButton;

    private void Start()
    {
        // Gán sự kiện Main Menu
        playButton.onClick.AddListener(OnPlayClicked);
        exitButton.onClick.AddListener(OnExitClicked);
        settingsButton.onClick.AddListener(() => SwitchPanel(true)); // Mở Setting

        // Gán sự kiện Settings
        applyButton.onClick.AddListener(ApplyResolution);
        backButton.onClick.AddListener(() => SwitchPanel(false)); // Quay lại Menu

        // Khởi tạo trạng thái ban đầu
        SwitchPanel(false);
        widthInput.text = Screen.width.ToString();
        heightInput.text = Screen.height.ToString();
    }

    // --- LOGIC CHUYỂN ĐỔI UI ---
    private void SwitchPanel(bool isSettings)
    {
        PlayClickSound();
        mainMenuPanel.SetActive(!isSettings);
        settingsPanel.SetActive(isSettings);
    }

    // --- LOGIC CÀI ĐẶT ---
    public void ApplyResolution()
    {
        PlayClickSound();
        if (int.TryParse(widthInput.text, out int w) && int.TryParse(heightInput.text, out int h))
        {
            // Chỉnh độ phân giải dạng cửa sổ cho dễ test
            Screen.SetResolution(w, h, false);
            Debug.Log($"Đã đổi độ phân giải: {w}x{h}");
        }
    }

    // --- LOGIC MENU CHÍNH ---
    private void OnPlayClicked()
    {
        PlayClickSound();
        StartCoroutine(LoadSceneAfterDelay(1f));
    }

    private void OnExitClicked()
    {
        PlayClickSound();
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private void PlayClickSound()
    {
        if (audioSources != null && audioSources.Length > 0 && audioSources[0] != null)
            audioSources[0].Play();
    }

    private IEnumerator LoadSceneAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        SceneManager.LoadScene("Intro");
    }
}