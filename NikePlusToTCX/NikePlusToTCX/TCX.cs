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
                //get { return this.Id.ToString("yyyy-MM-ddTHH:mm:ssK"); }
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
            public TrainingCenterDatabaseActivitiesActivityLap() { } //default
            public TrainingCenterDatabaseActivitiesActivityLap(NikeJson.Activity nikeActivity)
            {
                this.jsonActivity = nikeActivity;

                List<TCX.TrainingCenterDatabaseActivitiesActivityLapTrackpoint> trackPointsList = new List<TCX.TrainingCenterDatabaseActivitiesActivityLapTrackpoint>();
                

                // TODO : use try catch in case of no metrics speed
                NikeJson.Metric metricsDistance = nikeActivity.metrics.Where(m => (m.metricType == "DISTANCE")).First();
                //NikeJson.Metric metricsSpeed = nikeActivity.metrics.Where(m => (m.metricType == "SPEED")).First();
                // replace . with , otherwise convertion could not be possible
                metricsDistance.values = metricsDistance.values.Select(x => x.Replace('.', ',')).ToList();
                //metricsSpeed.values = metricsSpeed.values.Select(x => x.Replace('.', ',')).ToList();


                List<double> newSpeeds = new List<double>();
                for (int i = 0; i < metricsDistance.values.Count; i++)
                {
                    double alphaDistance = 0;
                    if (i == 0)
                    {
                        alphaDistance = double.Parse(metricsDistance.values[i]);
                    }
                    else
                    {
                        alphaDistance = double.Parse(metricsDistance.values[i]) - double.Parse(metricsDistance.values[i - 1]);
                    }
                    double speed = 1000 * (alphaDistance / 10);
                    newSpeeds.Add(speed);
                }

                metricsDistance.newSpeedValues = newSpeeds;
                MaximumSpeed = metricsDistance.newSpeedValues.Max();
                // obligatoire
                TotalTimeSeconds = nikeActivity.metricSummary.TotalTimeSecondsInt;
                var endTime = nikeActivity.startTime.AddSeconds(TotalTimeSeconds);

                if (nikeActivity.isGpsActivity)
                {
                    // étalement sur secondes // adding new lists inside metrics with extended times and distances
                    ExpandDataToSeconds(metricsDistance);
                    //ExpandDataToSeconds(metricsSpeed);

                    double ratio = (double)metricsDistance.newSecondValues.Count / (double)nikeActivity.activityGps.waypoints.Count;

                    for (int i = 0; i < nikeActivity.activityGps.waypoints.Count; i++)
                    {
                        // index to use
                        var secondsIndex = ratio * i;

                        // Corresponding rounded index in list
                        int secondsIndexRounded = (int)Math.Round(secondsIndex, MidpointRounding.AwayFromZero);

                        TCX.TrainingCenterDatabaseActivitiesActivityLapTrackpoint trackpoint = new TCX.TrainingCenterDatabaseActivitiesActivityLapTrackpoint();

                        trackpoint.AltitudeMeters = nikeActivity.activityGps.waypoints[i].elevation;
                        trackpoint.AltitudeMetersSpecified = true;
                        trackpoint.Position = new TrainingCenterDatabaseActivitiesActivityLapTrackpointPosition()
                        {
                            LatitudeDegrees = nikeActivity.activityGps.waypoints[i].latitude,
                            LongitudeDegrees = nikeActivity.activityGps.waypoints[i].longitude,
                        };
                        var timeStamp = nikeActivity.startTime.AddSeconds(secondsIndexRounded);

                        trackpoint.Time = timeStamp;
                        trackpoint.DistanceMeters = metricsDistance.newSecondValues[secondsIndexRounded] * 1000;
                        trackPointsList.Add(trackpoint);

                        /*
                         * 
                         * Finally no need to the speed !!!!!!!!!!
                         * 
                         * 
                        if (timeStamp < endTime)
                        {
                            trackpoint.Time = timeStamp;
                            trackpoint.DistanceMeters = metricsDistance.newSecondValues[secondsIndexRounded] * 1000;

                            //RunCadence = 100,
                            /*
                            var speed = metricsSpeed.newSecondValues[secondsIndexRounded];

                            if (speed > 0)
                            {
                                trackpoint.Extensions = new TCX.TrainingCenterDatabaseActivitiesActivityLapTrackpointExtensions();
                                trackpoint.Extensions.TPX = new TCX.TPX();
                                trackpoint.Extensions.TPX.Speed = metricsSpeed.newSecondValues[secondsIndexRounded];
                            }
                            
                            // add the trackpoint
                            trackPointsList.Add(trackpoint);
                        }
                        else
                        {
                            trackpoint.Time = endTime;
                            trackpoint.DistanceMeters = metricsDistance.newSecondValues[metricsDistance.newSecondValues.Count - 1] * 1000;

                            //RunCadence = 100,

                            var speed = metricsSpeed.newSecondValues[metricsSpeed.newSecondValues.Count - 1];
                            /*
                            if (speed > 0)
                            {
                                trackpoint.Extensions = new TCX.TrainingCenterDatabaseActivitiesActivityLapTrackpointExtensions();
                                trackpoint.Extensions.TPX = new TCX.TPX();
                                trackpoint.Extensions.TPX.Speed = metricsSpeed.newSecondValues[metricsSpeed.newSecondValues.Count - 1];
                            }
                            
                            // add the trackpoint
                            trackPointsList.Add(trackpoint);
                            break;
                        }
                        */
                    }
                }
                else
                {
                    for (int i = 0; i < metricsDistance.values.Count; i++)
                    //for (int i = 0; i < TotalTimeSeconds; i++)
                    {
                        TCX.TrainingCenterDatabaseActivitiesActivityLapTrackpoint trackpoint = new TCX.TrainingCenterDatabaseActivitiesActivityLapTrackpoint();
                        var timeStamp = nikeActivity.startTime.AddSeconds((double)metricsDistance.intervalMetric * i);
                        if (timeStamp < endTime)
                        {
                            trackpoint.Time = timeStamp;
                            trackpoint.DistanceMeters = double.Parse(metricsDistance.values[i]) * 1000;

                            ////var speed = double.Parse(metricsSpeed.values[i]);
                            //var speed = metricsSpeed.newSpeedValues[i];
                            //if (speed > 0)
                            //{
                            //    trackpoint.Extensions = new TCX.TrainingCenterDatabaseActivitiesActivityLapTrackpointExtensions();
                            //    trackpoint.Extensions.TPX = new TCX.TPX();
                            //    trackpoint.Extensions.TPX.Speed = speed;
                            //}

                            
                            trackPointsList.Add(trackpoint);

                            if (i == metricsDistance.values.Count - 1)
                            {
                                AddFinalTrackPointNoGPS(trackPointsList, metricsDistance, endTime, trackpoint);
                                break;
                            }
                        }
                        else
                        {
                            AddFinalTrackPointNoGPS(trackPointsList, metricsDistance, endTime, trackpoint);
                            break;
                        }
                    }
                }

                TCX.TrainingCenterDatabaseActivitiesActivityLapTrack track = new TrainingCenterDatabaseActivitiesActivityLapTrack()
                {
                    Trackpoints = trackPointsList,
                };


                //AverageHeartRateBpm = new TCX.TrainingCenterDatabaseActivitiesActivityLapAverageHeartRateBpm() { Value = 160 };
                //MaximumHeartRateBpm = new TCX.TrainingCenterDatabaseActivitiesActivityLapMaximumHeartRateBpm() { Value = 188 };

                decimal decimalTimeTotal = Convert.ToDecimal(TotalTimeSeconds);
                decimal totalTimeMinutes = decimal.Round(decimal.Divide(decimalTimeTotal, 60), MidpointRounding.AwayFromZero);

                /*
                 * 
                 * Nike Cadence = total steps per minute
                 * Garmin Cadence = (totalsteps / 2 ) per minute (I think for biking)
                 * Here I use nike Cadence (total steps / total minutes)
                 * finally I divide cadence / 2
                 * 
                Cadence = Convert.ToInt32(
                    decimal.Round(
                        decimal.Divide(
                                        decimal.Parse(nikeActivity.metricSummary.steps),
                                        totalTimeMinutes
                                        ),
                                    MidpointRounding.AwayFromZero)
                                );
                */

                TotalSteps = nikeActivity.metricSummary.stepsInt; 
                Cadence = (TotalSteps / Convert.ToInt32(totalTimeMinutes)) / 2;

                Calories = int.Parse(nikeActivity.metricSummary.calories);
                
                // the last trackpoint contains le total distance ;)
                DistanceMeters = double.Parse(nikeActivity.metricSummary.distance.Replace('.', ',')) * 1000; //trackPointsList.Max(x => x.DistanceMeters);
                //Intensity = "active",
                StartTime = nikeActivity.startTime;
                TriggerMethod = "MarwenK-Script";
                Track = trackPointsList;
            }

            private static void AddFinalTrackPointNoGPS(List<TCX.TrainingCenterDatabaseActivitiesActivityLapTrackpoint> trackPointsList, NikeJson.Metric metricsDistance, DateTime endTime, TCX.TrainingCenterDatabaseActivitiesActivityLapTrackpoint trackpoint)
            {
                trackpoint.Time = endTime;
                trackpoint.DistanceMeters = 1000 * double.Parse(metricsDistance.values[metricsDistance.values.Count - 1]);// // double.Parse(nikeActivity.metricSummary.distance.Replace('.', ','));
                trackPointsList.Add(trackpoint);
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
                //// if totalTimeSeconds > (generated) total recorded * 10sec
                //if (metricsMesure.newSecondValues.Count < this.TotalTimeSeconds)
                //{
                //    metricsMesure.newSecondValues.Add(val_i);
                //}
            }

            //public int TotalTimeSeconds
            //{
            //    get
            //    {
            //        //string[] durationTab = jsonActivity.metricSummary.duration.Split(':');
            //        return jsonActivity.metricSummary.TotalTimeSeconds;
            //    }
            //}

            public double DistanceMeters { get; set; }

            public double MaximumSpeed { get; set; }

            public int TotalSteps { get; set; }
            public int Calories { get; set; }
            public int TotalTimeSeconds { get; set; }

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
            [XmlElement()]
            public double Speed { get; set; }

            [XmlAttributeAttribute()]
            public string CadenceSensor { get; set; }
        }


    }




































}
