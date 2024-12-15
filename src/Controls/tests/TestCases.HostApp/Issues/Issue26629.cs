namespace Maui.Controls.Sample.Issues
{
    [Issue(IssueTracker.Github, 26629, "ScrollView resizes when content changes", PlatformAffected.All)]
    public class Issue26629 : ContentPage
    {
        public Issue26629()
        {
	        var vsl = new VerticalStackLayout { Spacing = 16 };
	        var scrollView = new ScrollView();
	        var scrollViewVsl = new VerticalStackLayout();
	        var button = new Button
	        {
		        Text = "Add Label",
		        AutomationId = "AddLabelButton",
	        };
	        var sizeLabel = new Label
	        {
		        AutomationId = "SizeLabel",
	        };
	        sizeLabel.SetBinding(Label.TextProperty, new Binding("Height", source: scrollView));

	        var i = 0;
	        scrollView.BackgroundColor = Colors.LightBlue;
	        scrollViewVsl.Children.Add(CreateLabel("Label0"));
	        button.Clicked += (sender, args) =>
	        {
		        scrollViewVsl.Children.Add(CreateLabel($"Label{++i}"));
	        };
	        
	        scrollView.Content = scrollViewVsl;
	        vsl.Children.Add(button);
	        vsl.Children.Add(sizeLabel);
	        vsl.Children.Add(scrollView);
	        
	        Content = vsl;
        }

        static Label CreateLabel(string automationId)
        {
	        return new Label
	        {
		        Text = "Huge Label",
		        FontSize = 50,
		        AutomationId = automationId,
	        };
        }
    }
}