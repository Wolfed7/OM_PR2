namespace OM_PR2;

public class Fibonacci : IMinSearchMethod1D
{
   private double _min;
   public double Min => _min;
   public double Eps { get; init; }
   public int FunctionCount { get; private set; }
   public Fibonacci(double eps)
       => Eps = eps;

   public void Compute(IFunction function, Interval interval, PointND direction, PointND point)
   {
      int n = 1;
      double Fn_2;

      while (interval.Length / Eps > BinetRatio(n + 2))
         n++;

      Fn_2 = BinetRatio(n + 2);
      double x1 = interval.LeftBoundary + BinetRatio(n) / Fn_2 * interval.Length;
      double x2 = interval.LeftBoundary + BinetRatio(n + 1) / Fn_2 * interval.Length;

      double f1 = function.Compute(point + x1 * direction);
      double f2 = function.Compute(point + x2 * direction);

      // temp
      double previousLength = interval.Length;
      FunctionCount += 2;
      // temp

      for (int k = 1; k < n; k++)
      {

         //// temp
         //Console.Write(x1.ToString("e9").PadLeft(20));
         //Console.Write(x2.ToString("e9").PadLeft(20));
         //Console.Write(function.Func(x1).ToString("e9").PadLeft(20));
         //Console.Write(function.Func(x2).ToString("e9").PadLeft(20));
         //// temp

         if (f1 <= f2)
         {
            interval = new(interval.LeftBoundary, x2);
            x2 = x1;
            f2 = f1;
            //x1 = interval.LeftBoundary + (interval.RightBoundary - x2);
            x1 = interval.LeftBoundary + BinetRatio(n - k + 1) / BinetRatio(n - k + 3) * interval.Length;
            f1 = function.Compute(point + x1 * direction);
         }
         else 
         {
            interval = new(x1, interval.RightBoundary);
            x1 = x2;
            f1 = f2;
            //x2 = interval.RightBoundary - (x1 - interval.LeftBoundary);
            x2 = interval.LeftBoundary + BinetRatio(n - k + 2) / BinetRatio(n - k + 3) * interval.Length;
            f2 = function.Compute(point + x2 * direction);
         }

         FunctionCount += 1;

         //// temp
         //Console.Write(interval.LeftBoundary.ToString("e9").PadLeft(20));
         //Console.Write(interval.RightBoundary.ToString("e9").PadLeft(20));
         //Console.Write(interval.Length.ToString("e9").PadLeft(20));
         //Console.WriteLine((previousLength / interval.Length).ToString("f3").PadLeft(8));
         //previousLength = interval.Length;
         //// temp
      }

      // temp
      //Console.Write($"{functionCount}".PadLeft(4));
      //Console.WriteLine(Math.Log10(Eps).ToString("f0").PadLeft(5));
      // temp
      _min = interval.Center;
   }

   // Формула Бинэ.
   private double BinetRatio(int n) 
      => (Math.Pow((1 + Math.Sqrt(5)) / 2, n) 
      - Math.Pow((1 - Math.Sqrt(5)) / 2, n)) / Math.Sqrt(5);
}
