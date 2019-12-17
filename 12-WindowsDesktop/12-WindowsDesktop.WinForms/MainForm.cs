using System;
using System.Drawing;
using System.Windows.Forms;
using _12_WindowsDesktop.gRPC.Contract;
using Grpc.Core;
using Grpc.Net.Client;

namespace _12_WindowsDesktop.WinForms
{
    public partial class MainForm : Form
    {
        private readonly PictureBox _draggableBox = new PictureBox();
        private Point _previousLocation;
        private Streamer.StreamerClient _client;
        private AsyncDuplexStreamingCall<StreamRequest, StreamResponse> _duplexStream;
        private string _clientId = $"{Guid.NewGuid()}";

        public MainForm()
        {
            InitializeComponent();

            _draggableBox.Left = 10;
            _draggableBox.Top = 10;
            _draggableBox.Width = 100;
            _draggableBox.Height = 100;
            _draggableBox.BackColor = Color.Red;
            _draggableBox.MouseMove += DraggableBox_MouseMove;
            Controls.Add(_draggableBox);
            Load += MainForm_Load;

            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            _client = new Streamer.StreamerClient(channel);

            _duplexStream = _client.Do(new CallOptions());
        }

        private async void MainForm_Load(object sender, System.EventArgs e)
        {
            await foreach (var message in _duplexStream.ResponseStream.ReadAllAsync())
            {
                _draggableBox.SetBounds(message.X, message.Y, 100, 100);
            }
        }

        private bool _isDragging;

        private async void DraggableBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isDragging && e.Button == MouseButtons.Left)
            {
                _isDragging = true;
                int xOffset = e.X - _previousLocation.X;
                int yOffset = e.Y - _previousLocation.Y;
                _previousLocation = e.Location;
                _draggableBox.SetBounds(_draggableBox.Left + xOffset, _draggableBox.Top + yOffset, 100, 100);

                await _duplexStream.RequestStream.WriteAsync(
                    new StreamRequest { X = _draggableBox.Left, Y = _draggableBox.Top, ClientId = _clientId });
                _isDragging = false;
            }

            _previousLocation = new Point(e.X, e.Y);

        }
    }
}