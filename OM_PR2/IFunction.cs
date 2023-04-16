namespace OM_PR2;

public interface IFunction
{
   double Compute(PointND point);
}

class QuadraticFunction : IFunction
{
   public double Compute(PointND point)
      => 100 * (point[1] - point[0]) * (point[1] - point[0])
      + (1 - point[0]) * (1 - point[0]);
}

class RosenbrockFunction : IFunction
{
   public double Compute(PointND point) 
      => 100 * (point[1] - point[0] * point[0]) * (point[1] - point[0] * point[0]) 
      + (1 - point[0]) * (1 - point[0]);
}

// Целевая функция варианта.
// ДЛЯ НЕЁ ИЩЕТСЯ МАКСИМУМ.
class TargetFunction : IFunction
{
   public double Compute(PointND point)
        => - // <---- Минус для разворота функции вниз, так найдём максимум.
        (3.0 / (1 + (point[0] - 2) * (point[0] - 2) +
        ((point[1] - 2) / 2) * (point[1] - 2) / 2) +
        2.0 / (1 + ((point[0] - 2) / 3) * ((point[0] - 2) / 3) +
        (point[1] - 3) * (point[1] - 3)));
}
