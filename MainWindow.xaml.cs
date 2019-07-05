using System;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        void Task()
        {
            // подключаемся к удаленному хосту
            string message = input.Text;
            byte[] data = Encoding.ASCII.GetBytes(message);
            socket.Send(data);
            // получаем ответ
            data = new byte[256]; // буфер для ответа
            StringBuilder builder = new StringBuilder();
            int bytes = 0; // количество полученных байт

            if (socket.Connected)
            {
                bytes = socket.Receive(data, data.Length, 0);
                builder.Append(Encoding.ASCII.GetString(data, 0, bytes));
                MessageBox.Show("Результат: " + builder.ToString(), "");
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Task();
        }

        private void Input_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9./-]");
            e.Handled = regex.IsMatch(e.Text);
        }

        public void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int port = 8888;
            string address = "192.168.12.142";
            if (!socket.Connected)
            {
                try
                {
                    IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);
                    socket.Connect(ipPoint);
                    connect_btn.IsEnabled = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (socket.Connected)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
                this.Close();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (socket.Connected)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
        }
    }
}
