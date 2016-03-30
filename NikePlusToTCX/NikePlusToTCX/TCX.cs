using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NikePlusToTCX
{
    public class TCX
    {
        //////////////////////////////////////////////////////////

        [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.garmin.com/xmlschemas/TrainingCenterDatabase/v2")]
        [XmlRootAttribute(Namespace = "http://www.garmin.com/xmlschemas/TrainingCenterDatabase/v2", IsNullable = false)]
        public partial class TrainingCenterDatabase
        {
            public TrainingCenterDatabaseActivities Activities { get; set; }
            //public object Folders {get;set;}
        }

        /// <remarks/>
        [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.garmin.com/xmlschemas/TrainingCenterDatabase/v2")]
        public partial class TrainingCenterDatabaseActivities
        {
            public TrainingCenterDatabaseActivitiesActivity Activity { get; set; }
        }

        /// <remarks/>
        [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.garmin.com/xmlschemas/TrainingCenterDatabase/v2")]
        public partial class TrainingCenterDatabaseActivitiesActivity
        {
            [XmlIgnore]
            public DateTime Id { get; set; }
            [XmlElement("Id")]
            public string FormattedId
            {
                get { return this.Id.ToString("yyyy-MM-ddTHH:mm:ss.fffK"); }
                set { this.Id = DateTime.Parse(value); }
            }

            /// <remarks/>
            [XmlElementAttribute("Lap")]
            public List<TrainingCenterDatabaseActivitiesActivityLap> Laps { get; set; }

            /// <remarks/>
            [XmlAttributeAttribute()]
            public string Sport { get; set; }
        }

        /// <remarks/>
        [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.garmin.com/xmlschemas/TrainingCenterDatabase/v2")]
        public partial class TrainingCenterDatabaseActivitiesActivityLap
        {

            public TrainingCenterDatabaseActivitiesActivityLap()
            {

            }

            public TrainingCenterDatabaseActivitiesActivityLap(NikeJson.Activity nikeActivity)
            {
                // TODO: Complete member initialization
                this.jsonActivity = nikeActivity;

                List<TCX.TrainingCenterDatabaseActivitiesActivityLapTrackpoint> trackPointsList = new List<TCX.TrainingCenterDatabaseActivitiesActivityLapTrackpoint>();

                NikeJson.Metric metricsDistance = nikeActivity.metrics.Where(m => (m.metricType == "DISTANCE")).First();
                NikeJson.Metric metricsSpeed = nikeActivity.metrics.Where(m => (m.metricType == "SPEED")).First();



                // replace . with , otherwise convertion would not be possible
                metricsDistance.values = metricsDistance.values.Select(x => x.Replace('.', ',')).ToList();
                metricsSpeed.values = metricsSpeed.values.Select(x => x.Replace('.', ',')).ToList();

                // beta
                /*
                if (nikeActivity.isGpsActivity)
                {
                    metricsDistance.newSecondValues = new List<double>();
                    ExpandDataToSeconds(metricsDistance);
                    metricsSpeed.newSecondValues = new List<double>();
                    ExpandDataToSeconds(metricsSpeed);

                    double ratio = (double)nikeActivity.activityGps.waypoints.Count / (double)metricsDistance.newSecondValues.Count;
                    for (int i = 0; i < metricsDistance.newSecondValues.Count; i++)
                    {
                        var wayPointIndex = i * ratio;

                        bool valUsed = false;
                        if (!valUsed)
                        {
                            var val = decimal.Round((decimal)wayPointIndex, MidpointRounding.AwayFromZero);
                            valUsed = true;
                        }
                        while (valUsed)
                        {

                            continue;
                        }
                    }
                }
                */





                for (int i = 0; i < metricsDistance.values.Count; i++)
                {
                    TCX.TrainingCenterDatabaseActivitiesActivityLapTrackpoint trackpoint = new TCX.TrainingCenterDatabaseActivitiesActivityLapTrackpoint();


                    // GPS : Position (LatitudeDegrees + LongitudeDegrees) + AltitudeMeters
                    if (nikeActivity.isGpsActivity)
                    {
                        // régle de 3 pour récupérer la position correspondante
                        var wayPointIndex = (nikeActivity.activityGps.waypoints.Count * (i * metricsDistance.intervalMetric) / TotalTimeSeconds) - 1; // -1 pour ne pas dépasser l'index de la liste;
                        if (i == 0)
                        {
                            wayPointIndex++;
                        }
                        if (wayPointIndex >= nikeActivity.activityGps.waypoints.Count)
                        {
                            wayPointIndex = nikeActivity.activityGps.waypoints.Count - 1;
                        }
                        trackpoint.AltitudeMeters = nikeActivity.activityGps.waypoints[wayPointIndex].elevation;
                        trackpoint.AltitudeMetersSpecified = true;
                        trackpoint.Position = new TrainingCenterDatabaseActivitiesActivityLapTrackpointPosition()
                        {
                            LatitudeDegrees = nikeActivity.activityGps.waypoints[wayPointIndex].latitude,
                            LongitudeDegrees = nikeActivity.activityGps.waypoints[wayPointIndex].longitude,
                        };
                    }


                    trackpoint.Time = nikeActivity.startTime.AddSeconds((double)metricsDistance.intervalMetric * i);
                    trackpoint.DistanceMeters = double.Parse(metricsDistance.values[i]) * 1000;


                    //RunCadence = 100,
                    trackpoint.Extensions = new TCX.TrainingCenterDatabaseActivitiesActivityLapTrackpointExtensions();
                    trackpoint.Extensions.TPX = new TCX.TPX();
                    trackpoint.Extensions.TPX.Speed = double.Parse(metricsSpeed.values[i]);



                    trackPointsList.Add(trackpoint);
                }
                // adding the last seconds (diffrence before the 10 sec)
                TCX.TrainingCenterDatabaseActivitiesActivityLapTrackpoint tp = new TCX.TrainingCenterDatabaseActivitiesActivityLapTrackpoint();
                tp.Time = nikeActivity.startTime.AddSeconds(this.TotalTimeSeconds);
                tp.DistanceMeters = double.Parse(nikeActivity.metricSummary.distance.Replace('.', ','), System.Globalization.NumberFormatInfo.CurrentInfo) * 1000;
                trackPointsList.Add(tp);

                if (trackPointsList.Max(x => x.DistanceMeters) > tp.DistanceMeters)
                {
                    tp.DistanceMeters = trackPointsList.Max(x => x.DistanceMeters);
                }


                TCX.TrainingCenterDatabaseActivitiesActivityLapTrack track = new TrainingCenterDatabaseActivitiesActivityLapTrack()
                {
                    Trackpoints = trackPointsList,
                };


                //AverageHeartRateBpm = new TCX.TrainingCenterDatabaseActivitiesActivityLapAverageHeartRateBpm() { Value = 160 };
                //MaximumHeartRateBpm = new TCX.TrainingCenterDatabaseActivitiesActivityLapMaximumHeartRateBpm() { Value = 188 };

                MaximumSpeed = metricsSpeed.values.Max().First();
                //Cadence = 0,
                Calories = int.Parse(nikeActivity.metricSummary.calories);
                // the last trackpoint contains le total distance ;)
                DistanceMeters = trackPointsList.Max(x => x.DistanceMeters);
                //Intensity = "active",
                StartTime = nikeActivity.startTime;
                TriggerMethod = "Manual";
                Track = trackPointsList;
            }

            private static void ExpandDataToSeconds(NikeJson.Metric metricsMesure)
            {
                for (int i = 0; i < metricsMesure.values.Count - 1; i++)
                {
                    double delta = (double.Parse(metricsMesure.values[i + 1]) - double.Parse(metricsMesure.values[i])) / 10;
                    for (int j = 0; j < 10; j++)
                    {
                        metricsMesure.newSecondValues.Add(double.Parse(metricsMesure.values[i]) + (j * delta));
                    }
                }
            }


            private NikeJson.Activity jsonActivity;

            private int totalTimeSecondsField;
            public int TotalTimeSeconds
            {
                get
                {
                    string[] durationTab = jsonActivity.metricSummary.duration.Split(':');
                    TimeSpan timeDuration = new TimeSpan(Int32.Parse(durationTab[0]), Int32.Parse(durationTab[1]), Int32.Parse(durationTab[2].Split('.')[0]));
                    this.totalTimeSecondsField = (int)timeDuration.TotalSeconds;

                    return this.totalTimeSecondsField;
                }
                set
                {
                    this.totalTimeSecondsField = value;
                }
            }

            public double DistanceMeters { get; set; }

            /// <remarks/>
            public double MaximumSpeed { get; set; }

            /// <remarks/>
            public int Calories { get; set; }

            public TrainingCenterDatabaseActivitiesActivityLapAverageHeartRateBpm AverageHeartRateBpm { get; set; }

            public TrainingCenterDatabaseActivitiesActivityLapMaximumHeartRateBpm MaximumHeartRateBpm { get; set; }

            /// <remarks/>
            public string Intensity { get; set; }

            public int Cadence { get; set; }

            /// <remarks/>
            public string TriggerMethod { get; set; }

            [XmlArrayItemAttribute("Trackpoint", IsNullable = false)]
            public List<TrainingCenterDatabaseActivitiesActivityLapTrackpoint> Track { get; set; }


            [XmlIgnoreAttribute]
            public DateTime StartTime { get; set; }
            [XmlAttributeAttribute("StartTime")]
            public string StartTimeFormatted
            {
                get { return this.StartTime.ToString("yyyy-MM-ddTHH:mm:ss.fffK"); }
                set { this.StartTime = DateTime.Parse(value); }
            }



        }
        [XmlTypeAttribute(AnonymousType = true)]
        public partial class TrainingCenterDatabaseActivitiesActivityLapTrack
        {
            public List<TrainingCenterDatabaseActivitiesActivityLapTrackpoint> Trackpoints { get; set; }
        }
        [XmlTypeAttribute(AnonymousType = true)]
        public partial class TrainingCenterDatabaseActivitiesActivityLapAverageHeartRateBpm
        {
            public int Value { get; set; }
        }
        [XmlTypeAttribute(AnonymousType = true)]
        public partial class TrainingCenterDatabaseActivitiesActivityLapMaximumHeartRateBpm
        {
            public int Value { get; set; }
        }

        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class TrainingCenterDatabaseActivitiesActivityLapTrackpoint
        {
            [XmlIgnore]
            public System.DateTime Time { get; set; }
            [XmlElement("Time")]
            public string TimeFormatted
            {
                get { return this.Time.ToString("yyyy-MM-ddTHH:mm:ss.fffK"); }
                set { this.Time = DateTime.Parse(value); }
            }



            public double DistanceMeters { get; set; }


            public TrainingCenterDatabaseActivitiesActivityLapTrackpointPosition Position { get; set; }

            public double AltitudeMeters { get; set; }


            [XmlIgnoreAttribute()]
            public bool AltitudeMetersSpecified { get; set; }

            public TrainingCenterDatabaseActivitiesActivityLapTrackpointHeartRateBpm HeartRateBpm { get; set; }

            public TrainingCenterDatabaseActivitiesActivityLapTrackpointExtensions Extensions { get; set; }
        }


        [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.garmin.com/xmlschemas/TrainingCenterDatabase/v2")]
        public partial class TrainingCenterDatabaseActivitiesActivityLapTrackpointPosition
        {
            public double LatitudeDegrees { get; set; }

            public double LongitudeDegrees { get; set; }
        }

        [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.garmin.com/xmlschemas/TrainingCenterDatabase/v2")]
        public partial class TrainingCenterDatabaseActivitiesActivityLapTrackpointHeartRateBpm
        {
            public int Value { get; set; }
        }

        /// <remarks/>
        [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.garmin.com/xmlschemas/TrainingCenterDatabase/v2")]
        public partial class TrainingCenterDatabaseActivitiesActivityLapTrackpointExtensions
        {
            [XmlElementAttribute(Namespace = "http://www.garmin.com/xmlschemas/ActivityExtension/v2")]
            public TPX TPX { get; set; }
        }


        [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.garmin.com/xmlschemas/ActivityExtension/v2")]
        [XmlRootAttribute(Namespace = "http://www.garmin.com/xmlschemas/ActivityExtension/v2", IsNullable = false)]
        public partial class TPX
        {
            public double Speed { get; set; }

            [XmlAttributeAttribute()]
            public string CadenceSensor { get; set; }
        }


    }




































}
