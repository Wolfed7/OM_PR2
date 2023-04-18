//namespace OM_PR2;

//public class QuadraticInterpolation : IMinSearchMethod1D
//{
//   public int FunctionComputings { get; private set; }
//   private double _min;
//   public double Min => _min;
//   public double MaxIters => 10000;
//   public double Eps { get; init; }

//   public QuadraticInterpolation(double eps)
//       => Eps = eps;

//   public void Compute(IFunction function, Interval interval, PointND direction, PointND point)
//   {
//      FunctionComputings = 0;
//      double f0, f1, f2;
//      int iters;
//      double x1 = interval.LeftBoundary;
//      double x0 = interval.Center;
//      double x2 = interval.RightBoundary;
//      double xkprev = 0;

//      f1 = function.Compute(point + x1 * direction);
//      f0 = function.Compute(point + x0 * direction);
//      f2 = function.Compute(point + x2 * direction);
//      FunctionComputings += 3;

//      for (iters = 0; iters < MaxIters; iters++)
//      {

//         double c2 = (f0 - f1) / (x0 - x1);
//         double c3 = ((f2 - f1) / (x2 - x1) - (f0 - f1) / (x0 - x1)) / (x2 - x0);

//         double xk = (x1 + x0 - c2 / c3) / 2.0;
//         double fx = function.Compute(point + xk * direction);
//         FunctionComputings += 1;

//         if (Math.Abs(xk - xkprev) < Eps)
//         {
//            _min = xk;
//            break;
//         }

//         xkprev = xk;
//         if (xk > f0)
//         {
//            if (fx > f2)
//            {
//               x2 = xk;
//               f2 = fx;
//            }
//            else
//            {
//               x1 = x0;
//               f1 = f0;
//               x0 = xk;
//               f0 = fx;
//            }
//         }
//         else
//         {
//            if (fx < f0)
//            {
//               x2 = x0;
//               f2 = f0;
//               x0 = xk;
//               f0 = fx;
//            }
//            else
//            {
//               x1 = xk;
//               f1 = fx;
//            }
//         }

//      }
//   }
//}





//Предположительно не работатет.
namespace OM_PR2;

public class QuadraticInterpolation : IMinSearchMethod1D
{
   public int FunctionComputings { get; private set; }
   private double _min;
   public double Min => _min;
   public double MaxIters => 1000;
   public double Eps { get; init; }

   public QuadraticInterpolation(double eps)
       => Eps = eps;

   public void Compute(IFunction function, Interval interval, PointND direction, PointND point)
   {
      FunctionComputings = 0;
      double f0, f1, f2, xk, x1, x2, b, c;
      int iters;
      double x0 = interval.Center;
      double step = interval.Length / 2.0;

      for (iters = 0; iters < MaxIters; iters++)
      {

         x1 = x0 - step;
         x2 = x0 + step;

         f0 = function.Compute(point + x0 * direction);
         f1 = function.Compute(point + x1 * direction);
         f2 = function.Compute(point + x2 * direction);
         FunctionComputings += 3;

         //b = (-f1 * (2 * x0 + step) + 4 * f0 * x0 - f2 * (2 * x0 - step)) / (2 * step * step);
         //c = (f1 - 2 * f0 + f2) / (2 * step * step);
         //xk = -b / (2 * c);

         // Вроде бы эквивалентно.
         xk = x0 - 0.5 * step * (f2 - f1) / (f2 - 2.0 * f0 + f1);
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

