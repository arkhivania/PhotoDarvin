using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace OSWrap.Windows
{
    public class Dialogs : OSWrap.IDialogs
    {
        private readonly Window parent;

        public Dialogs(Window parent)
        {
            this.parent = parent;
        }

        public string SelectFolder(string initialFolder)
        {
            var sfd = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            sfd.SelectedPath = initialFolder;
            if(sfd.ShowDialog(parent) == true)
                return sfd.SelectedPath;

            return null;
        }
    }
}
