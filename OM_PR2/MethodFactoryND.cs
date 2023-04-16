namespace OM_PR2;

public class MethodFactoryND
{
   private PointND _startPoint;
   private IMinSearchMethodND _minSearchMethod;
   private IFunction _function;

   public MethodFactoryND(IMinSearchMethodND minSearchMethod, IFunction function, PointND startPoint)
   {
      _startPoint = startPoint;
      _minSearchMethod = minSearchMethod;
      _function = function;
   }

   public void Compute()
      => _minSearchMethod.Compute(_startPoint, _function);
   
   public PointND GetMinPoint() 
      => _minSearchMethod.Min;
   public void SetMinSearchMethod(IMinSearchMethodND method) 
      => _minSearchMethod = method;
   public void SetFunction(IFunction function) 
      => _function = function;
}
