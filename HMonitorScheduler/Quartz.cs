using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using Quartz;
using System.Configuration;


//
//Sadrži klase koje odrađuju posao vezan uz određeni schedule
//

namespace HMonitorScheduler
{
    class WeatherQuartz : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                Console.WriteLine(string.Format("\r\n>> Job 'JOB1'executed at: {0}", DateTime.Now));
                Console.WriteLine("----------------------------------------\r\n");
                var currentWeather = new Weather();

                //var serviceWebaAddress = "http://api.wunderground.com/api/dfc7c71a78849bb0/conditions/pws:1/q/pws:IUNDEFIN41.xml";
                //const string serviceWebaAddress = "http://api.wunderground.com/api/dfc7c71a78849bb0/conditions/pws:1/q/pws:IZAGREB5.xml";
                const string serviceWebaAddress = "http://api.wunderground.com/api/dfc7c71a78849bb0/conditions/pws:1/q/pws:ICITYOFZ3.xml";
                

                if (currentWeather.GetCurrentWeather(serviceWebaAddress))       //Ako nema greške pri dohvatu podataka izvrši snimanje u bazu
                {

                    var dateTime = DateTime.Now;
                    var sd = new SensorData();
                    //Console.WriteLine("Ušao je u 1. stupanj snimanja");
                    //Snimi temperaturu
                    sd.Save("WUTEMP__01", null, currentWeather.temperature_C, currentWeather.observation_time_rfc822);
                    //relative_humidity     WUHUMIDI01
                    sd.Save("WUHUMIDI01", null, currentWeather.relative_humidity, currentWeather.observation_time_rfc822);
                    //observation_location  WULOCATI01++
                    sd.Save("WULOCATI01", currentWeather.location, null, currentWeather.observation_time_rfc822);
                    //station_id            WUSTATID01++
                    sd.Save("WUSTATID01", currentWeather.station_id, null, currentWeather.observation_time_rfc822);
                    //weather               WUWEATHE01++
                    sd.Save("WUWEATHE01", currentWeather.weather, null, currentWeather.observation_time_rfc822);
                    //wind_string           WUWIND__01++
                    sd.Save("WUWIND__01", currentWeather.wind_string, null, currentWeather.observation_time_rfc822);
                    //wind_dir              WUWINDIR01+
                    sd.Save("WUWINDIR01", currentWeather.wind_dir, null, currentWeather.observation_time_rfc822);
                    //wind_degrees          WUWINDEG01+
                    sd.Save("WUWINDEG01", null, currentWeather.wind_degrees, currentWeather.observation_time_rfc822);
                    //wind_kph              WUWINDKM01+
                    sd.Save("WUWINDKM01", null, currentWeather.wind_kph, currentWeather.observation_time_rfc822);
                    //wind_gust_kph         WUWINGUS01+         
                    sd.Save("WUWINGUS01", null, currentWeather.wind_gust_kph, currentWeather.observation_time_rfc822);
                    //pressure_mb           PRESSURE01+
                    sd.Save("PRESSURE01", null, currentWeather.pressure_mb, currentWeather.observation_time_rfc822);
                    //pressure_trend        PRESTREN01+
                    sd.Save("PRESTREN01", currentWeather.pressure_trend, null, currentWeather.observation_time_rfc822);
                    //dewpoint_c            DEWPOINT01+
                    sd.Save("DEWPOINT01", null, currentWeather.dewpoint_c, currentWeather.observation_time_rfc822);
                    //wind_chill            WUWINDCH01
                    sd.Save("WUWINDCH01", null, currentWeather.windchill_c, currentWeather.observation_time_rfc822);
                    //feelslike_c           FEELSLIK01+
                    sd.Save("FEELSLIK01", null, currentWeather.feelslike_c, currentWeather.observation_time_rfc822);
                    //visibility_km         VISIBILI01+
                    sd.Save("VISIBILI01", null, currentWeather.visibility_km, currentWeather.observation_time_rfc822);
                    //UV                    UV______01+
                    sd.Save("UV______01", null, currentWeather.UV, currentWeather.observation_time_rfc822);
                    //precip_1hr_metric     PRECIPIT01+
                    sd.Save("PRECIPIT01", null, currentWeather.precip_1hr_metric, currentWeather.observation_time_rfc822);
                    //precip_today_metric   PRECTODA01+
                    sd.Save("PRECTODA01", null, currentWeather.precip_today_metric, currentWeather.observation_time_rfc822);
                    //icon_url              ICONURL_01+
                    sd.Save("ICONURL_01", currentWeather.icon_url, null, currentWeather.observation_time_rfc822);


                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(String.Format("{0}timerfc822: {1}", "\t", Convert.ToString(currentWeather.observation_time_rfc822)));
                    Console.WriteLine(String.Format("{0}WUTEMP__01: {1}", "\t", Convert.ToString(currentWeather.temperature_C)));
                    Console.WriteLine(String.Format("{0}WUHUMIDI01: {1}", "\t", Convert.ToString(currentWeather.relative_humidity)));
                    Console.WriteLine(String.Format("{0}WUWEATHE01: {1}", "\t", currentWeather.weather));
                    Console.WriteLine(String.Format("{0}WUWIND__01: {1}", "\t", currentWeather.wind_string));
                    Console.WriteLine(String.Format("{0}WUWINDIR01: {1}", "\t", currentWeather.wind_dir));
                    Console.WriteLine(String.Format("{0}WUWINDEG01: {1}", "\t", Convert.ToString(currentWeather.wind_degrees)));
                    Console.WriteLine(String.Format("{0}WUWINDKM01: {1}", "\t", Convert.ToString(currentWeather.wind_kph)));
                    Console.WriteLine(String.Format("{0}WUWINGUS01: {1}", "\t", Convert.ToString(currentWeather.wind_gust_kph)));
                    Console.WriteLine(String.Format("{0}PRESSURE01: {1}", "\t", Convert.ToString(currentWeather.pressure_mb)));
                    Console.WriteLine(String.Format("{0}PRESTREN01: {1}", "\t", currentWeather.pressure_trend));
                    Console.WriteLine(String.Format("{0}DEWPOINT01: {1}", "\t", Convert.ToString(currentWeather.dewpoint_c)));
                    Console.WriteLine(String.Format("{0}WUWINDCH01: {1}", "\t", Convert.ToString(currentWeather.windchill_c)));
                    Console.WriteLine(String.Format("{0}FEELSLIK01: {1}", "\t", Convert.ToString(currentWeather.feelslike_c)));
                    Console.WriteLine(String.Format("{0}VISIBILI01: {1}", "\t", Convert.ToString(currentWeather.visibility_km)));
                    Console.WriteLine(String.Format("{0}UV______01: {1}", "\t", Convert.ToString(currentWeather.UV)));
                    Console.WriteLine(String.Format("{0}PRECIPIT01: {1}", "\t", Convert.ToString(currentWeather.precip_1hr_metric)));
                    Console.WriteLine(String.Format("{0}PRECTODA01: {1}", "\t", Convert.ToString(currentWeather.precip_today_metric)));
                    Console.WriteLine(String.Format("{0}ICONURL_01: {1}", "\t", currentWeather.icon_url));
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("SaveToDatabase: Error executing job (time: {0})", DateTime.Now);
                    Console.WriteLine("No data saved for this timestamp.");
                    Console.WriteLine();
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.WriteLine();
                Console.ResetColor();
                throw;
            }
        }
    }

    //internal class SerialQuartz : IJob
    //{
    //    public void Execute(IJobExecutionContext context)
    //    {
    //        //Priprema za serial boiller reader
    //    }
    //}

    internal class SensorPlinskoBrojilo : IJob
    {
        //Deklariraj varijable s nazivima pripadajućih
        private const string _sensorNamePlinCount = "PLINCOUNT_";
        private decimal _sensorValuePlinCount = 0;
        //private string sensorNamePlinVolt = "PLINVOLT__";
        //private decimal sensorValuePlinVolt = 0;               
        private const string _sensorNamePlinTime = "PLINTIME__";
        private decimal _sensorValuePlinTime = 0;

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                Console.WriteLine(string.Format("\r\n>> Job 'JOB2' executed at: {0}", DateTime.Now));
                Console.WriteLine("----------------------------------------\r\n");

                if (!ArduinoComunicate())
                {
                    Console.WriteLine("\t3 uzastopne greške u komunikaciji");
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);

                
                if (ex.Message.Equals("The write timed out."))
                {
                    SerialCommunicatorStatic.CloseCommunication();
                    Console.WriteLine("Closing COM port.");
                }

                Console.WriteLine();
                Console.ResetColor();                
                
                throw;
            }
        }

        private bool ArduinoComunicate() //
        {
            bool greska = true; //true = nema greške; false = došlo je do greške
            
            //Za svaki slučaj otvori komunikaciju
            SerialCommunicatorStatic.OpenCommunication();

            //Naredba za čitanje vrijednosti s Arduino stacka
            var komanda = new byte[] {0x73};

            //Iteracije
            for (int i = 1; i <= 3; i++ )
            {
                //Pošalji komandu
                SerialCommunicatorStatic.SendData(komanda, komanda.Count());

                //Primi i procesuiraj odgovor
                var returnData = SerialCommunicatorStatic.ReceiveDataString(1500); //Ovaj timeout je potreban kako bi Arduino stigao pripremiti podatke5

                if (returnData.Any())
                {
                    //procesuiraj podatke
                    if (ParseSensorData(string.Join("", returnData))) //ako vrati true = nema greske
                        {
                            //snimi podatke
                            SaveSensor();
                            //TODO: Arduinu treba poslati komandu da resetira brojač plina
                            greska = true;
                            break;
                        }
                    else
                        {
                            Console.WriteLine("\tDošlo je do greške u parsiranju podataka, pokušavam ponovo.");
                            greska = false;
                        }
                }
            }

            return greska;
        
        }

        private void SaveSensor()
        {
            //if (_sensorValuePlinCount > 0)
            {
                var dateTime = DateTime.Now;
                var sd = new SensorData();
                sd.Save(_sensorNamePlinCount, null, _sensorValuePlinCount, dateTime);
                sd.Save(_sensorNamePlinTime, null, _sensorValuePlinTime, dateTime);

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine(String.Format("{0}{1}: {2}", "\t", _sensorNamePlinCount, Convert.ToString(_sensorValuePlinCount)));
                Console.WriteLine(String.Format("{0}{1}: {2}", "\t", _sensorNamePlinTime, Convert.ToString(_sensorValuePlinTime)));
                Console.WriteLine("\tSave: OK");
                Console.ResetColor();
            }
        }
    
        private bool ParseSensorData (string returnData)        //vraca true = nema greske ; false = greska
        {
            bool greska = true;
            
            
            //#region Ispis za potrebe testiranja
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("\tReceived  : ");
            foreach (byte b in returnData)
            {
                Console.Write(Convert.ToChar(b));
            }
            Console.ResetColor();
            //#endregion
            
            
            //V = 2.23; L = 0; C = 0; T = 0
            
            string[] sensors = returnData.Split(';');

            foreach (var sensor in sensors)
            {
                //if (sensor.Substring(0, 2) == "V=")
                //{
                    //voltaža
                    //sensorValuePlinVolt = Convert.ToInt16(sensor.Substring(2, sensor.Length));
                //}
                //else if (sensor.Substring(0, 2) == "L=")
                //{
                //    //vjerojatno nebitno, ukloniti

                //}
                //else 
                if (sensor.Substring(0, 2) == "C=")
                {
                    //counter
                    //_sensorValuePlinCount = Convert.ToDecimal(sensor.Substring(2, sensor.Length - 2));
                    if (!Decimal.TryParse(sensor.Substring(2, sensor.Length - 2), out _sensorValuePlinCount))
                    {
                        //greška u parsiranju
                        greska = false;
                    }
                }
                else if (sensor.Substring(0, 2) == "T=")
                {
                    //vrijeme u koje se odazvao arduino
                    // = Convert.ToDecimal(sensor.Substring(2, sensor.Length - 2));
                    if (!Decimal.TryParse(sensor.Substring(2, sensor.Length - 2), out _sensorValuePlinTime))
                    {
                        //greška u parsiranju
                        greska = false;
                    }
                }
            }

            return greska;
        }
    }
}