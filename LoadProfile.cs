using MCSECDIS.Models.Attributes;
using MCSECDIS.Models.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Xml;
using Esri.ArcGISRuntime.Hydrography;
namespace MCSECDIS.Business
{
    public class LoadProfile
    {
        private List<XmlNode> _xmlNodes = new List<XmlNode>();
        private XmlDocument _document = new XmlDocument();
        public LoadProfile() { }
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
        private void TraverseNodes(Type type, object appProfile, XmlNodeList nodes)
        {
            object value = appProfile;
            PropertyInfo TargetProperty = null;
            foreach (XmlNode node in nodes)
            {
                if (node.ToString().Contains("XmlElement"))
                {
                    Type t = type;
                    foreach (PropertyInfo property in type.GetProperties())
                    {
                        SerializerType serializerType = SerializerType.Normal;
                        serializerType = GetSerialzerAttributeType(property);

                        if (node.Name == property.Name && serializerType != SerializerType.Ignore)
                        {
                            if ((!property.PropertyType.IsClass || property.PropertyType == typeof(string) || property.PropertyType == typeof(Color)))
                            {
                                if (property.PropertyType == typeof(Color))
                                {
                                    Color color = Color.FromName(node.InnerText.Split("[")[1].Split("]")[0]);
                                    property.SetValue(appProfile, color, null);
                                }
                                else if (property.PropertyType.IsEnum) {
                                   
                                    property.SetValue(appProfile, Enum.ToObject(property.PropertyType, int.Parse(node.InnerText)), null);

                                }
                                else
                                    property.SetValue(appProfile, Convert.ChangeType(node.InnerText, property.PropertyType), null);
                            }
                            t = property.PropertyType;
                        }
                        if (property.Name == node.Name && property.PropertyType.IsClass  && !property.PropertyType.IsEnum  && property.PropertyType != typeof(string) && property.PropertyType != typeof(Color) && serializerType != SerializerType.Ignore)
                        {
                            TargetProperty = property;
                            value = TargetProperty.GetValue(appProfile, null);
                        }
                    }
                    if(value.GetType() == t)
                    TraverseNodes(t, value, node.ChildNodes);
                }
            }
        }
        public bool DeSerializeXML(Type type, object appProfile, string filePath)
        {
            try
            {
                if (File.Exists(filePath)){
                    var document = new XmlDocument();
                    document.Load(filePath);
                    TraverseNodes(type, appProfile, document.ChildNodes);
                    return true;
                }
                else return false;
            }
            catch (Exception ex) { return false; }
        }
    }
}
