public class Ingredient
{
    public string Name { get; }
    public DietOption DietOption { get; }

    public Ingredient(string name, DietOption dietOption)
    {
        Name = name;
        DietOption = dietOption;
    }

    public bool IsOkInDiet(DietOption dietOption)
    {
        if (dietOption == DietOption.Omnivore)
        {
            return true;
        }

        if (dietOption == DietOption.Vegan)
        {
            return DietOption == DietOption.Vegan;
        }

        return
            DietOption == DietOption.Vegan ||
            DietOption == DietOption.Vegetarian;

    }

    public override string ToString()
    {
        return Name;
    }
}
