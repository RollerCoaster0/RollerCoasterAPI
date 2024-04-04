namespace RollerCoaster.Tests;

[TestClass]
public class CharacterClassServiceTest
{ private readonly CharacterClassServiceTest _characterClassServiceTest;

    public CharacterClassServiceTest()
    {
        _characterClassServiceTest = new CharacterClassServiceTest();
    }
    [TestMethod]
    public void ClassService_ReturnFalse()
    {
        bool result = ClassService_ReturnFalse(0);

        Assert.IsFalse(result, "1 should not be prime");
    }
}
