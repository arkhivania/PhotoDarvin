using System.IO;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

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
            var target = Path.Combine(targetFolder, Path.GetFileName(explorerOperatingState.SelectedPhoto.FilePath));
            var targetFileName = target;
            
            int index = 1;
            while (File.Exists(targetFileName) && !TheSame(explorerOperatingState.SelectedPhoto.FilePath, targetFileName))
                targetFileName = CorrectTarget(target, index++);
            File.Copy(explorerOperatingState.SelectedPhoto.FilePath, targetFileName, true);
            Console.Beep(400, 100);
        }

        private bool TheSame(string path1, string path2)
        {
            var md5 = MD5.Create();
            using (var fs1 = new FileStream(path1, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var fs2 = new FileStream(path1, FileMode.Open, FileAccess.Read, FileShare.Read))
                return md5.ComputeHash(fs1).SequenceEqual(md5.ComputeHash(fs2));
        }

        private string CorrectTarget(string filePath, int index)
        {
            var dirName = Path.GetDirectoryName(filePath);
            var fileWOExt = Path.GetFileNameWithoutExtension(filePath);
            var ext = Path.GetExtension(filePath);
            return Path.Combine(dirName, string.Format("{0}({1}){2}", fileWOExt, index, ext));
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
