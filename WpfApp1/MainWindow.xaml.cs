using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
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

namespace WpfApp1
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
			using (var sharedMemory = MemoryMappedFile.OpenExisting("SharedMemory"))
			{
				await Task.Run(() =>
				{
					var latestMsg = string.Empty;
					while (true)
					{
						using (var accessor = sharedMemory.CreateViewAccessor())
						{
							var size = accessor.ReadInt32(0);
							var data = new char[size];
							accessor.ReadArray<char>(sizeof(int), data, 0, data.Length);
							var str = new string(data);
							if (latestMsg != str)
							{
								latestMsg = str;
								Dispatcher.Invoke(() => textBlock.Text += ("Data :" + str + "\r\n"));
							}
						}

						Thread.Sleep(100);
					}
				});
			}
		}
    }
}
