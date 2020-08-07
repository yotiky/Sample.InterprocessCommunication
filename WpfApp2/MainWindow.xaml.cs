using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Security.Principal;
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

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Run(async () =>
            {
                using (var stream = new NamedPipeClientStream("NamedPipe"))
                {
                    await stream.ConnectAsync();

                    using (var reader = new StreamReader(stream))
                    {
                        while (stream.IsConnected)
                        {
                            var str = await reader.ReadLineAsync();
                            if (str != null)
                            {
                                Dispatcher.Invoke(() => textBlock.Text += "Data :" + str + "\r\n");
                            }
                            Thread.Sleep(100);
                        }
                    }
                }
            });
            textBlock.Text += "End of sample.";
        }
    }
}
