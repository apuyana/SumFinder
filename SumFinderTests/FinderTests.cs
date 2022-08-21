namespace SumFinderTests
{
    public class Tests
    {
        public static IEnumerable<FinderSettingsTestConfig> PrepareSumTestCases()
        {
            List<FinderSettingsTestConfig> testSettings = new List<FinderSettingsTestConfig>();

            // Simple result.
            testSettings.Add(new FinderSettingsTestConfig()
            {
                numbers = new List<int>() { 1, 2 },
                target = 3,
                expectedResults = new List<string>() { "2, 1" }
            });

            // No inputs.
            testSettings.Add(new FinderSettingsTestConfig()
            {
                numbers = new List<int>(),
                target = 0,
                expectedResults = new List<string>()
            });

            // Sample test.
            testSettings.Add(new FinderSettingsTestConfig()
            {
                numbers = new List<int>() { 1, 9, 5, 0, 20, -4, 12, 16, 7 },
                target = 12,
                expectedResults = new List<string>() { "12, 0", "7, 5", "16, -4" }
            });

            // No results.
            testSettings.Add(new FinderSettingsTestConfig()
            {
                numbers = new List<int>() { 1, 2 },
                target = 4,
                expectedResults = new List<string>()
            });

            // Add a lot of values.
            int topValue = 10000;

            List<int> numbers = new List<int>();

            for (int i = 0; i < topValue; i++)
            {
                numbers.Add(i);
            }

            testSettings.Add(new FinderSettingsTestConfig()
            {
                numbers = numbers,
                target = 1,
                expectedResults = new List<string>() { "1, 0" }
            });

            return testSettings;
        }

        [SetUp]
        public void Setup()
        {
        }

        [TestCaseSource(
            sourceType: typeof(Tests),
            sourceName: nameof(PrepareSumTestCases))]
        public async Task TestSum(FinderSettingsTestConfig testConfig)
        {
            Finder finder = new Finder();

            var results = await finder.SumWithTargetAsync(testConfig.numbers, testConfig.target);

            results.Should().BeEquivalentTo(testConfig.expectedResults);
        }
    }
}