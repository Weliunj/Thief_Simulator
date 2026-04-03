using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class Graphic
{
    [UnityTest]
    public IEnumerator Test_ChangeResolution_ByInputFields()
    {
        SceneManager.LoadScene("HomeMenu");
        yield return null; 

        HomeMenu menu = Object.FindAnyObjectByType<HomeMenu>();
        Assert.IsNotNull(menu, "Không tìm thấy script HomeMenu!");

        // 3. GIẢ LẬP NHẬP LIỆU: Thử đổi sang 1280x720
        menu.widthInput.text = "1920";
        menu.heightInput.text = "1080";

        menu.applyButton.onClick.Invoke();


        yield return new WaitForSecondsRealtime(1.5f);

        // 6. KIỂM TRA (Assert): Xem độ phân giải thực tế có đúng như mong đợi không
        Assert.AreEqual(1920, Screen.width, "Lỗi: Chiều rộng không khớp!");
        Assert.AreEqual(1080, Screen.height, "Lỗi: Chiều cao không khớp!");

        Debug.Log("Test Pass: Độ phân giải đã được thay đổi thành công qua InputField.");
    }
}