﻿using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;

namespace UITests;

[SetUpFixture]
public class AppiumSetup
{
    private static AppiumDriver? driver;

    public static AppiumDriver App => driver ?? throw new NullReferenceException("AppiumDriver is null");

    public AppiumSetup()
    {
    }

    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        var appiumOptions = new AppiumOptions
        {
            DeviceName = "iPhone 14 Pro",
            PlatformName = "ios",
            PlatformVersion = "16",
        };
        driver = new AndroidDriver(new Uri("http://127.0.0.1:4723/wd/hub"), appiumOptions);
    }

    [OneTimeTearDown]
    public void RunAfterAnyTests()
    {
        driver?.Quit();
    }
}
