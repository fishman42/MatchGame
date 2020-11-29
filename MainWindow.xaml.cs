using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace MatchGame
{
    public partial class MainWindow : Window
    {
        // Variables
        DispatcherTimer timer = new DispatcherTimer();
        int tenthsOfSecondsElapsed;
        int matchesFound;
        int clicks = 0;
        TextBlock currentTextBlock;
        TextBlock lastTextBlock;
        bool findingMatch = false;

        // Main thread
        public MainWindow()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;
            SetUpGame();
        }

        // Converts timer to string and displays
        private void Timer_Tick(object sender, EventArgs e)
        {
            tenthsOfSecondsElapsed++;
            timeTextBlock.Text = (tenthsOfSecondsElapsed / 10F).ToString("0.0s");
            if (matchesFound == 8)
            {
                timer.Stop();
                timeTextBlock.Text = timeTextBlock.Text + " - Play again?";
            }
        }

        // Randomly assigns icons to locations within grid and resets timer
        private void SetUpGame()
        {
            tenthsOfSecondsElapsed = 0;
            matchesFound = 0;
            timer.Start();
            List<string> animalEmoji = new List<string>()
            {
                "🐔","🐔",
                "🐶","🐶",
                "🐷","🐷",
                "🐸","🐸",
                "🐼","🐼",
                "🐴","🐴",
                "🐯","🐯",
                "🐺","🐺"
            };
            Random random = new Random();
            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
            {
                if (textBlock.Name != "timeTextBlock")
                {
                    textBlock.Background = Brushes.Black;
                    int index = random.Next(animalEmoji.Count);
                    string nextEmoji = animalEmoji[index];
                    textBlock.Text = nextEmoji;
                    animalEmoji.RemoveAt(index);
                }
            }
        }

        // Handles two clicks, compares them, and either leaves them revealed or hides icons
        public async void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (clicks != 2)
            { 
                currentTextBlock = sender as TextBlock;
                if (findingMatch == false)
                {
                    clicks++;
                    lastTextBlock = currentTextBlock;
                    currentTextBlock.Background = null;
                    findingMatch = true;
                }
                else if (currentTextBlock.Text == lastTextBlock.Text)
                {
                    clicks++;
                    currentTextBlock.Background = null;
                    matchesFound++;
                    findingMatch = false;
                    clicks = 0;
                }
                else
                {
                    clicks++;
                    currentTextBlock.Background = null;
                    await Task.Delay(700);
                    currentTextBlock.Background = Brushes.Black;
                    lastTextBlock.Background = Brushes.Black;
                    findingMatch = false;
                    clicks = 0;
                }
            }
        }

        // Restarts game if timer clicked
        private void TimeTextBlock_MouseDown(object sender, MouseButtonEventArgs e) 
        {
            if (matchesFound == 8)
            {
                SetUpGame();
            }
        }
    }
}
