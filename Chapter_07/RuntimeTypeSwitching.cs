namespace Chapter_07.RuntimeTypeSwitching;

// This design breaks the LSP, because it uses runtime type switching.
// We have a special path in the FuelAll method checking if a given object
// is of a specific type.
public class FuelStation_BreakingLsp
{
    public void FuelAll(List<IFuelable> fuelables, FuelHose hose)
    {
        foreach (var fuelable in fuelables)
        {
            if (fuelable is KeroseneLamp)
            {
                var lamp = fuelable as KeroseneLamp;
                var adapter = new HoseAdapter(lamp);
                adapter.Attach(hose);
            }
            else
            {
                fuelable.Attach(hose);
            }

            hose.Fuel(fuelable);
        }
    }
}

// One solution is to make KeroseneLamp deal by itself with the special case it needs.
// Compare the KeroseneLamp_BreakingLsp and KeroseneLamp types.
// For KeroseneLamp, special case is not needed in FuelAll, 
// because it can handle the Attach method itself.
public class FuelStation
{
    public void FuelAll(List<IFuelable> fuelables, FuelHose hose)
    {
        foreach (var fuelable in fuelables)
        {
            fuelable.Attach(hose);
            hose.Fuel(fuelable);
        }
    }
}

public class KeroseneLamp : IFuelable
{
    public int MaxFuel => 3;
    public int RemainingFuel => 3;

    public void Attach(FuelHose hose)
    {
        var adapter = new HoseAdapter(this);
        adapter.Attach(hose);
    }
}

public interface IFuelable
{
    int MaxFuel { get; }
    int RemainingFuel { get; }

    void Attach(FuelHose hose);
}

public class FuelHose
{
    public void Fuel(IFuelable fuelable)
    {
        //fuel the fuelable
    }
}

public class KeroseneLamp_BreakingLsp : IFuelable
{
    public int MaxFuel => 3;
    public int RemainingFuel => 3;

    public void Attach(FuelHose hose)
    {
        throw new InvalidOperationException(
            "Can't attach fuel hose");
    }
}

public class HoseAdapter
{
    private KeroseneLamp _lamp;

    public HoseAdapter(KeroseneLamp? lamp)
    {
        _lamp = lamp;
    }

    public void Attach(FuelHose hose)
    {
        //attach hose to the adapter
    }
}