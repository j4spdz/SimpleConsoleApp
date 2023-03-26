using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI.Helper
{
    /// <summary>
    /// Reference: https://stackoverflow.com/questions/25730816/how-to-return-xml-as-utf-8-instead-of-utf-16
    /// </summary>
    public class Utf8StringWriter : StringWriter
    {
        // Use UTF8 encoding but write no BOM to the wire
        public override Encoding Encoding
        {
            get { return new UTF8Encoding(false); } // in real code I'll cache this encoding.
        }
    }
}
