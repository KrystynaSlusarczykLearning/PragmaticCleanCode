Plane_BreakingLsp plane = new ToyPlane_BreakingLsp();
// This will give an error - ToyPlane cannot calculate remaining fuel.
var remainingFuel = plane.PercentOfRemainingFuel();
Console.WriteLine(remainingFuel);

Console.WriteLine("Press any key to close.");
Console.ReadKey();

// This design breaks the LSP - ToyPlane is forced to provide MaxFuel and RemainingFuel,
// resulting with an error in executing PercentOfRemainingFuel
// (the result will be NaN - Not a Number).
public abstract class Plane_BreakingLsp
{
    public virtual int MaxFuel => 1000;
    public virtual int RemainingFuel => 1000;

    public float PercentOfRemainingFuel()
    {
        return ((float)RemainingFuel / MaxFuel) * 100;
    }

    public abstract string Land();
}

// Cargo plane is a true plane and it does not cause a surprising behavior...
public class CargoPlane_BreakingLsp : Plane_BreakingLsp
{
    public override int MaxFuel => 2000;
    public override int RemainingFuel => 2000;

    public override string Land()
    {
        return "Land on the airstrip near the cargo terminal";
    }
}

// ...but Toy plane is not really a plane, and forcing it to declare its fuel levels
// results in an error in the calculation of remaining fuel (dividing 0 by 0 results in NaN)
public class ToyPlane_BreakingLsp : Plane_BreakingLsp
{
    public override int MaxFuel => 0;
    public override int RemainingFuel => 0;

    public override string Land()
    {
        return "Just don't hit the floor too hard.";
    }
}

// One solution is to have a dedicated interface for things that can be fueled.
// In this case, ToyPlane can remain a Plane, but it will not implement IFuelable.

public interface IFuelable
{
    int MaxFuel { get; }
    int RemainingFuel { get; }
}

public abstract class Plane
{
    public abstract string Land();
}

public class CargoPlane : Plane, IFuelable
{
    public int MaxFuel => 2000;
    public int RemainingFuel => 2000;

    public override string Land()
    {
        return "Land on the airstrip near the cargo terminal";
    }
}

public class AreobaticPlane : Plane, IFuelable
{
    public int MaxFuel => 200;
    public int RemainingFuel => 200;

    public override string Land()
    {
        return "Land in fashion.";
    }
}
public class ToyPlane : Plane
{
    public override string Land()
    {
        return "Just don't hit the floor too hard.";
    }
}

public class FuelCalculator
{
    public float GetPercentOfRemainingFuel(IFuelable fuelable)
    {
        return ((float)fuelable.RemainingFuel / fuelable.MaxFuel) * 100;
    }
}

