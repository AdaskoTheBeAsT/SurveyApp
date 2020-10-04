using System;
using Alba;

namespace SurveyApp.IntegrationTest
{
    public sealed class WebAppFixture
        : IDisposable
    {
        public WebAppFixture()
        {
            var host = Program.BuildWebHost(Array.Empty<string>());

            SystemUnderTest
                = new SystemUnderTest(host);
        }

        public SystemUnderTest SystemUnderTest { get; }

        public void Dispose()
        {
            SystemUnderTest?.Dispose();
        }
    }
}