using PrettyDocComments.Model;

namespace UnitTests;

[TestClass]
public class CssColorTests
{
    [TestMethod]
    public void TestRgbAbsValues()
    {
        // Arrange, Act
        bool result = CssColor.TryFromRgb("rgb(255, 100, 90)", out CssColor color);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(255, color.ToMediaColor().R);
        Assert.AreEqual(100, color.ToMediaColor().G);
        Assert.AreEqual(90, color.ToMediaColor().B);
        Assert.AreEqual(255, color.ToMediaColor().A);
    }

    [TestMethod]
    public void TestRgbPercentValues()
    {
        // Arrange, Act
        bool result = CssColor.TryFromRgb("rgb(100%, 50%, 25%)", out CssColor color);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(255, color.ToMediaColor().R);
        Assert.AreEqual(128, color.ToMediaColor().G);
        Assert.AreEqual(64, color.ToMediaColor().B);
        Assert.AreEqual(255, color.ToMediaColor().A);
    }

    [TestMethod]
    public void TestRgbFail1()
    {
        // Arrange, Act
        bool result = CssColor.TryFromRgb("rgb(100, 50, 2, 3)", out _);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void TestRgbFail2()
    {
        // Arrange, Act
        bool result = CssColor.TryFromRgb("rgb(100, 50, five)", out _);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void TestRgbaAbsValues()
    {
        // Arrange, Act
        bool result = CssColor.TryFromRgba("rgba(255, 100, 90, 0.5)", out CssColor color);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(255, color.ToMediaColor().R);
        Assert.AreEqual(100, color.ToMediaColor().G);
        Assert.AreEqual(90, color.ToMediaColor().B);
        Assert.AreEqual(128, color.ToMediaColor().A);
    }

    [TestMethod]
    public void TestRgbaPercentValues()
    {
        // Arrange, Act
        bool result = CssColor.TryFromRgba("rgba(100%, 50%, 25%, 50%)", out CssColor color);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(255, color.ToMediaColor().R);
        Assert.AreEqual(128, color.ToMediaColor().G);
        Assert.AreEqual(64, color.ToMediaColor().B);
        Assert.AreEqual(128, color.ToMediaColor().A);
    }

    [TestMethod]
    public void TestRgbaFail1()
    {
        // Arrange, Act
        bool result = CssColor.TryFromRgba("rgba(100, 50, 25)", out _);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void TestRgbaFail2()
    {
        // Arrange, Act
        bool result = CssColor.TryFromRgba("rgb(100, 50, 25)", out _);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void TestHex3Values()
    {
        // Arrange, Act
        bool result = CssColor.TryFromHex("#F84", out CssColor color);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(255, color.ToMediaColor().R);
        Assert.AreEqual(136, color.ToMediaColor().G);
        Assert.AreEqual(68, color.ToMediaColor().B);
        Assert.AreEqual(255, color.ToMediaColor().A);
    }

    [TestMethod]
    public void TestHex4Values()
    {
        // Arrange, Act
        bool result = CssColor.TryFromHex("#F842", out CssColor color);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(255, color.ToMediaColor().R);
        Assert.AreEqual(136, color.ToMediaColor().G);
        Assert.AreEqual(68, color.ToMediaColor().B);
        Assert.AreEqual(34, color.ToMediaColor().A);
    }

    [TestMethod]
    public void TestHex6Values()
    {
        // Arrange, Act
        bool result = CssColor.TryFromHex("#FF8040", out CssColor color);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(255, color.ToMediaColor().R);
        Assert.AreEqual(128, color.ToMediaColor().G);
        Assert.AreEqual(64, color.ToMediaColor().B);
        Assert.AreEqual(255, color.ToMediaColor().A);
    }

    [TestMethod]
    public void TestHex8Values()
    {
        // Arrange, Act
        bool result = CssColor.TryFromHex("#FF804020", out CssColor color);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(255, color.ToMediaColor().R);
        Assert.AreEqual(128, color.ToMediaColor().G);
        Assert.AreEqual(64, color.ToMediaColor().B);
        Assert.AreEqual(32, color.ToMediaColor().A);
    }

    [TestMethod]
    public void TestHslRed0()
    {
        // Arrange, Act
        bool result = CssColor.TryFromHsl("hsl(0, 100%, 50%)", out CssColor color);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(255, color.ToMediaColor().R);
        Assert.AreEqual(0, color.ToMediaColor().G);
        Assert.AreEqual(0, color.ToMediaColor().B);
        Assert.AreEqual(255, color.ToMediaColor().A);
    }

    [TestMethod]
    public void TestHslRed360()
    {
        // Arrange, Act
        bool result = CssColor.TryFromHsl("hsl(360, 100%, 50%)", out CssColor color);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(255, color.ToMediaColor().R);
        Assert.AreEqual(0, color.ToMediaColor().G);
        Assert.AreEqual(0, color.ToMediaColor().B);
        Assert.AreEqual(255, color.ToMediaColor().A);
    }

    [TestMethod]
    public void TestHslYellow()
    {
        // Arrange, Act
        bool result = CssColor.TryFromHsl("hsl(60, 100%, 50%)", out CssColor color);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(255, color.ToMediaColor().R);
        Assert.AreEqual(255, color.ToMediaColor().G);
        Assert.AreEqual(0, color.ToMediaColor().B);
        Assert.AreEqual(255, color.ToMediaColor().A);
    }

    [TestMethod]
    public void TestHslGreen()
    {
        // Arrange, Act
        bool result = CssColor.TryFromHsl("hsl(120, 100%, 50%)", out CssColor color);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(0, color.ToMediaColor().R);
        Assert.AreEqual(255, color.ToMediaColor().G);
        Assert.AreEqual(0, color.ToMediaColor().B);
        Assert.AreEqual(255, color.ToMediaColor().A);
    }

    [TestMethod]
    public void TestHslCyan()
    {
        // Arrange, Act
        bool result = CssColor.TryFromHsl("hsl(180, 100%, 50%)", out CssColor color);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(0, color.ToMediaColor().R);
        Assert.AreEqual(255, color.ToMediaColor().G);
        Assert.AreEqual(255, color.ToMediaColor().B);
        Assert.AreEqual(255, color.ToMediaColor().A);
    }

    [TestMethod]
    public void TestHslBlue()
    {
        // Arrange, Act
        bool result = CssColor.TryFromHsl("hsl(240, 100%, 50%)", out CssColor color);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(0, color.ToMediaColor().R);
        Assert.AreEqual(0, color.ToMediaColor().G);
        Assert.AreEqual(255, color.ToMediaColor().B);
        Assert.AreEqual(255, color.ToMediaColor().A);
    }

    [TestMethod]
    public void TestHslMagenta()
    {
        // Arrange, Act
        bool result = CssColor.TryFromHsl("hsl(300, 100%, 50%)", out CssColor color);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(255, color.ToMediaColor().R);
        Assert.AreEqual(0, color.ToMediaColor().G);
        Assert.AreEqual(255, color.ToMediaColor().B);
        Assert.AreEqual(255, color.ToMediaColor().A);
    }

    [TestMethod]
    public void TestHsl1()
    {
        // Arrange, Act
        bool result = CssColor.TryFromHsl("hsl(33, 65%, 61%)", out CssColor color);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(220, color.ToMediaColor().R);
        Assert.AreEqual(162, color.ToMediaColor().G);
        Assert.AreEqual(91, color.ToMediaColor().B);
        Assert.AreEqual(255, color.ToMediaColor().A);
    }

    [TestMethod]
    public void TestHsl2()
    {
        // Arrange, Act
        bool result = CssColor.TryFromHsl("hsl(143, 72%, 32%)", out CssColor color);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(23, color.ToMediaColor().R);
        Assert.AreEqual(140, color.ToMediaColor().G);
        Assert.AreEqual(68, color.ToMediaColor().B);
        Assert.AreEqual(255, color.ToMediaColor().A);
    }

    [TestMethod]
    public void TestHsla()
    {
        // Arrange, Act
        bool result = CssColor.TryFromHsla("hsla(0, 100%, 50%, 0.5)", out CssColor color);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(255, color.ToMediaColor().R);
        Assert.AreEqual(0, color.ToMediaColor().G);
        Assert.AreEqual(0, color.ToMediaColor().B);
        Assert.AreEqual(128, color.ToMediaColor().A);
    }

    [TestMethod]
    public void TestByName1()
    {
        // Arrange, Act
        bool result = CssColors.TryGetByName("red", out CssColor color);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(255, color.ToMediaColor().R);
        Assert.AreEqual(0, color.ToMediaColor().G);
        Assert.AreEqual(0, color.ToMediaColor().B);
        Assert.AreEqual(255, color.ToMediaColor().A);
    }

    [TestMethod]
    public void TestByName2()
    {
        // Arrange, Act
        bool result = CssColors.TryGetByName("BlanchedAlmond", out CssColor color);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(255, color.ToMediaColor().R);
        Assert.AreEqual(235, color.ToMediaColor().G);
        Assert.AreEqual(205, color.ToMediaColor().B);
        Assert.AreEqual(255, color.ToMediaColor().A);
    }
}