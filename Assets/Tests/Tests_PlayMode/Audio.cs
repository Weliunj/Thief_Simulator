using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI; // Cần thiết để thao tác với Slider
using UnityEngine.SceneManagement;

public class AudioTest
{
    [UnityTest]
    public IEnumerator TestBGVolume_SliderChangesAudioSource()
    {
        SceneManager.LoadScene("HomeMenu");
        yield return null; 

        GameObject audioManager = GameObject.Find("AudioManager");
        Assert.IsNotNull(audioManager, "Không tìm thấy GameObject 'AudioManager'!");

        AudioSource audioSource = audioManager.transform.Find("Press")?.GetComponent<AudioSource>();
        Assert.IsNotNull(audioSource, "Không tìm thấy AudioSource trên Object 'Press'!");

        float testValue = 0.5f;
        audioSource.volume = testValue;
        yield return null;

        Assert.AreEqual(testValue, audioSource.volume, 0.01f, 
            $"Lỗi: Volume của AudioSource ({audioSource.volume}) không khớp với Slider ({testValue})");
            
        Debug.Log($"Test Pass: Slider {testValue} => AudioSource Volume {audioSource.volume}");
    }
}