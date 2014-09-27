using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photo.SourceExplorer.PhotosList.ViewModel
{
    class PhotosListViewModel
    {
        readonly Base.OperatingState operatingState;

        public Base.OperatingState OperatingState
        {
            get { return operatingState; }
        } 


        public PhotosListViewModel(Base.OperatingState operatingState)
        {
            this.operatingState = operatingState;
        }
    }
}
