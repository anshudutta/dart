using System;
using System.Xml.Serialization;

namespace Analyzer.Framework
{
    /// <summary>
    /// Class is not used
    /// Future plans ?
    /// </summary>
    /// <typeparam name="TAbstractType"></typeparam>
    public class AbstractXmlSerializer<TAbstractType> : IXmlSerializable
    {
        /// <summary>
        /// **DO NOT USE** This is only added to enable XML Serialization.
        /// </summary>
        /// <remarks>DO NOT USE THIS CONSTRUCTOR</remarks>
        public AbstractXmlSerializer()
        {
            // Default Ctor (Required for Xml Serialization - DO NOT USE)
        }

        /// <summary>
        /// Initialises the Serializer to work with the given data.
        /// </summary>
        /// <param name="data">Concrete Object of the AbstractType Specified.</param>
        public AbstractXmlSerializer(TAbstractType data)
        {
            _data = data;
        }

        // This gives the power to your C# class, which can accepts any reasonably convertible data
        // type without type casting           
        /// <summary>
        ///  Override the Implicit Conversions since the XmlSerializer casts to/from the required types implicitly.
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static implicit operator TAbstractType(AbstractXmlSerializer<TAbstractType> o)
        {
            return o.Data;
        }

        public static implicit operator AbstractXmlSerializer<TAbstractType>(TAbstractType o)
        {
            // posible compare of value type with null
            // please explain the logic in comment
            return o == null ? null : new AbstractXmlSerializer<TAbstractType>(o);
        }

        System.Xml.Schema.XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        private TAbstractType _data;
        /// <summary>
        /// [Concrete] Data to be stored/is stored as XML.
        /// </summary>
        public TAbstractType Data
        {
            get { return _data; }
            set { _data = value; }
        }

        void IXmlSerializable.ReadXml(System.Xml.XmlReader reader)
        {
            string typeAttribute = reader.GetAttribute("type");
            if (typeAttribute != null)
            {
                Type type = Type.GetType(typeAttribute);
                reader.ReadStartElement();
                if (type != null)
                    Data = (TAbstractType)new XmlSerializer(type).Deserialize(reader);
            }
            reader.ReadEndElement();

        }

        void IXmlSerializable.WriteXml(System.Xml.XmlWriter writer)
        {
            Type type = _data.GetType();

            //writer.WriteAttributeString("type", type.AssemblyQualifiedName);
            new XmlSerializer(type).Serialize(writer, _data);
        }
    }



}
