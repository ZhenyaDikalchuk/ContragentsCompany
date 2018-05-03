using System;
using System.Data.SQLite;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ContragentsCompany.Forms.InfoContragents
{
    /// <summary>
    /// Interaction logic for ContragentInfoForm.xaml
    /// </summary>
    public partial class ContragentInfoForm : Window
    {
        private const string databaseName = @"Resources\Database\contractorsCopy_v1.db";
        private SQLiteConnection connection;
        private SQLiteCommand command;
        private SQLiteDataReader dataReader;
        private string map = "";
        private ContragentChangeForm contragentChange = new ContragentChangeForm();

        public ContragentInfoForm()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Connection
            connection = new SQLiteConnection(string.Format("Data Source={0}; Version=3", databaseName));
            try
            {
                connection.Open();
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }

            //EDRPOU to CompanyName
            if (tbComName.Text != "")
            {
                command = new SQLiteCommand("select CompanyName from Company where EDRPOU = '" + tbComName.Text + "'", connection);
                try
                {
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        tbComName.Text = dataReader["CompanyName"].ToString();
                    }
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            //Boss to CompanyName
            if (tbComName.Text != "")
            {
                command = new SQLiteCommand("select CompanyName from Company where Boss = '" + tbComName.Text + "'", connection);
                try
                {
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        tbComName.Text = dataReader["CompanyName"].ToString();
                    }
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            string cName = "", rName = "";

            //get Company Short Name
            tbShortName.Clear();
            command = new SQLiteCommand("select CompanyShortName from Company where Company.CompanyName = '" + tbComName.Text + "'", connection);
            try
            {
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    tbShortName.Text = dataReader["CompanyShortName"].ToString();
                }
                if (tbShortName.Text == "") tbShortName.Text = "Данні відсутні";
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }

            //get Company EDRPOU
            tbEDRPOU.Clear();
            command = new SQLiteCommand("select EDRPOU from Company where Company.CompanyName = '" + tbComName.Text + "'", connection);
            try
            {
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    tbEDRPOU.Text = dataReader["EDRPOU"].ToString();
                }
                if (tbEDRPOU.Text == "") tbEDRPOU.Text = "Данні відсутні";
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }

            //get Company Address
            tbAddress.Clear();
            FileInfo file = new FileInfo("Resources/Queries/address.sql");
            string script = file.OpenText().ReadToEnd();
            command = new SQLiteCommand(script + " where Company.CompanyName = '" + tbComName.Text + "'", connection);
            try
            {
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    tbAddress.Text = (dataReader["RegionName"] + ", " + dataReader["RayonName"] +
                        ", " + dataReader["CityName"] + ", " + dataReader["StreetName"] +
                        ", " + dataReader["BuildNumb"] + ", " + dataReader["Postcode"]);
                    rName = dataReader["RegionName"].ToString();
                    cName = dataReader["CityName"].ToString();
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }

            //get Map Info
            command = new SQLiteCommand("select MapInfo from City inner join Region on Region.id = City.id_Region where Region.RegionName like '%" + rName +
                        "%' and City.CityName like '%" + cName + "%'", connection);
            try
            {
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    map = dataReader["MapInfo"].ToString();
                    break;
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (map != "") bMap.Visibility = Visibility.Visible;

            //get Company Boss
            tbBoss.Clear();
            command = new SQLiteCommand("select BOSS from Company where Company.CompanyName = '" + tbComName.Text + "'", connection);
            try
            {
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    tbBoss.Text = dataReader["BOSS"].ToString();
                }
                if (tbBoss.Text == "") tbBoss.Text = "Данні відсутні";
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }

            //get Company Kved
            tbKVED.Clear();
            command = new SQLiteCommand("select KVED from Company where Company.CompanyName = '" + tbComName.Text + "'", connection);
            try
            {
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    tbKVED.Text = dataReader["KVED"].ToString();
                }
                if (tbKVED.Text == "") tbKVED.Text = "Данні відсутні";
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }

            //get Company Stan
            tbStan.Clear();
            command = new SQLiteCommand("select StanInfo from Stan inner join Company on Company.id_Stan = Stan.id where Company.CompanyName = '" + tbComName.Text + "'", connection);
            try
            {
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    tbStan.Text = dataReader["StanInfo"].ToString();
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }

            lbFounders.Items.Clear();
            command = new SQLiteCommand("select FounderInfo, Price from Founder inner join Company on Company.id = Founder.id_Company where Company.CompanyName = '" + tbComName.Text + "'", connection);
            try
            {
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    lbFounders.Items.Add(dataReader["FounderInfo"] + ", розмiр внеску до статутного фонду - " + dataReader["Price"]);
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);      
            }

            command = new SQLiteCommand("select CompanyName from Company where Company.CompanyName = '" + tbComName.Text + "'", connection);
            try
            {
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    tbComName.Text = dataReader["CompanyName"].ToString();
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void bMap_Click(object sender, RoutedEventArgs e)
        {
            ContragentMapForm mapForm = new ContragentMapForm();
            FileInfo file = new FileInfo("Resources/Images/" + map);
            string script = file.FullName.ToString();
            BitmapImage b = new BitmapImage();
            b.BeginInit();
            b.UriSource = new Uri(script);
            b.EndInit();
            mapForm.image.Source = b;
            mapForm.Show();
        }

        private void bСhange_Click(object sender, RoutedEventArgs e)
        {
            contragentChange.tbComName.Text = tbComName.Text;
            contragentChange.Show();
            this.Hide();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
        }
    }
}
