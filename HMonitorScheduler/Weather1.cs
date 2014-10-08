using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Quartz;
using Website.Data;

namespace HMonitorScheduler
{
    class Weather1
    {
        public string location { get; set; }
        public decimal temperature_C { get; set; }
        public DateTime observation_time_rfc822 { get; set; }
        //public string local_time_rfc822 { get; set; }
        public string weather { get; set; }
        public string station_id { get; set; }
        public decimal relative_humidity { get; set; }
        public string wind_string { get; set; }
        public string wind_dir { get; set; }
        public decimal wind_degrees { get; set; }
        public decimal wind_kph { get; set; }
        public decimal wind_gust_kph { get; set; }
        public decimal pressure_mb { get; set; }
        public string pressure_trend { get; set; }
        public decimal dewpoint_c { get; set; }
        public decimal windchill_c { get; set; }
        public decimal feelslike_c { get; set; }
        public decimal visibility_km { get; set; }
        //public string solarradiation { get; set; }
        public decimal UV { get; set; }
        public decimal precip_1hr_metric { get; set; }
        public decimal precip_today_metric { get; set; }
        public string icon_url { get; set; }

        public bool GetCurrentWeather(string weatherQuery)
        {
            try
            {
                //dohvati podatke sa webservera
                var weatherData = new XmlDocument();
                //weatherData.XmlResolver = null;

                weatherData.Load(weatherQuery);

                //temp_c                WUTEMP__01
                var xmlNode = weatherData.GetElementsByTagName("temp_c").Item(0);
                if (xmlNode != null)
                    temperature_C = Convert.ToDecimal(xmlNode.InnerText.Replace(".", ","));

                //relative_humidity     WUHUMIDI01
                xmlNode = weatherData.GetElementsByTagName("relative_humidity").Item(0);
                if (xmlNode != null)
                    relative_humidity = Convert.ToDecimal(xmlNode.InnerText.Replace("%", String.Empty));

                //observation_time_rfc822   -
                xmlNode = weatherData.GetElementsByTagName("observation_time_rfc822").Item(0);
                if (xmlNode != null)
                    observation_time_rfc822 = Convert.ToDateTime(xmlNode.InnerText);

                //observation_location  WULOCATI01
                xmlNode = weatherData.GetElementsByTagName("full").Item(0);
                if (xmlNode != null)
                    location = xmlNode.InnerText;

                //station_id            WUSTATID01
                xmlNode = weatherData.GetElementsByTagName("station_id").Item(0);
                if (xmlNode != null)
                    station_id = xmlNode.InnerText;

                //weather               WUWEATHER01
                xmlNode = weatherData.GetElementsByTagName("weather").Item(0);
                if (xmlNode != null)
                    weather = xmlNode.InnerText;

                //wind_string           WUWIND01
                xmlNode = weatherData.GetElementsByTagName("wind_string").Item(0);
                if (xmlNode != null)
                    wind_string = xmlNode.InnerText;

                //wind_dir              WUWINDIR01
                xmlNode = weatherData.GetElementsByTagName("wind_dir").Item(0);
                if (xmlNode != null)
                    wind_dir = xmlNode.InnerText;

                //wind_degrees          WUWINDEG01
                xmlNode = weatherData.GetElementsByTagName("wind_degrees").Item(0);
                if (xmlNode != null)
                    wind_degrees = Convert.ToDecimal(xmlNode.InnerText.Replace(".", ","));

                //wind_kph              WUWINDKM01
                xmlNode = weatherData.GetElementsByTagName("wind_kph").Item(0);
                if (xmlNode != null)
                    wind_kph = Convert.ToDecimal(xmlNode.InnerText.Replace(".", ","));

                //wind_gust_kph         WUWINGUS01
                xmlNode = weatherData.GetElementsByTagName("wind_gust_kph").Item(0);
                if (xmlNode != null)
                    wind_gust_kph = Convert.ToDecimal(xmlNode.InnerText.Replace(".", ","));

                //pressure_mb           PRESSURE01
                xmlNode = weatherData.GetElementsByTagName("pressure_mb").Item(0);
                if (xmlNode != null)
                    pressure_mb = Convert.ToDecimal(xmlNode.InnerText.Replace(".", ","));

                //pressure_trend        PRESTREN01
                xmlNode = weatherData.GetElementsByTagName("pressure_trend").Item(0);
                if (xmlNode != null)
                    pressure_trend = xmlNode.InnerText;

                //dewpoint_c            DEWPOINT01
                xmlNode = weatherData.GetElementsByTagName("dewpoint_c").Item(0);
                if (xmlNode != null)
                    dewpoint_c = Convert.ToDecimal(xmlNode.InnerText.Replace(".", ","));

                //windchill_c            WUWINDCH01
                xmlNode = weatherData.GetElementsByTagName("windchill_c").Item(0);
                if (xmlNode != null)
                    windchill_c = Convert.ToDecimal(xmlNode.InnerText.Replace(".", ","));

                //feelslike_c           FEELSLIK01
                xmlNode = weatherData.GetElementsByTagName("feelslike_c").Item(0);
                if (xmlNode != null)
                    feelslike_c = Convert.ToDecimal(xmlNode.InnerText.Replace(".", ","));

                //visibility_km         VISIBILI01
                xmlNode = weatherData.GetElementsByTagName("visibility_km").Item(0);
                if (xmlNode != null)
                    visibility_km = Convert.ToDecimal(xmlNode.InnerText.Replace(".", ","));

                //solarradiation
                //Xml je prazan

                //UV                    UV01
                xmlNode = weatherData.GetElementsByTagName("UV").Item(0);
                if (xmlNode != null)
                    UV = Convert.ToDecimal(xmlNode.InnerText.Replace(".", ","));

                //precip_1hr_metric     PRECIPIT01
                xmlNode = weatherData.GetElementsByTagName("precip_1hr_metric").Item(0);
                if (xmlNode != null)
                    precip_1hr_metric = Convert.ToDecimal(xmlNode.InnerText.Replace(".", ","));

                //precip_today_metric   PRECTODA01
                xmlNode = weatherData.GetElementsByTagName("precip_today_metric").Item(0);
                if (xmlNode != null)
                    precip_today_metric = Convert.ToDecimal(xmlNode.InnerText.Replace(".", ","));

                //icon_url              ICONURL01
                xmlNode = weatherData.GetElementsByTagName("icon_url").Item(0);
                if (xmlNode != null)
                {
                    icon_url = xmlNode.InnerText;
                    int index = icon_url.LastIndexOf("/") + 1;
                    icon_url = icon_url.Substring(index, icon_url.Length - index);

                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
                //throw;
            }
        }

        public void SaveCurrentWeather(string sensorCode, string dataText, decimal? dataNumeric, DateTime sampleDt)
        {
            //Spremi podatke u bazu prema senzoru, prikupljenoj vrijednosti i vremenu prikupljanja
            try
            {
                #region Deprecated: Snimanje podataka kroz Disposable klasu
                //Console.WriteLine(HMonitorScheduler.Properties.Settings.Default.HMonitorDataConnection);
                //using (var dc = new HMonitorData(HMonitorScheduler.Properties.Settings.Default.HMonitorDataConnection))
                //{
                //    var first = dc.Sensors.First(s => s.Code == sensorCode);
                //    Console.WriteLine("Save weather - Sensor name: " + first.Name);
                //    if (first != null)
                //    {
                //        var sensorId = first.SensorId;
                //        Console.WriteLine("Save weather - Sensor Id:" + Convert.ToString(sensorId));
                //        var sensorHistoryData = new SensorHistoryData()
                //        {
                //            SensorId = sensorId,
                //            DataNumeric = dataNumeric,
                //            DataText = dataText,
                //            SampledDT = sampleDt,
                //            InsertedDT = DateTime.Now
                //        };
                //        dc.Add(sensorHistoryData);
                //        dc.SaveChanges();
                //    }
                //    else
                //    {
                //        //TODO: Obavijestiti da ne postoji sensorCode
                //    }
                //}
                #endregion

                var dc = new HMonitorData(HMonitorScheduler.Properties.Settings.Default.HMonitorDataConnection);

                var firstSensor = dc.Sensors.Single(s => s.Code == sensorCode);
                //Console.WriteLine("Save weather - Sensor name: " + firstSensor.Name);
                
                if (firstSensor != null)
                {
                    var sensorId = firstSensor.SensorId;
                    var sensorHistoryData = new SensorHistoryData()
                                                {
                                                    SensorId = sensorId,
                                                    DataNumeric = dataNumeric,
                                                    DataText = dataText,
                                                    SampledDT = sampleDt,
                                                    InsertedDT = DateTime.Now
                                                };
                    dc.Add(sensorHistoryData);
                    dc.SaveChanges();
                }
                else
                {
                    Console.WriteLine(String.Format("You are trying to insert data for unregistered sensor ({0})", sensorCode));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

    }
}
