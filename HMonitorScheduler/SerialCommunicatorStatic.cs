using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace HMonitorScheduler
{
    public static class SerialCommunicatorStatic
    {
        public static SerialPort _serialPort;

        static SerialCommunicatorStatic()
        {
            _serialPort = new SerialPort();
        }

        public static void OpenCommunication ()
        {
            if(!_serialPort.IsOpen)
            {
                //_serialPort.PortName = serialPortName;
                //_serialPort.BaudRate = 9600;
                //_serialPort.DataBits = 8;
                //_serialPort.StopBits = StopBits.One;
                //_serialPort.Handshake = Handshake.None;
                //_serialPort.WriteTimeout = 500;
                //_serialPort.ReadTimeout = 500;
                //_serialPort.ReceivedBytesThreshold = 1;

                _serialPort.Open();
            }
            //TODO: Vidjeti što napraviti ako je port već otvoren, da li ga zatvoriti
            //      pa ponovno otvoriti ili napraviti nešto drugo
            //      možda treba flushati sadržaj serijskog stacka
        }
        
        public static void CloseCommunication(string serialPortName)
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Close();
            }
        }

        public static void SendData(byte[] txData, int numberOfChars)
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Write(txData, 0, numberOfChars);
            }
            else
            {
                Console.WriteLine(string.Format("Serijski port '{0}' nije otvoren.", _serialPort.PortName));
                //TODO: Razmisliti što dalje s aplikacijom u ovom trenutku...
            }
        }

        public static int[] ReceiveDataBytes(int timeout)
        {
            System.Threading.Thread.Sleep(timeout);
            //Inicijaliziraj buffer
            int numberOfBytes = _serialPort.BytesToRead;
            int[] array1_rxData = new int[numberOfBytes];

            for (int x = 0; x <= numberOfBytes - 1; x++)
            {
                array1_rxData[x] = _serialPort.ReadByte();
            }

            return array1_rxData;
        }

        public static string ReceiveDataString(int timeout)
        {
            System.Threading.Thread.Sleep(timeout);
            //Inicijaliziraj buffer

            //StringBuilder StrBuilder = new StringBuilder();

            int numberOfBytes = _serialPort.BytesToRead;
            byte[] array1_rxData = new byte[numberOfBytes];
            //int readByte;

            for (int x = 0; x <= numberOfBytes - 1; x++)
            {
                array1_rxData[x] = (byte)_serialPort.ReadByte();
                //StrBuilder.Append(
            }

            string result = System.Text.Encoding.UTF8.GetString(array1_rxData);


            return result;
        }    
    
    }
}


/*
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net.Mime;
using System.Text;

namespace HMonitorScheduler
{
    public class SerialCommunicator
    {
        private SerialPort _serialPort;

        public SerialCommunicator(string serialPortName)
        {
            _serialPort = new SerialPort
            {
                PortName = serialPortName,
                BaudRate = 9600,
                DataBits = 8,
                StopBits = StopBits.One,
                Handshake = Handshake.None,
                WriteTimeout = 500,
                ReadTimeout = 500,
                ReceivedBytesThreshold = 1,
                //Encoding = Encoding.Default  //System.Text.Encoding.GetEncoding(1252)
            };

            //Inicijaliziraj varijable i event handlere
            //brojElemenataDictionaryaStart = dictionaryNaredbiStart.Count;
            //brojElemenataDictionarya = dictionaryNaredbi.Count;
            //_serialPort.DataReceived += new SerialDataReceivedEventHandler(ReceiveData);
        }

        public void OpenCommunication()
        {
            if (!_serialPort.IsOpen)
            {
                _serialPort.Open();
            }

            //TODO: Vidjeti što napraviti ako je port već otvoren, da li ga zatvoriti
            //      pa ponovno otvoriti ili napraviti nešto drugo
            //      možda treba flushati sadržaj serijskog stacka
        }

        public void CloseCommunication()
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Close();
            }
        }

        //public bool BoillerPing()
        //{
        //    int iterationCount = 0;
        //    int successCount = 0;

        //    do
        //    {
        //        var Command = new byte[] { 0x07, 0x02, 0x00, 0x00, 0x00, 0x04, 0xC4 };
        //        SendData(Command, Command.Count());

        //        var returnCommand = ReceiveData(1000);      //Ovaj timeout je bezveze - nije implementiran

        //        if (returnCommand.Any())
        //        {
        //            //ovako treba izgledati niz koji vraća bojler
        //            var exampleCommand = new int[] { 0x08, 0x00, 0x00, 0x93, 0x0B, 0x09, 0xFB, 0x0B };
        //            if (exampleCommand.SequenceEqual(returnCommand))
        //            {
        //                successCount++;
        //                Console.WriteLine("OK");
        //            }
        //            else
        //            {
        //                Console.WriteLine("NOK");
        //            }
        //            //procesuiraj podatke
        //            //provjeri da li su vraćeni podaci uredu
        //        }
        //        iterationCount++;

        //    } while (iterationCount < 10 || successCount < 2);

        //    return (successCount > 2);      //vrati true akoje success count veći od 2

        //}

        public void SendData(byte[] txData, int numberOfChars)
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Write(txData, 0, numberOfChars);
            }
            else
            {
                Console.WriteLine(string.Format("Serijski port '{0}' nije otvoren.", _serialPort.PortName));
                //TODO: Razmisliti što dalje s aplikacijom u ovom trenutku...
            }
        }

        public int[] ReceiveDataBytes(int timeout)
        {
            System.Threading.Thread.Sleep(timeout);
            //Inicijaliziraj buffer
            int numberOfBytes = _serialPort.BytesToRead;
            int[] array1_rxData = new int[numberOfBytes];

            for (int x = 0; x <= numberOfBytes - 1; x++)
            {
                array1_rxData[x] = _serialPort.ReadByte();
            }

            return array1_rxData;
        }

        public string ReceiveDataString(int timeout)
        {
            System.Threading.Thread.Sleep(timeout);
            //Inicijaliziraj buffer

            //StringBuilder StrBuilder = new StringBuilder();

            int numberOfBytes = _serialPort.BytesToRead;
            byte[] array1_rxData = new byte[numberOfBytes];
            //int readByte;

            for (int x = 0; x <= numberOfBytes - 1; x++)
            {
                array1_rxData[x] = (byte)_serialPort.ReadByte();
                //StrBuilder.Append(
            }

            string result = System.Text.Encoding.UTF8.GetString(array1_rxData);


            return result;
        }
    }
}

*/