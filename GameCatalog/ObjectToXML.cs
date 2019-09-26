using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Net;
using System.IO;



namespace GameCatalog
{
    class ObjectToXML<T>
    {
        private string savePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\save.txt";
        public T Data { get; set; }

        public ObjectToXML(T data)
        {
            this.Data = data;
        }

        public void SaveData()
        {
            XmlSerializer writer = new XmlSerializer(typeof(T));
            using (StreamWriter file = File.CreateText(this.savePath))
            {
                writer.Serialize(file, this.Data);
            }
        }

        public void LoadData()
        {
            XmlSerializer reader = new XmlSerializer(typeof(T));
            StreamReader file = new StreamReader(savePath);
            this.Data = (T)reader.Deserialize(file);
            file.Close();
        }
    }
}
