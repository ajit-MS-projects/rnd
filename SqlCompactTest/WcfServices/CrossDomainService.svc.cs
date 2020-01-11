using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml;

namespace WcfServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CrossDomainService" in code, svc and config file together.
    public class CrossDomainService : ICrossDomain
    {
        public Message ProvidePolicyFile()
        {
            XmlReader xmlReader = MakeXml();

            return Message.CreateMessage(MessageVersion.None, "", xmlReader);
        }
        private XmlReader MakeXml()
        {
            TextReader reader =
                new StringReader(
                    @"<?xml version='1.0' encoding='utf-8'?>
                    <access-policy>
                      <cross-domain-access>
                        <policy>
                          <allow-from http-request-headers='SOAPAction'>
                            <domain uri='*'/>
                          </allow-from>
                          <grant-to>
                            <resource path='/' include-subpaths='true'/>
                          </grant-to>
                        </policy>
                      </cross-domain-access>
                    </access-policy>");

            return XmlReader.Create(reader);

        }
    }
}
