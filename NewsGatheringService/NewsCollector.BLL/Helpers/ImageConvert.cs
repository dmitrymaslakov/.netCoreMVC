using System.IO;
using System.Net;

namespace NewsCollector.BLL.Helpers
{
    public class ImageConvert
    {
        /// <summary>
        /// Convert image to a byte array
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <returns></returns>
        public static byte[] ImageUrlToByte(string imageUrl)
        {
            try
            {
                var stream = new WebClient().OpenRead(imageUrl);

                var dataImage = new byte[0];

                using (var streamReader = new MemoryStream())
                {
                    stream.CopyTo(streamReader);

                    dataImage = streamReader.ToArray();
                }

                return dataImage;
            }
            catch
            {
                throw;
            }
        }
    }
}
