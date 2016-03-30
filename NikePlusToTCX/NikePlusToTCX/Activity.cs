using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NikePlusToTCX
{
    public class NikeJson
    {
        public class Link
        {
            [JsonProperty("rel")]
            public string rel { get; set; }
            [JsonProperty("href")]
            public string href { get; set; }
        }

        public class MetricSummary
        {
            [JsonProperty("calories")]
            public string calories { get; set; }
            [JsonProperty("fuel")]
            public string fuel { get; set; }
            [JsonProperty("distance")]
            public string distance { get; set; }
            [JsonProperty("steps")]
            public string steps { get; set; }
            [JsonProperty("duration")]
            public string duration { get; set; }
        }

        public class Tag
        {
            [JsonProperty("tagType")]
            public string tagType { get; set; }
            [JsonProperty("tagValue")]
            public string tagValue { get; set; }
        }


        public class Activity
        {
            [JsonProperty("links")]
            public IList<Link> links { get; set; }
            [JsonProperty("activityId")]
            public string activityId { get; set; }
            [JsonProperty("activityType")]
            public string activityType { get; set; }

            [JsonProperty("startTime")]
            public DateTime startTime { get; set; }
            
            [JsonProperty("activityTimeZone")]
            public string activityTimeZone { get; set; }
            [JsonProperty("status")]
            public string status { get; set; }
            [JsonProperty("deviceType")]
            public string deviceType { get; set; }
            [JsonProperty("metricSummary")]
            public MetricSummary metricSummary { get; set; }
            [JsonProperty("tags")]
            public IList<Tag> tags { get; set; }
            [JsonProperty("metrics")]
            public IList<Metric> metrics { get; set; }
            [JsonProperty("isGpsActivity")]
            public bool isGpsActivity { get; set; }

            public ActivityGps activityGps { get; set; }


            internal void GetActivityDetails(string access_token)
            {
                WebClient wclient = new WebClient();
                using (var r = new StreamReader(wclient.OpenRead(links[0].href.Insert(links[0].href.Length, string.Format(@"?access_token={0}", access_token)))))
                {
                    string activityJsonData = r.ReadToEnd();
                    Activity activity = JsonConvert.DeserializeObject<Activity>(activityJsonData);
                    activity.jsonData = activityJsonData;
                    metrics = activity.metrics;
                    // todo : check again
                    isGpsActivity = activity.isGpsActivity;
                }

                if (isGpsActivity)
                {
                    wclient = new WebClient();
                    StringBuilder sb = new StringBuilder();

                    sb.Append(links[0].href);
                    sb.Append("/gps");
                    sb.AppendFormat(@"?access_token={0}", access_token);

                    using (var r = new StreamReader(wclient.OpenRead(sb.ToString())))
                    {
                        var jsonData = r.ReadToEnd();
                        ActivityGps activityGPS = JsonConvert.DeserializeObject<ActivityGps>(jsonData);
                        this.activityGps = activityGPS;
                    }
                }

                activityName = string.Format("{0}-{1}-GPS{2}", this.activityType, this.startTime.ToString("yyyyMMdd-HHmm"), this.isGpsActivity);
            }

            public string jsonData { get; set; }

            public string activityName { get; set; }
        }
        
        public class Paging
        {
            [JsonProperty("next")]
            public string next { get; set; }
            [JsonProperty("previous")]
            public object previous { get; set; }
        }

        public class ActivitiesContainer
        {
            [JsonProperty("data")]
            public IList<Activity> activities { get; set; }
            [JsonProperty("paging")]
            public Paging paging { get; set; }

            public string jsonData { get; set; }
        }


        public class Metric
        {
            [JsonProperty("intervalMetric")]
            public int intervalMetric { get; set; }
            [JsonProperty("intervalUnit")]
            public string intervalUnit { get; set; }
            [JsonProperty("metricType")]
            public string metricType { get; set; }
            [JsonProperty("values")]
            public IList<string> values { get; set; }
            public IList<double> newSecondValues { get; set; }
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        // GPS //
        ////////////////////////////////////////////////////////////////////////////////////////////////////



        public class ActivityGps
        {
            [JsonProperty("links")]
            public IList<Link> links { get; set; }
            [JsonProperty("elevationLoss")]
            public double elevationLoss { get; set; }
            [JsonProperty("elevationGain")]
            public double elevationGain { get; set; }
            [JsonProperty("elevationMax")]
            public double elevationMax { get; set; }
            [JsonProperty("elevationMin")]
            public double elevationMin { get; set; }
            [JsonProperty("intervalMetric")]
            public int intervalMetric { get; set; }
            [JsonProperty("intervalUnit")]
            public string intervalUnit { get; set; }
            [JsonProperty("waypoints")]
            public IList<Waypoint> waypoints { get; set; }
        }

        public class Waypoint
        {
            [JsonProperty("latitude")]
            public double latitude { get; set; }
            [JsonProperty("longitude")]
            public double longitude { get; set; }
            [JsonProperty("elevation")]
            public double elevation { get; set; }
        }
    }

}