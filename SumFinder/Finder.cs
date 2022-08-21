namespace SumFinder
{
    public class Finder
    {
        /// <summary>
        /// Cache if a pair is a valid sum.
        /// </summary>
        private Dictionary<string, bool> cacheResults = new Dictionary<string, bool>();

        /// <summary>
        /// List of found results.
        /// </summary>
        private List<string> result = new List<string>();

        private SemaphoreSlim semaphoreFind = new SemaphoreSlim(1, 1);

        /// <summary>
        /// Finds pairs of integers from a list that sum to a given value.
        /// </summary>
        /// <param name="numbers">List of integers.</param>
        /// <param name="target">Given value.</param>
        /// <returns>List of results.</returns>
        public async ValueTask<IList<string>> SumWithTargetAsync(IList<int> numbers, int target)
        {
            try
            {
                // Prevent multiple concurrent executions.
                await semaphoreFind.WaitAsync();

                if (numbers == null || numbers?.Count == 0)
                {
                    result = new List<string>();
                    return result;
                }

                // Prepare capacity to optimize allocations.
                result = new List<string>(numbers!.Count);
                cacheResults = new Dictionary<string, bool>(numbers!.Count);
                SortedList<int, int> sortedNumbers = new SortedList<int, int>(numbers!.Count);

                for (int i = 0; i < numbers.Count; i++)
                {
                    sortedNumbers.Add(numbers[i], numbers[i]);

                    SumForValueWithTarget(numbers[i], sortedNumbers.Values, target);
                }
            }
            finally
            {
                semaphoreFind.Release();
            }

            return result;
        }

        private static bool IsExpectedSum(int value1, int value2, int target)
        {
            return value1 + value2 == target;
        }

        private static bool IsPairBiggerThanTarget(int firstValue, int target, int secondValue)
        {
            return firstValue + secondValue > target;
        }

        private static bool IsPairSameValue(int firstValue, int secondValue)
        {
            return firstValue == secondValue;
        }

        /// <summary>
        /// Generate key for a pair of values.
        /// Make sure the key is always the bigger value first.
        /// </summary>
        /// <param name="value1">First value to use.</param>
        /// <param name="value2">Second value to use.</param>
        /// <returns>Generated key.</returns>
        private string GetPairKey(int value1, int value2)
        {
            return (value1 > value2) ? $"{value1}, {value2}" : $"{value2}, {value1}";
        }

        /// <summary>
        /// Insert results to the cache.
        /// </summary>
        /// <param name="key">Key to use.</param>
        /// <param name="isSum">Result to cache.</param>
        private void InsertResult(string key, bool isSum)
        {
            cacheResults.Add(key, isSum);

            if (isSum)
            {
                result.Add(key);
            }
        }

        private void SumForValueWithTarget(int firstValue, IList<int> sortedValues, int target)
        {
            if (sortedValues.Count <= 1)
            {
                return;
            }

            int secondValue;
            string key;

            for (int i = 0; i < sortedValues.Count; i++)
            {
                secondValue = sortedValues[i];

                // Assume that we are looking for pairs that are
                // not the same value.
                if (IsPairSameValue(firstValue, secondValue))
                {
                    continue;
                }

                if (IsPairBiggerThanTarget(firstValue, target, secondValue))
                {
                    break;
                }

                key = GetPairKey(firstValue, secondValue);

                if (!cacheResults.ContainsKey(key))
                {
                    InsertResult(key, IsExpectedSum(firstValue, secondValue, target));
                }
            }
        }
    }
}