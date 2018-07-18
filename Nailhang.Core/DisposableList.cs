using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nailhang.Core
{
    public class DisposableList : List<IDisposable>, IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public DisposableList()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        public DisposableList(IEnumerable<IDisposable> list)
        {
            AddRange(list);
        }

        protected void Push(IDisposable disposable)
        {
            Add(disposable);
        }

        #region IDisposable Members
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            foreach (var q in this)
                q.Dispose();
            Clear();
        }
        #endregion
    }
}
