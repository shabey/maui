﻿using OpenQA.Selenium.Appium;
using UITest.Core;

namespace UITest.Appium;

class AppiumIOSStepperActions : ICommandExecutionGroup
{
	const string IncreaseCommand = "increaseStepper";
	const string DecreaseCommand = "decreaseStepper";

	readonly List<string> _commands = new()
	{
		IncreaseCommand,
		DecreaseCommand
	};

	readonly AppiumApp _appiumApp;

	public AppiumIOSStepperActions(AppiumApp appiumApp)
	{
		_appiumApp = appiumApp;
	}

	public bool IsCommandSupported(string commandName)
	{
		return _commands.Contains(commandName, StringComparer.OrdinalIgnoreCase);
	}

	public CommandResponse Execute(string commandName, IDictionary<string, object> parameters)
	{
		return commandName switch
		{
			IncreaseCommand => Increase(parameters),
			DecreaseCommand => Decrease(parameters),
			_ => CommandResponse.FailedEmptyResponse,
		};
	}

	CommandResponse Increase(IDictionary<string, object> parameters)
	{
		var stepper = GetAppiumElement(parameters["element"]);

		if (stepper is null)
			return CommandResponse.FailedEmptyResponse;

		var buttons = AppiumQuery.ByClass("XCUIElementTypeButton").FindElements(stepper, _appiumApp);

		if (buttons is not null && buttons.Count > 1)
		{
			var increaseButton = buttons.LastOrDefault();
			increaseButton?.Tap();
		}

		return CommandResponse.SuccessEmptyResponse;
	}

	CommandResponse Decrease(IDictionary<string, object> parameters)
	{
		var stepper = GetAppiumElement(parameters["element"]);

		if (stepper is null)
			return CommandResponse.FailedEmptyResponse;

		var buttons = AppiumQuery.ByClass("XCUIElementTypeButton").FindElements(stepper, _appiumApp);

		if (buttons is not null && buttons.Count > 1)
		{
			var decreaseButton = buttons.FirstOrDefault();
			decreaseButton?.Tap();
		}

		return CommandResponse.SuccessEmptyResponse;
	}

	static AppiumElement? GetAppiumElement(object element) =>
		element switch
		{
			AppiumElement appiumElement => appiumElement,
			AppiumDriverElement driverElement => driverElement.AppiumElement,
			_ => null
		};
}