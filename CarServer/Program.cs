
using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;

namespace CarServer
{
    internal class Program
    {        
        static void Main(string[] args)
        {
            TcpListener tcpListener = new TcpListener(new IPEndPoint(IPAddress.Parse("192.168.0.109"), 63291));
            BinaryReader br;
            BinaryWriter bw;
            CarDatabase database = new CarDatabase()
            {
                cars =new List<Car>() {
                    new Car()
                    {
                    Id = 1,
                    Model="c220",
                    Vendor="Mercedes",
                    SeatCount=5
                    },
                     new Car()
                    {
                    Id = 2,
                    Model="e240",
                    Vendor="Mercedes",
                    SeatCount=5
                    },
                      new Car()
                    {
                    Id = 3,
                    Model="m3",
                    Vendor="BMW",
                    SeatCount=5
                    },
                       new Car()
                    {
                    Id = 4,
                    Model="m5",
                    Vendor="BMW",
                    SeatCount=5
                    }
                }
            };
            tcpListener.Start();
            while (true)
            {
                var client = tcpListener.AcceptTcpClient();

                Task.Run(() =>
                {
                    Console.WriteLine($"{client.Client.RemoteEndPoint} connected...");
                    var stream = client.GetStream();
                    br = new BinaryReader(stream);
                    while (true)
                    {                        
                        var msg =JsonConvert.DeserializeObject<Request>( br.ReadString());
                        if (msg.rqst == "Get")
                        {
                            bw = new BinaryWriter(stream);
                            bw.Write(database.GetCars());
                        }
                        else if(msg.rqst == "Add")
                        {
                            database.Add(msg.car);
                        }
                        else if (msg.rqst == "Update")
                        {
                            database.Update(msg.car);
                        }
                        else if(msg.rqst == "Delete")
                        {
                            database.Delete(msg.car);
                        }
                    }
                });
            }
        }
    }
}
