using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace INM379CWCGA
{
    class ScoreManager
    {
        public List<ScoresFile> Rankings { get; private set; }
        public List<ScoresFile> Highscores { get; private set; }

        private static string FileName = "Ranking.xml";

        public ScoreManager() : this(new List<ScoresFile>()) { }
        public ScoreManager(List<ScoresFile> rankings)
        {
            Rankings = rankings;
            HighscoresUpdate();
        }

        public static ScoreManager LoadFile()
        {
            if (!File.Exists(FileName))
                return new ScoreManager();

            using (StreamReader read = new StreamReader(new FileStream(FileName, FileMode.Open)))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<ScoresFile>));

                var ranking = (List<ScoresFile>)serializer.Deserialize(read);

                return new ScoreManager(ranking);
            }
        }

        public static void SaveFile(ScoreManager scoreM)
        {
            using (StreamWriter write = new StreamWriter(new FileStream(FileName, FileMode.Create)))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<ScoresFile>));

                serializer.Serialize(write, scoreM.Rankings);
            }
        }

        public void HighscoresUpdate()
        {
            Highscores = Rankings.Take(8).ToList();
        }

        public void AddScore(ScoresFile score)
        {
            Rankings.Add(score);

            Rankings = Rankings.OrderByDescending(a => a.Value).ToList();

            HighscoresUpdate();
        }
    }
}
