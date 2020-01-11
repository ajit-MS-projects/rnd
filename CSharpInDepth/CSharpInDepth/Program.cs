using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpInDepth
{
    public delegate TOutput Converter1<in TInput, out TOutput>(TInput input);
    class Program
    {
        static List<int> ints = new List<int>();

        
        static void Main(string[] args)
        {
            ints.Add(1);
            ints.Add(2);
            ints.Add(3);
            ints.Add(4);
            Converter1<int, double> converter = TakeSquareRoot;
            List<double> doubles = ints.MyConvertAll(TakeSquareRoot);
            foreach (var d in doubles)
            {
                Console.WriteLine(d);
            }

            List<float> floats = ints.MyConvertAll(TakeSquareRootf);
            foreach (var d in floats)
            {
                Console.WriteLine(d);
            }
            Console.ReadLine();
        }
        static double TakeSquareRoot(int x)
        {
            return Math.Sqrt(x);
        }
        static float TakeSquareRootf(int x)
        {
            return (float)Math.Sqrt(x);
        }
    }

    public static class Ext
    {
        public static List<TOut> MyConvertAll<TOut, TIn>(this List<TIn> listIn, Converter1<TIn,TOut> input)
        {
            List <TOut> retVal = new List<TOut>();
            foreach (var inp in listIn)
            {
                retVal.Add(input(inp));
            }

            return retVal;
        }
    }
}
