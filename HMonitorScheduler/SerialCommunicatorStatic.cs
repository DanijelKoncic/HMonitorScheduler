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
                _serialPort.Open();
            }
            //TODO: Vidjeti što napraviti ako je port već otvoren, da li ga zatvoriti
            //      pa ponovno otvoriti ili napraviti nešto drugo
            //      možda treba flushati sadržaj serijskog stacka
        }
        
        public static void CloseCommunication()
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