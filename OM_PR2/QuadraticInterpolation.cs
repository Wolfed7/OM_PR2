namespace OM_PR2;

public class QuadraticInterpolation : IMinSearchMethod1D
{
   public double FunctionComputingCounter { get; private set; }
   private double _min;
   public double Min => _min;
   public double MaxIters => 10000;
   public double Eps { get; init; }

   public QuadraticInterpolation(double eps)
       => Eps = eps;

   public void Compute(IFunction function, Interval interval, PointND direction, PointND point)
   {
      double f0, f1, f2, xk, x1, x2, b, c;
      int iters;
      double x0 = interval.Center;
      double step = (interval.RightBoundary - interval.LeftBoundary) / 2;

      for (iters = 0; iters < MaxIters; iters++)
      {
         x1 = x0 - step;
         x2 = x0 + step;

         f0 = function.Compute(point + x0 * direction);
         f1 = function.Compute(point + x1 * direction);
         f2 = function.Compute(point + x2 * direction);
         FunctionComputingCounter += 3;

         //b = (-f1 * (2 * x0 + step) + 4 * f0 * x0 - f2 * (2 * x0 - step)) / (2 * step * step);
         //c = (f1 - 2 * f0 + f2) / (2 * step * step);
         //xk = -b / (2 * c);

         // Вроде бы эквивалентно.
         xk = x0 - 0.5 * step * (f2 - f1) / (f2 - 2 * f0 + f1);

         if (Math.Abs(xk - x0) < Eps)
         {
            _min = xk;
            break;
         }
         else
         {
            x0 = xk;
         }
      }
   }
}