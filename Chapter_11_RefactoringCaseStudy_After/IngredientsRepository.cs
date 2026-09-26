public class IngredientsRepository : IIngredientsRepository
{
    private const string IngredientsFile = "ingredients.txt";
    private readonly IFileAccess _fileAccess;

    public IngredientsRepository(IFileAccess fileAccess)
    {
        _fileAccess = fileAccess;
    }

    public List<Ingredient> GetAllIngredients()
    {
        var result = new List<Ingredient>();

        var allLines = _fileAccess.ReadAllLines(IngredientsFile);

        foreach (var line in allLines)
        {
            result.Add(BuildIngredientFrom(line));
        }

        return result;
    }

    private static Ingredient BuildIngredientFrom(string line)
    {
        var split = line.Split(',');
        Enum.TryParse(split[1], ignoreCase: true, out DietOption dietOption);
        return new Ingredient(split[0], dietOption);
    }
}
