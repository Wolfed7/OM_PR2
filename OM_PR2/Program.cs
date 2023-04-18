using OM_PR2;
using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography;

CultureInfo.CurrentCulture = new CultureInfo("en-US");

//--------------------------------------------//
//                Ручное упр.                 //
//--------------------------------------------//


////PointND startPoint = PointND.Parse(new double[] { -1, -2 }); // Показательный для многогранника.
//PointND startPoint = PointND.Parse(new double[] {-6, 4}); // Показательный для BFGS.
//double eps = 1e-3;

////IFunction function = new TargetFunction();
////IFunction function = new QuadraticFunction();
//IFunction function = new RosenbrockFunction();

////MethodFactoryND MF = new(new BFGSMethod(1000, eps, new Fibonacci(1e-3)), function, startPoint);
//MethodFactoryND MF = new(new BFGSMethod(1000, eps, new QuadraticInterpolation(1e-3)), function, startPoint);
////MethodFactoryND MF = new(new SimplexMethod(1000, eps), function, startPoint);
//MF.Compute();
//PointND min = MF.GetMinPoint();
//Console.WriteLine("{0:f8}", min);
//Console.WriteLine("{0:f8}", function.Compute(min));


//--------------------------------------------//
//                Полуавтомат                 //
//--------------------------------------------//

//PointND startPoint = PointND.Parse(new double[] { -2, 3 }); // Показательный для многогранника.
//PointND startPoint = PointND.Parse(new double[] {-1, 2}); // Показательный для BFGS.

PointND startPoint = PointND.Parse(new double[] {3, -4}); // Показательный для BFGS.
IFunction function;
MethodFactoryND MF;

double[] epss = new double[] { 1e-3, 1e-4, 1e-5, 1e-6, 1e-7 };

foreach (var eps in epss)
{
   int pad = 10;
   Console.WriteLine(" Целевая: ");
   Console.Write("{0:e1}", $"{eps}".PadRight(pad));
   function = new QuadraticFunction();

   Console.WriteLine("Бройден (Фиб.):".PadRight(pad));
   MF = new(new BFGSMethod(1000, eps, new Fibonacci(1e-7)), function, startPoint);
   MF.Compute();
   Console.Write("{0:f8}".PadRight(pad), MF.GetMinPoint()[0]);
   Console.Write("{0:f8}".PadRight(pad), MF.GetMinPoint()[1]);
   Console.WriteLine("{0:f8}\n".PadRight(pad), -function.Compute(MF.GetMinPoint()));

   Console.Write("{0:e1}", $"{eps}".PadRight(pad));
   Console.WriteLine("Бройден (квадр. инт.):".PadRight(pad));
   MF = new(new BFGSMethod(1000, eps, new QuadraticInterpolation(1e-7)), function, startPoint);
   MF.Compute();
   Console.Write("{0:f8}".PadRight(pad), MF.GetMinPoint()[0]);
   Console.Write("{0:f8}".PadRight(pad), MF.GetMinPoint()[1]);
   Console.WriteLine("{0:f8}\n".PadRight(pad), -function.Compute(MF.GetMinPoint()));

   Console.Write("{0:e1}", $"{eps}".PadRight(pad));
   Console.WriteLine("Деф. мног.:".PadRight(pad));
   MF = new(new SimplexMethod(1000, eps), function, startPoint);
   MF.Compute();
   Console.Write("{0:f8}".PadRight(pad), MF.GetMinPoint()[0]);
   Console.Write("{0:f8}".PadRight(pad), MF.GetMinPoint()[1]);
   Console.WriteLine("{0:f8}\n".PadRight(pad), -function.Compute(MF.GetMinPoint()));



   //Console.WriteLine(" Квадратичная: ");
   //Console.Write("{0:e1}", $"{eps}".PadRight(pad));
   //function = new QuadraticFunction();

   //Console.WriteLine("Бройден (Фиб.):".PadRight(pad));
   //MF = new(new BFGSMethod(1000, eps, new Fibonacci(1e-7)), function, startPoint);
   //MF.Compute();
   //Console.Write("{0:f8}".PadRight(pad), MF.GetMinPoint()[0]);
   //Console.Write("{0:f8}".PadRight(pad), MF.GetMinPoint()[1]);
   //Console.WriteLine("{0:f8}\n".PadRight(pad), function.Compute(MF.GetMinPoint()));

   //Console.Write("{0:e1}", $"{eps}".PadRight(pad));
   //Console.WriteLine("Бройден (квадр. инт.):".PadRight(pad));
   //MF = new(new BFGSMethod(1000, eps, new QuadraticInterpolation(1e-7)), function, startPoint);
   //MF.Compute();
   //Console.Write("{0:f8}".PadRight(pad), MF.GetMinPoint()[0]);
   //Console.Write("{0:f8}".PadRight(pad), MF.GetMinPoint()[1]);
   //Console.WriteLine("{0:f8}\n".PadRight(pad), function.Compute(MF.GetMinPoint()));

   //Console.Write("{0:e1}", $"{eps}".PadRight(pad));
   //Console.WriteLine("Деф. мног.:".PadRight(pad));
   //MF = new(new SimplexMethod(1000, eps), function, startPoint);
   //MF.Compute();
   //Console.Write("{0:f8}".PadRight(pad), MF.GetMinPoint()[0]);
   //Console.Write("{0:f8}".PadRight(pad), MF.GetMinPoint()[1]);
   //Console.WriteLine("{0:f8}\n".PadRight(pad), function.Compute(MF.GetMinPoint()));




   //Console.WriteLine(" Розенброк: ");
   //Console.Write("{0:e1}", $"{eps}".PadRight(pad));
   //function = new RosenbrockFunction();

   //Console.WriteLine("Бройден (Фиб.):".PadRight(pad));
   //MF = new(new BFGSMethod(1000, eps, new Fibonacci(1e-7)), function, startPoint);
   //MF.Compute();
   //Console.Write("{0:f8}".PadRight(pad), MF.GetMinPoint()[0]);
   //Console.Write("{0:f8}".PadRight(pad), MF.GetMinPoint()[1]);
   //Console.WriteLine("{0:f8}\n".PadRight(pad), function.Compute(MF.GetMinPoint()));

   //Console.Write("{0:e1}", $"{eps}".PadRight(pad));
   //Console.WriteLine("Бройден (квадр. инт.):".PadRight(pad));
   //MF = new(new BFGSMethod(1000, eps, new QuadraticInterpolation(1e-7)), function, startPoint);
   //MF.Compute();
   //Console.Write("{0:f8}".PadRight(pad), MF.GetMinPoint()[0]);
   //Console.Write("{0:f8}".PadRight(pad), MF.GetMinPoint()[1]);
   //Console.WriteLine("{0:f8}\n".PadRight(pad), function.Compute(MF.GetMinPoint()));

   //Console.Write("{0:e1}", $"{eps}".PadRight(pad));
   //Console.WriteLine("Деф. мног.:".PadRight(pad));
   //MF = new(new SimplexMethod(1000, eps), function, startPoint);
   //MF.Compute();
   //Console.Write("{0:f8}".PadRight(pad), MF.GetMinPoint()[0]);
   //Console.Write("{0:f8}".PadRight(pad), MF.GetMinPoint()[1]);
   //Console.WriteLine("{0:f8}\n".PadRight(pad), function.Compute(MF.GetMinPoint()));
}






//var psi = new ProcessStartInfo();
//psi.FileName = "Graphics.py";
////psi.Arguments = "\"Graphics.py\"";

//psi.UseShellExecute = false;
//psi.CreateNoWindow = true;
//psi.RedirectStandardOutput = true;
//psi.RedirectStandardError = true;

//using (var process = Process.Start(psi))
//{

//}