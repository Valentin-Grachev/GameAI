
public static class Exstensions
{

    public static bool Between(this int value, int fromInclusive, int toExclusive) 
        => fromInclusive <= value && value < toExclusive;
    

}
