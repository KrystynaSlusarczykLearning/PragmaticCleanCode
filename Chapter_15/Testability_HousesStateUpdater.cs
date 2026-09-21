using Moq;
using NUnit.Framework;

public class HousesStateUpdater
{
    private IHousesRepository _housesRepository;

    public HousesStateUpdater(IHousesRepository housesRepository)
    {
        _housesRepository = housesRepository;
    }

    public void UpdateHouseOwner(int houseId, Owner owner)
    {
        var house = new House(houseId, owner);

        _housesRepository.Save(house);
    }
}

[TestFixture]
public class HousesStateUpdaterTests
{
    // This test will fail because of what happens in the House class constructor.
    [Test]
    [Ignore("Fails because House class constructor does risky things.")]
    public void UpdateHouseOwner_ShouldSaveHouseWithCorrectParameters()
    {
        int houseId = 1;
        var owner = new Owner { Name = "John Doe" };
        var housesRepositoryMock = new Mock<IHousesRepository>();
        var housesStateUpdater = new HousesStateUpdater(
            housesRepositoryMock.Object);

        housesStateUpdater.UpdateHouseOwner(houseId, owner);

        housesRepositoryMock.Verify(
            mock => mock.Save(
                It.Is<House>(h => h.HouseId == houseId && h.Owner == owner)),
                Times.Once);
    }
}

public interface IHousesRepository
{
    bool DoesHouseExist(int houseId);
    bool DoesOwnerExist(Owner owner);
    void Save(House house);
}

public class Owner
{
    public string Name { get; init; }
}

public class House
{
    public int HouseId { get; }
    public Owner Owner { get; }

    // This constructor connects to some database, 
    // which can affect the testability negatively.
    public House(int houseId, Owner owner)
    {
        var housesRepository = new HousesRepository();

        if (!housesRepository.DoesHouseExist(houseId))
        {
            throw new ArgumentException(
                $"A house with ID {houseId} does not exist.");
        }

        if (!housesRepository.DoesOwnerExist(owner))
        {
            throw new ArgumentException(
                $"An owner named {owner.Name} does not exist.");
        }

        HouseId = houseId;
        Owner = owner;
    }
}

// This call calls the constructor of T at a point in time which can be hard to predict.
// That's why no constructor should perform any advanced or performance-heavy logic. 
public class Lazy<T> where T : new()
{
    private T _instance;
    private bool _isCreated = false;

    public T Instance
    {
        get
        {
            if (!_isCreated)
            {
                _instance = new T();
                _isCreated = true;
            }

            return _instance;
        }
    }
}

public class HousesRepository : IHousesRepository
{
    public void Save(House house)
    {
        throw new NotImplementedException();
    }

    public bool DoesHouseExist(int houseId)
    {
        throw new NotImplementedException();
    }

    public bool DoesOwnerExist(Owner owner)
    {
        throw new NotImplementedException();
    }
}