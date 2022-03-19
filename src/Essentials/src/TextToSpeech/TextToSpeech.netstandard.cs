using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Maui.Media
{
	/// <include file="../../docs/Microsoft.Maui.Essentials/TextToSpeech.xml" path="Type[@FullName='Microsoft.Maui.Essentials.TextToSpeech']/Docs" />
	partial class TextToSpeechImplementation : ITextToSpeech
	{
		public Task SpeakAsync(string text, CancellationToken cancelToken) =>
			throw ExceptionUtils.NotSupportedOrImplementedException;

		public Task SpeakAsync(string text, SpeechOptions options, CancellationToken cancelToken) =>
			throw ExceptionUtils.NotSupportedOrImplementedException;

		public Task<IEnumerable<Locale>> GetLocalesAsync() =>
			throw ExceptionUtils.NotSupportedOrImplementedException;
	}
}
