using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DAS_SDK.MVC.Model.Debug;

namespace DAS_SDK.PGL_2
{
    class XML_Lib
    {
        XmlReader xmlReader;
        Base_Debug debug;

        public XML_Lib(Base_Debug debug)
        {
            this.debug = debug;
            xmlReader = XmlReader.Create(Directory.GetCurrentDirectory() + @"\xml_pgl.xml");
           
        }

        public void ReadAtributes()
        {
            while (xmlReader.Read())
            {
                if (xmlReader.HasAttributes)
                {
                    debug.AddMessage<object>(new Message<object>(xmlReader.GetAttribute("id")));
                }
            }
        }

        public void ReadValues(string nameOfVal)
        {
            while (xmlReader.Read())
            {
                if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == nameOfVal)
                {
                    xmlReader.Read();
                    debug.AddMessage<object>(new Message<object>(xmlReader.Value));
                }
            }
        }

        public void ReadTags()
        {
            while (xmlReader.Read())
            {
                if(xmlReader.NodeType == XmlNodeType.Element)
                {
                    debug.AddMessage<object>(new Message<object>(xmlReader.Name));
                } 
            }
        }

        public void ReadAll()
        {
            List<Message<object>> messages = new List<Message<object>>();
            Message<object> message;

            for (int i = 0; xmlReader.Read(); i++)
            {
                if (xmlReader.HasAttributes)
                {
                    messages[messages.Count - 1].MessageContent += "ID: [" + xmlReader.GetAttribute("id") + "]";
                }

                if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "name")
                {
                    messages.Add(new Message<object>(""));
                    xmlReader.Read();    
                    messages[messages.Count - 1].MessageContent += "Song Name: ["+ xmlReader.Value +"] ";
                }
              
             
            }

            foreach (var item in messages)
            {
                debug.AddMessage<object>(item);
            }

        }

    }
}
