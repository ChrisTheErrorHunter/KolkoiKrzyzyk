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
using System.Net.Sockets;
using System.ComponentModel;
using System.Threading;

namespace KółkoiKrzyżyk
{
    public partial class MultiPlayerWindow : Window
    {
        private Socket socket;
        bool ifisHost;
        //private BackgroundWorker moveReciver = new BackgroundWorker();
        private TcpListener server = null;
        private TcpClient client;
        private StanyKafelka[] stanyKafelka;
        private bool tura, turabegin;
        int numerTury = 0, P1Score = 0, P2Score = 0;
        private bool gameOver;
        private StanyKafelka playerSign, oponnentSign;
        public MultiPlayerWindow(bool isHost, string IPv4)
        {
            ifisHost = isHost;
            InitializeComponent();
            //moveReciver.DoWork += MoveReciver_DoWork;
            if (isHost)
            {
                server = new TcpListener(System.Net.IPAddress.Any, 6969);
                server.Start();
                socket = server.AcceptSocket();
                playerSign = StanyKafelka.O;
                oponnentSign = StanyKafelka.X;
                turabegin = false;
            }
            else
            {
                playerSign = StanyKafelka.X;
                oponnentSign = StanyKafelka.O;
                turabegin = true;
                try
                {
                    client = new TcpClient(IPv4, 6969);
                    socket = client.Client;
                    //moveReciver.RunWorkerAsync();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    Close();
                }
            }
            new Thread(() =>
            {
                while (true)
                {
                    ReciveMove();
                }
            }).Start();
            label1.Content = "You are " + playerSign;
            NewGame();
        }


        //private void MoveReciver_DoWork(object sender, DoWorkEventArgs e)
        //{
        //label1.Content = "Worker";
        //EndGame();

        //ReciveMove();
        //}

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
            NewGameButton.IsEnabled = false;
            NextRoundButton.IsEnabled = false;
            if(tura) TurnLabel.Content = playerSign + " Turn";
            else TurnLabel.Content = oponnentSign + " Turn";

        }
        private void DisconnetButton_Click(object sender, RoutedEventArgs e)
        {
            var index = 10;
            if (ifisHost)
            {
                byte[] buffer = BitConverter.GetBytes(index);
                socket.Send(buffer);
                server.Stop();
                Close();
            }
            else
            {
                byte[] buffer = BitConverter.GetBytes(index);
                socket.Send(buffer);
                client.Close();
                Close();
            }
        }
        private void NextRoundButton_Click(object sender, RoutedEventArgs e)
        {
            var index = 11;
            byte[] buffer = BitConverter.GetBytes(index);
            socket.Send(buffer);
            NewGame();
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            
            var index = 12;
            byte[] buffer = BitConverter.GetBytes(index);
            socket.Send(buffer);
            P1Score = 0;
            P2Score = 0;
            labelP1.Content = "X Score: 0";
            NewGame();
        }

        private void ScoreUpdate(int x)
        {
            if (stanyKafelka[x] == StanyKafelka.X)
            {
                P1Score++;
                labelP1.Content = "X Score: " + P1Score;
                TurnLabel.Content = "X WINS!";
            }
            else
            {
                P2Score++;
                labelP2.Content = "O Score: " + P2Score;
                TurnLabel.Content = "O WINS!";
            }
        }

        private void EndGame()
        {
            if (numerTury > 8)
            {
                TurnLabel.Content = "DRAW!";
                gameOver = true;
                turabegin = !turabegin;
                NewGameButton.IsEnabled = true;
                NextRoundButton.IsEnabled = true;
                return;
            }
            if (stanyKafelka[0] != StanyKafelka.Blank && (stanyKafelka[0] & stanyKafelka[1] & stanyKafelka[2]) == stanyKafelka[0])
            {
                B11.Background = B12.Background = B13.Background = Brushes.Green;
                gameOver = true;
                turabegin = !turabegin;
                ScoreUpdate(0);
                NewGameButton.IsEnabled = true;
                NextRoundButton.IsEnabled = true;
                return;
            }
            if (stanyKafelka[3] != StanyKafelka.Blank && (stanyKafelka[3] & stanyKafelka[4] & stanyKafelka[5]) == stanyKafelka[3])
            {
                B21.Background = B22.Background = B23.Background = Brushes.Green;
                gameOver = true;
                turabegin = !turabegin;
                ScoreUpdate(3);
                NewGameButton.IsEnabled = true;
                NextRoundButton.IsEnabled = true;
                return;
            }
            if (stanyKafelka[6] != StanyKafelka.Blank && (stanyKafelka[6] & stanyKafelka[7] & stanyKafelka[8]) == stanyKafelka[6])
            {
                B31.Background = B32.Background = B33.Background = Brushes.Green;
                gameOver = true;
                turabegin = !turabegin;
                ScoreUpdate(6);
                NewGameButton.IsEnabled = true;
                NextRoundButton.IsEnabled = true;
                return;
            }
            if (stanyKafelka[0] != StanyKafelka.Blank && (stanyKafelka[0] & stanyKafelka[3] & stanyKafelka[6]) == stanyKafelka[0])
            {
                B11.Background = B21.Background = B31.Background = Brushes.Green;
                gameOver = true;
                turabegin = !turabegin;
                ScoreUpdate(0);
                NewGameButton.IsEnabled = true;
                NextRoundButton.IsEnabled = true;
                return;
            }
            if (stanyKafelka[1] != StanyKafelka.Blank && (stanyKafelka[1] & stanyKafelka[4] & stanyKafelka[7]) == stanyKafelka[1])
            {
                B12.Background = B22.Background = B32.Background = Brushes.Green;
                gameOver = true;
                turabegin = !turabegin;
                ScoreUpdate(1);
                NewGameButton.IsEnabled = true;
                NextRoundButton.IsEnabled = true;
                return;
            }
            if (stanyKafelka[2] != StanyKafelka.Blank && (stanyKafelka[2] & stanyKafelka[5] & stanyKafelka[8]) == stanyKafelka[2])
            {
                B13.Background = B23.Background = B33.Background = Brushes.Green;
                gameOver = true;
                turabegin = !turabegin;
                ScoreUpdate(2);
                NewGameButton.IsEnabled = true;
                NextRoundButton.IsEnabled = true;
                return;
            }
            if (stanyKafelka[0] != StanyKafelka.Blank && (stanyKafelka[0] & stanyKafelka[4] & stanyKafelka[8]) == stanyKafelka[0])
            {
                B11.Background = B22.Background = B33.Background = Brushes.Green;
                gameOver = true;
                turabegin = !turabegin;
                ScoreUpdate(0);
                NewGameButton.IsEnabled = true;
                NextRoundButton.IsEnabled = true;
                return;
            }
            if (stanyKafelka[2] != StanyKafelka.Blank && (stanyKafelka[2] & stanyKafelka[4] & stanyKafelka[6]) == stanyKafelka[2])
            {
                B31.Background = B22.Background = B13.Background = Brushes.Green;
                gameOver = true;
                turabegin = !turabegin;
                ScoreUpdate(2);
                NewGameButton.IsEnabled = true;
                NextRoundButton.IsEnabled = true;
                return;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var column = Grid.GetColumn(button);
            var row = Grid.GetRow(button);
            var index = column + (row * 3);

            if (stanyKafelka[index] != StanyKafelka.Blank || gameOver || !tura) return;
            stanyKafelka[index] = playerSign;
            button.Content = stanyKafelka[index];
            byte[] buffer = BitConverter.GetBytes(index);
            socket.Send(buffer);
            //moveReciver.RunWorkerAsync();
            //zmiana tury gracza i gry raz dwa trzy
            tura = !tura;
            TurnLabel.Content = oponnentSign + " Turn";
            numerTury++;
            EndGame();

        }

        private void TilesUpdate()
        {
            //this.Dispatcher.BeginInvoke(() =>
            //{
            if (stanyKafelka[0] != StanyKafelka.Blank) B11.Content = stanyKafelka[0];
            if (stanyKafelka[1] != StanyKafelka.Blank) B12.Content = stanyKafelka[1];
            if (stanyKafelka[2] != StanyKafelka.Blank) B13.Content = stanyKafelka[2];
            if (stanyKafelka[3] != StanyKafelka.Blank) B21.Content = stanyKafelka[3];
            if (stanyKafelka[4] != StanyKafelka.Blank) B22.Content = stanyKafelka[4];
            if (stanyKafelka[5] != StanyKafelka.Blank) B23.Content = stanyKafelka[5];
            if (stanyKafelka[6] != StanyKafelka.Blank) B31.Content = stanyKafelka[6];
            if (stanyKafelka[7] != StanyKafelka.Blank) B32.Content = stanyKafelka[7];
            if (stanyKafelka[8] != StanyKafelka.Blank) B33.Content = stanyKafelka[8];
            //});
        }

        private void ChLabel1()
        {
            if (tura) TurnLabel.Content = playerSign + " Turn";
            else TurnLabel.Content = playerSign + " Turn";
        }
        
        private void ChPScoreLabels()
        {
            labelP1.Content = "X Score: 0";
            labelP2.Content = "O Score: 0";
        }

        private void RunByDispatcher(Action methodeName)
        {
            ThreadStart metoda = new ThreadStart(methodeName);
            this.Dispatcher.BeginInvoke(metoda);
        }

        private void ReciveMove()
        {
            byte[] buffer = new Byte[4];
            socket.Receive(buffer);
            int index;
            index = BitConverter.ToInt32(buffer, 0);
            if(index == 10)
            {
                if (ifisHost)
                {
                    MessageBox.Show("Przeciwnik rozłączył się!");
                    server.Stop();
                    RunByDispatcher(Close);
                    return;
                }
                else
                {
                    MessageBox.Show("Przeciwnik rozłączył się!");
                    client.Close();
                    RunByDispatcher(Close);
                    return;
                }
            }
            if(index == 11)
            {
                RunByDispatcher(NewGame);
                return;
            }
            if (index == 12)
            {
                P1Score = 0;
                P2Score = 0;
                RunByDispatcher(ChPScoreLabels);
                RunByDispatcher(NewGame);
                return;
            }
            stanyKafelka[index] = oponnentSign;
            RunByDispatcher(TilesUpdate);
            numerTury++;
            tura = !tura;
            RunByDispatcher(ChLabel1);
            RunByDispatcher(EndGame);
        }
    }
}
