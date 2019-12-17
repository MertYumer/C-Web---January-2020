namespace ParallelMergeSort
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public class StartUp
    {
        public static void Main()
        {
            var array = File
                .ReadAllText("..//..//..//TestData.txt")
                .Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(element => int.Parse(element))
                .ToArray();

            Task sort = SortAlgorithm<int>.MergeSortAsync(array);
            sort.Wait();

            Console.WriteLine(string.Join(", ", array));
        }
    }
}
