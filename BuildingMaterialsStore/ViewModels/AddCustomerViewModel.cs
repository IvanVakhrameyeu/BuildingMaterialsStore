using BuildingMaterialsStore.Models;
using System;
using System.Windows;
using System.Windows.Input;

namespace BuildingMaterialsStore.ViewModels
{
    class AddCustomerViewModel : ViewModel
    {
        private string _custLastName;
        private string _custFirstName;
        private string _custPatronymic;
        private string _sex;
        private DateTime _custDateOfBirth;
        private string _custAddress;
        private string _custPhoneNumber;

        public string CustLastName
        {
            get
            {
                return _custLastName;
            }
            set
            {
                _custLastName = value;
            }
        }
        public string CustFirstName
        {
            get
            {
                return _custFirstName;
            }
            set
            {
                _custFirstName = value;
            }
        }
        public string CustPatronymic
        {
            get
            {
                return _custPatronymic;
            }
            set
            {
                _custPatronymic = value;
            }
        }
        public string Sex
        {
            get
            {
                return _sex;
            }
            set
            {                
                _sex = value;
            }
        }
        public DateTime CustDateOfBirth
        {
            get
            {
                return _custDateOfBirth;
            }
            set
            {
                if (((DateTime.Today.Day - value.Day) / 365) < 18)
                {
                    MessageBox.Show("ВАМ МЕНЬШЕ 18!!!!АДЫН");
                    return;
                }
                _custDateOfBirth = value;
            }
        }
        public string CustAddress
        {
            get
            {
                return _custAddress;
            }
            set
            {
                _custAddress = value;
            }
        }
        public string CustPhoneNumber
        {
            get
            {
                return _custPhoneNumber;
            }
            set
            {
                _custPhoneNumber = value;
            }
        }

        public ICommand QuitAplicationCommand { get; }
        public ICommand AddCommand { get; }

        public AddCustomerViewModel()
        {
            QuitAplicationCommand = new DelegateCommand(CloseExcute);
            AddCommand = new DelegateCommand(Add);

            
        }
        private void Add(object t)
        {

            foreach (Window item in Application.Current.Windows)
            {
                if (item.ToString() == "BuildingMaterialsStore.Views.WindowAddCustomer")
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
                if (item.ToString() == "BuildingMaterialsStore.Views.WindowAddCustomer")
                    item.Close();
            }
        }
    }
}
