﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PangyaAPI.SuperSocket.Ext
{
    /// <summary>
    /// Configuration extension class
    /// </summary>
    public static class ConfigurationExtension
    {
        /// <summary>
        /// Gets the value from namevalue collection by key.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static string GetValue(this NameValueCollection collection, string key)
        {
            return GetValue(collection, key, string.Empty);
        }

        /// <summary>
        /// Gets the value from namevalue collection by key.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static string GetValue(this NameValueCollection collection, string key, string defaultValue)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            if (collection == null)
                return defaultValue;

            var e = collection[key];

            if (e == null)
                return defaultValue;

            return e;
        }

        /// <summary>
        /// Deserializes the specified configuration section.
        /// </summary>
        /// <typeparam name="TElement">The type of the element.</typeparam>
        /// <param name="section">The section.</param>
        /// <param name="reader">The reader.</param>
        public static void Deserialize<TElement>(this TElement section, XmlReader reader)
            where TElement : ConfigurationElement
        {
            if (section is ConfigurationElementCollection)
            {
                var collectionType = section.GetType();
                var att = collectionType.GetCustomAttributes(typeof(ConfigurationCollectionAttribute), true).FirstOrDefault() as ConfigurationCollectionAttribute;

                if (att != null)
                {
                    var property = collectionType.GetProperty("AddElementName", BindingFlags.NonPublic | BindingFlags.Instance);
                    property.SetValue(section, att.AddItemName, null);

                    property = collectionType.GetProperty("RemoveElementName", BindingFlags.NonPublic | BindingFlags.Instance);
                    property.SetValue(section, att.RemoveItemName, null);

                    property = collectionType.GetProperty("ClearElementName", BindingFlags.NonPublic | BindingFlags.Instance);
                    property.SetValue(section, att.ClearItemsName, null);
                }
            }

            var deserializeElementMethod = typeof(TElement).GetMethod("DeserializeElement", BindingFlags.NonPublic | BindingFlags.Instance);
            deserializeElementMethod.Invoke(section, new object[] { reader, false });
        }

        /// <summary>
        /// Deserializes the child configuration.
        /// </summary>
        /// <typeparam name="TConfig">The type of the configuration.</typeparam>
        /// <param name="childConfig">The child configuration string.</param>
        /// <returns></returns>
        public static TConfig DeserializeChildConfig<TConfig>(string childConfig)
            where TConfig : ConfigurationElement, new()
        {
            // removed extra namespace prefix
            childConfig = childConfig.Replace("xmlns=\"http://schema.supersocket.net/supersocket\"", string.Empty);

            XmlReader reader = new XmlTextReader(new StringReader(childConfig));

            var config = new TConfig();

            reader.Read();
            config.Deserialize(reader);

            return config;
        }

        /// <summary>
        /// Gets the child config.
        /// </summary>
        /// <typeparam name="TConfig">The type of the config.</typeparam>
        /// <param name="childElements">The child elements.</param>
        /// <param name="childConfigName">Name of the child config.</param>
        /// <returns></returns>
        public static TConfig GetChildConfig<TConfig>(this NameValueCollection childElements, string childConfigName)
            where TConfig : ConfigurationElement, new()
        {
            var childConfig = childElements.GetValue(childConfigName, string.Empty);

            if (string.IsNullOrEmpty(childConfig))
                return default(TConfig);

            return DeserializeChildConfig<TConfig>(childConfig);
        }

       
    }
}
