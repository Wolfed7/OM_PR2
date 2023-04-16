namespace OM_PR2;

public interface IMinSearchMethod1D
{
   public double Min { get; }
   public void Compute(IFunction function, Interval interval, PointND argument, PointND direction);
}