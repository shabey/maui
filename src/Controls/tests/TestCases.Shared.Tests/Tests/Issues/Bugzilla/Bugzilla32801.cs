﻿using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues;

public class Bugzilla32801 : _IssuesUITest
{
#if ANDROID
        const string Tab1 = "TAB 1";    
#else
        const string Tab1 = "Tab 1";
#endif
        const string TabAddButton = "TabAddButton";
        const string Level2AddButton = "Level2AddButton";
        const string Level3StackButton = "Level3StackButton";
        const string Tab1StackButton = "Tab1StackButton";


        public Bugzilla32801(TestDevice testDevice) : base(testDevice)
        {
        }

        public override string Issue => "Memory Leak in TabbedPage + NavigationPage";

        [Test]
        [Category(UITestCategories.TabbedPage)]
        public void Bugzilla32801Test()
        {
                App.WaitForElement(TabAddButton);
                App.Tap(TabAddButton);
                App.WaitForElementTillPageNavigationSettled(Level2AddButton);
                App.Tap(Level2AddButton);
                App.WaitForElementTillPageNavigationSettled(Level3StackButton);
                App.Tap(Level3StackButton);
                App.WaitForElement("Stack 3");
                App.Tap(Tab1);
                App.WaitForElementTillPageNavigationSettled(Tab1StackButton);
                App.Tap(Tab1StackButton);
                App.WaitForElement("Stack 1");
        }
}