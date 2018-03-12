using System;
using System.Collections.Generic;
using System.Text;



namespace Formulas
{

    public enum MtMathRoundType
    {
        Floor,
        Ceiling,
        Round

    }//enRoundType



    public static class MtMath
    {
        /// <summary>
        /// Dokładność obliczeń
        /// </summary>
        public static double EPSILON;

        static MtMath()
        {
            EPSILON = 0.01;
        }

        /// <summary>
        /// Zaokrąglenie
        /// </summary>
        /// <param name="number">Liczba do zaokrąglenia</param>
        /// <param name="prec">Dokładność zaokrąglenia</param>
        /// <param name="rType">Typ zaokrąglenia</param>
        /// <returns></returns>
        public static double Round(double number, double prec, MtMathRoundType rType)
        {

            // extract number (xc) and rest (r)

            double x = number / prec;

            double xc = Math.Floor(x);

            double r = x - xc;

            // which variant of rounding 
            double dx = 0;

            switch (rType)
            {
                case MtMathRoundType.Floor:
                    dx = 0;
                    break;

                case MtMathRoundType.Ceiling:
                    dx = 1;
                    break;

                default:
                    if (r < 0.5)
                        dx = 0;
                    else
                        dx = 1;
                    break;
            }//switch

            // calculate rounded number

            double rx = xc + dx;

            double result = rx * prec;

            return result;

        }//Round

        /// <summary>
        /// Czy dwie liczby są sobie równe z dokładnością do Epsilona
        /// </summary>
        /// <param name="q1"></param>
        /// <param name="q2"></param>
        /// <returns></returns>
        public static bool isEqual(double q1, double q2)
        {
            double resultOfSubstr = Math.Abs(q2 - q1);
            if (resultOfSubstr < EPSILON) return true;
            return false;

        }//isEqual

        public static bool isEqual(double q1, double q2, double epsilon)
        {
            if (Math.Abs(q2 - q1) < epsilon) return true;
            return false;

        }


        /// <summary>
        /// Czy liczba jest zerem
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        public static bool isZero(double q)
        {
            if (Math.Abs(q) < EPSILON) return true;
            return false;

        }

        public static bool isZero(double q, double epsilon)
        {
            if (Math.Abs(q) < epsilon) return true;
            return false;

        }


        /// <summary>
        /// Konwersja ze stopni na radiany
        /// </summary>
        /// <param name="deg">Kąt w radianach</param>
        /// <returns>Kąt w stopniach</returns>
        public static double deg2rad(double deg)
        {
            return (deg * Math.PI) / 180;

        }//deg2rad

        /// <summary>
        /// Konwersja z radianów na stopnie
        /// </summary>
        /// <param name="rad">Kąt w radianach</param>
        /// <returns>Kąt w stopniach</returns>
        public static double rad2deg(double rad)
        {
            return (rad * 180) / Math.PI;

        }//rad2deg

        /// <summary>
        /// Zamiana wartości zmiennych
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        public static void varSwap(ref double A, ref double B)
        {
            double C = A;
            A = B;
            B = C;
        }

        /// <summary>
        /// Zamiana wartości zmiennych 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        public static void varSwap(ref int A, ref int B)
        {
            int C = A;
            A = B;
            B = C;
        }

        /// <summary>
        /// Czy liczba jest parzysta?
        /// </summary>
        /// <param name="Number"></param>
        /// <returns></returns>
        public static bool isNumberEven(double Number)
        {
            if ((Number / 2.0) == Math.Round(Number / 2.0))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Czy liczba jest parzysta?
        /// </summary>
        /// <param name="Number"></param>
        /// <returns></returns>
        public static bool isNumberEven(int Number)
        {
            double d = (double)Number;
            return isNumberEven(d);
        }

        public static double Max(params double[] values)
        {
            double max = values[0];
            for (int i = 1; i < values.Length; i++)
            {
                if (values[i] > max) max = values[i];
            }
            return max;
        }

        public static int MaxInt(params int[] values)
        {
            int max = values[0];
            for (int i = 1; i < values.Length; i++)
            {
                if (values[i] > max) max = values[i];
            }
            return max;
        }

        public static int MaxInt(List<int> values)
        {

            int max = values[0];
            for (int i = 1; i < values.Count; i++)
            {
                if (values[i] > max) max = values[i];
            }
            return max;

        }

        public static double MaxAbs(params double[] values)
        {
            double max = values[0];
            for (int i = 1; i < values.Length; i++)
            {
                if (Math.Abs(values[i]) > Math.Abs(max)) max = values[i];
            }
            return max;
        }


        public static double Min(params double[] values)
        {
            double min = values[0];
            for (int i = 1; i < values.Length; i++)
            {
                if (values[i] < min) min = values[i];
            }
            return min;
        }

        public static double Min(List<double> values)
        {

            double[] array = new double[values.Count];
            for (int i = 0; i < values.Count; i++)
            {
                array[i] = values[i];
            }

            return Min(array);
        }



        public static int MinInt(params int[] values)
        {
            int min = values[0];
            for (int i = 1; i < values.Length; i++)
            {
                if (values[i] < min) min = values[i];
            }

            return min;
        }




        public static double MinAbs(params double[] values)
        {
            double min = values[0];
            for (int i = 1; i < values.Length; i++)
            {
                if (Math.Abs(values[i]) < Math.Abs(min)) min = values[i];
            }
            return min;
        }



        /// <summary>
        /// Czy dana liczba zawiera się w przedziale
        /// </summary>
        /// <param name="minVal">Wartość minimalna</param>
        /// <param name="val">Sprawdzana wartość</param>
        /// <param name="maxVal">Wartość maksymalna</param>
        /// <returns></returns>
        public static bool isInRange(double minVal, double val, double maxVal)
        {
            return isInRange(minVal, val, maxVal, EPSILON);

        }

        public static bool isInRange(double minVal, double val, double maxVal, double epsilon)
        {

            return ((val >= minVal - epsilon) && (val <= maxVal + epsilon));

        }

        public static bool isInRange(int minVal, int val, int maxVal)
        {

            if ((val >= minVal) && (val <= maxVal)) return true;
            return false;
        }


        /// <summary>
        /// Funkcja zwraca:
        /// 1 jeżeli liczba jest wieksza od zera 
        /// -1 jeżeli liczba jest mniejsza od zera /n
        /// 0 jeżeli liczba jest równa 0
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int Sign(double val)
        {
            if (val > 0 + EPSILON) return 1;
            else if (val < 0 - EPSILON) return -1;
            else return 0;
        }

        /// <summary>
        /// Wartość pomiędzy dwoma liczbami
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <returns></returns>
        public static double ValueBetween(double d1, double d2)
        {
            return d1 + (d2 - d1) / 2;

        }

        public static double Sqrt(double value)
        {
            return Math.Pow(value, 0.5);
        }



        public static double GetAverageValue(List<double> values)
        {
            double k = 1.0 / values.Count;
            double average = 0;

            foreach (double value in values)
                average += value * k;

            return average;
        }

        public static bool XOR(bool condition1, bool condition2)
        {
            if ((condition1 && !condition2) || (!condition1 && condition2)) return true;
            return false;
        }

        public static bool isDoubleOnList(List<double> listOfdoubles, double value)
        {
            bool isOnList = false;

            foreach (double dlb in listOfdoubles)
                if (isEqual(dlb, value)) isOnList = true;

            return isOnList;

        }

        public static double GetCircleArea(double radius)
        {
            return Math.PI * radius * radius;
        }

        public static double GetCircleCircumference(double radius)
        {
            return 2 * Math.PI * radius;

        }

        public static double Interpolate(double x1, double y1, double x2, double y2, double x)
        {
            return y1 + ((y2 - y1) / (x2 - x1)) * (x - x1);
        }





    }//mTools




}//MTools