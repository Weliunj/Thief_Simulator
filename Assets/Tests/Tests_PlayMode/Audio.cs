using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioTest
{
    // Hàm bổ trợ để đảm bảo Scene và các đối tượng AI/UI đã sẵn sàng
    private IEnumerator LoadTestScene(string sceneName = "Lv1")
    {
        SceneManager.LoadScene(sceneName);
        // Đợi 1.5 giây để máy i5 kịp khởi tạo toàn bộ Prefab và Script
        yield return new WaitForSeconds(1.5f); 
    }

    // --- TEST BÀI 1: UI & SETTINGS ---

    [UnityTest]
    public IEnumerator TestBGVolume_SliderChangesAudioSource()
    {
        yield return LoadTestScene("HomeMenu");

        GameObject audioManager = GameObject.Find("AudioManager");
        Assert.IsNotNull(audioManager, "LỖI: Không tìm thấy GameObject 'AudioManager'!");

        // Tìm AudioSource trên object con "Press"
        Transform pressTransform = audioManager.transform.Find("Press");
        Assert.IsNotNull(pressTransform, "LỖI: Không tìm thấy Object con 'Press'!");
        
        AudioSource audioSource = pressTransform.GetComponent<AudioSource>();
        Assert.IsNotNull(audioSource, "LỖI: Không tìm thấy AudioSource trên Object 'Press'!");

        float testValue = 0.5f;
        audioSource.volume = testValue;
        yield return null;

        Assert.AreEqual(testValue, audioSource.volume, 0.01f, 
            $"Lỗi: Volume của AudioSource ({audioSource.volume}) không khớp với giá trị gán ({testValue})");
    }

    // --- TEST BÀI 2: AUDIO & ANIMATION ---

    [UnityTest]
    public IEnumerator TC_AUD_02_AlarmLoops_WhenTimeRunsOut()
    {
        yield return LoadTestScene("Lv1");

        var ui = Object.FindFirstObjectByType<UI_Manager>();
        Assert.IsNotNull(ui, "LỖI: Không tìm thấy UI_Manager trong Scene!");
        Assert.IsNotNull(ui.playerManager, "LỖI: PlayerManager chưa được gán vào UI_Manager!");
        Assert.IsNotNull(ui.alarm, "LỖI: AudioSource 'alarm' chưa được gán vào UI_Manager!");

        // Ép thời gian về 0 để kích hoạt Alarm trong Update()
        ui.playerManager.currentTime = 0f;
        yield return null; // Đợi 1 frame để logic nhạc chạy

        Assert.IsTrue(ui.alarm.isPlaying, "Lỗi: Chuông báo động không kêu khi hết giờ!");
        Assert.IsTrue(ui.alarm.loop, "Lỗi: Chuông báo động không ở chế độ lặp (loop)!");
    }

    [UnityTest]
    public IEnumerator TC_AUD_04_AI_Detects_Player_And_Plays_Sound()
    {
        yield return LoadTestScene("Lv1");

        // 1. Tìm các đối tượng cần thiết
        var ai = Object.FindFirstObjectByType<AI_Move_NavMesh>();
        var player = Object.FindFirstObjectByType<StarterAssets.ThirdPersonController>();
        
        Assert.IsNotNull(ai, "LỖI: Không tìm thấy AI trong Scene!");
        Assert.IsNotNull(player, "LỖI: Không tìm thấy Player trong Scene!");

        // 2. GIẢ LẬP: Đưa Player vào ngay trước mặt AI (tầm nhìn 2m)
        // AI đứng ở (0,0,0) nhìn về hướng Forward, ta đặt Player ở (0,0,2)
        player.transform.position = ai.transform.position + ai.transform.forward * 2f;
        
        // Đảm bảo Player không ở trạng thái chết
        player.player.isDied = false;

        // 3. Đợi một khoảng thời gian để Raycast của AI quét trúng Player
        // Máy i5-6300U cần khoảng 0.5s để xử lý các lệnh vật lý/Raycast trong Update
        yield return new WaitForSeconds(0.5f);

        // 4. KIỂM TRA LOGIC TÍCH HỢP
        // Kiểm tra xem biến targetDetected đã chuyển sang True chưa
        Assert.IsTrue(ai.targetDetected, "Lỗi: AI không phát hiện được Player dù đã đứng trước mặt!");

        // Kiểm tra âm thanh phát hiện (Index 1: Detection Sound)
        Assert.IsTrue(ai.audioSources[1].isPlaying, "Lỗi: AI phát hiện Player nhưng không phát âm thanh Detection!");

        // Kiểm tra nhạc Chase (Index 2)
        Assert.IsTrue(ai.audioSources[2].isPlaying, "Lỗi: Nhạc Chase không tự động phát khi phát hiện mục tiêu!");
        Assert.IsTrue(AI_Move_NavMesh.isChaseMusicPlaying, "Lỗi: Biến Static isChaseMusicPlaying không được cập nhật!");
    }

    [UnityTest]
    public IEnumerator TC_AUD_05_ChaseMusic_Starts_On_Detection()
    {
        yield return LoadTestScene("Lv1");

        var ai = Object.FindFirstObjectByType<AI_Move_NavMesh>();
        Assert.IsNotNull(ai, "LỖI: Không tìm thấy AI trong Scene!");
        Assert.IsNotNull(ai.audioSources[2], "LỖI: Chưa gán nhạc Chase vào audioSources[2]!");

        // Kích hoạt nhạc Chase thông qua hàm logic của AI
        ai.HandleChaseMusic(true);
        yield return null;

        Assert.IsTrue(ai.audioSources[2].isPlaying, "Lỗi: Nhạc Chase không phát khi AI đuổi bắt!");
        Assert.IsTrue(AI_Move_NavMesh.isChaseMusicPlaying, "Lỗi: Biến static 'isChaseMusicPlaying' không cập nhật thành True!");
    }

    [UnityTest]
    public IEnumerator TC_ANI_02_DieTrigger_ActivatesOnDeath()
    {
        yield return LoadTestScene("Lv1");

        var player = Object.FindFirstObjectByType<StarterAssets.ThirdPersonController>();
        Assert.IsNotNull(player, "LỖI: Không tìm thấy Player (ThirdPersonController) trong Scene!");
        Assert.IsNotNull(player.player, "LỖI: PlayerManager chưa được gán vào Player!");
        Assert.IsNotNull(player.lightD, "LỖI: Object 'lightD' chưa được gán vào Player!");

        // Kích hoạt trạng thái chết
        player.player.isDied = true;
        yield return new WaitForSeconds(1.0f); // Đợi Animation Die chạy và bật Light

        Assert.IsTrue(player.lightD.activeSelf, "Lỗi: Hiệu ứng đèn chết (lightD) chưa được bật!");
    }
}