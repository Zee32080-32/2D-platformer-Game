using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Coursework_Retake
{
    class Loader
    {
        private Stream mFileStream = null;

        public Loader(Stream stream)
        {
            mFileStream = stream;
        }

        public string ReadTextFileComplete()
        {
            StringBuilder result = new StringBuilder();
            try
            {

                using (StreamReader reader = new StreamReader(mFileStream))
                {

                    result.Append(reader.ReadToEnd());
                }
            }
            catch (Exception e)
            {

                Console.WriteLine("ERROR: File could not be read!");
                Console.WriteLine("Exception Message: " + e.Message);
            }

            // Return the resulting string
            return result.ToString();
        }

        public List<string> ReadLinesFromTextFile()
        {

            string line = "";

            // Initialise a list to contain the results
            List<string> lines = new List<string>();

            try
            {
                using (StreamReader reader = new StreamReader(mFileStream))
                {

                    while ((line = reader.ReadLine()) != null)
                    {
                        // Add the line to the collection
                        lines.Add(line);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: File could not be read!");
                Console.WriteLine("Exception Message: " + e.Message);
            }

            return lines;
        }

        public void ReadXML(string filename)
        {
            try
            {
                using (StreamReader reader = new StreamReader(filename))
                {
                    Game_Engine.Instance = (Game_Engine)new XmlSerializer(typeof(Game_Engine)).Deserialize(reader.BaseStream);
                }
            }
            catch (Exception e)
            {

                Console.WriteLine("ERROR: XML File could not be deserialized!");
                Console.WriteLine("Exception Message: " + e.Message);
            }
        }

    }
}
