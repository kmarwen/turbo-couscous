using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tcx = NikePlusToTCX.TCX;
using NikeJson = NikePlusToTCX.NikeJson;

namespace NikePlusToTCX
{
    public partial class Form1 : Form
    {
        NikeJson.ActivitiesContainer activitiesContainer = null;
        private string access_token = string.Empty;
        public Form1()
        {
            InitializeComponent();

            // Probably shouldnt touch anything below here, unless something drastically changes
            /*	string nikeplus_base_url = "https://api.nike.com";

                string nikeplus_headers = {	'Appid' : nikeplus_app_id,
                            'Accept' : 'application/json'}

                nikeplus_authentication_parameters = {'app' : nikeplus_app_id,
                            'client_id' : nikeplus_client_id,
                            'client_secret' : nikeplus_client_secret}

                nikeplus_endpoints = {	'aggregate_sports_data' : 	nikeplus_base_url+'/me/sport',
                            'list_activities' : 	  	nikeplus_base_url+'/me/sport/activities',
                            'activity_detail' :		nikeplus_base_url+'/me/sport/activities/%(activity_id)s',
                            'gps_data' : 			nikeplus_base_url+'/me/sport/activities/%(activity_id)s/gps',
                            'login' :			nikeplus_base_url+'/nsl/v2.0/user/login'}

                nikeplus_activity_list_limit = 50 #API shits itself if we set this too high
                nikeplus_timeout_seconds = 30
                    }
                    */
            dateTimePicker_startDate.Value = dateTimePicker_startDate.Value.AddDays(-10);

        }

                
        
        private string getRequestURL()
        {
            access_token = txb_Token.Text; 
            string count = tb_count.Text;
            string startDate = dateTimePicker_startDate.Value.ToString("yyyy-MM-dd");
            string endDate = dateTimePicker_endDate.Value.ToString("yyyy-MM-dd");
            string requestUrl = tb_url.Text
                    .Replace("{access_token}", access_token)
                    .Replace("{startDate}", startDate)
                    .Replace("{endDate}", endDate)
                    .Replace("{count}", count)
                    ;
            return requestUrl;
        }

        Dictionary<string, NikeJson.Activity> dicActivities;

        private void btn_GetAllActivities_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            string requestUrl = getRequestURL();
            
            try
            {
                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(
                            String.Format("Server error (HTTP {0}: {1}).",
                                            response.StatusCode,
                                            response.StatusDescription));

                    using (Stream responseStream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                        string activitiesJsonData = reader.ReadToEnd();
                        activitiesContainer = JsonConvert.DeserializeObject<NikeJson.ActivitiesContainer>(activitiesJsonData);
                        activitiesContainer.jsonData = activitiesJsonData;

                        dicActivities = new Dictionary<string, NikeJson.Activity>();

                        foreach (NikeJson.Activity activity in activitiesContainer.activities)
                        {
                            activity.GetActivityDetails(access_token);

                            dicActivities.Add(activity.activityName, activity);
                            listBox1.Items.Add(activity.activityName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                richTextBox1.Text += ex.Message;
            }
        }

        private void btn_Convert_Click(object sender, EventArgs e)
        {
            foreach (var selectedActivity in listBox1.SelectedItems)
            {
                var activityName = selectedActivity as string;

                NikeJson.Activity jsonActivity = dicActivities[activityName];
                if (jsonActivity != null)
                {
                    // Only One lap for now, Nike+ doesn't store laps
                    List<TCX.TrainingCenterDatabaseActivitiesActivityLap> Laps = new List<TCX.TrainingCenterDatabaseActivitiesActivityLap>();
                    TCX.TrainingCenterDatabaseActivitiesActivityLap lap = new TCX.TrainingCenterDatabaseActivitiesActivityLap(jsonActivity);

                    Laps.Add(lap);

                    TCX.TrainingCenterDatabaseActivitiesActivity activity = new TCX.TrainingCenterDatabaseActivitiesActivity();
                    activity.Sport = "Running";
                    activity.Id = jsonActivity.startTime.ToUniversalTime();
                    activity.Laps = Laps;


                    // Only One lap for now, Nike+ doesn't store laps
                    TCX.TrainingCenterDatabaseActivities activities = new TCX.TrainingCenterDatabaseActivities()
                    {
                        Activity = activity
                    };
                    TCX.TrainingCenterDatabase tcx = new TCX.TrainingCenterDatabase()
                    {
                        Activities = activities
                    };



                    // generation TCX File
                    string filePath = @"C:\Users\KAROUI\Desktop\Nike\NikePlus-MKExporter\" + activityName + ".tcx";
                    System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(tcx.GetType());

                    using (StreamWriter streamWriter = new StreamWriter(filePath))
                    {
                        serializer.Serialize(streamWriter, tcx);
                        StringBuilder sb = new StringBuilder();
                        using (StringWriter sw = new StringWriter(sb))
                        {
                            sw.Write(streamWriter);
                            richTextBox1.Text += filePath + " OK\n";
                        }
                    }
                    /*
                                New Mizuno Inspire 11 ^_^ 
                                Course Importé de nike+ avec mon outil perso génial ^_^
                    */
                    //    System.IO.File.WriteAllText(filePath, textWriter.ToString(), Encoding.UTF8);

                }
            }
        }

        private void dateTimePicker_startDate_ValueChanged(object sender, EventArgs e)
        {
            dateTimePicker_endDate.Value = dateTimePicker_startDate.Value;
        }
    }
}
