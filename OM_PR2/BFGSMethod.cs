using System.Drawing;

namespace OM_PR2;

public class BFGSMethod : IMinSearchMethodND
{
   // Этому методу требуется одномерный поиск.
   private IMinSearchMethod1D _minSearchMethod1D;
   private PointND _min;

   public PointND Min => _min;
   public bool Need1DSearch => true;
   public int MaxIters { get; init; }
   public double Eps { get; init; }
   public double IntervalEps => 1e-9;

   public BFGSMethod(int maxIters, double eps, IMinSearchMethod1D minSearchMethod1D)
   {
      MaxIters = maxIters;
      Eps = eps;
      _minSearchMethod1D = minSearchMethod1D;

      _min = new PointND(0);
   }

   public void Compute(PointND startPoint, IFunction function)
   {
      double lambda;
      int pointDimention = startPoint.Dimention;
      double h = 1E-14; // Шаг для численной производной.

      List<PointND> coords = new(); // Координаты для графики.
      List<double> funcs = new(); // Значения функции.
      List<PointND> dirs = new(); // Значения направлений поиска.
      List<double> lambdas= new();
      List<double> corner = new(); // Угол между векторами (xi, yi) и (s1, s2)
      int iters = 0; // Количество итераций.
      int FunctionsCalcs = 0; // Количество вычислений функции.
      List<PointND> gradfs = new();
      List<Matrix> matrices = new();

      Matrix H = new(pointDimention);
      Matrix deltaH = new(pointDimention);
      PointND nextPoint;
      PointND direction = new(pointDimention); // Вектор направления.
      PointND nablaF = new(pointDimention);
      PointND nablaF1 = new(pointDimention); // Производные по n переменным(в нашем случае 2D).
      PointND s; // Шаг алгоритма xk1 - xk на итерации.
      PointND y; // Изменение градиента на итерации.

      PointND denominatorAsVector;
      PointND numerator;
      double denominatorAsNumber;

      for (int i = 0; i < H.Size; i++) // Начальный гессиан (единичная матрица).
         H[i, i] = 1;

      for (iters = 0; iters < MaxIters; iters++)
      {
         direction.Fill(0);
         coords.Add(startPoint);

         denominatorAsNumber = 0;

         if (iters % pointDimention == 0 && iters != 0)
         {
            H.Clear();

            for (int i = 0; i < H.Size; i++)
               H[i, i] = 1;
         }

         for (int i = 0; i < pointDimention; i++)
            nablaF[i] = Derivative(startPoint, function, i, h);


         funcs.Add(function.Compute(startPoint)); // Не считаю за вычисление функции, т.к. не относится к методу.
         gradfs.Add((PointND)nablaF.Clone());
         matrices.Add(H);


         if (Norm(nablaF) < Eps)
         {
            _min = (PointND)startPoint.Clone();
            break;
         }

         direction = -H * nablaF;

         var intrvl = IntervalSearch.Search(function, 0, IntervalEps, direction, startPoint);
         _minSearchMethod1D.Compute(function, intrvl, direction, startPoint);
         lambda = _minSearchMethod1D.Min;

         FunctionsCalcs += _minSearchMethod1D.FunctionComputings; // Поиск одномерный вычисляет функцию столько раз.

         nextPoint = (PointND)(startPoint + lambda * direction).Clone();


         dirs.Add((PointND)direction.Clone());
         lambdas.Add(lambda);

         for (int i = 0; i < pointDimention; i++)
         {
            nablaF1[i] = Derivative(nextPoint, function, i, h);
            FunctionsCalcs += 2; // Функция вычисляется для каждой компоненты дважды.
         }   

         y = nablaF1 - nablaF; // Изменение градиента на итерации. (delta gk)
         s = nextPoint - startPoint; // Шаг алгоритма на итерации. (delta xk)

         //if (s.Equals(lambda * direction))
         //{
         //   H.Clear();
         //   for (int i = 0; i < H.Size; i++)
         //      H[i, i] = 1;
         //   startPoint = (PointND)nextPoint.Clone();
         //   continue;
         //}

         denominatorAsVector = s - H * y; // Вектор в знаменателе соотношения для deltaH.

         for (int i = 0; i < pointDimention; i++)
            denominatorAsNumber += denominatorAsVector[i] * y[i]; // Подсчитанный знаменатель.

         numerator = (PointND)denominatorAsVector.Clone();

         for (int i = 0; i < pointDimention; i++)
            for (int j = 0; j < pointDimention; j++)
               deltaH[i, j] = numerator[i] * numerator[j];

         //if (Math.Abs(denominatorAsNumber) < Eps)
         //{
         //   H.Clear();
         //   for (int i = 0; i < H.Size; i++)
         //      H[i, i] = 1;
         //   startPoint = (PointND)nextPoint.Clone();
         //   continue;
         //}

         deltaH /= denominatorAsNumber;
         H += deltaH;
         startPoint = (PointND)nextPoint.Clone();
      }

      _min = startPoint;

      var sw = new StreamWriter("coords.txt");
      using (sw)
      {
         for (int i = 0; i < coords.Count; i++)
            sw.WriteLine(coords[i]);
      }

      for (int i = 0; i < iters; i++)
      {
         double crn =
            Math.Acos
            (
             (coords[i][0] * dirs[i][0] + coords[i][1] * dirs[i][1])
             / (Norm(coords[i]) * Norm(dirs[i]))
            );
         corner.Add(crn);
      }

      Output(coords, funcs, dirs, corner, iters, FunctionsCalcs, lambdas, gradfs, matrices);
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

   private static double Derivative(PointND point, IFunction function, int current, double h)
   {
      PointND arg = new(point.Dimention);
      arg[current] = h;

      return (function.Compute(point + arg) - function.Compute(point - arg)) / (2 * h);
      //return (function.Compute(point + arg) - function.Compute(point)) / h;
   }

   private static void Output
   (
   List<PointND> coords,
   List<double> funcs,
   List<PointND> dirs,
   List<double> corners,
   int iters,
   int FunctionsCalcs,
   List<double> lambdas,
   List<PointND> gradfs,
   List<Matrix> matrices
   )
   {
      //for (int i = 0; i <= iters; i++)
      //{
      //   Console.Write("{0:f6}".PadRight(10), coords[i][0]);
      //   Console.Write("{0:f6}".PadRight(10), coords[i][1]);


      //   Console.Write("{0:f6}".PadRight(10), funcs[i]);

      //   if (i == iters)
      //   {
      //      Console.Write("---------".PadRight(10));
      //      Console.Write("---------".PadRight(10));
      //      Console.Write("---------".PadRight(10));
      //   }
      //   else
      //   {
      //      Console.Write("{0:f6}".PadRight(10), dirs[i][0]);
      //      Console.Write("{0:f6}".PadRight(10), dirs[i][1]);
      //      Console.Write("{0:f6}".PadRight(10), lambdas[i]);
      //   }

      //   if (i == 0)
      //   {
      //      Console.Write("---------".PadRight(10));
      //      Console.Write("---------".PadRight(10));
      //      Console.Write("---------".PadRight(10));
      //   }
      //   else
      //   {
      //      Console.Write("{0:f6}".PadRight(10), Math.Abs(coords[i][0] - coords[i - 1][0]));
      //      Console.Write("{0:f6}".PadRight(10), Math.Abs(coords[i][1] - coords[i - 1][1]));
      //      Console.Write("{0:f6}".PadRight(10), Math.Abs(funcs[i] - funcs[i - 1]));
      //   }

      //   if (i == iters)
      //   {
      //      Console.Write("---------".PadRight(10));
      //   }
      //   else
      //   {
      //      Console.Write("{0:f6}".PadRight(15), corners[i]);
      //   }

      //   Console.Write("{0:f6}".PadRight(10), gradfs[i][0]);
      //   Console.Write("{0:f6}".PadRight(10), gradfs[i][1]);
                                          
      //   Console.Write("{0:f6}".PadRight(10), matrices[i][0, 0]);
      //   Console.Write("{0:f6}".PadRight(10), matrices[i][0, 1]);
      //   Console.Write("{0:f6}".PadRight(10), matrices[i][1, 0]);
      //   Console.WriteLine("{0:f6}".PadRight(10), matrices[i][1, 1]);
      //}

      Console.Write($"{iters}".PadRight(10));
      Console.Write($"{FunctionsCalcs}".PadRight(10));
      //Console.WriteLine();
   }
}
