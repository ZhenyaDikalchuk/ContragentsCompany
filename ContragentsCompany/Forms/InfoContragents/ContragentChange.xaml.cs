using System.Data.SQLite;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;

namespace ContragentsCompany.Forms.InfoContragents
{
    /// <summary>
    /// Interaction logic for ContragentChangeForm.xaml
    /// </summary>
    public partial class ContragentChangeForm : Window
    {
        private const string databaseName = @"Resources\Database\contractorsCopy_v1.db";
        private SQLiteConnection connection;
        private SQLiteCommand command;
        private SQLiteDataReader dataReader;
        
        public ContragentChangeForm()
        {
            InitializeComponent();
        }

        //Regular only Number
        private static bool RegularNumber(string text)
        {
            Regex regex = new Regex("[^0-9]+");
            return !regex.IsMatch(text);
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
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }

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
            command = new SQLiteCommand("select StanInfo from Stan inner join Company on Company.id_Stan = Stan.id where Company.CompanyName = '" +
                tbComName.Text + "'", connection);
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
            command = new SQLiteCommand("select FounderInfo, Price from Founder inner join Company on Company.id = Founder.id_Company" +
                " where Company.CompanyName = '" + tbComName.Text + "'", connection);
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
        }

        private void bSave_Click(object sender, RoutedEventArgs e)
        {
            ContragentInfoForm contragentInfo = new ContragentInfoForm();
            //update Company Short Name
            command = new SQLiteCommand("update Company set CompanyShortName = '" + tbShortName.Text.ToUpper() + "' where Company.CompanyName = '" +
                tbComName.Text + "'", connection);
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
                contragentInfo.Show();
                this.Hide();
            }

            //update Boss
            command = new SQLiteCommand("update Company set Boss = '" + tbBoss.Text.ToUpper() + "' where Company.CompanyName = '" +
                tbComName.Text + "'", connection);
            try
            {
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    tbBoss.Text = dataReader["CompanyShortName"].ToString();
                }
                if (tbBoss.Text == "") tbBoss.Text = "Данні відсутні";
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                contragentInfo.Show();
                this.Hide();
            }

            //update Kved
            command = new SQLiteCommand("update Company set Kved = '" + tbKVED.Text + "' where Company.CompanyName = '" +
                tbComName.Text + "'", connection);
            try
            {
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    tbKVED.Text = dataReader["CompanyShortName"].ToString();
                }
                if (tbKVED.Text == "") tbKVED.Text = "Данні відсутні";
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                contragentInfo.Show();
                this.Hide();
            }

            MessageBox.Show("Дані успішно змінені", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
            contragentInfo.tbComName.Text = tbComName.Text;
            contragentInfo.Show();
            this.Hide();
        }

        private void bCancel_Click(object sender, RoutedEventArgs e)
        {
            ContragentInfoForm contragentInfo = new ContragentInfoForm();
            contragentInfo.tbComName.Text = tbComName.Text;
            contragentInfo.Show();
            this.Hide();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ContragentInfoForm contragentInfo = new ContragentInfoForm();
            contragentInfo.tbComName.Text = tbComName.Text;
            contragentInfo.Show();
            this.Hide();
        }
    }
}
