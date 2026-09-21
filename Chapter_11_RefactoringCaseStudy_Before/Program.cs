var pizzaGenerator = new PizzaGenerator(
    new RandomWrapper(), new FileWrapper());

var pizza = pizzaGenerator.Generate(3, DietOption.Vegetarian);
Console.WriteLine(pizza);

Console.ReadKey();
