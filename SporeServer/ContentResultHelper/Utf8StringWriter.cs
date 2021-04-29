using System.IO;
using System.Text;

namespace SporeServer.ContentResultHelper
{
    /// <summary>
    ///     StringWriter with UTF8
    /// </summary>
    public class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
    }
}
