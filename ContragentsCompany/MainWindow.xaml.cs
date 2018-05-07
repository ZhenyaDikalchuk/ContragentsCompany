using System.Data.SQLite;
using System.Diagnostics;
using System.Windows;
using ContragentsCompany.Forms.CreateContragents;
using ContragentsCompany.Forms.DataGraphics;
using ContragentsCompany.Forms.InfoContragents;
using ContragentsCompany.Forms.SearchContragents;

namespace ContragentsCompany
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //open List Contragents Form
        private void bCompanyName_Click(object sender, RoutedEventArgs e)
        {
            ContragentListForm contragentList = new ContragentListForm();
            contragentList.Show();
            this.Hide();
        }

        //open Create Record Contragents Form
        private void bCreateRecord_Click(object sender, RoutedEventArgs e)
        {
            ContragentCreateForm contragentCreate = new ContragentCreateForm();
            contragentCreate.Show();
            this.Hide();
        }

        //open Search Contragents Form
        private void bSearch_Click(object sender, RoutedEventArgs e)
        {
            ContragentSearchForm contragentSearch = new ContragentSearchForm();
            contragentSearch.Show();
            this.Hide();
        }

        //open Data Graphics Form
        private void bGraphics_Click(object sender, RoutedEventArgs e)
        {
            DataGraphicsForm dataGraphics = new DataGraphicsForm();
            dataGraphics.Show();
            this.Hide();
        }

        ////close program
        private void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
            Process.GetCurrentProcess().Kill();
        }
    }   
}
