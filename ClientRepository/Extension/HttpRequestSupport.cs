using System.Net.Http.Headers;

namespace ClientRepository.Extension
{
     public static class HttpRequestSupport
    {
        public static string GetQueryPath(Dictionary<string,string>? param)
        {
            if (param == null)
                return "";
            string path = "?";
            foreach (var item in param)
            {
                path += item.Key;
                path += "=" + item.Value;
                path += "&";                
            }
            path = path.Remove(path.Length - 1);
            return path;
        }
    }
}
