var pizzaGenerator = new PizzaGenerator(
    new IngredientsRepository(new FileWrapper()),
    new CollectionRandomizer(new RandomWrapper()));

var pizza = pizzaGenerator.Generate(3, DietOption.Vegetarian);

Console.WriteLine(pizza);

Console.ReadKey();
