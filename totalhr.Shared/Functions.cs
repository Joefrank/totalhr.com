using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Net.Mime;
using System.Text.RegularExpressions;

namespace totalhr.Shared
{
    public class Functions
    {
        private static Regex isGuid =
            new Regex(@"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$", RegexOptions.Compiled);

        public static string FileContentFromPath(string sPath)
        {
            try
            {
                using (StreamReader sr = new StreamReader(sPath, Encoding.Default))
                {
                    return sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public static string GetFileContentFromURI(string sURI)
        {
            WebResponse response = null;
            StreamReader reader = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sURI);
                request.Method = "GET";
                response = request.GetResponse();
                reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);
                return reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                // handle error
                return ex.Message;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (response != null)
                    response.Close();
            }

        }

        
        public static bool IsGuid(string candidate)
        {
            if (string.IsNullOrEmpty(candidate))
                return false;

            return isGuid.IsMatch(candidate);
        }

    }
}
