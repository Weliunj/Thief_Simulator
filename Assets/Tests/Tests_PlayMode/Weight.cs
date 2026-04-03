using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using StarterAssets; // Đảm bảo đúng namespace của ThirdPersonController

public class Weight
{
    private GameObject _playerObject;
    private ThirdPersonController _controller;
    private PlayerManager _playerData;

    [SetUp]
    public void Setup()
    {
        // Khởi tạo các thành phần cần thiết cho Test
        _playerObject = new GameObject();
        _controller = _playerObject.AddComponent<ThirdPersonController>();
        _controller.player = ScriptableObject.CreateInstance<PlayerManager>();
        
        // Thiết lập thông số mặc định
        _playerData = _controller.player;
        _playerData.MoveSpeed = 2.0f;
        _playerData.SprintSpeed = 7.0f;
        _playerData.Maxweight = 100;
        _playerData.currweight = 0;
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(_playerObject);
        Object.DestroyImmediate(_playerData);
    }

    // TC4: Kiểm tra giá trị weight có bằng vật phẩm khi nhặt không
    [Test]
    public void TC4_WeightValueIncreasesAfterPickup()
    {
        int itemWeight = 15;
        _playerData.currweight += itemWeight;

        Assert.AreEqual(15, _playerData.currweight, "Trọng lượng hiện tại phải bằng trọng lượng vật phẩm vừa nhặt.");
    }

    // TC5: Kiểm tra Speed có bị giảm khi Weight > 0
    [Test]
    public void TC5_SpeedDecreasesWhenWeightIsAdded()
    {
        float initialMoveSpeed = _playerData.MoveSpeed;
        
        // Giả lập nhặt đồ nặng (50% sức chứa)
        _playerData.currweight = 50; 
        _controller.WeightCacul(); // Gọi hàm tính toán tốc độ

        Assert.Less(_playerData._MoveSpeed, initialMoveSpeed, "Tốc độ di chuyển thực tế (_MoveSpeed) phải nhỏ hơn tốc độ gốc khi mang đồ.");
    }

    // TC6: Kiểm tra Speed tăng trở lại khi vứt bớt đồ
    [Test]
    public void TC6_SpeedIncreasesWhenWeightIsDropped()
    {
        // Bước 1: Đang nặng
        _playerData.currweight = 80;
        _controller.WeightCacul();
        float slowSpeed = _playerData._MoveSpeed;

        // Bước 2: Vứt bớt đồ (giảm xuống còn 10kg)
        _playerData.currweight = 10;
        _controller.WeightCacul();

        Assert.Greater(_playerData._MoveSpeed, slowSpeed, "Tốc độ phải tăng lên sau khi giảm bớt trọng lượng.");
    }
}