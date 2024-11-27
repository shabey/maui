#nullable disable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Microsoft.Maui.Controls.Platform
{
	internal class ObservableItemTemplateCollection : ObservableList<ItemTemplateContext>
	{
		readonly IList _itemsSource;
		readonly DataTemplate _itemTemplate;
		readonly BindableObject _container;
		readonly IMauiContext _mauiContext;
		readonly double _itemHeight;
		readonly double _itemWidth;
		readonly Thickness _itemSpacing;
		readonly NotifyCollectionChangedEventHandler _collectionChanged;
		readonly WeakNotifyCollectionChangedProxy _proxy = new();

		bool _innerCollectionChange = false;
		bool _observeChanges = true;

		~ObservableItemTemplateCollection() => _proxy.Unsubscribe();

		public ObservableItemTemplateCollection(IList itemsSource, DataTemplate itemTemplate, BindableObject container,
			double? itemHeight = null, double? itemWidth = null, Thickness? itemSpacing = null, IMauiContext mauiContext = null)
		{
			if (!(itemsSource is INotifyCollectionChanged notifyCollectionChanged))
			{
				throw new ArgumentException($"{nameof(itemsSource)} must implement {nameof(INotifyCollectionChanged)}");
			}

			_itemsSource = itemsSource;
			_itemTemplate = itemTemplate;
			_container = container;
			_mauiContext = mauiContext;
			if (itemHeight.HasValue)
				_itemHeight = itemHeight.Value;

			if (itemWidth.HasValue)
				_itemWidth = itemWidth.Value;

			if (itemSpacing.HasValue)
				_itemSpacing = itemSpacing.Value;

			// We're using this as a source for a ListViewBase, and we need INCC to work. So ListViewBase is going
			// to iterate over the entire source list right off the bat, no matter what we do. Creating one
			// ItemTemplateContext per item in the collection is unavoidable. Luckily, ITC is pretty cheap.
			AddItemTemplateContexts(itemsSource);

			_collectionChanged = InnerCollectionChanged;
			_proxy.Subscribe(notifyCollectionChanged, _collectionChanged);

			CollectionChanged += TemplateCollectionChanged;
		}

		public void CleanUp()
		{
			CollectionChanged -= TemplateCollectionChanged;
			_proxy.Unsubscribe();
		}

		void TemplateCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
		{
			if (!_innerCollectionChange)
			{
				// When the template collection changes not as result of an inner collection change.
				// The only time this happens is during a drag/drop item reorder (CanReorderItems).
				// The ListView/GridView has notified us now we need to move those changes into the source.
				// One might think it would be a "Move" event but it is actually a "Remove" followed by "Add".
				_observeChanges = false;

				switch (args.Action)
				{
					case NotifyCollectionChangedAction.Add:
						AddToSource(args);
						break;
					case NotifyCollectionChangedAction.Remove:
						RemoveFromSource(args);
						break;
					default:
						break;
				}

				_observeChanges = true;
			}
		}

		void AddToSource(NotifyCollectionChangedEventArgs args)
		{
			var startIndex = args.NewStartingIndex > -1 ? args.NewStartingIndex : IndexOf((ItemTemplateContext)args.NewItems[0]);

			var count = args.NewItems.Count;

			for (int n = 0; n < count; n++)
			{
				var newItem = (ItemTemplateContext)args.NewItems[n];
				_itemsSource.Insert(startIndex, newItem.Item);
			}
		}

		void RemoveFromSource(NotifyCollectionChangedEventArgs args)
		{
			var startIndex = args.OldStartingIndex;

			if (startIndex < 0)
			{
				// INCC implementation isn't giving us enough information to know where the removed items were in the
				return;
			}

			var count = args.OldItems.Count;

			for (int n = startIndex + count - 1; n >= startIndex; n--)
			{
				_itemsSource.RemoveAt(n);
			}
		}

		void InnerCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
		{
			if (!_observeChanges)
			{
				return;
			}

			_container.Dispatcher.DispatchIfRequired(() => InnerCollectionChanged(args));
		}

		void InnerCollectionChanged(NotifyCollectionChangedEventArgs args)
		{
			_innerCollectionChange = true;
			switch (args.Action)
			{
				case NotifyCollectionChangedAction.Add:
					Add(args);
					break;
				case NotifyCollectionChangedAction.Move:
					Move(args);
					break;
				case NotifyCollectionChangedAction.Remove:
					Remove(args);
					break;
				case NotifyCollectionChangedAction.Replace:
					Replace(args);
					break;
				case NotifyCollectionChangedAction.Reset:
					Reset();
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			_innerCollectionChange = false;
		}

		void Add(NotifyCollectionChangedEventArgs args)
		{
			var index = args.NewStartingIndex > -1 ? args.NewStartingIndex : _itemsSource.IndexOf(args.NewItems[0]);

			AddItemTemplateContexts(args.NewItems, startIndex);
		}

		private void AddItemTemplateContexts(IList newItems, int startIndex = 0)
		{
			var count = newItems.Count;

			if (count == 1)
			{
				Add(new ItemTemplateContext(_itemTemplate, newItems[0], _container, _itemHeight, _itemWidth, _itemSpacing, _mauiContext));
				return;
			}

			List<ItemTemplateContext> itemsToAdd = new(capacity: count);

			for (int n = 0; n < count; n++)
			{
				itemsToAdd.Add(new ItemTemplateContext(_itemTemplate, newItems[n], _container, _itemHeight, _itemWidth, _itemSpacing, _mauiContext));
			}

			if (startIndex == Items.Count)
			{
				AddRange(itemsToAdd);
			}
			else
			{
				InsertRange(startIndex, itemsToAdd);
			}
		}

		void Move(NotifyCollectionChangedEventArgs args)
		{
			var count = args.NewItems.Count;

			if (args.OldStartingIndex > args.NewStartingIndex)
			{
				for (int n = 0; n < count; n++)
				{
					Move(args.OldStartingIndex + n, args.NewStartingIndex + n);
				}

				return;
			}

			for (int n = count - 1; n >= 0; n--)
			{
				Move(args.OldStartingIndex + n, args.NewStartingIndex + n);
			}
		}

		void Remove(NotifyCollectionChangedEventArgs args)
		{
			var startIndex = args.OldStartingIndex;

			if (startIndex < 0)
			{
				// INCC implementation isn't giving us enough information to know where the removed items were in the
				// collection. So the best we can do is a full Reset.
				Reset();
				return;
			}

			var count = args.OldItems.Count;

			if (count == 1)
			{
				Remove(Items[startIndex]);
				return;
			}

			List<ItemTemplateContext> itemsToRemove = new(capacity: count);

			for (int n = startIndex + count - 1; n >= startIndex; n--)
			{
				var itemToRemve = Items[n];
				itemsToRemove.Add(itemToRemve);
			}

			RemoveRange(itemsToRemove);
		}

		void Replace(NotifyCollectionChangedEventArgs args)
		{
			var newItemCount = args.NewItems.Count;

			if (newItemCount == args.OldItems.Count)
			{
				var items = Items.ToList();

				for (int n = 0; n < newItemCount; n++)
				{
					var index = args.OldStartingIndex + n;
					var newItem = new ItemTemplateContext(_itemTemplate, args.NewItems[n], _container, _itemHeight, _itemWidth, _itemSpacing, _mauiContext);
					items[index] = newItem;
				}

				ReplaceRange(args.OldStartingIndex, items);
			}
			else
			{
				// If we're replacing one set with an equal size set, we can do a soft reset; if not, we have to completely
				// rebuild the collection
				Reset();
			}
		}

		void Reset()
		{
			Items.Clear();
			for (int n = 0; n < _itemsSource.Count; n++)
			{
				Items.Add(new ItemTemplateContext(_itemTemplate, _itemsSource[n], _container, _itemHeight, _itemWidth, _itemSpacing, _mauiContext));
			}

			var reset = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
			OnCollectionChanged(reset);
		}
	}
}