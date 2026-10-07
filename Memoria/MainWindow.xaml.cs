using System.Diagnostics.Metrics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
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
        int valasztottPalya = 0; //listboxbol valasztott palya indexe a palyak listabol
        int lepesek = 1;//felforditasok szama+1 
        int parok = 0;//megtalalt parok szama
        int rosszLepesek = 0;//rossz parok szama
        int elozoPontszam = 0;//elozo jatek pontszama, a felfedes megjelenitesehez van hasznalva
        int remainingItems;//a kartyak szama alapol, -2 mindenn jo valaszkor, ha 0 vege a jateknak
        List<string> palyak = new List<string> { "F1", "emojik", "Fővárosok", "matek" };
        Button choiceOne = null;//elso felforditott kartya
        Button choiceTwo = null;//masodik felforditott kartya
        List<string> meret = new List<string> { "2x2", "4x4", "6x6" };
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
            lbox_meret.ItemsSource = meret;
            lbox_palyak.ItemsSource = palyak;
        }
        private void GenerateGrid()
        {
            int gridSize = 0;
            switch (lbox_meret.SelectedIndex)
            {
                case 0:
                    gridSize = 4;
                    break;
                case 1:
                    gridSize = 16;
                    break;
                case 2:
                    gridSize = 36;
                    break;
            }
            remainingItems = gridSize;
            string[] items = new string[gridSize];//palya meretene nagysagu tomb a gomboknak
            valasztottPalya = lbox_palyak.SelectedIndex;
            switch (valasztottPalya)//bemásolja az items listába az első gridSize elemet az adott listából
            {
                case 0:
                    f1.CopyTo(0, items, 0, gridSize);//első 0: az index amitől az index tömbbe másol, második 0: az index amitől másolja a listából az elemeket
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
            rng.Shuffle(items);//összekeveri a tömböt
            for (int i = 0; i < Math.Sqrt(gridSize); i++)
            {
                gameGrid.RowDefinitions.Add(new RowDefinition());
                gameGrid.ColumnDefinitions.Add(new ColumnDefinition());

            }
            int counter = 0;//név adáshoz kell
            for (int i = 0; i < Math.Sqrt(gridSize); i++)
            {
                for (int j = 0; j < Math.Sqrt(gridSize); j++)
                {
                    Button gomb = new Button
                    {
                        Height = 75,
                        Tag = items[counter],//tag lesz a felfordított kártya contentje
                        Name = "btn_" + counter,//kell, hogy ne tudja a játékos egy pár helyett kétszer ugyanazt a gombot felfordítani
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
        private async void Btn_Click(object sender, RoutedEventArgs e)//async mert így dinamikusabb a kód, nem baj ha gyorsan fordít a játékos
        {
            Button btn = sender as Button;
            btn.Content = btn.Tag.ToString();
            await Task.Delay(1000);
            if (lepesek % 2 != 0)
            {
                choiceOne = btn;
            }
            else
            {
                choiceTwo = btn;
                BtnHandling();
            }
            lepesek++;
        }
        private void BtnHandling()
        {
            int indexOne = 0;//az első felfordított kártya indexe a választott pályához tartozó listában
            int indexTwo = 0;//az második felfordított kártya indexe a választott pályához tartozó listában
            switch (valasztottPalya)
            {
                case 0:
                    indexOne = f1.IndexOf(choiceOne.Tag.ToString());
                    indexTwo = f1.IndexOf(choiceTwo.Tag.ToString());
                    break;
                case 1:
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

            if (valasztottPalya==0&& ((indexOne % 2 == 0 && (indexTwo - 1 == indexOne || indexTwo + 1 == indexOne) || (indexTwo % 2 == 0 && (indexOne - 1 == indexTwo || indexOne + 1 == indexTwo)))))
                //ha az első kártya indexe páros, akkor a -1/+1. index kell legyen a második kártya indexe és vica verza
            {
                choiceOne.Visibility = Visibility.Hidden;
                choiceTwo.Visibility = Visibility.Hidden;
                parok++;
                remainingItems -= 2;
            }
            else if (valasztottPalya==1&&(choiceOne.Tag== choiceTwo.Tag && choiceTwo.Name != choiceOne.Name))
            //ha nem kétszer ugyanarra a kártyára nyomunk és a két jártya tagje megegyezik
            {
                choiceOne.Visibility = Visibility.Hidden;
                choiceTwo.Visibility = Visibility.Hidden;
                parok++;
                remainingItems -= 2;
            }
            else if (valasztottPalya > 1&& ((indexOne % 2 == 0 && indexTwo - 1 == indexOne) || (indexOne % 2 != 0 && indexTwo + 1 == indexOne)))
            //ha az első kártya indexe páros, akkor a -1. index kell legyen a második kártya indexe és vica verza
            {
                choiceOne.Visibility = Visibility.Hidden;
                choiceTwo.Visibility = Visibility.Hidden;
                parok++;
                remainingItems -= 2;   
            }
            else//ha nem jó párt választott
            {
                rosszLepesek++;
                choiceOne.Content = "?";
                choiceTwo.Content = "?";
            }
            score.Text = "Párok: " + parok;
            tries.Text = "Lépések száma: " + (lepesek + 1) / 2;
            if (remainingItems == 0)
            {
                elozoPontszam = parok * 100 - (rosszLepesek * 50);
                MessageBox.Show("Gratulálok! Nyertél! A pontszámod: " + elozoPontszam);
                menu.Visibility = Visibility.Visible;
                gameBar.Visibility = Visibility.Hidden;
                gameGrid.Children.Clear();
                gameGrid.RowDefinitions.Clear();
                gameGrid.ColumnDefinitions.Clear();
                parok = 0;
                rosszLepesek = 0;
            }
            choiceOne = null;
            choiceTwo = null;
        }
        private void btn_start_Click(object sender, RoutedEventArgs e)
        {
            if (lbox_meret.SelectedItem == null || lbox_palyak.SelectedItem == null)
            {
                MessageBox.Show("Kérlek válassz egy pályát és egy méretet!");
            }
            else
            {
                lepesek = 1;
                GenerateGrid();
                menu.Visibility = Visibility.Hidden;
                gameBar.Visibility = Visibility.Visible;
                if (elozoPontszam < 0)//segítség gombo(ka)t revealeli ha előző körben kevés pontod volt
                {
                    btn_reveal.Visibility = Visibility.Visible;
                }
                reveal();
            }
        }
        private async void reveal()//async a click működéséből kiindulva
        {
            foreach (Button btn in gameGrid.Children)
            {
                btn.Content = btn.Tag.ToString();
                await Task.Delay(100);
            }
            await Task.Delay(200);
            foreach (Button btn in gameGrid.Children)
            {
                btn.Content = "?";
            }
        }
        private void btn_reveal_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            reveal();
            btn.Visibility = Visibility.Hidden;
        }
    }
}