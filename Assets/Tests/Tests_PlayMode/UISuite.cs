using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI; // QUAN TRỌNG

public class UISuite
{
    [UnityTest]
    public IEnumerator TestGameStart_Canvas()
    {
        // 1. Load Scene Menu
        SceneManager.LoadScene("HomeMenu");
        yield return null; 

        // 2. Tìm script HomeMenu trong Scene
        HomeMenu menuScript = Object.FindAnyObjectByType<HomeMenu>();
        Assert.IsNotNull(menuScript, "Không tìm thấy script HomeMenu trên Canvas!");

        // 3. Giả lập Click bằng cách gọi Invoke() - Cách nhanh nhất cho Canvas
        // Không cần tạo PointerEvent phức tạp như UI Toolkit
        Assert.IsNotNull(menuScript.playButton, "Nút Play chưa được kéo vào script ở Inspector!");
        menuScript.playButton.onClick.Invoke();

        // 4. Đợi logic chuyển Scene (Game delay 1s + load scene)
        float timeout = 5f;
        float timer = 0f;
        while (SceneManager.GetActiveScene().name != "Intro" && timer < timeout)
        {
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        // 5. Assert kết quả
        string currentScene = SceneManager.GetActiveScene().name;
        Assert.AreEqual("Intro", currentScene, $"Lỗi: Scene hiện tại vẫn là {currentScene}");
    }
}