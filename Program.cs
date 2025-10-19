using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

// 1. Додано проект для побудови модульних тестів MSTest
[TestClass]
public class PlantTests
{
    private Plant testPlant;
    private static Type plantType;
    private System.Reflection.FieldInfo countField;
    private System.Reflection.FieldInfo totalHeightField;

    // Використано атрибут [TestInitialize] для ініціалізації перед кожним тестом
    [TestInitialize]
    public void TestInitialize()
    {
        testPlant = new Plant("Тестова рослина", PlantType.Flower, 2, 0.5, new DateTime(2023, 5, 15));
        plantType = typeof(Plant);
        countField = plantType.GetField("_count", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        totalHeightField = plantType.GetField("_totalHeight", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

        countField.SetValue(null, 0);
        totalHeightField.SetValue(null, 0.0);
    }

    // Використано атрибут [TestCleanup] для очищення після кожного тесту
    [TestCleanup]
    public void TestCleanup()
    {
        testPlant = null;
    }

    // Тест-метод з назвою, що відображає суть тесту та структурою AAA
    [TestMethod]
    public void PlantConstructor_ValidParameters_ObjectInitializedCorrectly()
    {
        // Arrange
        string name = "Троянда";
        PlantType type = PlantType.Flower;
        int age = 2;
        double height = 0.5;
        DateTime plantingDate = new DateTime(2023, 5, 15);

        // Act
        Plant plant = new Plant(name, type, age, height, plantingDate);

        // Assert
        Assert.AreEqual(name, plant.Name);
        Assert.AreEqual(type, plant.Type);
        Assert.AreEqual(age, plant.Age);
        Assert.AreEqual(height, plant.Height);
        Assert.AreEqual(plantingDate, plant.PlantingDate);
    }

    // Тестування роботи секції set властивості Name з перевірками
    [TestMethod]
    public void NameSet_EmptyString_ThrowsArgumentException()
    {
        // Arrange
        string invalidName = "";

        // Act & Assert
        try
        {
            testPlant.Name = invalidName;
            Assert.Fail("Очікувалося виняток ArgumentException");
        }
        catch (ArgumentException)
        {
            Assert.IsTrue(true);
        }
    }

    // Тестування роботи секції set властивості Name з перевірками
    [TestMethod]
    public void NameSet_TooShort_ThrowsArgumentException()
    {
        // Arrange
        string invalidName = "А";

        // Act & Assert
        try
        {
            testPlant.Name = invalidName;
            Assert.Fail("Очікувалося виняток ArgumentException");
        }
        catch (ArgumentException)
        {
            Assert.IsTrue(true);
        }
    }

    // Тестування роботи секції set властивості Height з перевірками
    [TestMethod]
    public void HeightSet_NegativeValue_ThrowsArgumentException()
    {
        // Arrange
        double invalidHeight = -1.0;

        // Act & Assert
        try
        {
            testPlant.Height = invalidHeight;
            Assert.Fail("Очікувалося виняток ArgumentException");
        }
        catch (ArgumentException)
        {
            Assert.IsTrue(true);
        }
    }

    // Тестування роботи секції set властивості Type з перевірками
    [TestMethod]
    public void TypeSet_InvalidEnumValue_ThrowsArgumentException()
    {
        // Arrange
        PlantType invalidType = (PlantType)100;

        // Act & Assert
        try
        {
            testPlant.Type = invalidType;
            Assert.Fail("Очікувалося виняток ArgumentException");
        }
        catch (ArgumentException)
        {
            Assert.IsTrue(true);
        }
    }

    // Тестування нестатичного методу GetDescription
    [TestMethod]
    public void GetDescription_ValidPlant_ReturnsCorrectDescription()
    {
        // Arrange
        string expectedDescription = "Тестова рослина (Flower) - 2 років, 0,5 м";

        // Act
        string actualDescription = testPlant.GetDescription();

        // Assert
        Assert.AreEqual(expectedDescription, actualDescription);
    }

    // Тестування нестатичного методу IsMature
    [TestMethod]
    public void IsMature_YoungPlant_ReturnsFalse()
    {
        // Arrange
        var youngPlant = new Plant("Молода рослина", PlantType.Tree, 1, 1.0, DateTime.Now);

        // Act
        bool result = youngPlant.IsMature();

        // Assert
        Assert.IsFalse(result);
    }

    // Тестування нестатичного методу IsMature
    [TestMethod]
    public void IsMature_AdultPlant_ReturnsTrue()
    {
        // Arrange
        var adultPlant = new Plant("Доросла рослина", PlantType.Tree, 10, 5.0, DateTime.Now.AddYears(-10));

        // Act
        bool result = adultPlant.IsMature();

        // Assert
        Assert.IsTrue(result);
    }

    // Тестування нестатичного методу Grow
    [TestMethod]
    public void Grow_WithPositiveGrowth_IncreasesHeight()
    {
        // Arrange
        double initialHeight = testPlant.Height;
        double growth = 0.3;

        // Act
        testPlant.Grow(growth);

        // Assert
        Assert.AreEqual(initialHeight + growth, testPlant.Height);
    }

    // Тестування нестатичного методу Grow з використанням Assert
    [TestMethod]
    public void Grow_WithNegativeGrowth_ThrowsArgumentException()
    {
        // Arrange
        double negativeGrowth = -0.1;

        // Act & Assert
        try
        {
            testPlant.Grow(negativeGrowth);
            Assert.Fail("Очікувалося виняток ArgumentException");
        }
        catch (ArgumentException)
        {
            Assert.IsTrue(true);
        }
    }

    // Тестування перевантаженого нестатичного методу Grow
    [TestMethod]
    public void Grow_WithoutParameters_IncreasesHeightByDefault()
    {
        // Arrange
        double initialHeight = testPlant.Height;

        // Act
        testPlant.Grow();

        // Assert
        Assert.IsTrue(testPlant.Height > initialHeight);
    }

    // Тестування властивості AgeCategory
    [TestMethod]
    public void AgeCategory_YoungPlant_ReturnsYoung()
    {
        // Arrange
        var youngPlant = new Plant("Молода", PlantType.Flower, 1, 0.1, DateTime.Now);

        // Act
        string category = youngPlant.AgeCategory;

        // Assert
        Assert.AreEqual("Молода", category);
    }

    // Тестування властивості AgeCategory
    [TestMethod]
    public void AgeCategory_AdultPlant_ReturnsAdult()
    {
        // Arrange
        var adultPlant = new Plant("Доросла", PlantType.Tree, 5, 3.0, DateTime.Now.AddYears(-5));

        // Act
        string category = adultPlant.AgeCategory;

        // Assert
        Assert.AreEqual("Доросла", category);
    }

    // Тестування статичного методу Parse
    [TestMethod]
    public void Parse_ValidString_ReturnsPlantObject()
    {
        // Arrange
        string validString = "Троянда,Flower,2,0.5,2023-05-15,True";

        // Act
        Plant plant = Plant.Parse(validString);

        // Assert
        Assert.IsNotNull(plant);
        Assert.AreEqual("Троянда", plant.Name);
        Assert.AreEqual(PlantType.Flower, plant.Type);
        Assert.AreEqual(2, plant.Age);
    }

    // Тестування статичного методу Parse з використанням Assert
    [TestMethod]
    public void Parse_InvalidString_ThrowsFormatException()
    {
        // Arrange
        string invalidString = "Неправильний,рядок";

        // Act & Assert
        try
        {
            Plant.Parse(invalidString);
            Assert.Fail("Очікувалося виняток FormatException");
        }
        catch (FormatException)
        {
            Assert.IsTrue(true);
        }
    }

    // Тестування статичного методу TryParse
    [TestMethod]
    public void TryParse_ValidString_ReturnsTrueAndPlant()
    {
        // Arrange
        string validString = "Кактус,Succulent,5,0.3,2020-10-10,False";

        // Act
        bool result = Plant.TryParse(validString, out Plant plant);

        // Assert
        Assert.IsTrue(result);
        Assert.IsNotNull(plant);
        Assert.AreEqual("Кактус", plant.Name);
    }

    // Тестування статичного методу TryParse
    [TestMethod]
    public void TryParse_InvalidString_ReturnsFalseAndNull()
    {
        // Arrange
        string invalidString = "Неправильний,рядок";

        // Act
        bool result = Plant.TryParse(invalidString, out Plant plant);

        // Assert
        Assert.IsFalse(result);
        Assert.IsNull(plant);
    }

    // Тестування статичної властивості Count
    [TestMethod]
    public void Count_AfterAddingPlants_ReturnsCorrectNumber()
    {
        // Arrange
        var plant1 = new Plant("Рослина1", PlantType.Tree, 1, 1.0, DateTime.Now);
        var plant2 = new Plant("Рослина2", PlantType.Flower, 2, 0.5, DateTime.Now);

        // Act
        Plant.AddPlantToCollection(plant1);
        Plant.AddPlantToCollection(plant2);

        // Assert
        Assert.AreEqual(2, Plant.Count);
    }

    // Тестування статичної властивості AverageHeight
    [TestMethod]
    public void AverageHeight_WithMultiplePlants_ReturnsCorrectAverage()
    {
        // Arrange
        var plant1 = new Plant("Висока", PlantType.Tree, 1, 10.0, DateTime.Now);
        var plant2 = new Plant("Низька", PlantType.Flower, 2, 2.0, DateTime.Now);

        // Act
        Plant.AddPlantToCollection(plant1);
        Plant.AddPlantToCollection(plant2);

        // Assert
        Assert.AreEqual(6.0, Plant.AverageHeight);
    }

    // Тестування нестатичного методу WaterPlant
    [TestMethod]
    public void WaterPlant_AfterWatering_UpdatesLastWatered()
    {
        // Arrange
        string initialLastWatered = testPlant.LastWatered;

        // Act
        testPlant.WaterPlant();

        // Assert
        Assert.AreNotEqual(initialLastWatered, testPlant.LastWatered);
        Assert.IsTrue(testPlant.LastWatered != "Ніколи");
    }

    // Тестування перевантаженого нестатичного методу WaterPlant
    [TestMethod]
    public void WaterPlant_WithWaterType_ExecutesSuccessfully()
    {
        // Arrange
        string waterType = "дощова";

        // Act & Assert
        testPlant.WaterPlant(waterType);
        Assert.IsTrue(true);
    }

    // Тестування конструктора з двома параметрами
    [TestMethod]
    public void Constructor_NameAndType_CreatesPlantWithDefaultValues()
    {
        // Arrange
        string name = "Нова рослина";
        PlantType type = PlantType.Shrub;

        // Act
        Plant plant = new Plant(name, type);

        // Assert
        Assert.AreEqual(name, plant.Name);
        Assert.AreEqual(type, plant.Type);
        Assert.AreEqual(1, plant.Age);
    }

    // Тестування конструктора з двома параметрами
    [TestMethod]
    public void Constructor_NameAndAge_CreatesPlantWithDefaultValues()
    {
        // Arrange
        string name = "Стара рослина";
        int age = 10;

        // Act
        Plant plant = new Plant(name, age);

        // Assert
        Assert.AreEqual(name, plant.Name);
        Assert.AreEqual(age, plant.Age);
        Assert.AreEqual(PlantType.Tree, plant.Type);
    }

    // Тестування методу ToString
    [TestMethod]
    public void ToString_ValidPlant_ReturnsCorrectFormat()
    {
        // Arrange
        var plant = new Plant("Тест", PlantType.Grass, 3, 0.7, new DateTime(2022, 6, 1));
        plant.IsFlowering = false;

        // Act
        string result = plant.ToString();

        // Assert
        StringAssert.Contains(result, "Тест");
        StringAssert.Contains(result, "Grass");
        StringAssert.Contains(result, "3");
        StringAssert.Contains(result, "0.7");
    }

    // Тестування роботи секції set властивості PlantingDate з перевірками
    [TestMethod]
    public void PlantingDateSet_FutureDate_ThrowsArgumentException()
    {
        // Arrange
        DateTime futureDate = DateTime.Now.AddDays(1);

        // Act & Assert
        try
        {
            testPlant.PlantingDate = futureDate;
            Assert.Fail("Очікувалося виняток ArgumentException");
        }
        catch (ArgumentException)
        {
            Assert.IsTrue(true);
        }
    }

    // Тестування роботи секції set властивості PlantingDate з перевірками
    [TestMethod]
    public void PlantingDateSet_VeryOldDate_ThrowsArgumentException()
    {
        // Arrange
        DateTime oldDate = new DateTime(1899, 1, 1);

        // Act & Assert
        try
        {
            testPlant.PlantingDate = oldDate;
            Assert.Fail("Очікувалося виняток ArgumentException");
        }
        catch (ArgumentException)
        {
            Assert.IsTrue(true);
        }
    }

    // Використано атрибут [DataRow] для параметризованого тесту
    [DataTestMethod]
    [DataRow(1, "Молода")]
    [DataRow(5, "Доросла")]
    [DataRow(15, "Стара")]
    public void AgeCategory_DifferentAges_ReturnsCorrectCategory(int age, string expectedCategory)
    {
        // Arrange
        var plant = new Plant("Тест", PlantType.Tree, age, 1.0, DateTime.Now.AddYears(-age));

        // Act
        string actualCategory = plant.AgeCategory;

        // Assert
        Assert.AreEqual(expectedCategory, actualCategory);
    }

    // Тестування статичного методу GetGlobalPlantInfo
    [TestMethod]
    public void GetGlobalPlantInfo_AfterAddingPlants_ReturnsCorrectInfo()
    {
        // Arrange
        var plant1 = new Plant("Рослина1", PlantType.Tree, 1, 5.0, DateTime.Now);
        var plant2 = new Plant("Рослина2", PlantType.Flower, 2, 3.0, DateTime.Now);
        Plant.AddPlantToCollection(plant1);
        Plant.AddPlantToCollection(plant2);

        // Act
        string info = Plant.GetGlobalPlantInfo();

        // Assert
        StringAssert.Contains(info, "2");
        StringAssert.Contains(info, "4,00");
    }

    // Тестування нестатичного методу GetPlantingInfo
    [TestMethod]
    public void GetPlantingInfo_ValidPlant_ReturnsCorrectString()
    {
        // Arrange
        DateTime plantingDate = new DateTime(2023, 5, 15);
        var plant = new Plant("Тест", PlantType.Flower, 1, 0.5, plantingDate);

        // Act
        string info = plant.GetPlantingInfo();

        // Assert
        StringAssert.Contains(info, "Тест");
        StringAssert.Contains(info, "15.05.2023");
    }

    // Тестування властивості IsFlowering
    [TestMethod]
    public void IsFlowering_SetAndGet_ReturnsCorrectValue()
    {
        // Arrange
        bool expectedValue = true;

        // Act
        testPlant.IsFlowering = expectedValue;

        // Assert
        Assert.AreEqual(expectedValue, testPlant.IsFlowering);
    }

    // Тестування статичного методу AddPlantToCollection
    [TestMethod]
    public void AddPlantToCollection_ValidPlant_IncreasesCountAndTotalHeight()
    {
        // Arrange
        var plant = new Plant("Нова", PlantType.Tree, 1, 2.0, DateTime.Now);
        int initialCount = Plant.Count;
        double initialTotalHeight = (double)totalHeightField.GetValue(null);

        // Act
        Plant.AddPlantToCollection(plant);

        // Assert
        Assert.AreEqual(initialCount + 1, Plant.Count);
        Assert.AreEqual(initialTotalHeight + plant.Height, (double)totalHeightField.GetValue(null));
    }

    // Тестування роботи секції set властивості Age з перевірками
    [TestMethod]
    public void AgeSet_InvalidValue_ThrowsArgumentException()
    {
        // Arrange
        int invalidAge = -1;

        // Act & Assert
        try
        {
            testPlant.Age = invalidAge;
            Assert.Fail("Очікувалося виняток ArgumentException");
        }
        catch (ArgumentException)
        {
            Assert.IsTrue(true);
        }
    }

    // Тестування перевантаженого нестатичного методу WaterPlant
    [TestMethod]
    public void WaterPlant_WithMilliliters_ExecutesSuccessfully()
    {
        // Arrange
        int milliliters = 500;

        // Act & Assert
        testPlant.WaterPlant(milliliters);
        Assert.IsTrue(true);
    }

    // Тестування перевантаженого нестатичного методу WaterPlant
    [TestMethod]
    public void WaterPlant_WithWaterTypeAndMilliliters_ExecutesSuccessfully()
    {
        // Arrange
        string waterType = "мінеральна";
        int milliliters = 300;

        // Act & Assert
        testPlant.WaterPlant(waterType, milliliters);
        Assert.IsTrue(true);
    }
}