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
            private NikeJson.Activity jsonActivity;
            public TrainingCenterDatabaseActivitiesActivityLap(){ } //default
            public TrainingCenterDatabaseActivitiesActivityLap(NikeJson.Activity nikeActivity)
            {
                this.jsonActivity = nikeActivity;

                List<TCX.TrainingCenterDatabaseActivitiesActivityLapTrackpoint> trackPointsList = new List<TCX.TrainingCenterDatabaseActivitiesActivityLapTrackpoint>();

                NikeJson.Metric metricsDistance = nikeActivity.metrics.Where(m => (m.metricType == "DISTANCE")).First();
                NikeJson.Metric metricsSpeed = nikeActivity.metrics.Where(m => (m.metricType == "SPEED")).First();
                // replace . with , otherwise convertion could not be possible
                metricsDistance.values = metricsDistance.values.Select(x => x.Replace('.', ',')).ToList();
                metricsSpeed.values = metricsSpeed.values.Select(x => x.Replace('.', ',')).ToList();

                // beta
                if (nikeActivity.isGpsActivity)
                {
                    ExpandDataToSeconds(metricsDistance);
                    ExpandDataToSeconds(metricsSpeed);

                    double ratio = (double)metricsDistance.newSecondValues.Count / (double)nikeActivity.activityGps.waypoints.Count;

                    List<int> usedIndexes = new List<int>();

                    for (int i = 0; i < nikeActivity.activityGps.waypoints.Count; i++)
                    {
                        var secondsIndex = ratio * i;

                        int secondsIndexRounded = (int)Math.Round(secondsIndex, MidpointRounding.AwayFromZero);

                        if (!usedIndexes.Contains(secondsIndexRounded))
                        {
                            // ajouter l'index dans la liste pour ne plus l'utiliser
                            usedIndexes.Add(secondsIndexRounded);
                            // ici faire la boucle pour insérer les temps

                            TCX.TrainingCenterDatabaseActivitiesActivityLapTrackpoint trackpoint = new TCX.TrainingCenterDatabaseActivitiesActivityLapTrackpoint();


                            // GPS : Position (LatitudeDegrees + LongitudeDegrees) + AltitudeMeters

                            // régle de 3 pour récupérer la position correspondante
                            //var wayPointIndex = (nikeActivity.activityGps.waypoints.Count * (i * metricsDistance.intervalMetric) / TotalTimeSeconds) - 1; // -1 pour ne pas dépasser l'index de la liste;
                            //if (i == 0)
                            //{
                            //    wayPointIndexRounded++;
                            //}
                            //if (wayPointIndexRounded >= nikeActivity.activityGps.waypoints.Count)
                            //{
                            //    wayPointIndexRounded = nikeActivity.activityGps.waypoints.Count - 1;
                            //}
                            trackpoint.AltitudeMeters = nikeActivity.activityGps.waypoints[i].elevation;
                            trackpoint.AltitudeMetersSpecified = true;
                            trackpoint.Position = new TrainingCenterDatabaseActivitiesActivityLapTrackpointPosition()
                            {
                                LatitudeDegrees = nikeActivity.activityGps.waypoints[i].latitude,
                                LongitudeDegrees = nikeActivity.activityGps.waypoints[i].longitude,
                            };


                            trackpoint.Time = nikeActivity.startTime.AddSeconds(/*(double)metricsDistance.intervalMetric*/secondsIndexRounded);
                            trackpoint.DistanceMeters = metricsDistance.newSecondValues[secondsIndexRounded] * 1000;


                            //RunCadence = 100,
                            trackpoint.Extensions = new TCX.TrainingCenterDatabaseActivitiesActivityLapTrackpointExtensions();
                            trackpoint.Extensions.TPX = new TCX.TPX();
                            trackpoint.Extensions.TPX.Speed = metricsSpeed.newSecondValues[secondsIndexRounded];


                            trackPointsList.Add(trackpoint);
                        }
                    }
                }
                // beta




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

            private void ExpandDataToSeconds(NikeJson.Metric metricsMesure)
            {
                metricsMesure.newSecondValues = new List<double>();

                double val_i = 0, val_i_1 = 0, delta = 0;

                for (int i = 0; i < metricsMesure.values.Count - 1; i++)
                {
                    val_i_1 = double.Parse(metricsMesure.values[i + 1]);
                    val_i = double.Parse(metricsMesure.values[i]);

                    delta = (val_i_1 - val_i) / 10;
                    for (int j = 1; j <= 10; j++)
                    {
                        metricsMesure.newSecondValues.Add(val_i + (j * delta));
                    }
                }
                // if totalTimeSeconds > (generated) total recorded * 10sec
                if (metricsMesure.newSecondValues.Count < this.TotalTimeSeconds)
                {
                    metricsMesure.newSecondValues.Add(val_i);
                }
            }

            public int TotalTimeSeconds
            {
                get
                {
                    //string[] durationTab = jsonActivity.metricSummary.duration.Split(':');
                    return jsonActivity.metricSummary.TotalTimeSeconds;
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
