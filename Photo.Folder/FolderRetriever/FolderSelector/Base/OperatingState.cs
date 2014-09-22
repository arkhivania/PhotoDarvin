using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Photo.Folder.FolderRetriever.FolderSelector.Base
{
    public class OperatingState
    {
        string folder;

        public string Folder
        {
            get { return folder; }
            set 
            {
                folder = value;
                if (FolderChanged != null)
                    FolderChanged(this, EventArgs.Empty);
            }
        }

        public event EventHandler FolderChanged;
    }
}
