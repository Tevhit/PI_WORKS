using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PI_WORKS
{
    class ExhibitValue
    {
        private string PLAY_ID;
        private int SONG_ID;
        private int CLIENT_ID;
        private DateTime PLAY_TS;

        public ExhibitValue(string PLAY_ID, int SONG_ID, int CLIENT_ID, DateTime PLAY_TS)
        {
            this.PLAY_ID = PLAY_ID;
            this.SONG_ID = SONG_ID;
            this.CLIENT_ID = CLIENT_ID;
            this.PLAY_TS = PLAY_TS;
        }

        public string getPLAY_ID()
        {
            return this.PLAY_ID;
        }

        public int getSONG_ID()
        {
            return this.SONG_ID;
        }

        public int getCLIENT_ID()
        {
            return this.CLIENT_ID;
        }

        public DateTime getPLAY_TS()
        {
            return this.PLAY_TS;
        }
    }

    class Program
    {
        private static string csv_path = "C:/Users/Batuhan Karsli/source/repos/PI_WORKS/PI_WORKS/exhibitA-input.csv";
        static void Main(string[] args)
        {
            List<ExhibitValue> exhibits_list = new List<ExhibitValue>();
            string[] lines = System.IO.File.ReadAllLines(csv_path);
            lines = lines.Skip(1).ToArray(); // skip first line that is "PLAY_ID SONG_ID CLIENT_ID PLAY_TS"

            // Get all records where day = August 10, 2016 
            // to list of "exhibits_list"
            string[] temp_values;
            DateTime temp_dt;
            foreach (string line in lines)
            {
                temp_values = line.Split('	');
                temp_dt = Convert.ToDateTime(temp_values[3]);
                if (temp_dt.Day == 10 && temp_dt.Month == 8 && temp_dt.Year == 2016)
                {
                    exhibits_list.Add(new ExhibitValue(
                        Convert.ToString(temp_values[0]),
                        Convert.ToInt32(temp_values[1]),
                        Convert.ToInt32(temp_values[2]),
                        temp_dt));
                }
            }
            // So, the "List<ExhibitValue> exhibits_list" shows the distribution 
            // distinct song play counts per user on date of August 10, 2016 ONLY

            // Getting distinct played songs id for all clients
            // (1, {9857, 3022})
            // (2, {7565, 5839, 7256, 217})
            // (3, {544, 376})
            // according to the example of [Q1]
            Dictionary<int, List<int>> distinct_clients_dic = new Dictionary<int, List<int>>();
            int temp_client_id;
            int temp_song_id;
            for (int i = 0; i < exhibits_list.Count; i++)
            {
                temp_client_id = exhibits_list[i].getCLIENT_ID();
                temp_song_id = exhibits_list[i].getSONG_ID();
                
                // Each client key will be unique
                if(!(distinct_clients_dic.ContainsKey(temp_client_id)))
                {
                    distinct_clients_dic.Add(temp_client_id, new List<int>());
                    distinct_clients_dic[temp_client_id].Add(temp_song_id);
                }
                else
                {
                    // Each song value will be unique for each client
                    if (!(distinct_clients_dic[temp_client_id].Contains(temp_song_id)))
                        distinct_clients_dic[temp_client_id].Add(temp_song_id);
                }
            }

            // Get "client" and "distinct song count" in a dictionary
            // (1, 2)
            // (2, 4)
            // (3, 2)
            // according to the example of [Q1]
            Dictionary<int, int> distinct_songs_dic = new Dictionary<int, int>();
            foreach (var item in distinct_clients_dic)
            {
                distinct_songs_dic.Add(item.Key, item.Value.Count);
                //Console.WriteLine("Client : {0} - Distinct Song Count : {1}", item.Key, item.Value.Count);
            }

            // Get "DISTINCT_PLAY_COUNT" and "CLIENT_COUNT" in a dictionary
            // (2, 2)
            // (4, 1)
            // according to the example of [Q1]
            Dictionary<int, int> distinct_clients_songs_dic = new Dictionary<int, int>();
            foreach (var item in distinct_songs_dic)
            {
                // Each DISTINCT_PLAY_COUNT will be unique
                if (!(distinct_clients_songs_dic.ContainsKey(item.Value)))
                    distinct_clients_songs_dic.Add(item.Value, 1);
                else
                    distinct_clients_songs_dic[item.Value] += 1;
            }

            // [Q2] How many users played 346 distinct songs?
            int count_346_distinct_songs = distinct_clients_songs_dic[346];
            Console.WriteLine("Count of users played 346 distinct songs = {0}", count_346_distinct_songs); // 22

            // [Q3] What is the maximum number of distinct songs  played?
            int maximum_number_of_distinct_songs_played = distinct_clients_songs_dic.Keys.Max();
            Console.WriteLine("Maximum number of distinct songs played = {0}", maximum_number_of_distinct_songs_played); // 393

            Console.ReadKey();
        }
    }
}
