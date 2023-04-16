namespace OM_PR2;

public static class IntervalSearch
{
   public static Interval Search(IFunction function, double x0, double sigma, PointND direction, PointND point)
   {
      double x, xk, xk1, h;

      if (function.Compute(point + x0 * direction) > function.Compute(point + (x0 + sigma) * direction))
      {
         xk = x0 + sigma;
         h = sigma;
      }
      else
      {
         xk = x0 - sigma;
         h = -sigma;
      }

      do
      {
         h *= 2;
         xk1 = xk + h;
         x = x0;
         x0 = xk;
         xk = xk1;

      } while (function.Compute(point + x0 * direction) > function.Compute(point + xk * direction));

      return (x < xk1) ? new(x, xk1) : new(xk1, x);
   }
}
