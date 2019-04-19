using BuildingMaterialsStore.Models;
using BuildingMaterialsStore.Views.Pages;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BuildingMaterialsStore.ViewModels
{
    class MainAdminViewModel : ViewModel
    {
        public ICommand QuitAplicationCommand { get; } = new DelegateCommand(p => Application.Current.Shutdown());
        private WindowState _currentSate;
        public ICommand HelpAplicationCommand{get;}

        public ICommand WindowStateCommand { get; }
        private Page _currentPage;
        public Page CurrentPage
        {
            get { return _currentPage; }
            set
            {
                _currentPage = value;
                OnPropertyChanged("CurrentPage");
            }
        }
        public WindowState CurrentWindowState
        {
            get { return _currentSate; }
            set
            {
                _currentSate = value;
                OnPropertyChanged("CurrentWindowState");
            }
        }
        private void OnCurrentWindowState(object p)
        {
            CurrentWindowState = WindowState.Minimized;
        }
        private Page CustomersPage;
        private Page EmployeePage;
        private Page RepPage;
        public MainAdminViewModel()
        {
            HelpAplicationCommand = new DelegateCommand(OnHelpCommandExecuted);

            EmployeePage = new MainAdminPage();
            CustomersPage = new CustomerPage ();
            RepPage = new ReportsPage ();

            CurrentPage = EmployeePage;
            WindowStateCommand = new DelegateCommand(OnCurrentWindowState);
        }
        private void OnHelpCommandExecuted(object o)
        {
            try
            {
                Process.Start("Help.chm");
            }
            catch { MessageBox.Show("Справка не найдена"); }
        }
        private int _selectedIndex = -1;
        public int SeletedIndex
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                if (_selectedIndex == value) return;
                _selectedIndex = value;
                LoadedPage(_selectedIndex);
            }
        }
        private void LoadedPage(int i)
        {
            switch (i)
            {
                case 0: { CurrentPage = EmployeePage; break; }
                case 1: { CurrentPage = CustomersPage; break; }
                case 2: { CurrentPage = RepPage; break; }
                default: { CurrentPage = null; break; }
            }
        }
    }
}

