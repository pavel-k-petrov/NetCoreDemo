using System.Linq;
using System.Text;

namespace HttpRequestProcessing
{
    public class Md5RequestToFileNameMapper : IRequestToFileNameMapper
    {
        public string GetFileNameForRequest(RequestModel request)
        {
            using(var md5 = System.Security.Cryptography.MD5.Create())
            {
                var stringToEncode = $"{request.Method}{request.Path}{request.QueryString}";
                var stringBytes = Encoding.UTF8.GetBytes(stringToEncode);
                var hashBytes = md5.ComputeHash(stringBytes);
                var encodedString = string.Concat(hashBytes.Select(b => b.ToString("x2")));
                return encodedString;
            }
        }
    }
}