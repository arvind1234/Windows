using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TicTacToe
{
    public class TicTacToeButtonData : DependencyObject
    {
        public static readonly DependencyProperty XProperty = DependencyProperty.RegisterAttached("X", typeof(int), typeof(TicTacToeButtonData), new PropertyMetadata(null));
        public static readonly DependencyProperty YProperty = DependencyProperty.RegisterAttached("Y", typeof(int), typeof(TicTacToeButtonData), new PropertyMetadata(null));


        public static int GetX(UIElement element)
        {
            return (int)element.GetValue(XProperty);
        }

        public static void SetX(UIElement element, int value)
        {
            element.SetValue(XProperty, value);
        }
        public static int GetY(UIElement element)
        {
            return (int)element.GetValue(YProperty);
        }
        public static void SetY(UIElement element, int value)
        {
            element.SetValue(YProperty, value);
        }
    }
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        enum Users
        {
            X = 0,
            O
        }

        enum GameState
        {
            Incomplete,
            Draw,
            XWins,
            OWins
        }

        int currentUser;
        int[][] board = new int[3][];

        private GameState getGameState()
        {

            if (board[1][1] != -1 && (board[0][0] == board[1][1] && board[1][1] == board[2][2] || board[0][2] == board[1][1] && board[1][1] == board[2][0]))
            {
                return board[1][1] == (int)Users.X ? GameState.XWins : GameState.OWins;
            }

            if (board[0][0] != -1 && board[0][0] == board[0][1] && board[0][1] == board[0][2])
            {
                return board[0][0] == (int)Users.X ? GameState.XWins : GameState.OWins;
            }

            if (board[1][0] != -1 && board[1][0] == board[1][1] && board[1][1] == board[1][2])
            {
                return board[1][0] == (int)Users.X ? GameState.XWins : GameState.OWins;
            }

            if (board[2][0] != -1 && board[2][0] == board[2][1] && board[2][1] == board[2][2])
            {
                return board[2][0] == (int)Users.X ? GameState.XWins : GameState.OWins;
            }

            if (board[0][0] != -1 && board[0][0] == board[1][0] && board[1][0] == board[2][0])
            {
                return board[0][0] == (int)Users.X ? GameState.XWins : GameState.OWins;
            }

            if (board[0][1] != -1 && board[0][1] == board[1][1] && board[1][1] == board[2][1])
            {
                return board[0][1] == (int)Users.X ? GameState.XWins : GameState.OWins;
            }

            if (board[0][2] != -1 && board[0][2] == board[1][2] && board[1][2] == board[2][2])
            {
                return board[0][2] == (int)Users.X ? GameState.XWins : GameState.OWins;
            }

            if (Array.IndexOf(board[0], -1) > -1 || Array.IndexOf(board[1], -1) > -1 || Array.IndexOf(board[2], -1) > -1)
            {
                return GameState.Incomplete;
            }

            return GameState.Draw;
        }

        private void resetGameState()
        {
            currentUser = (int)Users.X;
            for (var i = 0; i < 3; i++)
            {
                board[i] = new int[3] { -1, -1, -1 };
            }

            button1.Content = "-";
            button2.Content = "-";
            button3.Content = "-";
            button4.Content = "-";
            button5.Content = "-";
            button6.Content = "-";
            button7.Content = "-";
            button8.Content = "-";
            button9.Content = "-";

        }
        public MainPage()
        {
            this.InitializeComponent();
            resetGameState();
        }

        private void CommandInvokedHandler(IUICommand command)
        {
            this.resetGameState();
        }

        private async void ShowMessageDialogAsync(string message)
        {
            var messageDialog = new MessageDialog(message);

            // Add commands and set their callbacks; both buttons use the same callback function instead of inline event handlers
            messageDialog.Commands.Add(new UICommand(
                "Dismiss",
                new UICommandInvokedHandler(this.CommandInvokedHandler)));

            await messageDialog.ShowAsync();
        }
        private void gameButtonClick(object sender, RoutedEventArgs e)
        {
            int x, y;
            Button b = sender as Button;
            x = TicTacToeButtonData.GetX(b);
            y = TicTacToeButtonData.GetY(b);
            System.Diagnostics.Debug.WriteLine($"Button {x},{y} clicked");

            if (board[x][y] != -1)
            {
                System.Diagnostics.Debug.WriteLine("Clicked already taken cell, try again");
                return;
            }

            board[x][y] = currentUser;
            b.Content = (currentUser == (int)Users.X ? Users.X.ToString() : Users.O.ToString());

            var gameState = getGameState();
            switch (gameState)
            {
                case GameState.Incomplete:
                    System.Diagnostics.Debug.WriteLine("Game goes on...");
                    break;
                case GameState.Draw:
                    ShowMessageDialogAsync("Draw!");
                    break;
                case GameState.XWins:
                    ShowMessageDialogAsync("X Wins!");
                    break;
                case GameState.OWins:
                    ShowMessageDialogAsync("O Wins!");
                    break;
            }
            currentUser ^= 1;
        }

        private void resetClick(object sender, RoutedEventArgs e)
        {
            resetGameState();
        }
    }
}
