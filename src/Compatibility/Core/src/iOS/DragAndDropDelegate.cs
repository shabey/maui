
#if __MOBILE__
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using Foundation;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Platform;
using ObjCRuntime;
using UIKit;

namespace Microsoft.Maui.Controls.Compatibility.Platform.iOS
{
	[SupportedOSPlatform("ios11.0")]
	internal class DragAndDropDelegate : NSObject, IUIDragInteractionDelegate, IUIDropInteractionDelegate
	{
		[Export("dragInteraction:session:willEndWithOperation:")]
		[Preserve(Conditional = true)]
		public void SessionWillEnd(UIDragInteraction interaction, IUIDragSession session, UIDropOperation operation)
		{
			if ((operation == UIDropOperation.Cancel || operation == UIDropOperation.Forbidden) &&
				session.Items.Length > 0 &&
				session.Items[0].LocalObject is CustomLocalStateData cdi)
			{
				this.HandleDropCompleted(cdi.View);
			}
		}

		[Preserve(Conditional = true)]
		public UIDragItem[] GetItemsForBeginningSession(UIDragInteraction interaction, IUIDragSession session)
		{
			if (interaction.View is IVisualElementRenderer renderer && renderer.Element is View view)
			{
				return HandleDragStarting(view, renderer);
			}

			return Array.Empty<UIDragItem>();
		}

		[Export("dropInteraction:canHandleSession:")]
		[Preserve(Conditional = true)]
		public bool CanHandleSession(UIDropInteraction interaction, IUIDropSession session)
		{
			if (session.LocalDragSession == null)
			{
				return false;
			}

			if (session.LocalDragSession.Items.Length > 0 &&
				session.LocalDragSession.Items[0].LocalObject is CustomLocalStateData)
			{
				return true;
			}

			return false;
		}

		[Export("dropInteraction:sessionDidExit:")]
		[Preserve(Conditional = true)]
		public void SessionDidExit(UIDropInteraction interaction, IUIDropSession session)
		{
			if (interaction.View is IVisualElementRenderer renderer)
			{
				DataPackage package = null;

				if (session.LocalDragSession.Items.Length > 0 &&
					session.LocalDragSession.Items[0].LocalObject is CustomLocalStateData cdi)
				{
					package = cdi.DataPackage;
				}

				if (HandleDragLeave((View)renderer.Element, package))
				{
				}
			}
		}

		[Export("dropInteraction:sessionDidUpdate:")]
		[Preserve(Conditional = true)]
		public UIDropProposal SessionDidUpdate(UIDropInteraction interaction, IUIDropSession session)
		{
			UIDropOperation operation = UIDropOperation.Cancel;

			if (session.LocalDragSession == null)
			{
				return new UIDropProposal(operation);
			}

			if (interaction.View is IVisualElementRenderer renderer)
			{
				DataPackage package = null;

				if (session.LocalDragSession.Items.Length > 0 &&
					session.LocalDragSession.Items[0].LocalObject is CustomLocalStateData cdi)
				{
					package = cdi.DataPackage;
				}

				if (HandleDragOver((View)renderer.Element, package))
				{
					operation = UIDropOperation.Copy;
				}
			}

			return new UIDropProposal(operation);
		}

		[Export("dropInteraction:performDrop:")]
		[Preserve(Conditional = true)]
		public void PerformDrop(UIDropInteraction interaction, IUIDropSession session)
		{
			if (session.LocalDragSession == null)
			{
				return;
			}

			if (session.LocalDragSession.Items.Length > 0 &&
				session.LocalDragSession.Items[0].LocalObject is CustomLocalStateData cdi &&
				interaction.View is IVisualElementRenderer renderer &&
				renderer.Element is View view)
			{
				HandleDrop(view, cdi.DataPackage);
				HandleDropCompleted(cdi.View);
			}
		}


		void SendEventArgs<TRecognizer>(Action<TRecognizer> func, View view)
			where TRecognizer : class
		{
			var gestures =
				view.GestureRecognizers;

			if (gestures == null)
			{
				return;
			}

			foreach (var gesture in gestures)
			{
				if (gesture is TRecognizer recognizer)
				{
					func(recognizer);
				}
			}
		}

		public UIDragItem[] HandleDragStarting(View element, IVisualElementRenderer renderer)
		{
			UIDragItem[] returnValue = null;
			SendEventArgs<DragGestureRecognizer>(rec =>
			{
				if (!rec.CanDrag)

/* Unmerged change from project 'Compatibility(net8.0-maccatalyst)'
Before:
					return;

				var args = rec.SendDragStarting(element);

				if (args.Cancel)
After:
				{
*/
				{
					return;
				}

				var args = rec.SendDragStarting(element);

				if (args.Cancel)
				
/* Unmerged change from project 'Compatibility(net8.0-maccatalyst)'
Before:
#pragma warning disable CS0618 // Type or member is obsolete
				if (!args.Handled)
After:
				}

				var args = rec.SendDragStarting(element);

				if (args.Cancel)
				{
					return;
				}
#pragma warning disable CS0618 // Type or member is obsolete
				if (!args.Handled)
*/

/* Unmerged change from project 'Compatibility(net8.0-maccatalyst)'
Before:
							itemProvider = new NSItemProvider(uIImage);
						else
							itemProvider = new NSItemProvider(new NSString(""));
After:
						{
							itemProvider = new NSItemProvider(uIImage);
						}
						else
						{
							itemProvider = new NSItemProvider(new NSString(""));
						}
*/

/* Unmerged change from project 'Compatibility(net8.0-maccatalyst)'
Before:
							args.Data.Image = imageElement.Source;
After:
						{
							args.Data.Image = imageElement.Source;
						}
*/
{
					return;
		
/* Unmerged change from project 'Compatibility(net8.0-maccatalyst)'
Before:
					return;
After:
				{
					return;
				}
*/

/* Unmerged change from project 'Compatibility(net8.0-maccatalyst)'
Before:
					return;
After:
				{
					return;
				}
*/

/* Unmerged change from project 'Compatibility(net8.0-maccatalyst)'
Before:
					return;
After:
				{
					return;
*/
		}
#pragma warning disable CS0618 // Type or member is obsolete
				if (!args.Handled)
#pragma warning restore CS0618 // Type or member is obsolete
				{
					UIImage uIImage = null;
					string clipDescription = String.Empty;
					NSItemProvider itemProvider = null;

					if (renderer is IImageVisualElementRenderer iver)
					{
						uIImage = iver.GetImage()?.Image;
						if (uIImage != null)
						{
							itemProvider = new NSItemProvider(uIImage);
						}
						else
						{
							itemProvider = new NSItemProvider(new NSString(""));
						}

						if (args.Data.Image == null && renderer.Element is IImageElement imageElement)
						{
							args.Data.Image = imageElement.Source;
						}
					}
					else
					{
						string text = args.Data.Text ?? clipDescription;

						if (String.IsNullOrWhiteSpace(text))
						{
							itemProvider = new NSItemProvider(renderer.NativeView.ConvertToImage());
						}
						else
						{
							itemProvider = new NSItemProvider(new NSString(text));
						}
					}

					var dragItem = new UIDragItem(itemProvider);
					dragItem.LocalObject = new CustomLocalStateData()
					{
						Renderer = renderer,
						View = renderer.Element as View,
						DataPackage = args.Data
					};

					returnValue = new UIDragItem[] { dragItem };
				}
			},
			element);

			return returnValue ?? Array.Empty<UIDragItem>();
		}

		void HandleDropCompleted(View element)
		{
			var args = new DropCompletedEventArgs();
			SendEventArgs<DragGestureRecognizer>(rec => rec.SendDropCompleted(args), element);
		}

		bool HandleDragLeave(View element, DataPackage dataPackage)
		{
			var dragEventArgs = new DragEventArgs(dataPackage);

			bool validTarget = false;
			SendEventArgs<DropGestureRecognizer>(rec =>
			{
				if (!rec.AllowDrop)
				{
					return;
				}

				rec.SendDragLeave(dragEventArgs);
				validTarget = validTarget || dragEventArgs.AcceptedOperation != DataPackageOperation.None;
			}, element);

			return validTarget;
		}

		bool HandleDragOver(View element, DataPackage dataPackage)
		{
			var dragEventArgs = new DragEventArgs(dataPackage);

			bool validTarget = false;
			SendEventArgs<DropGestureRecognizer>(rec =>
			{
				if (!rec.AllowDrop)
				{
					return;
				}

				rec.SendDragOver(dragEventArgs);
				validTarget = validTarget || dragEventArgs.AcceptedOperation != DataPackageOperation.None;
			}, element);

			return validTarget;
		}

		void HandleDrop(View element, DataPackage datapackage)
		{
			var args = new DropEventArgs(datapackage?.View);
			SendEventArgs<DropGestureRecognizer>(async rec =>
			{
				if (!rec.AllowDrop)
				{
					return;
				}
				}

				try
				{
					await rec.SendDrop(args);
				}
				catch (Exception e)
				{
					Forms.MauiContext?.CreateLogger<DropGestureRecognizer>()?.LogWarning(e, null);
				}
			}, (View)element);
		}

		class CustomLocalStateData : NSObject
		{
			public View View { get; set; }
			public IVisualElementRenderer Renderer { get; set; }
			public DataPackage DataPackage { get; set; }
		}
	}

}
#endif
