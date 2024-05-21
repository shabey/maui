using Android.Text;
using Android.Text.Method;
using Android.Views;
using Microsoft.Maui.Controls;
using AView = Android.Views.View;

namespace Maui.Controls.Sample.Platform;

public class NumericKeyListener : NumberKeyListener
{
    public override InputTypes InputType { get; }

    protected override char[] GetAcceptedChars() => "0123456789-,.".ToCharArray();

    public NumericKeyListener(InputTypes inputType)
    {
        InputType = inputType;
    }

    public override bool OnKeyDown(AView view, IEditable content, Keycode keyCode, KeyEvent e)
    {
        Application.Current.MainPage.DisplayAlert("OnKeyDown", string.Empty, "Ok");
        return base.OnKeyDown(view, content, keyCode, e);
    }
}