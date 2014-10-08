using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Quartz;
using Quartz.Impl;


namespace HMonitorScheduler
{
    class Program
    {
        static void Main(string[] args)
        {

            const int job1Rerun = 20; //Minute imeđu dva Joba
            const int job2Rerun = 1; //Minute imeđu dva Joba            
            
            Console.WriteLine("HMonitor Scheduler 1.0");
            Console.WriteLine("----------------------\r\n");
            Console.WriteLine(string.Format("Current system Time: {0}", DateTime.Now));

            try
            {
                #region Definicija Schedulera
                // Dohvati Scheduler instancu iz Factory-a
                IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();

                // i pokreni ju
                scheduler.Start();
                #endregion

                #region Definicija JOB1 - Weather Underground
                // definiraj job i veži ga uz WeatherQuartz klasu
                IJobDetail job1 = JobBuilder.Create<WeatherQuartz>()
                    .WithIdentity("JOB1", "WeatherGroup")
                    .Build();

                //Izračunaj vrijeme na koje Job počinje s radom (zaokruži na 10 minuta)
                var dt1 = DateTime.Now;
                TimeSpan d1 = TimeSpan.FromMinutes(job1Rerun);
                DateTime runTime1 = new DateTime(((dt1.Ticks + d1.Ticks - job1Rerun) / d1.Ticks) * d1.Ticks);

                // Triggeriraj da Job krene s izvršenjem na runTime, i ponavljaj ga svakih 20 minuta
                ITrigger trigger1 = TriggerBuilder.Create()
                    .WithIdentity("trigger1", "WeatherGroup")
                    .StartAt(runTime1)
                    .WithSimpleSchedule(x => x
                        .WithIntervalInMinutes(job1Rerun)
                        .RepeatForever())
                    .Build();

                // Tell quartz to schedule the job using our trigger
                scheduler.ScheduleJob(job1, trigger1);
                Console.WriteLine("Scheduler started...\r\n");
                Console.WriteLine(String.Format("Job 'JOB1' will start executing at: {0}", Convert.ToString(runTime1)));
                Console.WriteLine(String.Format("Job 'JOB1' Re-run set to: {0}minutes\r\n", Convert.ToString(job1Rerun)));
                #endregion

                #region Definicija JOB2 - Plinsko brojilo
                // definiraj job i veži ga uz WeatherQuartz klasu
                IJobDetail job2 = JobBuilder.Create<SensorPlinskoBrojilo>()
                    .WithIdentity("JOB2", "Plin")
                    .Build();

                //Izračunaj vrijeme na koje Job počinje s radom (zaokruži na 10 minuta)
                var dt2 = DateTime.Now;
                TimeSpan d2 = TimeSpan.FromMinutes(job2Rerun);
                DateTime runTime2 = new DateTime(((dt2.Ticks + d2.Ticks - job2Rerun) / d2.Ticks) * d2.Ticks);

                // Triggeriraj da Job krene s izvršenjem na runTime, i ponavljaj ga svakih 20 minuta
                ITrigger trigger2 = TriggerBuilder.Create()
                    .WithIdentity("trigger2", "Plin")
                    .StartAt(runTime2)
                    .WithSimpleSchedule(x => x
                        .WithIntervalInMinutes(job2Rerun)
                        .RepeatForever())
                    .Build();

                // Tell quartz to schedule the job using our trigger
                scheduler.ScheduleJob(job2, trigger2);
                Console.WriteLine("Scheduler started...\r\n");
                Console.WriteLine(String.Format("Job 'JOB2' will start executing at: {0}", Convert.ToString(runTime2)));
                Console.WriteLine(String.Format("Job 'JOB2' Re-run set to: {0}minutes\r\n", Convert.ToString(job2Rerun)));
                #endregion

            }
            catch (SchedulerException se)
            {
                Console.WriteLine(se.Message);
            }
        }
    }
}

                //temp_c                WUTEMP__01
                //relative_humidity     WUHUMIDI01
                //observation_location  WULOCATI01
                //station_id            WUSTATID01
                //weather               WUWEATHE01
                //wind_string           WUWIND__01
                //wind_dir              WUWINDIR01
                //wind_degrees          WUWINDEG01
                //wind_kph              WUWINDKM01
                //wind_gust_kph         WUWINGUS01
                //pressure_mb           PRESSURE01
                //pressure_trend        PRESTREN01
                //dewpoint_c            DEWPOINT01
                //feelslike_c           FEELSLIK01
                //visibility_km         VISIBILI01
                //UV                    UV______01
                //precip_1hr_metric     PRECIPIT01
                //precip_today_metric   PRECTODA01
                //icon_url              ICONURL_01
