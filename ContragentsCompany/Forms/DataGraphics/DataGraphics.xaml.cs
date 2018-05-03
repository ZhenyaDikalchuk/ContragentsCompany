using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows;

namespace ContragentsCompany.Forms.DataGraphics
{
    /// <summary>
    /// Interaction logic for DataGraphicsForm.xaml
    /// </summary>
    public partial class DataGraphicsForm : Window
    {
        private const string databaseName = @"Resources\Database\contractorsCopy_v1.db";
        private SQLiteConnection connection;
        private SQLiteCommand command;
        private SQLiteDataReader dataReader;
        private MainWindow mainWindow = new MainWindow();

        public DataGraphicsForm()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //connection
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

            //get cout Microrayonss
            int numb = 0;
            command = new SQLiteCommand("select count() from Microrayon inner join City on City.id = Microrayon.id_City where CityName like '%Вінниця%'", connection);
            try
            {
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    numb = Int32.Parse(dataReader["count()"].ToString());
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }

            //Histogram
            List<KeyValuePair<string, int>> microrayonList = new List<KeyValuePair<string, int>>();
            string name;
            int count;
            for (int i = 1; i <= numb; i++)
            {
                command = new SQLiteCommand("select count(), MicroRayonName from Address inner join City on City.id = Address.id_City inner join Microrayon on" +
                 " Microrayon.id = Address.id_Microrayon where Microrayon.id = '" + i + "'", connection);
                try
                {
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        name = dataReader["MicroRayonName"].ToString();
                        count = Int32.Parse(dataReader["count()"].ToString());
                        microrayonList.Add(new KeyValuePair<string, int>(name, count));
                    }
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            columnChart.DataContext = microrayonList;

            //get count Regions
            command = new SQLiteCommand("select count() from Region", connection);
            try
            {
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    numb = Int32.Parse(dataReader["count()"].ToString());
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }

            //Pie Chart
            List<KeyValuePair<string, int>> regionList = new List<KeyValuePair<string, int>>();
            for (int i = 1; i <= numb; i++)
            {
                command = new SQLiteCommand("select count(), RegionName from Address inner join Region on Region.id = Address.id_Region where id_Region = '" + 
                    i + "'", connection);
                try
                {
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        count = Int32.Parse(dataReader["count()"].ToString());
                        if (count == 0) break;
                        name = dataReader["RegionName"].ToString();
                        regionList.Add(new KeyValuePair<string, int>(name, count));
                    }
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            pieChart.DataContext = regionList;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            mainWindow.Show();
            this.Hide();
        }
    }
}
