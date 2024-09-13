using System;
using System.IO;
using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using InTheHand.Net.Sockets;
using System.Text;
using System.IO.Ports;
using System.Security.Authentication.ExtendedProtection;
using Windows.Devices.Bluetooth;
using System.Net;

namespace WindowsFormsBL
{
    public partial class Form2 : Form
    {
        #region Fields

        List<BluetoothDeviceInfo> _bluetoothDevices;

        bool _IsSearchingForDevices;

        int i;

        #endregion Fields

        public bool IsSearchingForDevices
        {
            get => _IsSearchingForDevices;
            set
            {
                _IsSearchingForDevices = value;
                button1.Enabled = !_IsSearchingForDevices;
            }
        }

        public Form2()
        {
            InitializeComponent();
            _bluetoothDevices = new List<BluetoothDeviceInfo>();
            listBox1.DisplayMember = "DeviceName";
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            IsSearchingForDevices = true;
            _bluetoothDevices.Clear();
            listBox1.Items.Clear();
            try
            {
                var bluetoothDevices = await SearchDevicesAsync();
                _bluetoothDevices.AddRange(bluetoothDevices);
                //Poner filtro para borrar dispositivos que no son lily.
            }
            catch (Exception ex)
            {
                MessageBox.Show(text: $"Couldn't search for bluetooth devices: {ex.Message}");
            }
            if (_bluetoothDevices.Count > 0)
            {
                DisplayLbItems();
            }
            IsSearchingForDevices = false;
        }



        private void DisplayLbItems()
        {
            listBox1.DataSource = _bluetoothDevices;
        }

        public async Task<BluetoothDeviceInfo[]> SearchDevicesAsync()
        {
            var bluetoothClient = new BluetoothClient();
            var bluetoothDevices = await Task.Run(() => bluetoothClient.DiscoverDevices());
            bluetoothClient.Close();
            return bluetoothDevices;
        }

        public void readInfoClient()
        {
            String Pin = "";
            BluetoothSecurity.SetPin(_bluetoothDevices[i].DeviceAddress,Pin);
            BluetoothSecurity.PairRequest(_bluetoothDevices[i].DeviceAddress,Pin);
            BluetoothClient a = new BluetoothClient();
            a.SetPin(Pin);
            a.Connect(_bluetoothDevices[i].DeviceAddress, BluetoothService.SerialPort);
            Stream stream = a.GetStream();

            while (a.Connected && stream != null)
            {
                try
                {
                    byte[] byteReceived = new byte[1024];
                    int read = stream.Read(byteReceived, 0, byteReceived.Length);
                    if (read > 0)
                    {
                        textBox12.Text = Encoding.ASCII.GetString(byteReceived);
                    }
                    stream.Flush();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Erro: " + e.ToString());
                }
            }
            stream.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String busqueda = "SY-ANC886BT";
            for (i = 0; i != (_bluetoothDevices.Count); i++)
            {
                String a = _bluetoothDevices[i].DeviceName;
                 if (busqueda == a){
                    readInfoClient();
                 }
            }
            


            string stream = "$$03928029111192831923871428031111111111 ##";

            string Temperatura = stream.Substring(2,4);
            textBox1.Text = Temperatura;

            string Humedad = stream.Substring(6, 4);
            textBox2.Text = Humedad;

            string Micro = stream.Substring(10, 1);
            textBox3.Text = Micro;

            string Audio = stream.Substring(11, 1);
            textBox4.Text = Audio;

            string Bluetooth = stream.Substring(12, 1);
            textBox5.Text = Bluetooth;

            string Wifi = stream.Substring(13, 1);
            textBox6.Text = Wifi;

            string Volt1 = stream.Substring(14, 4);
            textBox7.Text = Volt1;

            string Volt2 = stream.Substring(18, 4);
            textBox8.Text = Volt2;

            string Amp = stream.Substring(22, 4);
            textBox9.Text = Amp;

            string Extras = stream.Substring(26, 4);
            textBox10.Text = Extras;

            string Led = stream.Substring(30, 10);
            textBox11.Text = Led;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //if ()
            //{

            //}
            //SerialPort port= new SerialPort();
            //port.PortName = "COM4";
            //port.BaudRate = 9600;
            //port.Parity = Parity.None;
            //port.DataBits = 8;
            //port.StopBits = StopBits.One;

            //try
            //{
            //    if (!port.IsOpen)
            //    {
            //        port.Open();
            //        Console.WriteLine("Puerto serial abierto correctamente.");
            //        System.Threading.Thread.Sleep(1000);
            //        string data = port.ReadLine();
            //        Console.WriteLine("Datos recibidos: " + data);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("Error : " + ex.Message);
            //}

            //port.Open();
            //if (true)
            //{
            //    Console.WriteLine("Proceso iniciado");
            //    string Datatosend = "Hello";
            //    port.Write(Datatosend);
            //    string data = port.ReadLine();
            //    Console.WriteLine("Datos recibidos: " + data);
            //}
            //port.Close();

            //Metodo que funciono al conectar, replicar aqproximacion por bluetoothdeviceinfo
            // tambien acomodarlo para usar el dispositivo seleccionado de la listbox

            // Descubrir dispositivos disponibles
            BluetoothAddress D = BluetoothAddress.Parse("78:21:84:E1:51:92");
            BluetoothClient bluetoothClient = new BluetoothClient();
            BluetoothDeviceInfo[] devices = bluetoothClient.DiscoverDevices();
            bool isPaired = false;
            Console.WriteLine("Dispositivos Bluetooth encontrados:");
            
            foreach (BluetoothDeviceInfo device in devices)
            {
                Console.WriteLine($"Nombre: {device.DeviceName}, Dirección: {device.DeviceAddress}, Guid´s: ");
                if (device.DeviceAddress == D)
                {
                    isPaired = true;
                    break;
                }
            }

            if (isPaired)
            {
                Console.WriteLine("El dispositivo está emparejado.");
            }
            else
            {
                Console.WriteLine("El dispositivo no está emparejado.");
            }

            // Seleccionar un dispositivo para conectar (supongamos que seleccionamos el primero)
            BluetoothDeviceInfo deviceToConnect = devices[0];
            try
            {
                var service = deviceToConnect.InstalledServices[0]; // Ejemplo de acceso, ajusta según tu contexto
                Console.WriteLine(service.ToString());  //0000110b-0000-1000-8000-00805f9b34fb / 0000110c-0000-1000-8000-00805f9b34fb / 0000110e-0000-1000-8000-00805f9b34fb
            }
            catch (IndexOutOfRangeException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            // Establecer la conexión
            BluetoothClient client = new BluetoothClient();
            Guid G = new Guid("0000110b-0000-1000-8000-00805f9b34fb");
            deviceToConnect.SetServiceState(G, true);
            BluetoothEndPoint endPoint = new BluetoothEndPoint(deviceToConnect.DeviceAddress, deviceToConnect.InstalledServices[0]);
            try
            {
                client.Connect(endPoint);
                Console.WriteLine("Conexión establecida con éxito.");

                // Aquí puedes enviar y recibir datos usando client.GetStream()
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al conectar: {ex.Message}");
            }

        }
    }
}
