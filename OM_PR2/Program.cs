using OM_PR2;
using System.Diagnostics;
using System.Globalization;

CultureInfo.CurrentCulture = new CultureInfo("en-US");

PointND startPoint = PointND.Parse(new double[] { -1, -2 });
//PointND startPoint = PointND.Parse(new double[] {-6, 4}); // Хороший для BFGS.
double eps = 1e-3;

IFunction function = new TargetFunction();
//IFunction function = new QuadraticFunction();
//IFunction function = new RosenbrockFunction();

//MethodFactoryND MF = new(new BFGSMethod(1000, eps, new Fibonacci(1e-7)), function, startPoint);
MethodFactoryND MF = new(new BFGSMethod(1000, eps, new QuadraticInterpolation(1e-7)), function, startPoint);
//MethodFactoryND MF = new(new SimplexMethod(1000, eps), function, startPoint);
MF.Compute();
PointND min = MF.GetMinPoint();
Console.WriteLine(min.ToString());

//var psi = new ProcessStartInfo();
//psi.FileName = "Graphics.py";
//psi.Arguments = "\"Graphics.py\"";

//psi.UseShellExecute = false;
//psi.CreateNoWindow = true;
//psi.RedirectStandardOutput = true;
//psi.RedirectStandardError = true;

//using (var process = Process.Start(psi))
//{

//}