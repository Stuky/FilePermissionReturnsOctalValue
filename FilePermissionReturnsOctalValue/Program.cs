using System;
using System.Collections.Generic;
using System.Linq;

namespace FilePermissionReturnsOctalValue
{
    // https://codereview.stackexchange.com/questions/186960/calculating-linux-based-octal-file-permission?rq=1

    public static class Program
    {
        public static IEnumerable<IList<T>> Split<T>(this IEnumerable<T> source, int batchLength)
        {
            var batch = new List<T>();
            foreach (var item in source)
            {
                batch.Add(item);
                if (batch.Count == batchLength)
                {
                    yield return batch;
                    batch = new List<T>();
                }
            }
        }

        public static int ToOctal(this char value)
        {
            switch (char.ToUpperInvariant(value))
            {
                case 'R': return 4;
                case 'W': return 2;
                case 'X': return 1;
                case '-': return 0;
                default: throw new ArgumentOutOfRangeException(paramName: nameof(value), message: "Value must be: R, W, X or -");
            }
        }

        public static IEnumerable<int> CalcPermissions(this string value)
        {
            const int batchLength = 3;

            if (value.Length % batchLength != 0)
            {
                throw new ArgumentException(paramName: nameof(value), message: $"Value length must be divisible by {batchLength}.");
            }

            return
                from tripple in value.Split(batchLength)
                select tripple.Select(c => c.ToOctal()).Sum();
        }


        public static void Main(string[] args)
        {
            // Should write 752
            var permission = CalcPermissions("rwxr-x-w-");

            foreach (var item in permission)
            {
                Console.Write(item.ToString());
            }

            Console.ReadLine();
        }
    }
}
