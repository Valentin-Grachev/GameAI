

public struct Neighbors<T>
{

    public T right;
    public T left;
    public T top;
    public T bottom;


    public Neighbors(T right, T left, T top, T bottom)
    {
        this.right = right;
        this.left = left;
        this.top = top;
        this.bottom = bottom;
    }




}
