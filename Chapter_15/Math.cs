class Math
{
    public static int Multiply(int a, int b)
    {
        return a * b;
    }

    public static int Add(int a, int b)
    {
        return a + b;
    }

    public static float Divide(int a, int b)
    {
        if (b == 0)
        {
            throw new ArgumentException("Attempted to divide by zero.");
        }

        return a / b;
    }
}
