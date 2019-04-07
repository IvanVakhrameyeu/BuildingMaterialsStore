using BuildingMaterialsStore.Models;
using BuildingMaterialsStore.Views.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BuildingMaterialsStore.ViewModels
{
    class MainAdminViewModel : ViewModel
    {
        public ICommand QuitAplicationCommand { get; } = new DelegateCommand(p => Application.Current.Shutdown());
        private WindowState _currentSate;
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
        private Page MainStoragePage;
        private Page CustomersPage;
        private Page EmployeePage;
        private Page StoragePage;
        public MainAdminViewModel()
        {
            MainStoragePage = new MainAdminPage("Работники");
            CustomersPage = new MainStorage("Отделочные материалы");
            EmployeePage = new MainStorage("Общестроительные");
            StoragePage = new MainStorage("Кровельные материалы");
            //FacadeMaterialsPage = new MainStorage("Фасадные материалы");

            CurrentPage = MainStoragePage;
            WindowStateCommand = new DelegateCommand(OnCurrentWindowState);
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
                case 0: { CurrentPage = MainStoragePage; break; }
                case 1: { CurrentPage = CustomersPage; break; }
                case 2: { CurrentPage = EmployeePage; break; }
                case 3: { CurrentPage = StoragePage; break; }
                default: { CurrentPage = null; break; }
            }
        }
    }
}

