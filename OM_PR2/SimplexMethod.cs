using System.Globalization;
using System.Runtime.CompilerServices;

namespace OM_PR2;

public class SimplexMethod : IMinSearchMethodND
{
   private PointND _min;
   private PointND[] _simplex;

   public PointND Min => _min;
   public static double Alpha => 1; // Коэффициент отражения
   public static double Beta => 0.5;
   public static double Gamma => 2;
   public static double Distance => 1;

   public double Eps { get; init; }
   public int MaxIters { get; init; }


   public SimplexMethod(int maxIters, double eps)
   {
      MaxIters = maxIters;
      Eps = eps;
      _min = new PointND(0);
      _simplex = new PointND[0];
   }

   public void Compute(PointND startPoint, IFunction function)
   {
      int PointDimension = startPoint.Dimention;
      int SimplexSize = PointDimension + 1;

      List<PointND> coords = new(); // Координаты для графики.
      List<double> funcs = new(); // Значения функции.
      List<PointND> dirs = new(); // Значения направлений поиска.
      List<double> corner = new(); // Угол между векторами (xi, yi) и (s1, s2)
      int iters = 0; // Количество итераций.
      int FunctionsCalcs = 0; // Количество вычислений функции.

      _min = startPoint;
      _simplex = new PointND[SimplexSize];
      for (int i = 0; i < SimplexSize; i++)
         _simplex[i] = new PointND(PointDimension);

      double d1 = Distance * (Math.Sqrt(PointDimension + 1) + PointDimension - 1) / (PointDimension * Math.Sqrt(2));
      double d2 = Distance / ((PointDimension * Math.Sqrt(2)) * (Math.Sqrt(PointDimension + 1) - 1));

      // TODO сделать симплекс зависимым от стартовой точки. PS: вроде готово...?
      _simplex[PointDimension] = startPoint;
      for (int i = 0; i < PointDimension; i++)
         for (int j = 0; j < PointDimension; j++)
         {
            if (i == j)
            {
               _simplex[i][j] = d1 + _simplex[PointDimension][j];
               continue;
            }
            _simplex[i][j] = d2 + _simplex[PointDimension][j];
         }

      PointND xr = new(PointDimension);
      PointND xg = new(PointDimension);
      PointND xe = new(PointDimension);
      PointND xc = new(PointDimension); // Центр тяжести симплекса.

      for (iters = 0; iters < MaxIters; iters++)
      {
         _simplex = _simplex.OrderBy(function.Compute).ToArray();
         xc.Fill(0);

         // Центр тяжести = сумма всех векторов (не скалярка), кроме xh 
         for (int i = 0; i < PointDimension; i++)
            for (int j = 0; j < PointDimension; j++)
               xc[i] += _simplex[j][i] / PointDimension;

         if (iters == 0)
         {
            coords.Add(startPoint);
         }
         else
         {
            //coords.Add((PointND)xc.Clone());
            coords.Add((PointND)_simplex[0].Clone());
         }
         funcs.Add(function.Compute(xc));

         // Выйдем, если достигли заданной точности.
         if (IsAccuracyAchieved(_simplex, xc, function))
         {
            _min = _simplex[0]; //xc
            break;
         }

         // Отразим наибольшую точку относительно центра тяжести.
         xr = Reflection(_simplex, xc);

         double fr = function.Compute(xr); // Новое значение функции.
         double fl = function.Compute(_simplex[0]); // Худшее значение функции.
         double fg = function.Compute(_simplex[PointDimension - 1]);
         double fh = function.Compute(_simplex[PointDimension]);

         if (fl < fr && fr < fg)
         {
            _simplex[PointDimension] = (PointND)xr.Clone();
         }
         else if (fr < fl)
         {
            // Производим растяжение.
            xe = Expansion(xc, xr);

            // fe < fr
            if (function.Compute(xe) < fr)
               _simplex[PointDimension] = (PointND)xe.Clone();
            else
               _simplex[PointDimension] = (PointND)xr.Clone();
         }
         else if (fr < fh)
         {
            // Вход в эту часть кода означает, что поменяны
            // местами xr и наибольший элемент симплекса.
            // Проведём сжатие.
            xg = OutsideContraction(xc, xr);

            // fg < fr - сжимаем ещё сильнее
            if (function.Compute(xg) < fr)
               _simplex[PointDimension] = (PointND)xg.Clone();
            // Иначе глобально сжимаем симплекс к наименьшей точке.
            else
               Shrink(_simplex);
         }
         else
         {
            // Не меняем xr и наибольший элемент симплекса, делаем сжатие.
            xg = InsideContraction(_simplex, xc);

            if (function.Compute(xg) < fh)
               _simplex[PointDimension] = (PointND)xg.Clone();
            else
               Shrink(_simplex);
         }
      }

      for (int i = 1; i <= iters; i++)
         dirs.Add(coords[i] - coords[i - 1]); // Направление поиска минимума.

      for (int i = 0; i < iters; i++)
      {
         double crn =
            Math.Acos
            (
             (coords[i][0] * dirs[i][0] + coords[i][1] * dirs[i][1])
             / (Norm(coords[i]) * Norm(dirs[i]))
            );
         corner.Add(crn); // Угл гугл хехе.
      }


      var sw = new StreamWriter("coords.txt");
      using (sw)
      {
         for (int i = 0; i < coords.Count; i++)
            sw.WriteLine(coords[i]);
      }
      _min = _simplex[0];

      Output(coords, funcs, dirs, corner, iters, FunctionsCalcs);
   }

   private bool IsAccuracyAchieved(PointND[] Simplex, PointND xc, IFunction function)
   {
      double sum = 0;

      for (int i = 0; i < xc.Dimention + 1; i++)
      {
         sum += (function.Compute(Simplex[i]) - function.Compute(xc)) *
                (function.Compute(Simplex[i]) - function.Compute(xc));
      }

      return Math.Sqrt(sum / (xc.Dimention + 1)) < Eps;
   }

   private static PointND Reflection(PointND[] Simplex, PointND xc)
      => (1 + Alpha) * xc - Alpha * Simplex[xc.Dimention];

   private static PointND Expansion(PointND xc, PointND xr)
      => (1 - Gamma) * xc + Gamma * xr;

   private static PointND OutsideContraction(PointND xc, PointND xr)
      => Beta * xr + (1 - Beta) * xc;

   private static PointND InsideContraction(PointND[] Simplex, PointND xc)
        => Beta * Simplex[xc.Dimention] + (1 - Beta) * xc;

   private static void Shrink(PointND[] Simplex)
   {
      for (int i = 1; i <= Simplex[0].Dimention; i++)
         Simplex[i] = (PointND)(Simplex[0] + (Simplex[i] - Simplex[0]) / 2).Clone();
   }
   private static double Norm(PointND arg)
   {
      double result = 0;

      for (int i = 0; i < arg.Dimention; i++)
      {
         result += arg[i] * arg[i];
      }

      return Math.Sqrt(result);
   }

   private static void Output
      (
      List<PointND> coords,
      List<double> funcs,
      List<PointND> dirs,
      List<double> corners,
      int iters,
      int FunctionsCalcs
      )
   {
      for (int i = 0; i <= iters; i++)
      {
         Console.Write("{0:f8}".PadRight(15), coords[i][0]);
         Console.Write("{0:f8}".PadRight(15), coords[i][1]);


         Console.Write("{0:f8}".PadRight(15), funcs[i]);

         if (i == iters)
         {
            Console.Write("----------".PadRight(15));
            Console.Write("----------".PadRight(15));
         }
         else
         {
            Console.Write("{0:f8}".PadRight(15), dirs[i][0]);
            Console.Write("{0:f8}".PadRight(15), dirs[i][1]);
         }

         if (i == 0)
         {
            Console.Write("----------".PadRight(15));
            Console.Write("----------".PadRight(15));
            Console.Write("----------".PadRight(15));
         }
         else
         {
            Console.Write("{0:f8}".PadRight(15), Math.Abs(coords[i][0] - coords[i - 1][0]));
            Console.Write("{0:f8}".PadRight(15), Math.Abs(coords[i][1] - coords[i - 1][1]));
            Console.Write("{0:f8}".PadRight(15), Math.Abs(funcs[i] - funcs[i - 1]));
         }

         if (i == iters)
         {
            Console.Write("----------".PadRight(15));
         }
         else
         {
            Console.WriteLine("{0:f8}".PadRight(15), corners[i]);
         }
      }
      Console.WriteLine();
   }
}                             
                              