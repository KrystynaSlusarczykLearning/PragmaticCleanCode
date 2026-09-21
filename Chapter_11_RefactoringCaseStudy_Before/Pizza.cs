public class Pizza
{
    private List<Ingredient> _ingredients;

    public Pizza(List<Ingredient> ingredients)
    {
        _ingredients = ingredients;
    }

    public override string ToString()
    {
        return $"A pizza with {string.Join(", ", _ingredients)}";
    }
}
