using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using _12_WindowsDesktop.gRPC.Contract;
using Grpc.Core;
using Grpc.Net.Client;

namespace _12_WindowsDesktop.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Point _previousLocation;
        private Streamer.StreamerClient _client;
        private AsyncDuplexStreamingCall<StreamRequest, StreamResponse> _duplexStream;
        private string _clientId = $"{Guid.NewGuid()}";
        private bool _isDragging;

        public MainWindow()
        {
            InitializeComponent();

            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            _client = new Streamer.StreamerClient(channel);

            _duplexStream = _client.Do(new CallOptions());
        }

        private async void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            await foreach (var message in _duplexStream.ResponseStream.ReadAllAsync())
            {
                _draggableRectangle.SetValue(Canvas.LeftProperty, (double)message.X);
                _draggableRectangle.SetValue(Canvas.TopProperty, (double)message.Y);
            }
        }

        private async void UIElement_OnMouseMove(object sender, MouseEventArgs e)
        {
            var position = e.GetPosition(_draggableRectangle);

            if (!_isDragging && e.LeftButton == MouseButtonState.Pressed)
            {
                _isDragging = true;
                double xOffset = position.X - _previousLocation.X;
                double yOffset = position.Y - _previousLocation.Y;
                _previousLocation = position;

                var left = (double)_draggableRectangle.GetValue(Canvas.LeftProperty);
                var top = (double)_draggableRectangle.GetValue(Canvas.TopProperty);

                _draggableRectangle.SetValue(Canvas.LeftProperty, left + xOffset);
                _draggableRectangle.SetValue(Canvas.TopProperty, top + yOffset);

                await _duplexStream.RequestStream.WriteAsync(
                    new StreamRequest { X = (int)(left + xOffset), Y = (int)(top + yOffset), ClientId = _clientId });
                _isDragging = false;
            }

            _previousLocation = position;
        }
    }
}