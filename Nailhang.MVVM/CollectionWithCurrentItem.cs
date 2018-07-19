using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Nailhang.MVVM
{
    public interface ICollectionWithCurrentItem : ICurrentItemState
    {
        IEnumerable Items { get; }
        event EventHandler ItemsChanged;
        void Clear();
        void SetItems(IEnumerable items);
    }

    public interface ICollectionWithCurrentItem<T> : ICurrentItemState<T>, ICollectionWithCurrentItem
    {
        new IEnumerable<T> Items { get; }
        void AddItems(IEnumerable<T> items);
    }

    public class CollectionWithCurrentItem<T> :
        CurrentItemState<T>, ICollectionWithCurrentItem<T>, IEnumerable<T>
    {
        public CollectionWithCurrentItem()
        {
            ItemsChanged += new EventHandler(CollectionWithCurrentItem_ItemsChanged);
        }

        void CollectionWithCurrentItem_ItemsChanged(object sender, EventArgs e)
        {
            base.PerformPropertyChanged("Items");
        }

        readonly List<T> items = new List<T>();

        public IEnumerable<T> Items
        {
            // такая форма записи для WPF чтобы возвращался как бы новый объект перечисления
            get { return items.Select(w => w); } 
        }

        public event EventHandler ItemsChanged;

        public int Count { get { return items.Count; } }

        public void AddItems(IEnumerable<T> items)
        {
            this.items.AddRange(items);
            if (ItemsChanged != null)
                ItemsChanged(this, EventArgs.Empty);
        }

        public void AddItem(T item)
        {
            items.Add(item);
            if (ItemsChanged != null)
                ItemsChanged(this, EventArgs.Empty);
        }

        public void RemoveItem(T item)
        {
            items.Remove(item);
            if (ItemsChanged != null)
                ItemsChanged(this, EventArgs.Empty);
        }

        public void RemoveItems(IEnumerable<T> items)
        {
            foreach(var i in items)
                this.items.Remove(i);
            if (ItemsChanged != null)
                ItemsChanged(this, EventArgs.Empty);
        }

        public void Clear()
        {
            items.Clear();
            if (ItemsChanged != null)
                ItemsChanged(this, EventArgs.Empty);
        }

        public void Sort(Comparison<T> compare)
        {
            items.Sort(compare);
            if (ItemsChanged != null)
                ItemsChanged(this, EventArgs.Empty);
        }

        #region ICollectionWithCurrentItem Members

        IEnumerable ICollectionWithCurrentItem.Items
        {
            get { return items; }
        }

        event EventHandler ICollectionWithCurrentItem.ItemsChanged
        {
            add { this.ItemsChanged += value; }
            remove { this.ItemsChanged -= value; }
        }

        #endregion


        #region IEnumerable<T> Members

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return items.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }

        #endregion        
    
        #region ICollectionWithCurrentItem Members


        public void SetItems(IEnumerable items)
        {
            this.items.Clear();
            this.items.AddRange(items.OfType<T>());

            if (ItemsChanged != null)
                ItemsChanged(this, EventArgs.Empty);
        }

        #endregion
    }
}
