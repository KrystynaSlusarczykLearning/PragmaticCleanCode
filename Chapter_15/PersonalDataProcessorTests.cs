using Moq;
using NUnit.Framework;

[TestFixture]
public class PersonalDataProcessorTests
{
    [Test]
    public void FindMaxAge_ReturnsTheAgeOfOldestPerson()
    {
        //Arrange
        var peopleRepositoryMock = new Mock<IPeopleRepository>();
        peopleRepositoryMock.Setup(repo => repo.GetAll()).Returns(
            new List<Person>
            {
                new Person("John", "Doe", 25 ),
                new Person("Jane", "Doe", 80 ),
                new Person("Max", "Smith", 30)
            });

        //Act
        var processor = new PersonalDataProcessor(peopleRepositoryMock.Object);

        //Assert
        var result = processor.FindMaxAge();

        Assert.That(result, Is.EqualTo(80));
    }
}
