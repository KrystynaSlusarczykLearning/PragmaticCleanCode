public class PizzaGenerator
{
    private readonly IIngredientsRepository _ingredientsRepository;
    private readonly ICollectionRandomizer _collectionRandomizer;

    public PizzaGenerator(
        IIngredientsRepository ingredientsRepository,
        ICollectionRandomizer collectionRandomizer)
    {
        _ingredientsRepository = ingredientsRepository;
        _collectionRandomizer = collectionRandomizer;
    }

    public Pizza Generate(int countOfIngredients, DietOption dietOption)
    {
        var allIngredientsForCurrentDietOption =
            _ingredientsRepository.GetAllIngredients()
            .Where(ingredient => ingredient.IsOkInDiet(dietOption))
            .ToList();

        var selectedIngredients =
            _collectionRandomizer.Shuffle(allIngredientsForCurrentDietOption)
            .Take(countOfIngredients)
            .ToList();

        return new Pizza(selectedIngredients);
    }
}