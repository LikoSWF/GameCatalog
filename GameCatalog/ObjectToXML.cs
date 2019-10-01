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

        private XmlSerializer serializer = new XmlSerializer(typeof(T));

        public ObjectToXML(T data)
        {
            this.Data = data;
        }

        public void SaveData()
        {
            using (StreamWriter file = File.CreateText(this.savePath))
            {
                this.serializer.Serialize(file, this.Data);
            }
        }

        public void LoadData()
        {
            using (StreamReader file = new StreamReader(this.savePath))
            {
                this.Data = (T)this.serializer.Deserialize(file);
            }
        }
    }
}
