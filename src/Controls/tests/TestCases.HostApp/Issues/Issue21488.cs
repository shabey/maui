namespace Maui.Controls.Sample.Issues;

[XamlCompilation(XamlCompilationOptions.Compile)]
[Issue(IssueTracker.Github, "21488", "Button text doesn't update when CharacterSpacing is applied", PlatformAffected.iOS)]
public partial class Issue21488 : ContentPage
{
	public Issue21488()
	{
		var buttonWithoutCharacterSpacing = new Button();
		var buttonWithCharacterSpacing = new Button() { CharacterSpacing = 15 };
		var entry = new Entry() { AutomationId = "Entry" };
		entry.TextChanged += (s, e) =>
		{
			buttonWithoutCharacterSpacing.Text = entry.Text;
			buttonWithCharacterSpacing.Text = entry.Text;
		};

		Content = new VerticalStackLayout()
		{
			new Label() { Text = "Button without character spacing:" },
			buttonWithoutCharacterSpacing,
			new Label() { Text = "Button with character spacing:" },
			buttonWithCharacterSpacing,
			entry
		};
	}
}