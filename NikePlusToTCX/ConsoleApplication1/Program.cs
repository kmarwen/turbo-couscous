using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Reading public time line");

            TimeZone zone = TimeZone.CurrentTimeZone;
            // Demonstrate ToLocalTime and ToUniversalTime.
            DateTime local = zone.ToLocalTime(DateTime.Now);
            DateTime universal = zone.ToUniversalTime(DateTime.Now);
            Console.WriteLine(local);
            Console.WriteLine(universal);
            Console.WriteLine(DateTime.Now.ToUniversalTime());
            //TimeZoneInfo.CreateCustomTimeZone()

            //TimeZone zone = TimeZone.CurrentTimeZone;
            DaylightTime time = zone.GetDaylightChanges(DateTime.Today.Year);
            Console.WriteLine("Start: {0}", time.Start);
            Console.WriteLine("End: {0}", time.End);
            Console.WriteLine("Delta: {0}", time.Delta);


            /*
            var cl = new WebClient();
            using (var r = new StreamReader(cl.OpenRead(@"https://api.nike.com/v1/me/sport/activities?access_token=ltKlTrMuKvplmQUAl8FjH709Sxbf&count=10")))
            {
                var data = r.ReadToEnd();
                Console.WriteLine(data);
            }
            */
            Console.ReadLine();
        }



    }

}
