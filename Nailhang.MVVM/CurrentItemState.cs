using System;
using System.ComponentModel;

namespace Nailhang.MVVM
{
    public interface ICurrentItemState
    {
        object ItemObject { get; set; }
        event EventHandler ItemObjectChanged;
    }

    public interface ICurrentItemState<T> : ICurrentItemState
    {
        T Item { get; set; }
        event EventHandler ItemChanged;

        T Value { get; set; }
        event EventHandler ValueChanged;
    }

    public class CurrentItemState<T> : ICurrentItemState, ICurrentItemState<T>, INotifyPropertyChanged
    {
        public CurrentItemState()
        {
            ItemChanged += CurrentItemState_ItemChanged;
        }

        private void CurrentItemState_ItemChanged(object sender, EventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("Item"));
                PropertyChanged(this, new PropertyChangedEventArgs("Value"));
            }
        }

        public delegate void EventHandlerEx(object sender, T oldItem, T newItem);

        T item;

        public T Item
        {
            get { return item; }
            set
            {
                var oldItem = item;

                if (item is IComparable)
                    if (((IComparable)item).CompareTo(value) == 0)
                        return;

                if (ItemChanging != null)
                    ItemChanging(this, EventArgs.Empty);

                item = value;

                if (ItemChanged != null)
                    ItemChanged(this, EventArgs.Empty);
                if (ItemChangedEx != null)
                    ItemChangedEx(this, oldItem, item);
                if (ItemChangedAction != null)
                    ItemChangedAction();
            }
        }

        public Action ItemChangedAction { get; set; }

        public event EventHandler ItemChanging;
        public event EventHandler ItemChanged;
        public event EventHandlerEx ItemChangedEx;
        public event PropertyChangedEventHandler PropertyChanged;

        protected void PerformPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #region ICurrentItemState Members

        object ICurrentItemState.ItemObject
        {
            get
            {
                return item;
            }
            set
            {
                Item = (T)value;
            }
        }

        event EventHandler ICurrentItemState.ItemObjectChanged
        {
            add { ItemChanged += value; }
            remove { ItemChanged -= value; }
        }

        #endregion

        #region ICurrentItemState<T> Members


        public T Value { get { return Item; } set { Item = value; } }

        public event EventHandler ValueChanged 
        {
            add { this.ItemChanged += value; } 
            remove { this.ItemChanged -= value; } 
        }

        #endregion
    }

    public class CurrentItemStateAdaptor<TResult, TSource> : CurrentItemState<TResult>, IDisposable
    {
        readonly CurrentItemState<TSource> sourceState;
        readonly Func<TSource, TResult> direct;

        private readonly Func<TResult, TSource> reverse;

        public CurrentItemStateAdaptor(CurrentItemState<TSource> sourceState, Func<TSource, TResult> direct) : this(sourceState, direct, null)
        {
        }

        public CurrentItemStateAdaptor(CurrentItemState<TSource> sourceState, Func<TSource, TResult> direct, Func<TResult, TSource> reverse)
        {
            this.reverse = reverse;
            this.direct = direct;
            this.sourceState = sourceState;

            this.Item = direct(sourceState.Item);

            sourceState.ItemChangedEx += new CurrentItemState<TSource>.EventHandlerEx(sourceState_ItemChangedEx);
            ItemChanged += new EventHandler(CurrentItemStateAdaptor_ItemChanged);
        }

        void CurrentItemStateAdaptor_ItemChanged(object sender, EventArgs e)
        {
            if (reverse != null)
            {
                sourceState.ItemChangedEx += new CurrentItemState<TSource>.EventHandlerEx(sourceState_ItemChangedEx);
                sourceState.Item = reverse(Item);
                sourceState.ItemChangedEx -= new CurrentItemState<TSource>.EventHandlerEx(sourceState_ItemChangedEx);
            }
        }

        void sourceState_ItemChangedEx(object sender, TSource oldItem, TSource newItem)
        {
            ItemChanged -= new EventHandler(CurrentItemStateAdaptor_ItemChanged);
            this.Item = direct(sourceState.Item);
            ItemChanged += new EventHandler(CurrentItemStateAdaptor_ItemChanged);
        }

        #region IDisposable Members

        public void Dispose()
        {
            ItemChanged -= new EventHandler(CurrentItemStateAdaptor_ItemChanged);
            sourceState.ItemChangedEx -= new CurrentItemState<TSource>.EventHandlerEx(sourceState_ItemChangedEx);
        }

        #endregion
    }
}
