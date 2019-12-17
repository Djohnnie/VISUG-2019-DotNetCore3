using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace _06_AsyncStreams.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void startSyncButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var dataPoint in FetchData())
            {
                syncOutputTextBlock.Text = $"{dataPoint}";
                syncProgressBar.Value = dataPoint;
            }
        }

        private async void startAsyncButton_Click(object sender, RoutedEventArgs e)
        {
            await foreach (var dataPoint in FetchDataAsync())
            {
                asyncOutputTextBlock.Text = $"{dataPoint}";
                asyncProgressBar.Value = dataPoint;
            }
        }

        private static IEnumerable<int> FetchData()
        {
            for (int i = 1; i <= 10; i++)
            {
                Thread.Sleep(100);
                yield return i;
            }
        }

        private static async IAsyncEnumerable<int> FetchDataAsync()
        {
            for (int i = 1; i <= 10; i++)
            {
                await Task.Delay(100);
                yield return i;
            }
        }
    }
}