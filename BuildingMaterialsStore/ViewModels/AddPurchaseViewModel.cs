using BuildingMaterialsStore.Models;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace BuildingMaterialsStore.ViewModels
{
    class AddPurchaseViewModel : ViewModel
    {
        private int _countPurchases = 0;
        private string _currentSection = null;
        private string _name = null;
        private string _description = null;
        private double _price = 0;
        private int _amountGoods = 0;
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
            set
            {
                _countPurchases = value;
                Total();
            }
        }
        public double TotalCost
        {
            get { return _totalCost; }
            set
            {
                _totalCost = value;
            }
        }
        public int AmountGoods
        {
            get { return _amountGoods; }
            set { _amountGoods = value; }
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
        public ICollectionView view { get; set; }
        public ICommand QuitAplicationCommand { get; }
        public ICommand AddCommand { get; }
        public AddPurchaseViewModel(Purchases purchases, int AmountGoods)
        {
            this.CurrentSection = purchases.storage.NameCategory;
            this.Name = purchases.storage.Name; 
            this.Description = purchases.storage.Description;
            this.Price = purchases.storage.Price;
            this.AmountGoods = AmountGoods;
            CountPurchases = 1;
            QuitAplicationCommand = new DelegateCommand(CloseExcute);
            AddCommand = new DelegateCommand(Add);
            this.purchases = purchases;
        }
        /// <summary>
        /// изменение общей стоимости товара
        /// </summary>
        private void Total() // фиксануть отсутствие ссылки в purchases при начальном запуске
        {
            try
            {
                TotalCost = Math.Round(Convert.ToDouble(CountPurchases) * Price - ((Convert.ToDouble(CountPurchases) * Price) * purchases.CurrentDiscountAmount / 100), 2);
                OnPropertyChanged("TotalCost");
            }
            catch (Exception ex) {
            //    MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        ///проверяется, есть ли уже такой товар в корзине
        ///0 если это копия и при этом попытка купить больше чем есть
        ///1 если копия, но можно купить сколько указано
        /// 2 если новый товар
        /// </summary>
        private int isCopyGoods()            
        {
            foreach (Purchases pr in StorageViewModel.purchases)
            {
                if (purchases.storage.Equals(pr.storage))
                {
                    if ((purchases.Count + pr.Count) > AmountGoods)
                    {
                        MessageBox.Show("На складе не достаточно материалов, вы можете купить: " + (AmountGoods - pr.Count).ToString()+ " товаров");
                        purchases.Count = 0;
                        return 0;
                    }
                    else
                    {
                        pr.Count += purchases.Count;
                        pr.Total = pr.Count * pr.storage.Price;
                        purchases.Count = 0;
                        purchases = null;
                        return 1;
                    }
                }
            }
            return 2;
        }
       /// <summary>
       /// добавление покупки с закрытием окна
       /// </summary>
       /// <param name="t"></param>
        private void Add(object t)
        {
            if (CountPurchases < 1)
            {
                MessageBox.Show("Нельзя купить меньше 1 товара"); return;
            }
            if (CountPurchases > AmountGoods)
            {
                MessageBox.Show("На складе не достаточно материалов, вы можете купить меньше"); return;
            }
            purchases.Count = Convert.ToInt32(CountPurchases);
            purchases.Total = Math.Round(Convert.ToDouble(CountPurchases) * Price-((Convert.ToDouble(CountPurchases) * Price)*purchases.CurrentDiscountAmount/100),2);            
            switch (isCopyGoods())
            {
                case 0: // если это копия и при этом попытка купить больше чем есть                
                    return;
                case 1: // если копия, но можно купить сколько указано
                    break;
                case 2: // если новый товар
                    purchases.Count = Convert.ToInt32(CountPurchases);
                    purchases.Total = Math.Round(Convert.ToDouble(CountPurchases) * Price - ((Convert.ToDouble(CountPurchases) * Price) * purchases.CurrentDiscountAmount / 100), 2);
                    break;
            }
            foreach (Window item in Application.Current.Windows)
            {
                if (item.ToString() == "BuildingMaterialsStore.Views.WindowAddPurchase")
                    item.Close();
            }
        }
        /// <summary>
        /// закрытие окна
        /// </summary>
        /// <param name="t">любой объект</param>
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
