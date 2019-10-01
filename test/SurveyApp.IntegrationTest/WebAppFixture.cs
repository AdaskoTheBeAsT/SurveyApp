using System;
using Alba;

namespace SurveyApp.IntegrationTest
{
    public class WebAppFixture
        : IDisposable
    {
        public WebAppFixture()
        {
            var host = SurveyApp.Program.BuildWebHost(Array.Empty<string>());

            SystemUnderTest
                = new SystemUnderTest(host);
        }

        public SystemUnderTest SystemUnderTest { get; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            SystemUnderTest?.Dispose();
        }
    }
}