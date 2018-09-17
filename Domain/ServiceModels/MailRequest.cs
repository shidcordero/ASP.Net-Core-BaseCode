using System;
using System.Xml;
using System.Xml.Serialization;

namespace Domain.ServiceModels
{
    [Serializable]
    [XmlRoot("MailRequest")]
    public class MailRequest
    {
        [XmlElement("To")]
        public string To { get; set; }

        [XmlElement("ToList")]
        public string[] ToList { get; set; }

        [XmlElement("Cc")]
        public string Cc { get; set; }

        [XmlElement("CcList")]
        public string[] CcList { get; set; }

        [XmlElement("Bcc")]
        public string Bcc { get; set; }

        [XmlElement("BccList")]
        public string[] BccList { get; set; }

        [XmlElement("Subject")]
        public string Subject { get; set; }

        [XmlElement("Body")]
        public string Body { get; set; }

        public static MailRequest CreateMailRequest(string xmlPath)
        {
            var reader = XmlReader.Create(xmlPath);
            var serializer = new XmlSerializer(typeof(MailRequest));
            var mailRequest = (MailRequest)serializer.Deserialize(reader);
            return mailRequest;
        }
    }
}