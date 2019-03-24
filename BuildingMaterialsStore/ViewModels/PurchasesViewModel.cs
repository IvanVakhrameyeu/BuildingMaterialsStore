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
        //static String connectionString = @"Data Source=DESKTOP-R50QS4G;Initial Catalog=Storedb;Integrated Security=True";
        //SqlConnection con;
        //private SqlCommand com;
        //SqlCommand cmd;
        //SqlDataAdapter adapter;
        //DataSet ds;
        public ObservableCollection<Purchases> purchases;
        public ICollectionView view { get; set; }
        private Purchases _selectItemDataGrid = null;
        public Purchases SelectItemDataGrid
        {
            get { return _selectItemDataGrid; }
            set
            {
                if (_selectItemDataGrid == value)
                    return;

                _selectItemDataGrid = value;
            }
        }
        public ICommand QuitAplicationCommand { get; }
        public ICommand FinishCommand { get; }
        private double _inTotal = 0;
        public double InTotal
        {
            get { return _inTotal; }
            set { _inTotal = value; }
        }
        public PurchasesViewModel(List<Purchases> InputPurchases)
        {
            QuitAplicationCommand = new DelegateCommand(CloseExcute);
            MessageBox.Show(InputPurchases[0].idstorage.ToString());
            FillListPurchases(InputPurchases);
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
        
        public void FillListPurchases(List<Purchases> InputPurchases)
        {
            try
            {
                if (purchases == null)
                    purchases = new ObservableCollection<Purchases>();

                foreach (Purchases dr in InputPurchases)
                {
                    purchases.Add(new Purchases
                    {
                        idstorage = dr.idstorage,
                        Count = dr.Count,
                        //storage = (new Storage
                        //{
                        //    idStorage = dr.storage.idStorage,
                        //    NameCategory = dr.storage.NameCategory,
                        //    UnitName = dr.storage.UnitName,
                        //    Name = dr.storage.Name,
                        //    Price = dr.storage.Price,
                        //    Description = dr.storage.Description
                        //}),
                        Total = dr.Total// (dr.Count * dr.storage.Price)
                    });
                    InTotal += dr.Total;
                }
                view = CollectionViewSource.GetDefaultView(purchases);
            }
            catch (Exception ex)
            {

            }
        }
        private void CloseExcute(object t)
        {
            foreach (Window item in Application.Current.Windows)
            {
                if (item.ToString() == "BuildingMaterialsStore.Views.WindowCustomerPurchases")
                    item.Close();
            }
        }
    }
}
