public class PizzaGenerator
{
    private const string IngredientsFile = "ingredients.txt";
    private IFileAccess _fileAccess;
    private IRandom _random;

    public PizzaGenerator(IRandom random, IFileAccess fileAccess)
    {
        _random = random;
        _fileAccess = fileAccess;
    }

    public Pizza Generate(int countOfIngredients, DietOption dietOption)
    {
        var allIngredientsForCurrentDietOption =
            GetAllIngredients()
            .Where(ingredient => IsOkInDiet(dietOption, ingredient))
            .ToList();

        var selectedIngredients =
            Shuffle(allIngredientsForCurrentDietOption)
            .Take(countOfIngredients)
            .ToList();

        return new Pizza(selectedIngredients);
    }

    private List<Ingredient> GetAllIngredients()
    {
        var result = new List<Ingredient>();

        var allLines = _fileAccess.ReadAllLines(IngredientsFile);

        foreach (var line in allLines)
        {
            var split = line.Split(',');
            Enum.TryParse(split[1], ignoreCase: true, out DietOption dietOption);
            result.Add(new Ingredient(split[0], dietOption));
        }

        return result;
    }

    private bool IsOkInDiet(DietOption dietOption, Ingredient ingredient)
    {
        if (dietOption == DietOption.Omnivore)
        {
            return true;
        }

        if (dietOption == DietOption.Vegan)
        {
            return ingredient.DietOption == DietOption.Vegan;
        }

        return
            ingredient.DietOption == DietOption.Vegan ||
            ingredient.DietOption == DietOption.Vegetarian;

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
