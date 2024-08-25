
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helpers
{
    public class ReadDocumentIntoText
    {
        private static Stream GetFileFromUrl(string presignUrl)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(presignUrl);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            try
            {
                MemoryStream mem = new MemoryStream();
                Stream stream = response.GetResponseStream();

                stream.CopyTo(mem, 4096);

                return mem;
            }
            finally
            {
                response.Close();
            }
        }
    }
}
