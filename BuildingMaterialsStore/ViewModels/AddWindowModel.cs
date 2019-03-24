using BuildingMaterialsStore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BuildingMaterialsStore.ViewModels
{
    class AddWindowModel : ViewModel
    {
        private int _countPurchases = 0;
        private string _currentSection = null;
        private string _name = null;
        private string _description = null;
        private double _price = 0;
        private double _totalCost = 1;
        


        private Purchases purchases;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
            }
        }
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        public int CountPurchases
        {
            get { return _countPurchases; }
            set {
                _countPurchases = value;
                 Total();
                //Change();
            }
        }
        public double TotalCost
        {
            get { return _totalCost; }
            set {
                _totalCost = value;
            }
        }
        public double Price
        {
            get { return _price; }
            set { _price = value; }
        }
        public string CurrentSection
        {
            get { return _currentSection; }
            set
            {
                _currentSection = value;
            }
        }


        //public delegate void ChangeCountPurchases();
        //public event ChangeCountPurchases Change;

        public ICollectionView view { get; set; }
        public ICommand QuitAplicationCommand { get; }
        public ICommand AddCommand { get; }

        public AddWindowModel(Purchases purchases,string NameCategory, string Name, string Description, double Price)
        {
            this.CurrentSection = NameCategory;
            this.Name = Name;
            this.Description = Description;
            this.Price = Price;
            CountPurchases = 1;
            QuitAplicationCommand = new DelegateCommand(CloseExcute);
            AddCommand = new DelegateCommand(Add);
            this.purchases = purchases;
            OnPropertyChanged("CountPurchases");
            OnPropertyChanged("TotalCost");
            //Change += Total;
        }
        private void Total()
        {
                TotalCost = Convert.ToDouble(CountPurchases) * Price;
        }
        private void Add(object t)
        {
            purchases.Count = Convert.ToInt32(CountPurchases);
            purchases.Total = Convert.ToDouble(CountPurchases) * Price ;
            foreach (Window item in Application.Current.Windows)
            {
                if (item.ToString() == "BuildingMaterialsStore.Views.WindowAddPurchase")
                    item.Close();
            }

        }
        private void CloseExcute(object t)
        {
            foreach (Window item in Application.Current.Windows)
            {
                if (item.ToString() == "BuildingMaterialsStore.Views.WindowAddPurchase")
                    item.Close();
            }
        }
    }
}
