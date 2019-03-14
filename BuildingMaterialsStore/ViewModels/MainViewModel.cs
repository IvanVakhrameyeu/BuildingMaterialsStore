using BuildingMaterialsStore.Models;
using BuildingMaterialsStore.Views.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BuildingMaterialsStore.ViewModels
{
    class MainViewModel : INotifyPropertyChanged
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
        private Page DecorationMaterialsPage;
        private Page GeneralConstructionPage;
        private Page RoofingMaterialsPage;
        private Page FacadeMaterialsPage;
        private Page ElectricianPage;
        private Page HardwareFastenersPage;
        private Page OvenMaterialsPage;
        private Page GardenPage;
        public MainViewModel()
        {
            MainStoragePage = new MainStorage("Главная");
            DecorationMaterialsPage = new MainStorage("Отделочные материалы");
            GeneralConstructionPage = new MainStorage("Общестроительные");
            RoofingMaterialsPage = new MainStorage("Кровельные материалы");
            FacadeMaterialsPage = new MainStorage("Фасадные материалы");
            ElectricianPage = new MainStorage("Электрика");
            HardwareFastenersPage = new MainStorage("Метизы и крепеж");
            OvenMaterialsPage = new MainStorage("Печные материалы");
            GardenPage = new MainStorage("Сад и огород");
            CurrentPage = MainStoragePage;
            WindowStateCommand = new DelegateCommand(OnCurrentWindowState);
        }
        #region обработчик нажатия кнопок
        public event PropertyChangedEventHandler PropertyChanged;
                       
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        #endregion


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
                case 1: { CurrentPage = DecorationMaterialsPage; break; }
                case 2: { CurrentPage = GeneralConstructionPage; break; }
                case 3: { CurrentPage = RoofingMaterialsPage; break; }
                case 4: { CurrentPage = FacadeMaterialsPage; break; }
                case 5: { CurrentPage = ElectricianPage; break; }
                case 6: { CurrentPage = HardwareFastenersPage; break; }
                case 7: { CurrentPage = OvenMaterialsPage; break; }
                case 8: { CurrentPage = GardenPage; break; }
                default: { CurrentPage = null; break; }
            }
        }
    }
}
