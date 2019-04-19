using BuildingMaterialsStore.ViewModels.Pages;
using System;
using System.Windows;
using System.Windows.Controls;

namespace BuildingMaterialsStore.Views.Pages
{
    public partial class GoodsIssue : Page
    {
        public GoodsIssue()
        {
            try
            {
                InitializeComponent();
                DataContext = new GoodsIssueVM();
            }
            catch(Exception ex) {
                //MessageBox.Show(ex.Message);
            }
        }
    }
}
