using Xunit;

namespace SurveyApp.IntegrationTest
{
    [CollectionDefinition(TestCollectionName.IntegrationParallel)]
    public class ParallelDatabaseCollection
        : ICollectionFixture<WebAppFixture>
    {
    }
}