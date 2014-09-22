using System;
using System.Linq;
using Microsoft.Practices.Prism.PubSubEvents;
using OSWrap;
using Photo.Folder.FolderRetriever.FolderSelector.Base;
using Microsoft.Practices.Prism.Commands;

namespace Photo.Folder.FolderRetriever.FolderSelector.ViewModel
{
    class SelectorViewModel
    {
        private readonly Photo.Base.IPhotoSource photoSource;

        private readonly IEventAggregator eventAggregator;

        private readonly OSWrap.IDialogs dialogs;

        private readonly Base.OperatingState operatingState;

        readonly DelegateCommand selectFolderCommand;
        public DelegateCommand SelectFolderCommand
        {
            get { return selectFolderCommand; }
        }

        public SelectorViewModel(Photo.Base.IPhotoSource photoSource, IEventAggregator eventAggregator, OSWrap.IDialogs dialogs, Base.OperatingState operatingState)
        {   
            this.photoSource = photoSource;
            this.eventAggregator = eventAggregator;
            this.dialogs = dialogs;
            this.operatingState = operatingState;

            selectFolderCommand = new DelegateCommand(SelectFolder);
        }
 
        private void SelectFolder()
        {
            var path = dialogs.SelectFolder(null);
            if (path != null)
            {
                operatingState.Folder = path;
                eventAggregator.GetEvent<Photo.Base.SelectedSourceChangedEvent>().Publish(photoSource);
            }
        }
    }
}
