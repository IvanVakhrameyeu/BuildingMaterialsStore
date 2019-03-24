using BuildingMaterialsStore.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace BuildingMaterialsStore.ViewModels
{
    class PurchasesViewModel : ViewModel
    {
        static String connectionString = @"Data Source=DESKTOP-R50QS4G;Initial Catalog=Storedb;Integrated Security=True";
        SqlConnection con;
        private SqlCommand com;
        SqlCommand cmd;
        SqlDataAdapter adapter;
        DataSet ds;
        public ObservableCollection<Purchases> purchases;
        public ICollectionView view { get; set; }
        private string _nameCategory = null;
        private string _name = null;
        private string _unitName = null;
        private int _countPurchases = 0;
        private double _price = 0;
        private double _amountPrice = 0;
        private string _description = null;
        private string _text;
        public string Text // конечная сумма
        {
            get { return _text; }
            set
            {
                if (_text == value) return;
                _text = value;
                if (_text == null) return;
                Total();
            }
        }
        public string NameCategory
        {
            get { return _nameCategory; }
            set
            {
                _nameCategory = value;
            }
        }
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
            }
        }
        public string UnitName
        {
            get { return _unitName; }
            set
            {
                _unitName = value;
            }
        }
        public int CountPurchases
        {
            get { return _countPurchases; }
            set { _countPurchases = value; }
        }
        public double Price
        {
            get { return _price; }
            set { _price = value; }
        }
        public double AmountPrice
        {
            get { return _amountPrice; }
            set { _amountPrice = value; }
        }
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public ICommand QuitAplicationCommand { get; }
        public ICommand FinishCommand { get; }
        public PurchasesViewModel(List<Purchases> purchases)
        {
            QuitAplicationCommand = new DelegateCommand(CloseExcute);
            //FinishCommand = new DelegateCommand(CloseExcute);
            //  ShoppingBasket = new DelegateCommand(ChangePage, OnClearFilterCommandCanExecuted);
            //this.purchases = purchases;
            //foreach (Purchases pr in purchases)
            //{
            //    this.purchases.Add(pr);
            //}
            //FillListPurchases();

        }
        private bool OnClearFilterCommandCanExecuted(object Select)
        {
            return true;
            // return !string.IsNullOrEmpty(SelectItem) || !string.IsNullOrEmpty(Text);
        }
        private void Total()
        {


        }
        public void FillListPurchases(List<Purchases> purchases2)
        {
            try
            {
                //foreach (Purchases dr in purchases)
                //{
                //    purchases.Add(new Purchases
                //    {
                //        Count = dr.Count,
                //        storage = (new Storage
                //        {
                //            idStorage = dr.storage.idStorage,
                //            NameCategory = dr.storage.NameCategory,
                //            UnitName = dr.storage.UnitName,
                //            Name = dr.storage.Name,
                //            Price = dr.storage.Price,
                //            Description = dr.storage.Description
                //        }),
                //        Total = (dr.Count*dr.storage.Price)
                //    });
                //}
            }
            catch (Exception ex)
            {

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
