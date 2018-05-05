using System;
using System.Data.SQLite;
using System.Text.RegularExpressions;
using System.Windows;

namespace ContragentsCompany.Forms.CreateContragents
{
    /// <summary>
    /// Interaction logic for ContragentCreateForm.xaml
    /// </summary>
    public partial class ContragentCreateForm : Window
    {
        private const string databaseName = @"Resources\Database\contractorsCopy_v1.db";
        private SQLiteConnection connection;
        private SQLiteCommand command;
        private SQLiteDataReader dataReader;

        public ContragentCreateForm()
        {
            InitializeComponent();
        }

        //Regular only Number
        private static bool RegularNumber(string text)
        {
            Regex regex = new Regex("[^0-9]+");
            return !regex.IsMatch(text);
        }

        //Clear Fields 
        private void ClearFields()
        {
            tbComName.Text = "";
            tbComSName.Text = "";
            tbEDRPOU.Text = "";
            tbRegion.Text = "";
            tbRayon.Text = "";
            tbCity.Text = "";
            tbMicrorayon.Text = "";
            tbStreet.Text = "";
            tbBuildNumb.Text = "";
            tbPostcode.Text = "";
            tbBoss.Text = "";
            tbKVED.Text = "";
            tbStan.Text = "";
            tbFounder.Text = "";
            tbPrice.Text = "";
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
            ClearFields();
        }

        //create Record
        private void bCreate_Click(object sender, RoutedEventArgs e)
        {
            string idRegion = "", idRayon = "", idCity = "", idMicrorayon = "", idStreet = "", idBuilding = "", idAddress = "", idStan = "", idCompany = "";
            if (tbComName.Text != "" && tbEDRPOU.Text != "" && tbRegion.Text != "" && tbRayon.Text != "" && tbCity.Text != "" && tbStreet.Text != "" && tbBuildNumb.Text != "" &&
                tbPostcode.Text != "" && tbStan.Text != "")
            {
                //insert Region
                command = new SQLiteCommand("INSERT INTO Region (RegionName) SELECT '" + tbRegion.Text + "' name WHERE NOT EXISTS(SELECT 1 FROM Region WHERE RegionName = name)", connection);
                try
                {
                    dataReader = command.ExecuteReader();
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }

                //get id Region
                command = new SQLiteCommand("select id from Region where RegionName = '" + tbRegion.Text + "'", connection);
                try
                {
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        idRegion = dataReader["id"].ToString();
                    }
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                    Application.Current.Shutdown();
                }

                //insert Rayon
                command = new SQLiteCommand("INSERT INTO Rayon (RayonName, id_Region) SELECT '" + tbRayon.Text + "' name, '" + idRegion +
                    "' idRegion WHERE NOT EXISTS(SELECT 1 FROM Rayon WHERE RayonName = name and id_Region = idRegion)", connection);
                try
                {
                    dataReader = command.ExecuteReader();
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }

                //get id Rayon
                command = new SQLiteCommand("select id from Rayon where RayonName = '" + tbRayon.Text + "' and id_Region = '" + idRegion + "'", connection);
                try
                {
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        idRayon = dataReader["id"].ToString();
                    }
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error); Application.Current.Shutdown();
                }

                //insert City
                command = new SQLiteCommand("INSERT INTO City (CityName, id_Region, id_Rayon) SELECT '" + tbCity.Text + "' name, '" + idRegion + "' idRegion, '" +
                    idRayon + "' idRayon WHERE NOT EXISTS(SELECT 1 FROM City WHERE CityName = name and id_Region = idRegion and id_Rayon = idRayon)", connection);
                try
                {
                    dataReader = command.ExecuteReader();
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }

                //get id City
                command = new SQLiteCommand("select id from City where CityName = '" + tbCity.Text + "' and id_Region = '" + idRegion + "' and id_Rayon = '" + idRayon + "'", connection);
                try
                {
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        idCity = dataReader["id"].ToString();
                    }
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if (tbMicrorayon.Text != "")
                {
                    //insert Microrayon
                    command = new SQLiteCommand("INSERT INTO Microrayon (MicroRayonName, id_City) SELECT '" + tbMicrorayon.Text + "' name, '" + idCity + "' idCity" +
                        " WHERE NOT EXISTS(SELECT 1 FROM Microrayon WHERE MicroRayonName = name and id_City = idCity)", connection);
                    try
                    {
                        dataReader = command.ExecuteReader();
                    }
                    catch (SQLiteException ex)
                    {
                        MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    //get id Microrayon
                    command = new SQLiteCommand("select id from Microrayon where MicroRayonName = '" + tbMicrorayon.Text + "' and id_City = '" + idCity + "'", connection);
                    try
                    {
                        dataReader = command.ExecuteReader();
                        while (dataReader.Read())
                        {
                            idMicrorayon = dataReader["id"].ToString();
                        }
                    }
                    catch (SQLiteException ex)
                    {
                        MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                //insert Street
                command = new SQLiteCommand("INSERT INTO Street (StreetName, id_City) SELECT '" + tbStreet.Text + "' name, '" + idCity +
                    "' idCity WHERE NOT EXISTS(SELECT 1 FROM Street WHERE StreetName = name and id_City = idCity)", connection);
                try
                {
                    dataReader = command.ExecuteReader();
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }

                //get id Street
                command = new SQLiteCommand("select id from Street where StreetName = '" + tbStreet.Text + "' and id_City = '" + idCity + "'", connection);
                try
                {
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        idStreet = dataReader["id"].ToString();
                    }
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }

                //insert Building
                command = new SQLiteCommand("INSERT INTO Building (BuildNumb, Postcode, id_Street) SELECT '" + tbBuildNumb.Text + "' numb, '" + tbPostcode.Text + "' postCode, '" +
                    idStreet + "' idStreet WHERE NOT EXISTS(SELECT 1 FROM Building WHERE BuildNumb = numb and Postcode = postCode and id_Street = idStreet)", connection);
                try
                {
                    dataReader = command.ExecuteReader();
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }

                //get id Building
                command = new SQLiteCommand("select id from Building where BuildNumb = '" + tbBuildNumb.Text + "' and Postcode = '" + tbPostcode.Text + "' and id_Street = '" +
                    idStreet + "'", connection);
                try
                {
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        idBuilding = dataReader["id"].ToString();
                    }
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if (tbMicrorayon.Text == "")
                {
                    //insert Address
                    command = new SQLiteCommand("INSERT INTO Address (id_Region, id_Rayon, id_City, id_Street, id_Building) SELECT '" + idRegion + "' idRegion, '" + idRayon +
                        "' idRayon, '" + idCity + "' idCity, '" + idStreet + "' idStreet, '" + idBuilding + "' idBuilding WHERE NOT EXISTS(SELECT 1 FROM Address WHERE " +
                        "id_Region = idRegion and id_Rayon = idRayon and id_City = idCity and id_Street = idStreet and id_Building = idBuilding)", connection);
                    try
                    {
                        dataReader = command.ExecuteReader();
                    }
                    catch (SQLiteException ex)
                    {
                        MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    //get id Address
                    command = new SQLiteCommand("select id from Address where id_Region = '" + idRegion + "' and id_Rayon = '" + idRayon + "' and id_City = '" + idCity +
                        "' and id_Street = '" + idStreet + "' and id_Building = '" + idBuilding + "'", connection);
                    try
                    {
                        dataReader = command.ExecuteReader();
                        while (dataReader.Read())
                        {
                            idAddress = dataReader["id"].ToString();
                        }
                    }
                    catch (SQLiteException ex)
                    {
                        MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else if (tbMicrorayon.Text != "")
                {
                    //insert Address
                    command = new SQLiteCommand("INSERT INTO Address (id_Region, id_Rayon, id_City, id_Microrayon, id_Street, id_Building) SELECT '" + idRegion + 
                        "' idRegion, '" + idRayon + "' idRayon, '" + idCity + "' idCity, '" + idMicrorayon + "' idMicrorayon, '" + idStreet + "' idStreet, '" + idBuilding + 
                        "' idBuilding WHERE NOT EXISTS(SELECT 1 FROM Address WHERE " + "id_Region = idRegion and id_Rayon = idRayon and id_City = idCity" + 
                        " and id_Street = idStreet and id_Building = idBuilding)", connection);
                    try
                    {
                        dataReader = command.ExecuteReader();
                    }
                    catch (SQLiteException ex)
                    {
                        MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    //get id Address
                    command = new SQLiteCommand("select id from Address where id_Region = '" + idRegion + "' and id_Rayon = '" + idRayon + "' and id_City = '" + idCity +
                        "' and id_Street = '" + idStreet + "' and id_Building = '" + idBuilding + "'", connection);
                    try
                    {
                        dataReader = command.ExecuteReader();
                        while (dataReader.Read())
                        {
                            idAddress = dataReader["id"].ToString();
                        }
                    }
                    catch (SQLiteException ex)
                    {
                        MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                //insert Stan
                command = new SQLiteCommand("INSERT INTO Stan (StanInfo) SELECT '" + tbStan.Text + "' info WHERE NOT EXISTS(SELECT 1 FROM Stan WHERE StanInfo = info)", connection);
                try
                {
                    dataReader = command.ExecuteReader();
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }

                //get id Stan
                command = new SQLiteCommand("select id from Stan where StanInfo = '" + tbStan.Text + "'", connection);
                try
                {
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        idStan = dataReader["id"].ToString();
                    }
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }

                //insert Company
                command = new SQLiteCommand("INSERT INTO Company (CompanyName, CompanyShortName, EDRPOU, id_Address, BOSS, KVED, id_Stan) SELECT '" + tbComName.Text.ToUpper() +
                    "' name, '" + tbComSName.Text.ToUpper() + "' sName, '" + tbEDRPOU.Text + "' edrpou, '" + idAddress + "' idAddress, '" + tbBoss.Text.ToUpper() + "' boss, '"
                    + tbKVED.Text + "' kved, '" + idStan + "' idStan WHERE NOT EXISTS(SELECT 1 FROM Company WHERE CompanyName = name and CompanyShortName = sName and EDRPOU = edrpou" +
                    " and id_Address = idAddress and BOSS = boss and KVED = kved and id_Stan = idStan)", connection);
                try
                {
                    dataReader = command.ExecuteReader();
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }

                //get id Company
                command = new SQLiteCommand("select id from Company where EDRPOU = '" + tbEDRPOU.Text + "'", connection);
                try
                {
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        idCompany = dataReader["id"].ToString();
                    }
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }

                //insert Founder
                if (tbFounder.Text == "") tbFounder.Text = "Дані відсутні";
                tbPrice.Text = (tbPrice.Text == "") ? tbPrice.Text = "Дані відсутні" : tbPrice.Text += " грн.";
                command = new SQLiteCommand("INSERT INTO Founder (FounderInfo, Price, id_Company) SELECT '" + tbFounder.Text + "' info, '" + tbPrice.Text + "' price, '" +
                    idCompany + "' idCompany WHERE NOT EXISTS(SELECT 1 FROM Founder WHERE FounderInfo = info and Price = price and id_Company = idCompany);", connection);
                try
                {
                    dataReader = command.ExecuteReader();
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, Application.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }

                MessageBox.Show("Запис успішно створено!", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                ClearFields();
            }
            else MessageBox.Show("Заповніть усі поля, позначені '*'!", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void tbPostcode_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !RegularNumber(e.Text);
        }

        private void tbEDRPOU_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !RegularNumber(e.Text);
        }

        private void tbPrice_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !RegularNumber(e.Text);
        }

        //close form
        private void Window_Closed(object sender, EventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Hide();
        }
    }
}
