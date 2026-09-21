public interface ICollectionRandomizer
{
    ICollection<T> Shuffle<T>(ICollection<T> collection);
}
