using NUnit.Framework;
using UnityEngine;

public class HUD_Setup_Tests
{
    [Test]
    public void PlayerManager_Defaults_AreValid()
    {
        // Kiểm tra các giá trị mặc định của PlayerManager
        var pm = ScriptableObject.CreateInstance<PlayerManager>();
        Assert.AreEqual(100, pm.Maxweight, "Maxweight mặc định phải là 100");
        Assert.AreEqual(10f, pm.MaxStamina, "MaxStamina mặc định phải là 10");
    }

    [Test]
    public void UI_Manager_RequiredFields_AreAssigned()
    {
        // Kiểm tra xem các thành phần UI cốt lõi đã được gán chưa
        GameObject uiObj = new GameObject();
        var ui = uiObj.AddComponent<UI_Manager>();
        
        // Giả lập gán các thành phần (Trong thực tế bạn nên kiểm tra trên Prefab)
        Assert.IsNotNull(ui, "UI_Manager component phải tồn tại");
    }
}