namespace OM_PR2;

public interface IMinSearchMethod1D
{
   public double Min { get; }
   public int FunctionComputings { get; }
   public void Compute(IFunction function, Interval interval, PointND direction, PointND point);
}