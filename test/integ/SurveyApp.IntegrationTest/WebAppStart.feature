@xunit:collection(IntegrationParallel)
Feature: WebAppStart

Scenario: Startup is launching properly
	Given I have web application
	When I invoke version endpoint
	Then I will receive version '1.0.0'