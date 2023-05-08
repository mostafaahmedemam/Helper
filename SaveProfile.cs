using MCSECDIS.Models.Attributes;
using MCSECDIS.Models.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Xml;

namespace MCSECDIS.Business
{
    public class SaveProfile
    {
        private List<XmlNode> _xmlNodes = new List<XmlNode>();
        private XmlDocument _document = new XmlDocument();
        public SaveProfile() { }
        private void ReadPropertiesRecursive(Type type, object appProfile, XmlElement parentNode)
        {
            var appProfileCopy = appProfile;
            parentNode = (XmlElement)parentNode.ChildNodes[^1];
            foreach (PropertyInfo property in type.GetProperties())
            {
                SerializerType serializerType = SerializerType.Normal;
                serializerType = GetSerialzerAttributeType(property);
                if (property.PropertyType.IsClass 
                    && property.PropertyType != typeof(string) 
                    && property.PropertyType != typeof(Color) 
                    && serializerType != SerializerType.Ignore 
                    && !property.PropertyType.IsEnum)
                {
                    XmlNode xmlNode = _document.CreateNode("element", property.Name, "");
                    parentNode.AppendChild(xmlNode);
                    var value = property.GetValue(appProfile, null);
                    ReadPropertiesRecursive(property.PropertyType, value, parentNode);
                }
                else if (serializerType != SerializerType.Ignore)
                {
                    if (property.PropertyType.IsEnum) {
                        XmlNode xmlNode = _document.CreateNode("element", property.Name, "");
                        xmlNode.InnerText = ((int)property.GetValue(appProfile, null)).ToString();
                        parentNode.AppendChild(xmlNode);
                    }
                    else { 
                        XmlNode xmlNode = _document.CreateNode("element", property.Name, "");
                        xmlNode.InnerText = property.GetValue(appProfile, null).ToString();
                        parentNode.AppendChild(xmlNode);
                    }
                }
            }
        }
        private SerializerType GetSerialzerAttributeType(PropertyInfo property)
        {
            object[] attributes = property.GetCustomAttributes(true);
            foreach (var attribute in attributes)
            {
                SerializerAttribute profileAttr = attribute as SerializerAttribute;
                if (profileAttr != null)
                {
                    return profileAttr.Type;
                }
            }
            return SerializerType.Normal;
        }
        public bool SerializeXml<T>(T AppProfile, string filePath)
        {
            try
            {
                _document.LoadXml("<AppProfile></AppProfile>");
                XmlNode xmlNode = _document.CreateNode("element", "ProfileSettings", "");
                XmlElement root = _document.DocumentElement;
                root.AppendChild(xmlNode);
                Type TargetClassType = AppProfile.GetType();
                ReadPropertiesRecursive(TargetClassType, AppProfile, root);

                XmlTextWriter textWriter = new XmlTextWriter(filePath, null);
                _document.Save(textWriter);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
