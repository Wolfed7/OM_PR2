namespace OM_PR2;

public class Interval
{
   public double LeftBoundary { get; }
   public double RightBoundary { get; }
   public double Center => (LeftBoundary + RightBoundary) / 2;
   public double Length => RightBoundary - LeftBoundary;

   public Interval()
   {
      LeftBoundary = 0;
      RightBoundary = 1;
   }

   public Interval (double leftBoundary, double rightBoundary)
   {
      if (leftBoundary <= rightBoundary)
      {
         LeftBoundary = leftBoundary;
         RightBoundary = rightBoundary;
      }
      else
      {
         throw new ArgumentException("Неверно задан интервал.");
      }
   }

   public static Interval Parse(string str)
   {
      var data = str.Split();
      return new Interval(double.Parse(data[0]), double.Parse(data[1]));
   }

   public bool Contains(double point)
    => (point >= LeftBoundary && point <= RightBoundary);

   public override string ToString()
   {
      return $"[{LeftBoundary}; {RightBoundary}]";
   }
}