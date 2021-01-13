using System;
using System.Collections.Generic;
using System.Xml;
using System.Windows.Forms;
using System.IO;
namespace PC_Monitor
{
    class LogHelper
    {
        private string _pathToFile = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()))+@"\criticalLogs.xml";
        private Dictionary<string, float> _criticalValues = new Dictionary<string, float>();
        private SQLHelper _toDB = new SQLHelper();
        public LogHelper() { }

        // Getting limit values from textboxes
        public void SetValuesToCheck(string sensor,float value)
        {
            if (!String.IsNullOrEmpty(sensor) || value != 0)
            {
                _criticalValues[sensor] = value;
            }
            else
            {
                MessageBox.Show("Invalid input");
            }
        }

        // Getting current values from selected sensors
        public void GetValuesToCheck(string sensor, float value)
        {
            if (_criticalValues.ContainsKey(sensor) && _criticalValues[sensor] < value)
            {
                WriteLog(value, _criticalValues[sensor], sensor);
            }
        }
        
        // Write all needed info to XML log
        private void WriteLog(float currentValue, float expectedValue,string sensor)
        {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(_pathToFile);
                XmlElement xRoot = xDoc.DocumentElement;
                XmlElement recording = xDoc.CreateElement("criticalInfo");
                XmlAttribute nameAttr = xDoc.CreateAttribute("Sensor");
                XmlElement expected = xDoc.CreateElement("expectedValue");
                XmlElement current = xDoc.CreateElement("currentValue");
                XmlElement date = xDoc.CreateElement("loggedDate");
                // создаем текстовые значения для элементов и атрибута
                XmlText nameText = xDoc.CreateTextNode(sensor);
                XmlText companyText = xDoc.CreateTextNode(expectedValue.ToString());
                XmlText ageText = xDoc.CreateTextNode(currentValue.ToString());
                var dateLog = DateTime.Now;
                XmlText ageText1 = xDoc.CreateTextNode(dateLog.ToString());

                //добавляем узлы
                nameAttr.AppendChild(nameText);
                expected.AppendChild(companyText);
                current.AppendChild(ageText);
                date.AppendChild(ageText1);
                recording.Attributes.Append(nameAttr);
                recording.AppendChild(expected);
                recording.AppendChild(current);
                recording.AppendChild(date);
                xRoot.AppendChild(recording);
                xDoc.Save(_pathToFile);
            _toDB.WriteToDataBase(sensor, currentValue,expectedValue, dateLog);
        }
    }
}
