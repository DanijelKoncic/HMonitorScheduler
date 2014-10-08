using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Website.Data;

namespace HMonitorScheduler
{
    static class SensorData
    {
        public static void Save(string sensorCode, string dataText, decimal? dataNumeric, DateTime sampleDt)
        {
            //Spremi podatke u bazu prema senzoru, prikupljenoj vrijednosti i vremenu prikupljanja
            try
            {
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
