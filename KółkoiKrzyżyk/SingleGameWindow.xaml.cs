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
using System.Windows.Shapes;

namespace KółkoiKrzyżyk
{
    /// <summary>
    /// Logika interakcji dla klasy SingleGameWindow.xaml
    /// </summary>
    public partial class SingleGameWindow : Window
    {
        private StanyKafelka[] stanyKafelka;
        private bool tura, turabegin = true;
        int numerTury = 0, P1Score = 0, P2Score = 0;
        private bool gameOver;
        public SingleGameWindow()
        {
            InitializeComponent();
            NewGame();
        }
        private void NewGame()
        {
            //Czyszczenie planszy
            stanyKafelka = new StanyKafelka[9];
            for (var i = 0; i < stanyKafelka.Length; i++) stanyKafelka[i] = StanyKafelka.Blank;
            //Ustawienie tury na gracza 1
            tura = turabegin;
            numerTury = 0;
            //czyszczenie guzików
            Container.Children.Cast<Button>().ToList().ForEach(button =>
            {
                button.Content = string.Empty;
                button.Background = Brushes.White;
            });
            //Otwarcie gameOver
            gameOver = false;
            if (tura) label1.Content = "X TURN!";
            else label1.Content = "O TURN!";


        }

        private void NextRoundButton_Click(object sender, RoutedEventArgs e)
        {
            NewGame();
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            P1Score = 0;
            P2Score = 0;
            labelP1.Content = "X Score: 0";
            labelP2.Content = "O Score: 0";
            NewGame();
        }

        private void ScoreUpdate(int x)
        {
            if (stanyKafelka[x] == StanyKafelka.X)
            {
                P1Score++;
                labelP1.Content = "X Score: " + P1Score;
                label1.Content = "X WINS!";
            }
            else
            {
                P2Score++;
                labelP2.Content = "O Score: " + P2Score;
                label1.Content = "O WINS!";
            }
        }

        private void EndGame()
        {
            //Horizontal
            if (numerTury > 8)
            {
                label1.Content = "DRAW!";
                gameOver = true;
                turabegin = !turabegin;
                return;
            }
            if (stanyKafelka[0] != StanyKafelka.Blank && (stanyKafelka[0] & stanyKafelka[1] & stanyKafelka[2]) == stanyKafelka[0])
            {
                B11.Background = B12.Background = B13.Background = Brushes.Green;
                gameOver = true;
                turabegin = !turabegin;
                ScoreUpdate(0);
                return;
            }
            if (stanyKafelka[3] != StanyKafelka.Blank && (stanyKafelka[3] & stanyKafelka[4] & stanyKafelka[5]) == stanyKafelka[3])
            {
                B21.Background = B22.Background = B23.Background = Brushes.Green;
                gameOver = true;
                turabegin = !turabegin;
                ScoreUpdate(3);
                return;
            }
            if (stanyKafelka[6] != StanyKafelka.Blank && (stanyKafelka[6] & stanyKafelka[7] & stanyKafelka[8]) == stanyKafelka[6])
            {
                B31.Background = B32.Background = B33.Background = Brushes.Green;
                gameOver = true;
                turabegin = !turabegin;
                ScoreUpdate(6);
                return;
            }
            if (stanyKafelka[0] != StanyKafelka.Blank && (stanyKafelka[0] & stanyKafelka[3] & stanyKafelka[6]) == stanyKafelka[0])
            {
                B11.Background = B21.Background = B31.Background = Brushes.Green;
                gameOver = true;
                turabegin = !turabegin;
                ScoreUpdate(0);
                return;
            }
            if (stanyKafelka[1] != StanyKafelka.Blank && (stanyKafelka[1] & stanyKafelka[4] & stanyKafelka[7]) == stanyKafelka[1])
            {
                B12.Background = B22.Background = B32.Background = Brushes.Green;
                gameOver = true;
                turabegin = !turabegin;
                ScoreUpdate(1);
                return;
            }
            if (stanyKafelka[2] != StanyKafelka.Blank && (stanyKafelka[2] & stanyKafelka[5] & stanyKafelka[8]) == stanyKafelka[2])
            {
                B13.Background = B23.Background = B33.Background = Brushes.Green;
                gameOver = true;
                turabegin = !turabegin;
                ScoreUpdate(2);
                return;
            }
            if (stanyKafelka[0] != StanyKafelka.Blank && (stanyKafelka[0] & stanyKafelka[4] & stanyKafelka[8]) == stanyKafelka[0])
            {
                B11.Background = B22.Background = B33.Background = Brushes.Green;
                gameOver = true;
                turabegin = !turabegin;
                ScoreUpdate(0);
                return;
            }
            if (stanyKafelka[2] != StanyKafelka.Blank && (stanyKafelka[2] & stanyKafelka[4] & stanyKafelka[6]) == stanyKafelka[2])
            {
                B31.Background = B22.Background = B13.Background = Brushes.Green;
                gameOver = true;
                turabegin = !turabegin;
                ScoreUpdate(2);
                return;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var column = Grid.GetColumn(button);
            var row = Grid.GetRow(button);
            var index = column + (row * 3);
            if (stanyKafelka[index] != StanyKafelka.Blank || gameOver) return;
            stanyKafelka[index] = tura ? StanyKafelka.X : StanyKafelka.O;
            button.Content = stanyKafelka[index];
            //zmiana tury gracza i gry
            tura = !tura;
            if (tura) label1.Content = "X TURN!";
            else label1.Content = "O TURN!";
            numerTury++;
            EndGame();

        }
    }
}
