using System;
using System.Dynamic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace h2n_test
{
    public enum Operations
    {
        None,
        Percent,
        Sqrt,
        Pow2,
        Pow3,
        OneX,
        Inverse,
        Sum,
        Subtract,
        Multiply,
        Divide,
    }

    public static class Calculator 
    {
        public static double CalcOperation(Operations operationType, double left, double right = 0, bool isPercent = false)
        {
            if (isPercent) right = right / 100 * left;
            switch (operationType)
            {
                case Operations.Percent:
                    return right / 100 * left;
                case Operations.Sqrt:
                    if (left < 0)
                        throw new ArithmeticException("Нельзя извлечь корень из отрицательного числа!");
                    return Math.Pow(left, 0.5);
                case Operations.Pow2:
                    return Math.Pow(left, 2);
                case Operations.Pow3:
                    return Math.Pow(left, 3);
                case Operations.OneX:
                    if (left == 0)
                        throw new DivideByZeroException("Нельзя делить на ноль!");
                    return 1 / left;
                case Operations.Inverse:
                    return left * -1;
                case Operations.Sum:
                    return left + right;
                case Operations.Subtract:
                    return left - right;
                case Operations.Multiply:
                        return left * right;
                case Operations.Divide:
                    if (right == 0)
                        throw new DivideByZeroException("Нельзя делить на ноль!");
                    return left / right;
                default:
                    throw new ArgumentOutOfRangeException(nameof(operationType), operationType.ToString());
            }
        }

        public static double Percent(Operations operation, double left, double right)
        {
            return CalcOperation(operation, left, right / 100 * left );
        }
    }
}
