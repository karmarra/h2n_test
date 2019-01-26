using System;
using System.Dynamic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace h2n_test
{

    public enum UnaryOperations
    {
        Sqrt,
        OneX,
        Inverse
    }

    public enum BinaryOperations
    {
        Sum,
        Subtract,
        Multiply,
        Divide,
    }

    public static class Calculator 
    {
        public static double CalcUnaryOperation(double value, UnaryOperations operation)
        {
            switch (operation)
            {
                case UnaryOperations.Sqrt:
                    if (value != 0)
                        return Math.Pow(value, 0.5);
                    else
                        throw new ArithmeticException("Нельзя извлечь корень из отрицательного числа!");
                case UnaryOperations.OneX:
                    if (value != 0)
                        return 1 / value;
                    else
                        throw new DivideByZeroException("Нельзя делить на ноль!");
                case UnaryOperations.Inverse:
                    return value * -1;
                default:
                    throw new ArgumentOutOfRangeException(nameof(operation), operation.ToString());
            }
        }

        public static double CalcBinaryOperation(double left, double right, BinaryOperations operation)
        {
            switch (operation)
            {
                case BinaryOperations.Sum:
                    return left + right;
                case BinaryOperations.Subtract:
                    return left - right;
                case BinaryOperations.Multiply:
                        return left * right;
                case BinaryOperations.Divide:
                    if (right != 0)
                        return left / right;
                    else 
                        throw new DivideByZeroException("Нельзя делить на ноль!");
                default:
                    throw new ArgumentOutOfRangeException(nameof(operation), operation.ToString());
            }
        }

        public static double CalcPercentOperation(double left, double right, BinaryOperations operation)
        {
            return CalcBinaryOperation(left, right / 100 * left, operation);
        }

    }
}
