using System;
using Microsoft.Maui.Controls.CustomAttributes;
using Microsoft.Maui.Controls.Internals;

namespace Maui.Controls.Sample.Issues;


[Issue(IssueTracker.Github, 1355, "Setting Main Page in quick succession causes crash on Android", PlatformAffected.Android, issueTestNumber: 1)]
public class Issue1355_Forms : TestContentPage
{
	int _runCount = 0;
	int _maxRunCount = 2;
	const string Success = "Success";

	protected override void Init()
	{
		Appearing += OnAppearing;
	}

	private void OnAppearing(object o, EventArgs eventArgs)
	{
		Application.Current.MainPage = CreatePage();
	}

	ContentPage CreatePage()
	{
		var page = new ContentPage
		{
			Content = new Label { Text = Success, AutomationId = Success },
			Title = $"CreatePage Iteration: {_runCount}"
		};

		page.Appearing += (sender, args) =>
		{
			_runCount += 1;
			if (_runCount <= _maxRunCount)
			{
				Application.Current.MainPage = new NavigationPage(CreatePage());
			}
		};

		return page;
	}
}
