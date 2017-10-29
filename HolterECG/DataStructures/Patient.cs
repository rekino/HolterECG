using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace HolterECG.DataStructures
{
    public enum Sex
    {
        Female,
        Male
    }

    public class Reading:IXmlSerializable
    {
        public DateTime Date { get; set; }
        public double Sys { get; set; }
        public double Dia { get; set; }
        public double HR { get; set; }
        public double MABP { get; set; }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            reader.MoveToContent();
            Date = DateTime.ParseExact(reader.GetAttribute("date"), "yyyy-MM-dd", null);
            Sys = float.Parse(reader.GetAttribute("sys"));
            Dia = float.Parse(reader.GetAttribute("dia"));
            HR = float.Parse(reader.GetAttribute("hr"));
            MABP = float.Parse(reader.GetAttribute("mabp"));
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("date", this.Date.ToString("yyyy-MM-dd"));
            writer.WriteAttributeString("sys", this.Sys.ToString());
            writer.WriteAttributeString("dia", this.Dia.ToString());
            writer.WriteAttributeString("hr", this.HR.ToString());
            writer.WriteAttributeString("mabp", this.MABP.ToString());
        }
    }
    public class Patient : System.ComponentModel.INotifyPropertyChanged
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public string Physician { get; set; }
        public Sex Sex { get; set; }
        public int Age { get; set; }
        public string Insurance { get; set; }
        public string Symptoms { get; set; }
        public string Medications { get; set; }
        public string MoreInformation { get; set; }
        public List<Reading> Readings { get; private set; }

        public Patient()
        {
            Readings = new List<Reading>();
            Random rand = new Random();
            for (int i = 0; i < 70; i++)
            {
                Readings.Add(new Reading { Date=DateTime.Now.AddHours(i), HR=rand.NextDouble() * 160 + 50,
                    Dia=rand.NextDouble() * 160 + 50, MABP=rand.NextDouble() * 160 + 50, Sys=rand.NextDouble() * 160 + 50 });
            }
        }
        public string Serialize()
        {
            XmlWriterSettings settings = new XmlWriterSettings() { OmitXmlDeclaration = true };
            XmlSerializer serializer = new XmlSerializer(typeof(Patient));
            StringWriter sw = new StringWriter();
            using (XmlWriter writer = XmlWriter.Create(sw, settings))
            {
                // removes namespace
                var xmlns = new XmlSerializerNamespaces();
                xmlns.Add(string.Empty, string.Empty);

                serializer.Serialize(writer, this, xmlns);
            }
            return sw.ToString();
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    }
}
