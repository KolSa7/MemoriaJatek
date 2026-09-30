using System.Diagnostics.Metrics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Memoria
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> palyak = new List<string> {"F1","emojik","Fővárosok","matek"};
        List<string> meret = new List<string> {"2x2","4x4","6x6"};
        List<string> matek = new List<string> {"2 + 3", "5",
    "10 - 4", "6",
    "3 * 4", "12",
    "20 / 5", "4",
    "7 + 8", "15",
    "15 - 7", "8",
    "6 * 6", "36",
    "30 / 6", "5",
    "12 + 9", "21",
    "25 - 10", "15",
    "5 * 7", "35",
    "40 / 8", "5",
    "13 + 6", "19",
    "18 - 9", "9",
    "4 * 8", "32",
    "50 / 10", "5",
    "11 + 12", "23",
    "30 - 12", "18"};
        List<string> emojik = new List<string> {":)",":)", ":D",":D", ">:(", ">:(",":c",":C", ":(",":(", ":O", ":O", "¯\\_(ツ)_/¯", "¯\\_(ツ)_/¯", ":>", ":>" };
        List<string> fovarosok = new List<string> {
    "Magyarország", "Budapest",
    "Németország", "Berlin",
    "Franciaország", "Párizs",
    "Olaszország", "Róma",
    "Spanyolország", "Madrid",
    "Portugália", "Lisszabon",
    "Ausztria", "Bécs",
    "Csehország", "Prága",
    "Lengyelország", "Varsó",
    "Görögország", "Athén",
    "Norvégia", "Oslo",
    "Svédország", "Stockholm",
    "Finnország", "Helsinki",
    "Dánia", "Koppenhága",
    "Hollandia", "Amszterdam",
    "Belgium", "Brüsszel",
    "Írország", "Dublin",
    "Svájc", "Bern"
};
        List<string> f1 = new List<string> {
    "Verstappen", "Redbull",
    "Norris", "McLaren",
    "Leclerc", "Ferrari",
    "Piastri", "McLaren",
    "Hamilton", "Ferrari",
    "Russell", "Mercedes",
    "Alonso", "Aston Martin",
    "Stroll", "Aston Martin",
    "Sainz", "Williams",
    "Albon", "Williams",
    "Tsunoda", "Reserve",
    "Gasly", "Alpine",
    "Ocon", "Haas",
    "Bearman", "Haas",
    "Hülkenberg", "Audi",
    "Antonelli", "Mercedes",
    "Colapinto", "Alpine",
    "Hadjar", "Redbull"
};
        public MainWindow()
        {
            InitializeComponent();
            lbox_meret.ItemsSource=meret;
            lbox_palyak.ItemsSource=palyak;
        }



        private void GenerateGrid()
        {
            int gridSize =0;
            int valasztottMeret = meret.IndexOf(lbox_meret.SelectedItem.ToString());
            switch (valasztottMeret) 
            { 
                case 0:
                    gridSize=4;
                    break;
                case 1:
                    gridSize = 16;
                    break;
                case 2:
                    gridSize = 36;
                    break;
                default:
                    gridSize=4;
                    break;
            }
            string[] items = new string[gridSize];
            int valasztottPalya = palyak.IndexOf(lbox_palyak.SelectedItem.ToString());
            switch (valasztottPalya)
            {
                case 0:
                    f1.CopyTo(0, items, 0, gridSize);
                    break;
                case 1:
                    emojik.CopyTo(0, items, 0, gridSize);
                    emojik.CopyTo(0, items, 0, gridSize);
                    break;
                case 2:
                    fovarosok.CopyTo(0, items, 0, gridSize);
                    break;
                case 3:
                    matek.CopyTo(0, items, 0, gridSize);
                    break;
            }
            items.Shuffle();
            for (int i = 0; i < 4; i++)
            {
                gameGrid.RowDefinitions.Add(new RowDefinition());
                gameGrid.ColumnDefinitions.Add(new ColumnDefinition());

            }
            int counter = 0;
            for (int i = 0; i < Math.Sqrt(gridSize); i++) 
            {
                for (int j = 0; j < Math.Sqrt(gridSize); j++)
                {
                    Button gomb = new Button
                    {
                        Width=20,
                        Height=20,
                        Name= items[counter],
                        Content = "?",
                        FontSize = 20,
                        FontWeight = FontWeights.Bold,
                        Margin = new Thickness(3)
                    };
                    gomb.Click += Btn_Click;
                    Grid.SetRow(gomb, i);
                    Grid.SetColumn(gomb, j);
                    gameGrid.Children.Add(gomb);
                    counter++;
                }
            }
        }
        private void Btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_start_Click(object sender, RoutedEventArgs e)
        {
            GenerateGrid();
            menu.Visibility = Visibility.Collapsed;
        }
    }
}