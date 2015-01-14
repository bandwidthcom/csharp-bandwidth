﻿using System.Globalization;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Bandwidth.Net.Xml.Verbs
{
    /// <summary>
    /// The SendMessage is used to send a text message.
    /// </summary>
    /// <seealso href="http://ap.bandwidth.com/docs/xml/message/"/>
    public class SendMessage: IXmlSerializable, IVerb
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public SendMessage()
        {
            RequestUrlTimeout = 30;
        }
        
        /// <summary>
        /// The number from the message will be sent
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// The number to send the message to
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// Relative or absolute URL to send event and request new BaML
        /// </summary>
        public string RequestUrl { get; set; }

        /// <summary>
        /// Integer time seconds to wait for requestUrl response
        /// </summary>
        public int RequestUrlTimeout { get; set; }

        /// <summary>
        /// Relative or absolute URL to send the message callback
        /// </summary>
        public string StatusCallbackUrl { get; set; }

        /// <summary>
        /// Text to send
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Optional attached media
        /// </summary>
        public Media Media { get; set; }

        System.Xml.Schema.XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(System.Xml.XmlReader reader)
        {
            throw new System.NotImplementedException();
        }

        void IXmlSerializable.WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteAttributeString("from", From);
            writer.WriteAttributeString("to", To);
            if (!string.IsNullOrEmpty(RequestUrl))
            {
                writer.WriteAttributeString("requestUrl", RequestUrl);
            }
            if (RequestUrlTimeout != 30)
            {
                writer.WriteAttributeString("requestUrlTimeout", RequestUrlTimeout.ToString(CultureInfo.InvariantCulture));
            }
            if (!string.IsNullOrEmpty(StatusCallbackUrl))
            {
                writer.WriteAttributeString("statusCallbackUrl", StatusCallbackUrl);
            }
            if (Media != null)
            {
                var serializer = new XmlSerializer(typeof(Media), "");
                var ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                serializer.Serialize(writer, Media, ns);
            }
            writer.WriteString(Text);
        }
    }

    /// <summary>
    /// Media is a noun that is exclusively placed within SendMessage to provide the messages with attached media (MMS) capability
    /// </summary>
    /// <seealso href="http://ap.bandwidth.com/docs/xml/xml-media/"/>
    public class Media
    {
        /// <summary>
        /// Urls of media resourses to send
        /// </summary>
        [XmlElement("Url")]
        public string[] Urls { get; set; }
    }
}
