using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using TechTalk.SpecFlow;

namespace SurveyApp.IntegrationTest
{
    [Binding]
    [Scope(Feature = "WebAppStart")]
    public sealed class WebAppStartSteps
        : IDisposable
    {
        private TestServer _testServer;
        private Version _result;

        [Given("I have web application")]
        public void GivenIHaveWebApplication()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (string.IsNullOrEmpty(environment))
            {
                environment = "Development";
            }

            _testServer = new TestServer(
                new WebHostBuilder()
                    .UseEnvironment(environment)
                    .UseConfiguration(Program.Configuration)
                    .UseStartup<Startup>()
                    .UseSerilog()
                    .UseKestrel());
        }

        [When("I invoke version endpoint")]
        public async Task WhenIInvokeVersionEndpoint()
        {
            using (var client = _testServer.CreateClient())
            {
#pragma warning disable CA2234 // Pass system uri objects instead of strings
                var response = await client.GetAsync("/api/version");
#pragma warning restore CA2234 // Pass system uri objects instead of strings
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var jobject = JObject.Parse(content);

                var major = jobject["major"].Value<int>();
                var minor = jobject["minor"].Value<int>();
                var build = jobject["build"].Value<int>();

                _result = new Version(major, minor, build);
            }
        }

        [Then("I will receive version '([^\\.]*)\\.([^\\.]*)\\.([^\\.]*)'")]
        public void ThenIWillReceiveVersion(int major, int minor, int build)
        {
            var expected = new Version(major, minor, build);
            _result.Should().Be(expected);
        }

        public void Dispose()
        {
            _testServer?.Dispose();
            _testServer = null;
        }
    }
}
