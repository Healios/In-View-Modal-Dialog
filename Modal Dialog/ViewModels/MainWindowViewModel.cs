using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;

namespace Modal_Dialog.ViewModels
{
    internal class MainWindowViewModel : BindableBase
    {
        // Constructor.
        public MainWindowViewModel()
        {
            AddItemCommand = new DelegateCommand(AddItemCommandExecute, AddItemCommandCanExecute);
            DeleteItemCommand = new DelegateCommand(DeleteItemCommandExecute, DeleteItemCommandCanExecute);
            DeleteItemRequest = new InteractionRequest<DeleteItemRequest>();
        }


        // Properties.
        private string itemName = "";
        public string ItemName
        {
            get { return itemName; }
            set { SetProperty(ref itemName, value); AddItemCommand.RaiseCanExecuteChanged(); }
        }

        public ObservableCollection<string> Items { get; set; } = new ObservableCollection<string>();

        private string selectedItem = "";
        public string SelectedItem
        {
            get { return selectedItem; }
            set { SetProperty(ref selectedItem, value); DeleteItemCommand.RaiseCanExecuteChanged(); }
        }

        public InteractionRequest<DeleteItemRequest> DeleteItemRequest { get; }
        

        // Commands.
        public DelegateCommand AddItemCommand { get; set; }
        private bool AddItemCommandCanExecute() => itemName.Length > 0 && !Items.Contains(itemName);
        private void AddItemCommandExecute()
        {
            Items.Add(itemName);
            AddItemCommand.RaiseCanExecuteChanged();
        }

        public DelegateCommand DeleteItemCommand { get; set; }
        private bool DeleteItemCommandCanExecute() => SelectedItem?.Length > 0;
        private void DeleteItemCommandExecute()
        {
            // Create a new request and set the Item property (it's used in the ConfirmDeleteView).
            DeleteItemRequest request = new DeleteItemRequest { Item = SelectedItem };

            // Bla.
            DeleteItemRequest.Raise(request, returned =>
            {
                if (!request.Confirmed) return;
                Items.Remove(SelectedItem);
            });

            // Refresh the "CanExecute" status of the "Add Item" button.
            AddItemCommand.RaiseCanExecuteChanged();
        }
    }
}