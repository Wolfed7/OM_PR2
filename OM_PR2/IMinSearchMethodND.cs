namespace OM_PR2;

public interface IMinSearchMethodND
{
   PointND Min { get; }
   public void Compute(PointND startPoint, IFunction function);
}