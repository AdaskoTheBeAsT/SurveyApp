using System;
using System.Threading.Tasks;
using Alba;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using TechTalk.SpecFlow;

namespace SurveyApp.IntegrationTest
{
    [Binding]
    [Scope(Feature = "WebAppStart")]
    public sealed class WebAppStartSteps
    {
        private readonly WebAppFixture _webAppFixture;
        private SystemUnderTest? _system;
        private Version? _result;

        public WebAppStartSteps(WebAppFixture webAppFixture)
        {
            _webAppFixture = webAppFixture ??
                throw new ArgumentNullException(nameof(webAppFixture));
        }

        [Given("I have web application")]
        public void GivenIHaveWebApplication()
        {
            _system = _webAppFixture.SystemUnderTest;
        }

        [When("I invoke version endpoint")]
        public async Task WhenIInvokeVersionEndpointAsync()
        {
            var response = await _system.Scenario(_ => _.Get.Url("/api/version")).ConfigureAwait(false);
            var jObject = response.ResponseBody.ReadAsJson<JObject>();

            var major = jObject["major"].Value<int>();
            var minor = jObject["minor"].Value<int>();
            var build = jObject["build"].Value<int>();

            _result = new Version(major, minor, build);
        }

        [Then("I will receive version '([^\\.]*)\\.([^\\.]*)\\.([^\\.]*)'")]
        public void ThenIWillReceiveVersion(int major, int minor, int build)
        {
            var expected = new Version(major, minor, build);
            _result.Should().Be(expected);
        }
    }
}
