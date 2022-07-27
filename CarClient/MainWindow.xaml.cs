using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
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

namespace CarClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window,INotifyPropertyChanged
    {
        public List<Car> cars;
        public List<Car> Cars { get => cars; set
            { 
                cars= value;
                NotifyPropertyChanged();
            } 
        }

        private Car s_car;
        public Car S_car { get => s_car; set
            {
                s_car = value;
                NotifyPropertyChanged();
            }
        }

        public string btn_name;
        public string Btn_name
        {
            get => btn_name; set
            {
                btn_name= value;
                NotifyPropertyChanged();
            }
        }
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        TcpClient tcpClient { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            tcpClient = new TcpClient();
            tcpClient.Connect(new IPEndPoint(IPAddress.Parse("192.168.0.109"), 63291));
            Btn_name = "Add";
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void Reload_Click(object sender, RoutedEventArgs e)
        {
            try
            {                
                if (tcpClient.Connected)
                {
                    var bw= new BinaryWriter(tcpClient.GetStream());
                    string msg = JsonConvert.SerializeObject(new Request() { rqst = "Get" });
                    bw.Write(msg);
                    var br= new BinaryReader(tcpClient.GetStream());
                    var crs = JsonConvert.DeserializeObject<List<Car>>(br.ReadString());
                    Cars = crs;
                }
            }
            catch (Exception ms)
            {

                MessageBox.Show(ms.Message);
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            tb_id.Text = "";
            tb_vendor.Text = "";
            tb_model.Text = "";
            tb_sc.Text = "";
            Btn_name="Add";
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            tb_id.Text = S_car.Id.ToString();
            tb_id.IsEnabled = false;
            tb_vendor.Text = S_car.Vendor.ToString();
            tb_model.Text = S_car.Model.ToString();
            tb_sc.Text = S_car.SeatCount.ToString();
            Btn_name = "Update";
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tcpClient.Connected)
                {
                    var bw = new BinaryWriter(tcpClient.GetStream());
                    string msg = JsonConvert.SerializeObject(new Request() { rqst = "Delete" ,car=S_car});
                    bw.Write(msg);                    
                }
            }
            catch (Exception ms)
            {

                MessageBox.Show(ms.Message);
            }
        }

        private void Add_Update_Click(object sender, RoutedEventArgs e)
        {
            if (Btn_name == "Add")
            {
                var bw = new BinaryWriter(tcpClient.GetStream());
                string msg = JsonConvert.SerializeObject(new Request()
                {
                    rqst = "Add",
                    car = new Car()
                    {
                        Id = int.Parse(tb_id.Text),
                        Vendor = tb_vendor.Text,
                        Model = tb_model.Text,
                        SeatCount = int.Parse(tb_sc.Text)
                    }
                }); 
                bw.Write(msg);                
            }
            else if(Btn_name == "Update")
            {
                var bw = new BinaryWriter(tcpClient.GetStream());
                string msg = JsonConvert.SerializeObject(new Request()
                {
                    rqst = "Update",
                    car = new Car()
                    {
                        Id = int.Parse(tb_id.Text),
                        Vendor = tb_vendor.Text,
                        Model = tb_model.Text,
                        SeatCount = int.Parse(tb_sc.Text)
                    }
                });
                bw.Write(msg);
            }
        }
    }
}
