using System;
using System.Collections.Generic;
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

namespace Gas_Station_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int updatingGasId = 0;
        public MainWindow()
        {
            InitializeComponent();
            DatabaseEntities1 db = new DatabaseEntities1();
            var info = from d in db.Gas
                       select new
                       {
                           GasName = d.Name,
                           GasType = d.Type
                       };
            foreach (var item in info)
            {
                Console.WriteLine(item.GasName);
                Console.WriteLine(item.GasType);
            }
            gridGas.ItemsSource = info.ToList();
        }

        private void ButtonAddClick(object sender, RoutedEventArgs e)
        {
            DatabaseEntities1 db = new DatabaseEntities1();
            Gas gasObj = new Gas()
            {
                Name = textName.Text,
                Type = textType.Text,
                Price = double.Parse(textPrice.Text)
            };
            db.Gas.Add(gasObj);
            db.SaveChanges();
            gridGas.ItemsSource = db.Gas.ToList();
            textName.Text = String.Empty;
            textType.Text = String.Empty;
            textPrice.Text = String.Empty;
        }
        private void ButtonLoadGas(object sender, RoutedEventArgs e)
        {
            DatabaseEntities1 db = new DatabaseEntities1();
            gridGas.ItemsSource = db.Gas.ToList();
        }

        private void gridGas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (gridGas.SelectedIndex >= 0)
            {
                if (gridGas.SelectedItems.Count >= 0)
                {
                    if (gridGas.SelectedItems[0].GetType() == typeof(Gas))
                    {
                        Gas gas = (Gas)gridGas.SelectedItems[0];
                        textNameToUpd.Text = gas.Name;
                        textTypeToUpd.Text = gas.Type;
                        textPriceToUpd.Text = gas.Price.ToString();
                        updatingGasId = gas.Id;

                    }
                }
            }
        }

        private void ButtonUpdateClick(object sender, RoutedEventArgs e)
        {
            DatabaseEntities1 db = new DatabaseEntities1();
            var r = from d in db.Gas
                    where d.Id == updatingGasId
                    select d;
            Gas obj = r.SingleOrDefault();
            if (obj != null)
            {
                obj.Name = textNameToUpd.Text;
                obj.Type = textTypeToUpd.Text;
                obj.Price = double.Parse(textPriceToUpd.Text);
                db.SaveChanges();
            }
        }

        private void ButtonDeleteClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult msgBoxResult = MessageBox.Show("Are you sure you want to delete item?", 
                "Delete item",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning,
                MessageBoxResult.No);
            if (msgBoxResult == MessageBoxResult.Yes)
            {
                DatabaseEntities1 db = new DatabaseEntities1();
                var r = from d in db.Gas
                        where d.Id == updatingGasId
                        select d;
                Gas obj = r.SingleOrDefault();
                if (obj != null)
                {
                    db.Gas.Remove(obj);
                    db.SaveChanges();
                }
            }
        }

    }
}
