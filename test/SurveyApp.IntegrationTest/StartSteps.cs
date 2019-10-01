using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace SurveyApp.IntegrationTest
{
    [Binding]
    [Scope(Feature = "Start")]
    public class StartSteps
    {
        private readonly List<int> _list = new List<int>();
        private int _result;

        public StartSteps()
        {
        }

        [Given("I have entered (.*) into the calculator")]
        public void GivenIHaveEnteredIntoTheCalculator(int p0)
        {
            _list.Add(p0);
        }

        [When("I press add")]
        public void WhenIPressAdd()
        {
            _result = _list.Sum();
        }

        [Then("the result should be (.*) on the screen")]
        public void ThenTheResultShouldBeOnTheScreen(int p0)
        {
            _result.Should().Be(p0);
        }
    }
}
