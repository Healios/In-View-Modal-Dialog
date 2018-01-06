using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;

namespace Modal_Dialog.ViewModels
{
    // We need to implement IInteractionRequestAware to be able to accept the sent request.
    internal class ConfirmDeleteViewModel : BindableBase, IInteractionRequestAware
    {
        // Constructor.
        public ConfirmDeleteViewModel()
        {
            ConfirmCommand = new DelegateCommand(ConfirmCommandExecute, ConfirmCommandCanExecute);
            CancelCommand = new DelegateCommand(CancelCommandExecute);
        }


        // Properties.
        private string itemToDelete = "";
        public string ItemToDelete
        {
            get { return itemToDelete; }
            set { SetProperty(ref itemToDelete, value); ConfirmCommand.RaiseCanExecuteChanged(); }
        }

        private string item = "";
        public string Item
        {
            get { return item; }
            set { SetProperty(ref item, value); }
        }


        // Commands.
        public DelegateCommand ConfirmCommand { get; set; }
        private bool ConfirmCommandCanExecute() => ItemToDelete.Equals(notification?.Item);
        private void ConfirmCommandExecute()
        {
            if (notification == null) return;
            notification.Confirmed = true;
            FinishInteraction();
        }

        public DelegateCommand CancelCommand { get; set; }
        private void CancelCommandExecute()
        {
            if (notification == null) return;
            notification.Confirmed = false;
            FinishInteraction();
        }


        // IInteractionRequestAware implementation.
        private DeleteItemRequest notification;
        public INotification Notification
        {
            get { return notification; }
            set
            {
                if (value is DeleteItemRequest) SetProperty(ref notification, value as DeleteItemRequest);
                Item = notification?.Item;
            }
        }
        public Action FinishInteraction { get; set; }
    }
}