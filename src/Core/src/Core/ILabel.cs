namespace Microsoft.Maui
{
	/// <summary>
	/// Represents a View that displays text.
	/// </summary>
	public interface ILabel : IView, IText
	{
		/// <summary>
		/// Gets the option for line breaking.
		/// </summary>
		LineBreakMode LineBreakMode { get; }

		/// <summary>
		/// Gets the maximum number of lines allowed in the Label.
		/// </summary>
		int MaxLines { get; }

		/// <summary>
		/// Gets the space between the text of the Label and it's border.
		/// </summary>
		Thickness Padding { get; }
	}
}