using GameServer.Types.Item;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace ItemGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            TypeBox.ItemsSource = Enum.GetValues(typeof(ItemType));
            WeaponType.ItemsSource = Enum.GetValues(typeof(WeaponTypes));
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Check so everything is correct
            if (true)
            {
                ItemBase item;

                ItemType type;
                Enum.TryParse<ItemType>(TypeBox.SelectedValue.ToString(), out type);
                WeaponTypes weapon = WeaponTypes.OneHand;
                if(WeaponType.SelectedValue != null)
                    Enum.TryParse<WeaponTypes>(WeaponType.SelectedValue.ToString(), out weapon);
                //Rewrite this later, ugly
                if (IsWeapon.IsChecked.Value)
                    item = new ItemBase(int.Parse(ID.Text), Name.Text, TextureName.Text, type, weapon, int.Parse(IntB.Text), int.Parse(StrB.Text), int.Parse(DexB.Text), int.Parse(Lower.Text), int.Parse(Upper.Text));
                else
                    item = new ItemBase(int.Parse(ID.Text), Name.Text, TextureName.Text, type, int.Parse(IntB.Text), int.Parse(StrB.Text), int.Parse(DexB.Text));
                string itemString = Newtonsoft.Json.JsonConvert.SerializeObject(item);

                string docPath = Directory.GetCurrentDirectory();
                using (StreamWriter sw =new StreamWriter(Path.Combine(docPath, "Files.json")))
                {
                    sw.WriteLine(itemString);
                }
            }
        }
    }
}
