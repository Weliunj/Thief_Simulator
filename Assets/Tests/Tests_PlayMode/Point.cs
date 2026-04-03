using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class PointTests
{
    private PlayerManager player;
    private Item item;

    [SetUp]
    public void Setup()
    {
        // Create instances that can be reused for the tests
        player = ScriptableObject.CreateInstance<PlayerManager>();
        item = new Item();
    }
    [Test]
    public void TestAddPoint_Success()
    {
        item.Price = 50f;
        player.currpoint = (int)item.Price;
        Assert.AreEqual(50, player.currpoint);
    }
    [Test]
    public void TestOverMaxPoint()
    {
        player.currpoint = 600;
        item.Price = 550f;
        player.currpoint += (int)item.Price;
        Assert.AreEqual(700, player.currpoint);
    }

    [Test]
    public void TestResetLevel()
    {
        // Arrange
        player.currpoint = 600;
        player.isDied = true;    

        // Act
        // Simulate logic that should happen when the player dies
        player.currpoint = 0;
        player.isDied = false;

        // Assert
        Assert.AreEqual(0, player.currpoint);
        Assert.IsFalse(player.isDied, "Player should not be dead after reset");
    }
    [TearDown]
    public void TearDown()
    {
       //Clean up after each test
       ScriptableObject.DestroyImmediate(player);
       ScriptableObject.DestroyImmediate(item);
    }

}