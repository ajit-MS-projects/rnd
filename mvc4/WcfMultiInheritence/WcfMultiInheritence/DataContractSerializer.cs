using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Xml;

namespace WcfMultiInheritence
{
    public class DogResolver : DataContractResolver
    {
        public override bool TryResolveType(Type dataContractType, Type declaredType, DataContractResolver knownTypeResolver, out XmlDictionaryString typeName, out XmlDictionaryString typeNamespace)
        {
            if (dataContractType == typeof(SparrenkonstruktionParameter))
            {
                XmlDictionary dictionary = new XmlDictionary();
                typeName = dictionary.Add("SparrenkonstruktionParameter");
                typeNamespace = dictionary.Add("pvscout.services");
                return true; // indicating that this resolver knows how to handle "Dog"
            }
            else
            {
                // Defer to the known type resolver
                return knownTypeResolver.TryResolveType(dataContractType, declaredType, null, out typeName, out typeNamespace);
            }
        }

        /// <summary>
        /// Override this method to map the specified xsi:type name and namespace to a data contract type during deserialization.
        /// </summary>
        /// <returns>
        /// The type the xsi:type name and namespace is mapped to. 
        /// </returns>
        /// <param name="typeName">The xsi:type name to map.</param><param name="typeNamespace">The xsi:type namespace to map.</param><param name="declaredType">The type declared in the data contract.</param><param name="knownTypeResolver">The known type resolver.</param>
        public override Type ResolveName(string typeName, string typeNamespace, Type declaredType, DataContractResolver knownTypeResolver)
        {
            if (typeName == "SparrenkonstruktionParameter" && typeNamespace == "pvscout.services")
            {
                return typeof(SparrenkonstruktionParameter);
            }
            else
            {
                // Defer to the known type resolver
                return knownTypeResolver.ResolveName(typeName, typeNamespace, declaredType, null) ?? declaredType;
            } 
        }

    }
}