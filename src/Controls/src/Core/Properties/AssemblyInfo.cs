using System.Runtime.CompilerServices;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Controls.StyleSheets;
using Compatibility = Microsoft.Maui.Controls.Compatibility;

[assembly: InternalsVisibleTo("iOSUnitTests")]
[assembly: InternalsVisibleTo("Microsoft.Maui.Controls.Compatibility.ControlGallery")]
[assembly: InternalsVisibleTo("Microsoft.Maui.Controls.Compatibility")]
[assembly: InternalsVisibleTo("Microsoft.Maui.Controls.Compatibility.Android")]
[assembly: InternalsVisibleTo("Microsoft.Maui.Controls.Compatibility.iOS")]
[assembly: InternalsVisibleTo("Microsoft.Maui.Controls.Compatibility.Windows")]
[assembly: InternalsVisibleTo("Microsoft.Maui.Controls.Core.Design")]
[assembly: InternalsVisibleTo("Microsoft.Maui.Controls.Core.UnitTests")]
[assembly: InternalsVisibleTo("Microsoft.Maui.Controls.Android.UnitTests")]
[assembly: InternalsVisibleTo("Microsoft.Maui.Controls.Compatibility.Android.UnitTests")]
[assembly: InternalsVisibleTo("Microsoft.Maui.Controls.Compatibility.UAP.UnitTests")]
[assembly: InternalsVisibleTo("Microsoft.Maui.Controls.Xaml")]
[assembly: InternalsVisibleTo("Microsoft.Maui.Controls.Compatibility.Maps")]
[assembly: InternalsVisibleTo("Microsoft.Maui.Controls.Compatibility.Maps.iOS")]
[assembly: InternalsVisibleTo("Microsoft.Maui.Controls.Compatibility.Maps.iOS.Classic")]
[assembly: InternalsVisibleTo("Microsoft.Maui.Controls.Compatibility.Maps.Android")]
[assembly: InternalsVisibleTo("Microsoft.Maui.Controls.Xaml.UnitTests")]
[assembly: InternalsVisibleTo("Microsoft.Maui.Controls.UITests")]
[assembly: InternalsVisibleTo("Microsoft.Maui.Controls.FlexLayout.UnitTests")]
[assembly: InternalsVisibleTo("Microsoft.Maui.Controls.Material")]

[assembly: InternalsVisibleTo("Microsoft.Maui.Controls.iOS.UITests")]
[assembly: InternalsVisibleTo("Microsoft.Maui.Controls.Android.UITests")]
[assembly: InternalsVisibleTo("Microsoft.Maui.Controls.Windows.UITests")]
[assembly: InternalsVisibleTo("Microsoft.Maui.Controls.macOS.UITests")]
[assembly: InternalsVisibleTo("Microsoft.Maui.Controls.iOS.UITests")]
[assembly: InternalsVisibleTo("Microsoft.Maui.Controls.Android.UITests")]
[assembly: InternalsVisibleTo("Microsoft.Maui.Controls.DeviceTests")]
[assembly: InternalsVisibleTo("Microsoft.Maui.Controls.Loader")] // Microsoft.Maui.Controls.Loader.dll, Microsoft.Maui.Controls.Internals.ResourceLoader.ResourceProvider, kzu@microsoft.com
[assembly: InternalsVisibleTo("Microsoft.Maui.Controls.HotReload.Forms")]
[assembly: InternalsVisibleTo("Microsoft.Maui.Controls.UITest.Validator")]
[assembly: InternalsVisibleTo("Microsoft.Maui.Controls.Build.Tasks")]
[assembly: InternalsVisibleTo("Microsoft.Maui")]
[assembly: InternalsVisibleTo("Microsoft.Maui.Controls.Pages")]
[assembly: InternalsVisibleTo("Microsoft.Maui.Controls.Pages.UnitTests")]
[assembly: InternalsVisibleTo("Microsoft.Maui.Controls.CarouselView")]
[assembly: InternalsVisibleTo("Microsoft.Maui.Controls.Foldable")]
[assembly: InternalsVisibleTo("Microsoft.Maui.Controls.Foldable.UnitTests")]
[assembly: InternalsVisibleTo("WinUI.UITests")]
[assembly: InternalsVisibleTo("Microsoft.Maui.Controls.DeviceTests")]

[assembly: InternalsVisibleTo("CommunityToolkit.Maui")]
[assembly: InternalsVisibleTo("CommunityToolkit.Maui.Core")]
[assembly: InternalsVisibleTo("CommunityToolkit.Maui.UnitTests")]
[assembly: InternalsVisibleTo("CommunityToolkit.Maui.Markup")]
[assembly: InternalsVisibleTo("CommunityToolkit.Maui.Markup.UnitTests")]

[assembly: Preserve]

[assembly: XmlnsDefinition("http://schemas.microsoft.com/dotnet/2021/maui", "Microsoft.Maui.Controls.Shapes")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/dotnet/2021/maui", "Microsoft.Maui.Controls")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/dotnet/2021/maui", "Microsoft.Maui", AssemblyName = "Microsoft.Maui")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/dotnet/2021/maui", "Microsoft.Maui.Graphics", AssemblyName = "Microsoft.Maui.Graphics")]

[assembly: XmlnsDefinition("http://schemas.microsoft.com/dotnet/2021/maui/design", "Microsoft.Maui.Controls.Shapes")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/dotnet/2021/maui/design", "Microsoft.Maui.Controls")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/dotnet/2021/maui/design", "Microsoft.Maui", AssemblyName = "Microsoft.Maui")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/dotnet/2021/maui/design", "Microsoft.Maui.Graphics", AssemblyName = "Microsoft.Maui.Graphics")]

[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml", "Microsoft.Maui.Controls.Xaml", AssemblyName = "Microsoft.Maui.Controls.Xaml")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml", "System", AssemblyName = "mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml", "System", AssemblyName = "System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2009/xaml", "Microsoft.Maui.Controls.Xaml", AssemblyName = "Microsoft.Maui.Controls.Xaml")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2009/xaml", "System", AssemblyName = "mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2009/xaml", "System", AssemblyName = "System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]

[assembly: XmlnsPrefix("http://schemas.microsoft.com/dotnet/2021/maui", "maui")]
[assembly: XmlnsPrefix("http://schemas.microsoft.com/dotnet/2021/maui/design", "d")]

[assembly: StyleProperty("background-color", typeof(VisualElement), nameof(VisualElement.BackgroundColorProperty))]
[assembly: StyleProperty("background", typeof(VisualElement), nameof(VisualElement.BackgroundProperty))]
[assembly: StyleProperty("background-image", typeof(Page), nameof(Page.BackgroundImageSourceProperty))]
[assembly: StyleProperty("border-color", typeof(IBorderElement), nameof(BorderElement.BorderColorProperty))]
[assembly: StyleProperty("border-radius", typeof(ICornerElement), nameof(CornerElement.CornerRadiusProperty))]
[assembly: StyleProperty("border-radius", typeof(Button), nameof(Button.CornerRadiusProperty))]
[assembly: StyleProperty("border-radius", typeof(Frame), nameof(Frame.CornerRadiusProperty))]
[assembly: StyleProperty("border-radius", typeof(ImageButton), nameof(BorderElement.CornerRadiusProperty))]
[assembly: StyleProperty("border-width", typeof(IBorderElement), nameof(BorderElement.BorderWidthProperty))]
[assembly: StyleProperty("color", typeof(IColorElement), nameof(ColorElement.ColorProperty), Inherited = true)]
[assembly: StyleProperty("color", typeof(ITextElement), nameof(TextElement.TextColorProperty), Inherited = true)]
[assembly: StyleProperty("text-transform", typeof(ITextElement), nameof(TextElement.TextTransformProperty), Inherited = true)]
[assembly: StyleProperty("color", typeof(ProgressBar), nameof(ProgressBar.ProgressColorProperty))]
[assembly: StyleProperty("color", typeof(Switch), nameof(Switch.OnColorProperty))]
[assembly: StyleProperty("column-gap", typeof(Compatibility.Grid), nameof(Compatibility.Grid.ColumnSpacingProperty))]
[assembly: StyleProperty("column-gap", typeof(Grid), nameof(Grid.ColumnSpacingProperty))]
[assembly: StyleProperty("direction", typeof(VisualElement), nameof(VisualElement.FlowDirectionProperty), Inherited = true)]
[assembly: StyleProperty("font-family", typeof(IFontElement), nameof(FontElement.FontFamilyProperty), Inherited = true)]
[assembly: StyleProperty("font-size", typeof(IFontElement), nameof(FontElement.FontSizeProperty), Inherited = true)]
[assembly: StyleProperty("font-style", typeof(IFontElement), nameof(FontElement.FontAttributesProperty), Inherited = true)]
[assembly: StyleProperty("height", typeof(VisualElement), nameof(VisualElement.HeightRequestProperty))]
[assembly: StyleProperty("margin", typeof(View), nameof(View.MarginProperty))]
[assembly: StyleProperty("margin-left", typeof(View), nameof(View.MarginLeftProperty))]
[assembly: StyleProperty("margin-top", typeof(View), nameof(View.MarginTopProperty))]
[assembly: StyleProperty("margin-right", typeof(View), nameof(View.MarginRightProperty))]
[assembly: StyleProperty("margin-bottom", typeof(View), nameof(View.MarginBottomProperty))]
[assembly: StyleProperty("max-lines", typeof(Label), nameof(Label.MaxLinesProperty))]
[assembly: StyleProperty("min-height", typeof(VisualElement), nameof(VisualElement.MinimumHeightRequestProperty))]
[assembly: StyleProperty("min-width", typeof(VisualElement), nameof(VisualElement.MinimumWidthRequestProperty))]
[assembly: StyleProperty("opacity", typeof(VisualElement), nameof(VisualElement.OpacityProperty))]
[assembly: StyleProperty("padding", typeof(IPaddingElement), nameof(PaddingElement.PaddingProperty))]
[assembly: StyleProperty("padding-left", typeof(IPaddingElement), nameof(PaddingElement.PaddingLeftProperty), PropertyOwnerType = typeof(PaddingElement))]
[assembly: StyleProperty("padding-top", typeof(IPaddingElement), nameof(PaddingElement.PaddingTopProperty), PropertyOwnerType = typeof(PaddingElement))]
[assembly: StyleProperty("padding-right", typeof(IPaddingElement), nameof(PaddingElement.PaddingRightProperty), PropertyOwnerType = typeof(PaddingElement))]
[assembly: StyleProperty("padding-bottom", typeof(IPaddingElement), nameof(PaddingElement.PaddingBottomProperty), PropertyOwnerType = typeof(PaddingElement))]
[assembly: StyleProperty("row-gap", typeof(Compatibility.Grid), nameof(Compatibility.Grid.RowSpacingProperty))]
[assembly: StyleProperty("row-gap", typeof(Grid), nameof(Grid.RowSpacingProperty))]
[assembly: StyleProperty("text-align", typeof(ITextAlignmentElement), nameof(TextAlignmentElement.HorizontalTextAlignmentProperty), Inherited = true)]
[assembly: StyleProperty("text-decoration", typeof(IDecorableTextElement), nameof(DecorableTextElement.TextDecorationsProperty))]
[assembly: StyleProperty("transform", typeof(VisualElement), nameof(VisualElement.TransformProperty))]
[assembly: StyleProperty("transform-origin", typeof(VisualElement), nameof(VisualElement.TransformOriginProperty))]
[assembly: StyleProperty("vertical-align", typeof(ITextAlignmentElement), nameof(TextAlignmentElement.VerticalTextAlignmentProperty))]
[assembly: StyleProperty("visibility", typeof(VisualElement), nameof(VisualElement.IsVisibleProperty), Inherited = true)]
[assembly: StyleProperty("width", typeof(VisualElement), nameof(VisualElement.WidthRequestProperty))]
[assembly: StyleProperty("letter-spacing", typeof(ITextElement), nameof(TextElement.CharacterSpacingProperty), Inherited = true)]
[assembly: StyleProperty("line-height", typeof(ILineHeightElement), nameof(LineHeightElement.LineHeightProperty), Inherited = true)]

//flex
[assembly: StyleProperty("align-content", typeof(FlexLayout), nameof(FlexLayout.AlignContentProperty))]
[assembly: StyleProperty("align-items", typeof(FlexLayout), nameof(FlexLayout.AlignItemsProperty))]
[assembly: StyleProperty("align-self", typeof(VisualElement), nameof(FlexLayout.AlignSelfProperty), PropertyOwnerType = typeof(FlexLayout))]
[assembly: StyleProperty("flex-direction", typeof(FlexLayout), nameof(FlexLayout.DirectionProperty))]
[assembly: StyleProperty("flex-basis", typeof(VisualElement), nameof(FlexLayout.BasisProperty), PropertyOwnerType = typeof(FlexLayout))]
[assembly: StyleProperty("flex-grow", typeof(VisualElement), nameof(FlexLayout.GrowProperty), PropertyOwnerType = typeof(FlexLayout))]
[assembly: StyleProperty("flex-shrink", typeof(VisualElement), nameof(FlexLayout.ShrinkProperty), PropertyOwnerType = typeof(FlexLayout))]
[assembly: StyleProperty("flex-wrap", typeof(VisualElement), nameof(FlexLayout.WrapProperty), PropertyOwnerType = typeof(FlexLayout))]
[assembly: StyleProperty("justify-content", typeof(FlexLayout), nameof(FlexLayout.JustifyContentProperty))]
[assembly: StyleProperty("order", typeof(VisualElement), nameof(FlexLayout.OrderProperty), PropertyOwnerType = typeof(FlexLayout))]
[assembly: StyleProperty("position", typeof(FlexLayout), nameof(FlexLayout.PositionProperty))]

// compatibility flex
[assembly: StyleProperty("align-content", typeof(Compatibility.FlexLayout), nameof(Compatibility.FlexLayout.AlignContentProperty))]
[assembly: StyleProperty("align-items", typeof(Compatibility.FlexLayout), nameof(Compatibility.FlexLayout.AlignItemsProperty))]
[assembly: StyleProperty("align-self", typeof(VisualElement), nameof(Compatibility.FlexLayout.AlignSelfProperty), PropertyOwnerType = typeof(Compatibility.FlexLayout))]
[assembly: StyleProperty("flex-direction", typeof(Compatibility.FlexLayout), nameof(Compatibility.FlexLayout.DirectionProperty))]
[assembly: StyleProperty("flex-basis", typeof(VisualElement), nameof(Compatibility.FlexLayout.BasisProperty), PropertyOwnerType = typeof(Compatibility.FlexLayout))]
[assembly: StyleProperty("flex-grow", typeof(VisualElement), nameof(Compatibility.FlexLayout.GrowProperty), PropertyOwnerType = typeof(Compatibility.FlexLayout))]
[assembly: StyleProperty("flex-shrink", typeof(VisualElement), nameof(Compatibility.FlexLayout.ShrinkProperty), PropertyOwnerType = typeof(Compatibility.FlexLayout))]
[assembly: StyleProperty("flex-wrap", typeof(VisualElement), nameof(Compatibility.FlexLayout.WrapProperty), PropertyOwnerType = typeof(Compatibility.FlexLayout))]
[assembly: StyleProperty("justify-content", typeof(Compatibility.FlexLayout), nameof(Compatibility.FlexLayout.JustifyContentProperty))]
[assembly: StyleProperty("order", typeof(VisualElement), nameof(Compatibility.FlexLayout.OrderProperty), PropertyOwnerType = typeof(Compatibility.FlexLayout))]
[assembly: StyleProperty("position", typeof(Compatibility.FlexLayout), nameof(Compatibility.FlexLayout.PositionProperty))]

//xf specific
[assembly: StyleProperty("-maui-placeholder", typeof(IPlaceholderElement), nameof(PlaceholderElement.PlaceholderProperty))]
[assembly: StyleProperty("-maui-placeholder-color", typeof(IPlaceholderElement), nameof(PlaceholderElement.PlaceholderColorProperty))]
[assembly: StyleProperty("-maui-max-length", typeof(InputView), nameof(InputView.MaxLengthProperty))]
[assembly: StyleProperty("-maui-bar-background-color", typeof(IBarElement), nameof(BarElement.BarBackgroundColorProperty))]
[assembly: StyleProperty("-maui-bar-text-color", typeof(IBarElement), nameof(BarElement.BarTextColorProperty))]
[assembly: StyleProperty("-maui-orientation", typeof(ScrollView), nameof(ScrollView.OrientationProperty))]
[assembly: StyleProperty("-maui-horizontal-scroll-bar-visibility", typeof(ScrollView), nameof(ScrollView.HorizontalScrollBarVisibilityProperty))]
[assembly: StyleProperty("-maui-vertical-scroll-bar-visibility", typeof(ScrollView), nameof(ScrollView.VerticalScrollBarVisibilityProperty))]
[assembly: StyleProperty("-maui-min-track-color", typeof(Slider), nameof(Slider.MinimumTrackColorProperty))]
[assembly: StyleProperty("-maui-max-track-color", typeof(Slider), nameof(Slider.MaximumTrackColorProperty))]
[assembly: StyleProperty("-maui-thumb-color", typeof(Slider), nameof(Slider.ThumbColorProperty))]
[assembly: StyleProperty("-maui-spacing", typeof(Compatibility.StackLayout), nameof(Compatibility.StackLayout.SpacingProperty))]
[assembly: StyleProperty("-maui-spacing", typeof(StackLayout), nameof(StackLayout.SpacingProperty))]
[assembly: StyleProperty("-maui-orientation", typeof(Compatibility.StackLayout), nameof(Compatibility.StackLayout.OrientationProperty))]
[assembly: StyleProperty("-maui-orientation", typeof(StackLayout), nameof(StackLayout.OrientationProperty))]

// TODO ezhart 2021-07-16 When we fix #1634, we'll need to enable this so the CSS applies 
//[assembly: StyleProperty("-maui-spacing", typeof(StackLayout), nameof(StackLayout.SpacingProperty))]

// TODO ezhart 2021-07-16 When we create the new composed StackLayout, we'll need to ensure we have this enabled 
//[assembly: StyleProperty("-maui-orientation", typeof(StackLayout), nameof(StackLayout.OrientationProperty))]

[assembly: StyleProperty("-maui-visual", typeof(VisualElement), nameof(VisualElement.VisualProperty))]
[assembly: StyleProperty("-maui-vertical-text-alignment", typeof(Label), nameof(TextAlignmentElement.VerticalTextAlignmentProperty))]
[assembly: StyleProperty("-maui-thumb-color", typeof(Switch), nameof(Switch.ThumbColorProperty))]

//shell
[assembly: StyleProperty("-maui-flyout-background", typeof(Shell), nameof(Shell.FlyoutBackgroundColorProperty))]
[assembly: StyleProperty("-maui-shell-background", typeof(Element), nameof(Shell.BackgroundColorProperty), PropertyOwnerType = typeof(Shell))]
[assembly: StyleProperty("-maui-shell-disabled", typeof(Element), nameof(Shell.DisabledColorProperty), PropertyOwnerType = typeof(Shell))]
[assembly: StyleProperty("-maui-shell-foreground", typeof(Element), nameof(Shell.ForegroundColorProperty), PropertyOwnerType = typeof(Shell))]
[assembly: StyleProperty("-maui-shell-tabbar-background", typeof(Element), nameof(Shell.TabBarBackgroundColorProperty), PropertyOwnerType = typeof(Shell))]
[assembly: StyleProperty("-maui-shell-tabbar-disabled", typeof(Element), nameof(Shell.TabBarDisabledColorProperty), PropertyOwnerType = typeof(Shell))]
[assembly: StyleProperty("-maui-shell-tabbar-foreground", typeof(Element), nameof(Shell.TabBarForegroundColorProperty), PropertyOwnerType = typeof(Shell))]
[assembly: StyleProperty("-maui-shell-tabbar-title", typeof(Element), nameof(Shell.TabBarTitleColorProperty), PropertyOwnerType = typeof(Shell))]
[assembly: StyleProperty("-maui-shell-tabbar-unselected", typeof(Element), nameof(Shell.TabBarUnselectedColorProperty), PropertyOwnerType = typeof(Shell))]
[assembly: StyleProperty("-maui-shell-title", typeof(Element), nameof(Shell.TitleColorProperty), PropertyOwnerType = typeof(Shell))]
[assembly: StyleProperty("-maui-shell-unselected", typeof(Element), nameof(Shell.UnselectedColorProperty), PropertyOwnerType = typeof(Shell))]
