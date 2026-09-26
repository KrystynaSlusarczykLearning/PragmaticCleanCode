public class CollectionRandomizer : ICollectionRandomizer
{
    private readonly IRandom _random;

    public CollectionRandomizer(IRandom random)
    {
        _random = random;
    }

    public ICollection<T> Shuffle<T>(ICollection<T> collection)
    {
        var collectionCopy = collection.ToList();
        int n = collectionCopy.Count;
        while (n > 1)
        {
            n--;
            int k = _random.Next(n + 1);
            T value = collectionCopy[k];
            collectionCopy[k] = collectionCopy[n];
            collectionCopy[n] = value;
        }

        return collectionCopy;
    }
}