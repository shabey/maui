using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Foldable;

namespace Microsoft.Maui.Foldable
{
	public class HingeAngleChangedEventArgs : EventArgs
	{
		public HingeAngleChangedEventArgs(double hingeAngleInDegrees)
		{
			HingeAngleInDegrees = hingeAngleInDegrees;
		}

		public double HingeAngleInDegrees { get; }
	}

	public partial class DualScreenInfo : INotifyPropertyChanged
	{
		Rect[] _spanningBounds;
		Rect _hingeBounds;
		bool _isLandscape;
		TwoPaneViewMode _spanMode;
		TwoPaneViewLayoutGuide _twoPaneViewLayoutGuide;
		IDualScreenService _dualScreenService;		
		IDualScreenService DualScreenService =>
			_dualScreenService ?? DependencyService.Get<IDualScreenService>() ?? NoDualScreenServiceImpl.Instance;

		internal VisualElement Element { get; }

		static Lazy<DualScreenInfo> _dualScreenInfo { get; } = new Lazy<DualScreenInfo>(OnCreate);

		public static DualScreenInfo Current => _dualScreenInfo.Value;
		public event PropertyChangedEventHandler PropertyChanged;

		public DualScreenInfo(VisualElement element) : this(element, null)
		{
		}

		internal DualScreenInfo(VisualElement element, IDualScreenService dualScreenService)
		{
			//HACK:FOLDABLE
			System.Diagnostics.Debug.Write("DualScreenInfo.ctor DualScreenService:" + dualScreenService, "JWM");

			_spanningBounds = new Rect[0];
			Element = element;
			_dualScreenService = dualScreenService;

			if (element == null)
			{
				System.Diagnostics.Debug.Write("     element == null", "JWM");
				_twoPaneViewLayoutGuide = TwoPaneViewLayoutGuide.Instance;
			}
			else
			{
				System.Diagnostics.Debug.Write($"     element == [{element}]", "JWM");
				_twoPaneViewLayoutGuide = new TwoPaneViewLayoutGuide(element, DualScreenService); // get if null
				_twoPaneViewLayoutGuide.PropertyChanged += OnTwoPaneViewLayoutGuideChanged;				
			}
		}


		EventHandler<HingeAngleChangedEventArgs> _hingeAngleChanged;
		int subscriberCount = 0;
		public event EventHandler<HingeAngleChangedEventArgs> HingeAngleChanged
		{
			add
			{
				ProcessHingeAngleSubscriberCount(Interlocked.Increment(ref subscriberCount));
				_hingeAngleChanged += value;
			}
			remove
			{
				ProcessHingeAngleSubscriberCount(Interlocked.Decrement(ref subscriberCount));
				_hingeAngleChanged -= value;
			}
		}


		public Rect[] SpanningBounds
		{
			get => GetSpanningBounds();
			set
			{
				if (_spanningBounds == null && value == null)
					return;

				if (_spanningBounds == null ||
					value == null ||
					!Enumerable.SequenceEqual(_spanningBounds, value))
				{
					SetProperty(ref _spanningBounds, value);
				}
			}
		}

		public Rect HingeBounds
		{
			get => GetHingeBounds();
			set
			{
				SetProperty(ref _hingeBounds, value);
			}
		}
		/// <summary>
		/// Used when app is detected to be on a single screen - 
		/// mainly on Surface Duo (although possible also on other
		/// foldable with multi-window enabled)
		/// </summary>
		public bool IsLandscape
		{
			get => GetIsLandscape();
			set
			{
				SetProperty(ref _isLandscape, value);
			}
		}
		/// <summary>
		/// Determines the layout direction of the panes
		/// SinglePane, Wide, Tall
		/// </summary>
		public TwoPaneViewMode SpanMode
		{
			get => GetSpanMode();
			set
			{
				SetProperty(ref _spanMode, value);
			}
		}

		Rect[] GetSpanningBounds()
		{
			var guide = _twoPaneViewLayoutGuide;
			var hinge = guide.Hinge;

			if (hinge == Rect.Zero)
				return new Rect[0];

			if (guide.Pane2 == Rect.Zero)
				return new Rect[0];
			
			//TODO: should this be checking SpanMode==Wide ???
			if(IsLandscape)
				return new[] { guide.Pane1, new Rect(0, hinge.Height + guide.Pane1.Height, guide.Pane2.Width, guide.Pane2.Height) };
			else
				return new[] { guide.Pane1, new Rect(hinge.Width + guide.Pane1.Width, 0, guide.Pane2.Width, guide.Pane2.Height) };
		}

		Rect GetHingeBounds()
		{
			var guide = _twoPaneViewLayoutGuide;
			return guide.Hinge;
		}

		bool GetIsLandscape() => _twoPaneViewLayoutGuide.IsLandscape;

		TwoPaneViewMode GetSpanMode() => _twoPaneViewLayoutGuide.Mode;

		static DualScreenInfo OnCreate()
		{
			DualScreenInfo dualScreenInfo = new DualScreenInfo(null);
//			dualScreenInfo._twoPaneViewLayoutGuide.WatchForChanges(); // HACK: no-op!
			dualScreenInfo._twoPaneViewLayoutGuide.PropertyChanged += dualScreenInfo.OnTwoPaneViewLayoutGuideChanged;
			return dualScreenInfo;
		}

		void OnTwoPaneViewLayoutGuideChanged(object sender, PropertyChangedEventArgs e)
		{
			SpanningBounds = GetSpanningBounds();
			IsLandscape = GetIsLandscape();
			HingeBounds = GetHingeBounds();
			SpanMode = GetSpanMode();

			//HACK:FOLDABLE
			System.Diagnostics.Debug.Write("DualScreenInfo.OnTwoPaneViewLayoutGuideChanged SpanningBounds:" + SpanningBounds.ToRectStrings(), "JWM");
			System.Diagnostics.Debug.Write("                                               IsLandscape:" + IsLandscape, "JWM");
			System.Diagnostics.Debug.Write("                                               HingeBounds:" + HingeBounds, "JWM");
			System.Diagnostics.Debug.Write("                                               SpanMode:" + SpanMode, "JWM");
		}

		bool SetProperty<T>(ref T backingStore, T value,
			[CallerMemberName]string propertyName = "",
			Action onChanged = null)
		{
			if (EqualityComparer<T>.Default.Equals(backingStore, value))
				return false;

			backingStore = value;
			onChanged?.Invoke();
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			return true;
		}
	}
	
	/// <summary>
	/// Microsoft.Maui.Graphics.Rectangle to string (for debugging)
	/// </summary>
	public static class BoundsExtensions
	{
		public static string ToRectStrings(this Rect[] bounds) {
			if (bounds.Length == 0)
				return "[]";
			string output = "";
			foreach (var rect in bounds)
			{
				output += rect.ToString() + " ";
			}
			return output;
		}
	}
}
