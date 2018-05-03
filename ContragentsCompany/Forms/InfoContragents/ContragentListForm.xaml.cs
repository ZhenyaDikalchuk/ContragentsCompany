using System.Data.SQLite;
using System.Windows;

namespace ContragentsCompany.Forms.InfoContragents
{
    /// <summary>
    /// Interaction logic for ContragentListForm.xaml
    /// </summary>
    public partial class ContragentListForm : Window
    {
        private const string databaseName = @"Resources\Database\contractorsCopy_v1.db";
        private SQLiteConnection connection;
        private SQLiteCommand command;
        private SQLiteDataReader dataReader;

        public ContragentListForm()
        {
            InitializeComponent();
        }

        //get count all Contragents
        private string GetCountAll()
        {
            command = connection.CreateCommand();
            command.CommandText = "select count(*) from Company";
            return "Кількість записів: " + command.ExecuteScalar().ToString();
        }

        //get count Contragents after search
        private string GetCountAfter()
        {
            command = connection.CreateCommand();
            command.CommandText = "select count(*) from Company where CompanyName like '%" + tbSearch.Text.ToUpper() + "%'";
            return "Кількість записів: " + command.ExecuteScalar().ToString();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
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

            lCount.Content = GetCountAll();
            lbComName.Items.Clear();
            command = new SQLiteCommand("select CompanyName from Company order by id ASC", connection);
            try
            {
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    lbComName.Items.Add(dataReader["CompanyName"]);
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //sort id ACS
        private void rbIdACS_Click(object sender, RoutedEventArgs e)
        {
            lbComName.Items.Clear();
            command = new SQLiteCommand("select CompanyName from Company order by id ASC", connection);
            try
            {
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    lbComName.Items.Add(dataReader["CompanyName"]);
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //sort Name ACS
        private void rbNameACS_Checked(object sender, RoutedEventArgs e)
        {
            lbComName.Items.Clear();
            command = new SQLiteCommand("select CompanyName from Company order by CompanyName ASC", connection);
            try
            {
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    lbComName.Items.Add(dataReader["CompanyName"]);
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //sort id DESC
        private void rbNameDESC_Checked(object sender, RoutedEventArgs e)
        {
            lbComName.Items.Clear();
            command = new SQLiteCommand("select CompanyName from Company order by CompanyName DESC", connection);
            try
            {
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    lbComName.Items.Add(dataReader["CompanyName"]);
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //search Name
        private void bSearch_Click(object sender, RoutedEventArgs e)
        {
            if (tbSearch.Text != "")
            {
                lCount.Content = GetCountAfter();
                lbComName.Items.Clear();
                command = new SQLiteCommand("select CompanyName from Company where CompanyName like '%" + tbSearch.Text.ToUpper() + "%'", connection);
                try
                {
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        lbComName.Items.Add(dataReader["CompanyName"]);
                    }
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void lbComName_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ContragentInfoForm contragentInfo = new ContragentInfoForm();
            //if id return Company Name
            if (rbIdACS.IsChecked == true)
            {
                contragentInfo.tbComName.Text = lbComName.SelectedItem.ToString();
            }
            //if Name(ACS) return Company Name
            if (rbNameACS.IsChecked == true)
            {
                contragentInfo.tbComName.Text = lbComName.SelectedItem.ToString();
            }
            //if Name(DESC) return Company Name
            if (rbNameDESC.IsChecked == true)
            {
                contragentInfo.tbComName.Text = lbComName.SelectedItem.ToString();
            }
            contragentInfo.Show();
        }

        //close form
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Hide();
        }
    }
}
