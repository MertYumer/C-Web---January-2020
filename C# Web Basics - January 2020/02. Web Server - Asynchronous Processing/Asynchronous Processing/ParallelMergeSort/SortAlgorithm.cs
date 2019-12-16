namespace ParallelMergeSort
{
	using System;
	using System.Threading.Tasks;

	public class SortAlgorithm<T> where T : IComparable<T>
	{
		static public void MergeSort(T[] array)
		{
			if (array.Length < 2)
				return;

			var leftArray = new T[array.Length / 2];
			var rightArray = new T[array.Length - leftArray.Length];

			Array.Copy(array, 0, leftArray, 0, leftArray.Length);
			Array.Copy(array, leftArray.Length, rightArray, 0, rightArray.Length);

			MergeSort(leftArray);
			MergeSort(rightArray);

			Merge(array, leftArray, rightArray);
		}

		static public async Task MergeSortAsync(T[] array, int thNum = 4)
		{
			if (array.Length < 2)
			{
				return;
			}

			var leftArray = new T[array.Length / 2];
			var rightArray = new T[array.Length - leftArray.Length];

			Array.Copy(array, 0, leftArray, 0, leftArray.Length);
			Array.Copy(array, leftArray.Length, rightArray, 0, rightArray.Length);

			if (thNum > 0)
			{
				Task leftSort = MergeSortAsync(leftArray);
				Task rightSort = MergeSortAsync(rightArray);

				await leftSort;
				await rightSort;
			}

			else
			{
				MergeSort(leftArray);
				MergeSort(rightArray);
			}

			Merge(array, leftArray, rightArray);
		}

		private static void Merge(T[] array, T[] leftArray, T[] rightArray)
		{
			int leftArrayIndex = 0,
			rightArrayIndex = 0,
			i = 0;

			while (leftArrayIndex < leftArray.Length && rightArrayIndex < rightArray.Length)
			{
				if (leftArray[leftArrayIndex].CompareTo(rightArray[rightArrayIndex]) <= 0)
				{
					array[i++] = leftArray[leftArrayIndex++];
				}

				else
				{
					array[i++] = rightArray[rightArrayIndex++];
				}
			}

			if (leftArrayIndex < leftArray.Length)
			{
				Array.Copy(leftArray, leftArrayIndex, array, i, leftArray.Length - leftArrayIndex);
			}

			else
			{
				Array.Copy(rightArray, rightArrayIndex, array, i, rightArray.Length - rightArrayIndex);
			}
		}
	}
}
