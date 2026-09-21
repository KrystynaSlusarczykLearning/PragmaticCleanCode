public class Ingredient
{
    public string Name { get; }
    public DietOption DietOption { get; }

    public Ingredient(string name, DietOption dietOption)
    {
        Name = name;
        DietOption = dietOption;
    }

    public override string ToString()
    {
        return Name;
    }
}
