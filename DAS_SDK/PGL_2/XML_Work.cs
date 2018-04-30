using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAS_SDK.PGL_2
{
    class Element
    {
        List<List<string>> element;
    }

    class XML_Work
    {
        string path;

        public XML_Work()
        {
            path = Directory.GetCurrentDirectory() + "xml_pgl.xml";
        }

        public List<string> WriteAllDown(string paramName = "name")
        {

            return null;
        }

        public Element GetElementById(int id)
        {
            return null;
        }

    }
}
