using System.IO;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photo.FolderStore.Tool.ViewModel
{
    class TargetFolderViewModel : IDisposable
    {
        private readonly Photo.SourceExplorer.Base.OperatingState explorerOperatingState;

        private readonly OSWrap.IDialogs dialogs;

        readonly DelegateCommand selectTargetFolderCommand;

        public DelegateCommand PutToFolderCommand
        {
            get
            {
                return this.putToFolderCommand;
            }
        }

        public DelegateCommand SelectTargetFolderCommand
        {
            get { return selectTargetFolderCommand; }
        }

        readonly DelegateCommand putToFolderCommand;

        string targetFolder;


        public TargetFolderViewModel(Photo.SourceExplorer.Base.OperatingState explorerOperatingState, OSWrap.IDialogs dialogs)
        {
            this.explorerOperatingState = explorerOperatingState;
            this.dialogs = dialogs;
            selectTargetFolderCommand = new DelegateCommand(SelectTargetCommand);
            putToFolderCommand = new DelegateCommand(PutToFolder, () => !string.IsNullOrEmpty(targetFolder) && explorerOperatingState.SelectedPhoto != null);

            explorerOperatingState.SelectedPhotoChanged += explorerOperatingState_SelectedPhotoChanged;
        }

        void explorerOperatingState_SelectedPhotoChanged(object sender, EventArgs e)
        {
            putToFolderCommand.RaiseCanExecuteChanged();
        }
 
        private void PutToFolder()
        {
            var targetFileName = Path.Combine(targetFolder, Path.GetFileName(explorerOperatingState.SelectedPhoto.FilePath));
            File.Copy(explorerOperatingState.SelectedPhoto.FilePath, targetFileName, true);
        }

        void SelectTargetCommand()
        {
            var folder = dialogs.SelectFolder(targetFolder);
            if (folder != null)
                targetFolder = folder;

            putToFolderCommand.RaiseCanExecuteChanged();
        }

        public void Dispose()
        {
            explorerOperatingState.SelectedPhotoChanged -= explorerOperatingState_SelectedPhotoChanged;
        }
    }
}
