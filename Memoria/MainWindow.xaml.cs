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
        int valasztottPalya = 0;
        int lepesek = 1;
        int pontszam = 0;

        int remainingItems;
        List<string> palyak = new List<string> {"F1","emojik","Fővárosok","matek"};
        Button choiceOne = null;
        Button choiceTwo = null;
        List<string> meret = new List<string> {"2x2","4x4","6x6"};
        List<string> matek = new List<string> {"2 + 3", "5",
    "10 - 4", "6",
    "3 * 4", "12",
    "20 / 5", "4",
    "7 - 8", "-1",
    "15 - 7", "8",
    "6 * 6", "36",
    "60 / 6", "10",
    "12 + 9", "21",
    "25 - 10", "15",
    "5 * 7", "35",
    "40 / 8", "5",
    "13 + 6", "19",
    "18 - 9", "9",
    "4 * 8", "32",
    "50 + 10", "60",
    "11 + 12", "23",
    "30 - 12", "18"};
        List<string> emojik = new List<string> {
            "🙂","🙂",
            "😀","😀",
            "😃","😃",
            "😅","😅",
            "😂","😂",
            "😇","😇",
            "😍","😍",
            "🤔","🤔",
            "😎","😎",
            "🤩","🤩",
            "🤯","🤯",
            "😴","😴",
            "😈","😈",
            "🤖","🤖",
            "👻","👻",
            "🎃","🎃",
            "🐶","🐶",
            "🤓","🤓"
        };
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
    "Hadjar", "Redbull",
    "Norris", "McLaren",
    "Piastri", "McLaren",
    "Leclerc", "Ferrari",
    "Hamilton", "Ferrari",
    "Russell", "Mercedes",
    "Antonelli", "Mercedes",
    "Alonso", "Aston Martin",
    "Stroll", "Aston Martin",
    "Sainz", "Williams",
    "Albon", "Williams",
    "Ocon", "Haas",
    "Bearman", "Haas",
    "Bortoleto", "Audi",
    "Hülkenberg", "Audi",
    "Gasly", "Alpine",
    "Colapinto", "Alpine"
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
            remainingItems = gridSize;
            string []items = new string[gridSize];
            valasztottPalya = palyak.IndexOf(lbox_palyak.SelectedItem.ToString());
            switch (valasztottPalya)
            {
                case 0:
                    f1.CopyTo(0, items, 0, gridSize);
                    break;
                case 1:
                    emojik.CopyTo(0, items, 0, gridSize);
                    break;
                case 2:
                    fovarosok.CopyTo(0, items, 0, gridSize);
                    break;
                case 3:
                    matek.CopyTo(0, items, 0, gridSize);
                    break;
            }
            Random rng = new Random();
            rng.Shuffle(items);
            for (int i = 0; i < Math.Sqrt(gridSize); i++)
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
                        Height=75,
                        Tag = items[counter],
                        Name= "btn_" + counter,
                        Content = "?",
                        FontSize = 20,
                        FontWeight = FontWeights.Bold,
                    };
                    gomb.Click += Btn_Click;
                    Grid.SetRow(gomb, i);
                    Grid.SetColumn(gomb, j);
                    gameGrid.Children.Add(gomb);
                    counter++;
                }
            }
        }
        async void Btn_Click(object sender, RoutedEventArgs e)//async mert lusta vagyok jobban optimizalni prolly
        {
            Button btn = sender as Button;
            btn.Content = btn.Tag.ToString();
            await Task.Delay(1000);
            if (lepesek %2!=0)
            {
                choiceOne = btn;
            }
            else 
            {
                choiceTwo = btn;
                choiceTwo.Content = choiceTwo.Tag.ToString();
                BtnHandling();
            }
            lepesek++;

        }

        private void BtnHandling()
        {
            tries.Text = "Lépések száma: " + (lepesek+1) / 2;
            int indexOne=0;
            int indexTwo=0;
            bool c1;
            bool c2;
            switch (valasztottPalya)
            {
                case 0:
                    indexOne = f1.IndexOf(choiceOne.Tag.ToString());
                    indexTwo = f1.IndexOf(choiceTwo.Tag.ToString());
                    c1= indexOne % 2 == 0&&(indexTwo - 1 == indexOne ||indexTwo+1==indexOne);
                    c2= indexTwo % 2 == 0&&(indexOne - 1 == indexTwo ||indexOne+1==indexTwo);
                    if (c1 || c2)
                    {
                        choiceOne.Visibility = Visibility.Hidden;
                        choiceTwo.Visibility = Visibility.Hidden;
                        pontszam+=100;
                        remainingItems -= 2;
                    }
                    else
                    {
                        choiceOne.Content = "?";
                        choiceTwo.Content = "?";
                    }
                    break;
                case 1:
                    if (choiceOne.Tag.ToString() == choiceTwo.Tag.ToString() && choiceTwo.Name != choiceOne.Name)
                    {
                        choiceOne.Visibility = Visibility.Hidden;
                        choiceTwo.Visibility = Visibility.Hidden;
                        pontszam+=100;
                        remainingItems -= 2;
                    }
                    else
                    {
                        choiceOne.Content = "?";
                        choiceTwo.Content = "?";
                    }
                    break;
                case 2:
                    indexOne = fovarosok.IndexOf(choiceOne.Tag.ToString());
                    indexTwo = fovarosok.IndexOf(choiceTwo.Tag.ToString());
                    break;
                case 3:
                    indexOne = matek.IndexOf(choiceOne.Tag.ToString());
                    indexTwo = matek.IndexOf(choiceTwo.Tag.ToString());
                    break;
            }
            if (valasztottPalya>1)
            {
                c1 = indexOne % 2 == 0 && indexTwo - 1 == indexOne;
                c2 = indexOne % 2 != 0 && indexTwo + 1 == indexOne;
                if (c1 || c2)
                {
                    choiceOne.Visibility = Visibility.Hidden;
                    choiceTwo.Visibility = Visibility.Hidden;
                    pontszam+=100;
                    remainingItems -= 2;
                }
                else
                {
                    choiceOne.Content = "?";
                    choiceTwo.Content = "?";
                }
            }
            choiceOne = null;
            choiceTwo = null;
            score.Text = "Pontszám: " + pontszam;
            if (remainingItems == 0)
            {
                MessageBox.Show("Gratulálok! Nyertél! A pontszámod: " + pontszam);
                menu.Visibility = Visibility.Visible;
                stats.Visibility = Visibility.Hidden;
                gameGrid.Children.Clear();
                gameGrid.RowDefinitions.Clear();
                gameGrid.ColumnDefinitions.Clear();
                lepesek = 1;
                pontszam = 0;
            }
        }
        private void btn_start_Click(object sender, RoutedEventArgs e)
        {
            if (lbox_meret.SelectedItem == null || lbox_palyak.SelectedItem == null)
            {
                MessageBox.Show("Kérlek válassz egy pályát és egy méretet!");
            }
            else
            {
            GenerateGrid();
            menu.Visibility = Visibility.Hidden;
            stats.Visibility = Visibility.Visible;
            }
        }
    }
}