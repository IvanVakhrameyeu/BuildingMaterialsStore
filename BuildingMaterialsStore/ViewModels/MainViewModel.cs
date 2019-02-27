using BuildingMaterialsStore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BuildingMaterialsStore.ViewModels
{
    class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        private int _Clicks;
        public int Clicks
        {
            get { return _Clicks; }
            set
            {
                _Clicks = value;
                OnPropertyChanged();
            }
        }
        public MainViewModel()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Task.Delay(1000).Wait();
                    Clicks++;
                }
            });
        }
        public ICommand ClickAdd
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    Clicks++;
                },(obj)=> Clicks<50);
            }
        }
        public bool passwordCheck()
        {
            bool result=false;

            return result;
        }
            
        public ICommand SignIn
        {
            get
            {
                return new DelegateCommand((obj)=>
                {
                    passwordCheck();
                    MessageBox.Show("SIGNIN");
                });
            }
        }
        public ICommand Exit
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    Setting.BSignIn = false;
                    MessageBox.Show("EXIT");
                });
            }
        }
    }
}
