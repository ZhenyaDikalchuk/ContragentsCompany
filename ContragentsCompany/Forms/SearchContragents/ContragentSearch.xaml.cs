using ContragentsCompany.Forms.InfoContragents;
using System;
using System.Data.SQLite;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;

namespace ContragentsCompany.Forms.SearchContragents
{
    /// <summary>
    /// Interaction logic for ContragentSearchForm.xaml
    /// </summary>
    public partial class ContragentSearchForm : Window
    {
        private const string databaseName = @"Resources\Database\contractorsCopy_v1.db";
        private SQLiteConnection connection;
        private SQLiteCommand command;
        private SQLiteDataReader dataReader;

        public ContragentSearchForm()
        {
            InitializeComponent();
        }

        //Regular only Number
        private static bool RegularNumber(string text)
        {
            Regex regex = new Regex("[^0-9]+");
            return !regex.IsMatch(text);
        }

        private void ClearFields()
        {
            tbName.Text = "";
            tbEDRPOU.Text = "";
            tbBoss.Text = "";
            tbRegion.Text = "";
            tbRayon.Text = "";
            tbCity.Text = "";
            tbStreet.Text = "";
            tbBuildNumb.Text = "";
            tbPostcode.Text = "";
            lCount.Content = "Кількість записів: 0";
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
            ClearFields();
        }

        private void bSearch_Click(object sender, RoutedEventArgs e)
        {
            //search Name
            if (cbName.IsChecked == true && tbName.Text != "")
            {
                lbResault.Items.Clear();
                command = new SQLiteCommand("select CompanyName from Company where CompanyName like '%" + tbName.Text.ToUpper() + "%'", connection);
                try
                {
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        lbResault.Items.Add(dataReader["CompanyName"]);
                    }
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }

                command = new SQLiteCommand("select count(*) from Company where CompanyName like '%" + tbName.Text.ToUpper() + "%'", connection);
                try
                {
                    lCount.Content = "Кількість записів: " + command.ExecuteScalar().ToString();
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            //search EDRPOU
            if (cbEDRPOU.IsChecked == true && tbEDRPOU.Text != "")
            {
                lbResault.Items.Clear();
                command = new SQLiteCommand("select EDRPOU from Company where EDRPOU like '%" + tbEDRPOU.Text.ToUpper() + "%'", connection);
                try
                {
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        lbResault.Items.Add(dataReader["EDRPOU"]);
                    }
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }

                command = new SQLiteCommand("select count(*) from Company where EDRPOU like '%" + tbEDRPOU.Text.ToUpper() + "%'", connection);
                try
                {
                    lCount.Content = "Кількість записів: " + command.ExecuteScalar().ToString();
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            //search Boss
            if (cbBoss.IsChecked == true && tbBoss.Text != "")
            {
                lbResault.Items.Clear();
                command = new SQLiteCommand("select Boss from Company where Boss like '%" + tbBoss.Text.ToUpper() + "%'", connection);
                try
                {
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        lbResault.Items.Add(dataReader["Boss"]);
                    }
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }

                command = new SQLiteCommand("select count(*) from Company where Boss like '%" + tbBoss.Text.ToUpper() + "%'", connection);
                try
                {
                    lCount.Content = "Кількість записів: " + command.ExecuteScalar().ToString();
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            //serach Address
            if (cbAddress.IsChecked == true && (tbRegion.Text != "" || tbRayon.Text != "" || tbCity.Text != "" ||
                                                tbStreet.Text != "" || tbBuildNumb.Text != "" || tbPostcode.Text != ""))
            {
                lbResault.Items.Clear();

                FileInfo file = new FileInfo("Resources/Queries/searchAddress.sql");
                string script = file.OpenText().ReadToEnd();
                command = new SQLiteCommand(script + "where Region.RegionName like '%" + tbRegion.Text + "%' and Rayon.RayonName like '%" + tbRayon.Text + "%'" +
                                            "and City.CityName like '%" + tbCity.Text + "%' and Street.StreetName like '%" + tbStreet.Text + "%'" +
                                            "and Building.BuildNumb like '%" + tbBuildNumb.Text + "%' and Building.Postcode like '%" + tbPostcode.Text + "%'", connection);
                try
                {
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        lbResault.Items.Add(dataReader["RegionName"] + ", " + dataReader["RayonName"] + ", " + dataReader["CityName"] + ", " + dataReader["StreetName"] +
                        ", " + dataReader["BuildNumb"] + ", " + dataReader["Postcode"]);
                    }
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }

                FileInfo fileCount = new FileInfo("Resources/Queries/searchCountAddress.sql");
                string scriptCount = fileCount.OpenText().ReadToEnd();
                command = new SQLiteCommand(scriptCount + "where Region.RegionName like '%" + tbRegion.Text + "%' and Rayon.RayonName like '%" + tbRayon.Text + "%'" +
                                            "and City.CityName like '%" + tbCity.Text + "%' and Street.StreetName like '%" + tbStreet.Text + "%'" +
                                            "and Building.BuildNumb like '%" + tbBuildNumb.Text + "%' and Building.Postcode like '%" + tbPostcode.Text + "%'", connection);
                try
                {
                    lCount.Content = "Кількість записів: " + command.ExecuteScalar().ToString();
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            //search Name and EDRPOU
            if ((cbName.IsChecked == true && tbName.Text != "") && (cbEDRPOU.IsChecked == true && tbEDRPOU.Text != ""))
            {
                lbResault.Items.Clear();
                command = new SQLiteCommand("select CompanyName, EDRPOU from Company where CompanyName like '%" + tbName.Text.ToUpper() + "%'" +
                    "and EDRPOU like '%" + tbEDRPOU.Text + "%'", connection);
                try
                {
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        lbResault.Items.Add(dataReader["CompanyName"] + ", " + dataReader["EDRPOU"]);
                    }
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }

                command = new SQLiteCommand("select count(*) from Company where CompanyName like '%" + tbName.Text.ToUpper() + "%'" +
                    "and EDRPOU like '%" + tbEDRPOU.Text + "%'", connection);
                try
                {
                    lCount.Content = "Кількість записів: " + command.ExecuteScalar().ToString();
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            //search Name and Boss
            if ((cbName.IsChecked == true && tbName.Text != "") && (cbBoss.IsChecked == true && tbBoss.Text != ""))
            {
                lbResault.Items.Clear();
                command = new SQLiteCommand("select CompanyName, Boss from Company where CompanyName like '%" + tbName.Text.ToUpper() + "%'" +
                    "and Boss like '%" + tbBoss.Text.ToUpper() + "%'", connection);
                try
                {
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        lbResault.Items.Add(dataReader["CompanyName"] + ", " + dataReader["Boss"]);
                    }
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);

                }

                command = new SQLiteCommand("select count(*) from Company where CompanyName like '%" + tbName.Text.ToUpper() + "%'" +
                    "and Boss like '%" + tbBoss.Text.ToUpper() + "%'", connection);
                try
                {
                    lCount.Content = "Кількість записів: " + command.ExecuteScalar().ToString();
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            //search Name and Address
            if ((cbName.IsChecked == true && tbName.Text != "") && (cbAddress.IsChecked == true && (tbRegion.Text != "" || tbRayon.Text != "" ||
                tbCity.Text != "" || tbStreet.Text != "" || tbBuildNumb.Text != "" || tbPostcode.Text != "")))
            {
                lbResault.Items.Clear();

                FileInfo file = new FileInfo("Resources/Queries/searchName_Address.sql");
                string script = file.OpenText().ReadToEnd();
                command = new SQLiteCommand(script + " where CompanyName like '%" + tbName.Text.ToUpper() + "%' and RegionName like '%" + tbRegion.Text +
                        "%' and RayonName like '%" + tbRayon.Text + "%' and CityName like '%" + tbCity.Text + "%' and StreetName like '%" +
                        tbStreet.Text + "%' and BuildNumb like '%" + tbBuildNumb.Text + "%' and Postcode like '%" + tbPostcode.Text + "%'", connection);
                try
                {
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        lbResault.Items.Add(dataReader["CompanyName"] + ", " + dataReader["RegionName"] + ", " + dataReader["RayonName"] + ", " + dataReader["CityName"] +
                            ", " + dataReader["StreetName"] + ", " + dataReader["BuildNumb"] + ", " + dataReader["Postcode"]);
                    }
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }

                FileInfo fileCount = new FileInfo("Resources/Queries/searchCountName_Address.sql");
                string scriptCount = fileCount.OpenText().ReadToEnd();
                command = new SQLiteCommand(scriptCount + " where CompanyName like '%" + tbName.Text.ToUpper() + "%' and RegionName like '%" + tbRegion.Text +
                        "%' and RayonName like '%" + tbRayon.Text + "%' and CityName like '%" + tbCity.Text + "%' and StreetName like '%" +
                        tbStreet.Text + "%' and BuildNumb like '%" + tbBuildNumb.Text + "%' and Postcode like '%" + tbPostcode.Text + "%'", connection);
                try
                {
                    lCount.Content = "Кількість записів: " + command.ExecuteScalar().ToString();
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            //search EDRPOU and Boss
            if ((cbEDRPOU.IsChecked == true && tbEDRPOU.Text != "") && (cbBoss.IsChecked == true && tbBoss.Text != ""))
            {
                lbResault.Items.Clear();
                command = new SQLiteCommand("select EDRPOU, Boss from Company where EDRPOU like '%" + tbEDRPOU.Text + "%'" +
                    "and Boss like '%" + tbBoss.Text.ToUpper() + "%'", connection);
                try
                {
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        lbResault.Items.Add(dataReader["EDRPOU"] + ", " + dataReader["Boss"]);
                    }
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }

                command = new SQLiteCommand("select count(*) from Company where EDRPOU like '%" + tbEDRPOU.Text + "%'" +
                    "and Boss like '%" + tbBoss.Text.ToUpper() + "%'", connection);
                try
                {
                    lCount.Content = "Кількість записів: " + command.ExecuteScalar().ToString();
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }

            //search EDRPOU and Address
            if ((cbEDRPOU.IsChecked == true && tbEDRPOU.Text != "") && (cbAddress.IsChecked == true && (tbRegion.Text != "" || tbRayon.Text != "" ||
                tbCity.Text != "" || tbStreet.Text != "" || tbBuildNumb.Text != "" || tbPostcode.Text != "")))
            {
                lbResault.Items.Clear();

                FileInfo file = new FileInfo("Resources/Queries/searchEDRPOU_Address.sql");
                string script = file.OpenText().ReadToEnd();
                command = new SQLiteCommand(script + " where EDRPOU like '%" + tbEDRPOU.Text + "%' and RegionName like '%" + tbRegion.Text +
                        "%' and RayonName like '%" + tbRayon.Text + "%' and CityName like '%" + tbCity.Text + "%' and StreetName like '%" +
                        tbStreet.Text + "%' and BuildNumb like '%" + tbBuildNumb.Text + "%' and Postcode like '%" + tbPostcode.Text + "%'", connection);
                try
                {
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        lbResault.Items.Add(dataReader["EDRPOU"] + ", " + dataReader["RegionName"] + ", " + dataReader["RayonName"] + ", " + dataReader["CityName"] +
                            ", " + dataReader["StreetName"] + ", " + dataReader["BuildNumb"] + ", " + dataReader["Postcode"]);
                    }
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }

                FileInfo fileCount = new FileInfo("Resources/Queries/searchCountEDRPOU_Address.sql");
                string scriptCount = fileCount.OpenText().ReadToEnd();
                command = new SQLiteCommand(scriptCount + " where EDRPOU like '%" + tbEDRPOU.Text + "%' and RegionName like '%" + tbRegion.Text +
                        "%' and RayonName like '%" + tbRayon.Text + "%' and CityName like '%" + tbCity.Text + "%' and StreetName like '%" +
                        tbStreet.Text + "%' and BuildNumb like '%" + tbBuildNumb.Text + "%' and Postcode like '%" + tbPostcode.Text + "%'", connection);
                try
                {
                    lCount.Content = "Кількість записів: " + command.ExecuteScalar().ToString();
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            //search Boss and Address
            if ((cbBoss.IsChecked == true && tbBoss.Text != "") && (cbAddress.IsChecked == true && (tbRegion.Text != "" || tbRayon.Text != "" ||
                tbCity.Text != "" || tbStreet.Text != "" || tbBuildNumb.Text != "" || tbPostcode.Text != "")))
            {
                lbResault.Items.Clear();
                FileInfo file = new FileInfo("Resources/Queries/searchBoss_Address.sql");
                string script = file.OpenText().ReadToEnd();
                command = new SQLiteCommand(script + " where Boss like '%" + tbBoss.Text.ToUpper() + "%' and RegionName like '%" + tbRegion.Text +
                        "%' and RayonName like '%" + tbRayon.Text + "%' and CityName like '%" + tbCity.Text + "%' and StreetName like '%" +
                        tbStreet.Text + "%' and BuildNumb like '%" + tbBuildNumb.Text + "%' and Postcode like '%" + tbPostcode.Text + "%'", connection);
                try
                {
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        lbResault.Items.Add(dataReader["Boss"] + ", " + dataReader["RegionName"] + ", " + dataReader["RayonName"] + ", " + dataReader["CityName"] +
                            ", " + dataReader["StreetName"] + ", " + dataReader["BuildNumb"] + ", " + dataReader["Postcode"]);
                    }
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }

                FileInfo fileCount = new FileInfo("Resources/Queries/searchCountBoss_Address.sql");
                string scriptCount = fileCount.OpenText().ReadToEnd();
                command = new SQLiteCommand(scriptCount + " where Boss like '%" + tbBoss.Text.ToUpper() + "%' and RegionName like '%" + tbRegion.Text +
                        "%' and RayonName like '%" + tbRayon.Text + "%' and CityName like '%" + tbCity.Text + "%' and StreetName like '%" +
                        tbStreet.Text + "%' and BuildNumb like '%" + tbBuildNumb.Text + "%' and Postcode like '%" + tbPostcode.Text + "%'", connection);
                try
                {
                    lCount.Content = "Кількість записів: " + command.ExecuteScalar().ToString();
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            //search Name and EDRPOU and Boss
            if ((cbName.IsChecked == true && tbName.Text != "") && (cbEDRPOU.IsChecked == true && tbEDRPOU.Text != "") && (cbBoss.IsChecked == true && tbBoss.Text != ""))
            {
                lbResault.Items.Clear();
                command = new SQLiteCommand("select CompanyName, EDRPOU, Boss from Company where CompanyName like '%" + tbName.Text.ToUpper() + "%' and EDRPOU like '%" +
                    tbEDRPOU.Text + "%'" + "and Boss like '%" + tbBoss.Text.ToUpper() + "%'", connection);
                try
                {
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        lbResault.Items.Add(dataReader["CompanyName"] + ", " + dataReader["EDRPOU"] + ", " + dataReader["Boss"]);
                    }
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }

                command = new SQLiteCommand("select count(*) from Company where CompanyName like '%" + tbName.Text.ToUpper() + "%' and EDRPOU like '%" +
                    tbEDRPOU.Text + "%'" + "and Boss like '%" + tbBoss.Text.ToUpper() + "%'", connection);
                try
                {
                    lCount.Content = "Кількість записів: " + command.ExecuteScalar().ToString();
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            //search Name and EDRPOU and Address
            if ((cbName.IsChecked == true && tbName.Text != "") && (cbEDRPOU.IsChecked == true && tbEDRPOU.Text != "") && (cbAddress.IsChecked == true &&
                (tbRegion.Text != "" || tbRayon.Text != "" || tbCity.Text != "" || tbStreet.Text != "" || tbBuildNumb.Text != "" || tbPostcode.Text != "")))
            {
                lbResault.Items.Clear();

                FileInfo file = new FileInfo("Resources/Queries/searchName_EDRPOU_Address.sql");
                string script = file.OpenText().ReadToEnd();
                command = new SQLiteCommand(script + " where CompanyName like '%" + tbName.Text.ToUpper() + "%' and EDRPOU like '%" + tbEDRPOU.Text +
                    "%' and RegionName like '%" + tbRegion.Text + "%' and RayonName like '%" + tbRayon.Text + "%' and CityName like '%" + tbCity.Text +
                    "%' and StreetName like '%" + tbStreet.Text + "%' and BuildNumb like '%" + tbBuildNumb.Text + "%' and Postcode like '%" + tbPostcode.Text + "%'", connection);
                try
                {
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        lbResault.Items.Add(dataReader["CompanyName"] + ", " + dataReader["EDRPOU"] + ", " + dataReader["RegionName"] + ", " + dataReader["RayonName"] +
                            ", " + dataReader["CityName"] + ", " + dataReader["StreetName"] + ", " + dataReader["BuildNumb"] + ", " + dataReader["Postcode"]);
                    }
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }

                FileInfo fileCount = new FileInfo("Resources/Queries/searchCountName_EDRPOU_Address.sql");
                string scriptCount = fileCount.OpenText().ReadToEnd();
                command = new SQLiteCommand(scriptCount + " where CompanyName like '%" + tbName.Text.ToUpper() + "%' and EDRPOU like '%" + tbEDRPOU.Text +
                    "%' and RegionName like '%" + tbRegion.Text + "%' and RayonName like '%" + tbRayon.Text + "%' and CityName like '%" + tbCity.Text +
                    "%' and StreetName like '%" + tbStreet.Text + "%' and BuildNumb like '%" + tbBuildNumb.Text + "%' and Postcode like '%" + tbPostcode.Text + "%'", connection);
                try
                {
                    lCount.Content = "Кількість записів: " + command.ExecuteScalar().ToString();
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            //search Name and Boss and Address
            if ((cbName.IsChecked == true && tbName.Text != "") && (cbBoss.IsChecked == true && tbBoss.Text != "") && (cbAddress.IsChecked == true &&
                (tbRegion.Text != "" || tbRayon.Text != "" || tbCity.Text != "" || tbStreet.Text != "" || tbBuildNumb.Text != "" || tbPostcode.Text != "")))
            {
                lbResault.Items.Clear();

                FileInfo file = new FileInfo("Resources/Queries/searchName_Boss_Address.sql");
                string script = file.OpenText().ReadToEnd();
                command = new SQLiteCommand(script + " where CompanyName like '%" + tbName.Text.ToUpper() + "%' and Boss like '%" + tbBoss.Text.ToUpper() +
                    "%' and RegionName like '%" + tbRegion.Text + "%' and RayonName like '%" + tbRayon.Text + "%' and CityName like '%" + tbCity.Text +
                    "%' and StreetName like '%" + tbStreet.Text + "%' and BuildNumb like '%" + tbBuildNumb.Text + "%' and Postcode like '%" + tbPostcode.Text + "%'", connection);
                try
                {
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        lbResault.Items.Add(dataReader["CompanyName"] + ", " + dataReader["Boss"] + ", " + dataReader["RegionName"] + ", " + dataReader["RayonName"] +
                            ", " + dataReader["CityName"] + ", " + dataReader["StreetName"] + ", " + dataReader["BuildNumb"] + ", " + dataReader["Postcode"]);
                    }
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }

                FileInfo fileCount = new FileInfo("Resources/Queries/searchCountName_Boss_Address.sql");
                string scriptCount = fileCount.OpenText().ReadToEnd();
                command = new SQLiteCommand(scriptCount + " where CompanyName like '%" + tbName.Text.ToUpper() + "%' and Boss like '%" + tbBoss.Text.ToUpper() +
                    "%' and RegionName like '%" + tbRegion.Text + "%' and RayonName like '%" + tbRayon.Text + "%' and CityName like '%" + tbCity.Text +
                    "%' and StreetName like '%" + tbStreet.Text + "%' and BuildNumb like '%" + tbBuildNumb.Text + "%' and Postcode like '%" + tbPostcode.Text + "%'", connection);
                try
                {
                    lCount.Content = "Кількість записів: " + command.ExecuteScalar().ToString();
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            //search EDRPOU and Boss and Address
            if ((cbEDRPOU.IsChecked == true && tbEDRPOU.Text != "") && (cbBoss.IsChecked == true && tbBoss.Text != "") && (cbAddress.IsChecked == true &&
                (tbRegion.Text != "" || tbRayon.Text != "" || tbCity.Text != "" || tbStreet.Text != "" || tbBuildNumb.Text != "" || tbPostcode.Text != "")))
            {
                lbResault.Items.Clear();

                FileInfo file = new FileInfo("Resources/Queries/searchEDRPOU_Boss_Address.sql");
                string script = file.OpenText().ReadToEnd();
                command = new SQLiteCommand(script + " where EDRPOU like '%" + tbEDRPOU.Text + "%' and Boss like '%" + tbBoss.Text.ToUpper() + "%' and RegionName like '%" +
                    tbRegion.Text + "%' and RayonName like '%" + tbRayon.Text + "%' and CityName like '%" + tbCity.Text + "%' and StreetName like '%" +
                    tbStreet.Text + "%' and BuildNumb like '%" + tbBuildNumb.Text + "%' and Postcode like '%" + tbPostcode.Text + "%'", connection);
                try
                {
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        lbResault.Items.Add(dataReader["EDRPOU"] + ", " + dataReader["Boss"] + ", " + dataReader["RegionName"] + ", " + dataReader["RayonName"] +
                            ", " + dataReader["CityName"] + ", " + dataReader["StreetName"] + ", " + dataReader["BuildNumb"] + ", " + dataReader["Postcode"]);
                    }
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }

                FileInfo fileCount = new FileInfo("Resources/Queries/searchCountEDRPOU_Boss_Address.sql");
                string scriptCount = fileCount.OpenText().ReadToEnd();
                command = new SQLiteCommand(scriptCount + " where EDRPOU like '%" + tbEDRPOU.Text + "%' and Boss like '%" + tbBoss.Text.ToUpper() + "%' and RegionName like '%" +
                    tbRegion.Text + "%' and RayonName like '%" + tbRayon.Text + "%' and CityName like '%" + tbCity.Text + "%' and StreetName like '%" +
                    tbStreet.Text + "%' and BuildNumb like '%" + tbBuildNumb.Text + "%' and Postcode like '%" + tbPostcode.Text + "%'", connection);
                try
                {
                    lCount.Content = "Кількість записів: " + command.ExecuteScalar().ToString();
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            //search Name and EDRPOU and Boss and Address
            if ((cbName.IsChecked == true && tbName.Text != "") && (cbEDRPOU.IsChecked == true && tbEDRPOU.Text != "") && (cbBoss.IsChecked == true && tbBoss.Text != "") &&
                (cbAddress.IsChecked == true && (tbRegion.Text != "" || tbRayon.Text != "" || tbCity.Text != "" || tbStreet.Text != "" || tbBuildNumb.Text != "" || tbPostcode.Text != "")))
            {
                lbResault.Items.Clear();

                FileInfo file = new FileInfo("Resources/Queries/searchAll.sql");
                string script = file.OpenText().ReadToEnd();
                command = new SQLiteCommand(script + " where CompanyName like '%" + tbName.Text.ToUpper() + "%' and EDRPOU like '%" + tbEDRPOU.Text + "%' and Boss like '%" +
                    tbBoss.Text.ToUpper() + "%' and RegionName like '%" + tbRegion.Text + "%' and RayonName like '%" + tbRayon.Text + "%' and CityName like '%" + tbCity.Text +
                    "%' and StreetName like '%" + tbStreet.Text + "%' and BuildNumb like '%" + tbBuildNumb.Text + "%' and Postcode like '%" + tbPostcode.Text + "%'", connection);
                try
                {
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        lbResault.Items.Add(dataReader["CompanyName"] + ", " + dataReader["EDRPOU"] + ", " + dataReader["Boss"] + ", " + dataReader["RegionName"] + ", "
                            + dataReader["RayonName"] + ", " + dataReader["CityName"] + ", " + dataReader["StreetName"] + ", " + dataReader["BuildNumb"] + ", " + dataReader["Postcode"]);
                    }
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }

                FileInfo fileCount = new FileInfo("Resources/Queries/searchCountAll.sql");
                string scriptCount = fileCount.OpenText().ReadToEnd();
                command = new SQLiteCommand(scriptCount + " where CompanyName like '%" + tbName.Text.ToUpper() + "%' and EDRPOU like '%" + tbEDRPOU.Text + "%' and Boss like '%" +
                    tbBoss.Text.ToUpper() + "%' and RegionName like '%" + tbRegion.Text + "%' and RayonName like '%" + tbRayon.Text + "%' and CityName like '%" + tbCity.Text +
                    "%' and StreetName like '%" + tbStreet.Text + "%' and BuildNumb like '%" + tbBuildNumb.Text + "%' and Postcode like '%" + tbPostcode.Text + "%'", connection);
                try
                {
                    lCount.Content = "Кількість записів: " + command.ExecuteScalar().ToString();
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void lbResault_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ContragentInfoForm contragentInfo = new ContragentInfoForm();

            //search Name
            if (cbName.IsChecked == true && cbEDRPOU.IsChecked != true && cbBoss.IsChecked != true && cbAddress.IsChecked != true)
            {
                contragentInfo.tbComName.Text = lbResault.SelectedItem.ToString();
                contragentInfo.Show();
            }

            //search EDRPOU
            if (cbName.IsChecked != true && cbEDRPOU.IsChecked == true && cbBoss.IsChecked != true && cbAddress.IsChecked != true)
            {
                contragentInfo.tbComName.Text = lbResault.SelectedItem.ToString();
                contragentInfo.Show();
            }

            //search Boss
            if (cbName.IsChecked != true && cbEDRPOU.IsChecked != true && cbBoss.IsChecked == true && cbAddress.IsChecked != true)
            {
                contragentInfo.tbComName.Text = lbResault.SelectedItem.ToString();
                contragentInfo.Show();
            }
        }

        //regular for EDRPOU
        private void tbEDRPOU_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !RegularNumber(e.Text);
        }
        
        //regular for Postcode
        private void tbPostcode_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !RegularNumber(e.Text);
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
