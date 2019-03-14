namespace BuildingMaterialsStore
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow() => InitializeComponent();
               
        private void DockPanel_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) => DragMove();

    }
}
