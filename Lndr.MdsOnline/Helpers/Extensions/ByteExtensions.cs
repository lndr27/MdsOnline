using System.Text;

namespace Lndr.MdsOnline.Helpers.Extensions
{
    public static class ByteExtensions
    {
        public static bool IsImage(this byte[] file)
        {
            return GetImageType(file) == null;
        }

        public static string GetImageType(this byte[] image)
        {
            string headerCode = GetHeaderInfo(image);

            if (headerCode.StartsWith("FFD8FFE0"))
            {
                return "JPG";
            }
            else if (headerCode.StartsWith("49492A"))
            {
                return "TIFF";
            }
            else if (headerCode.StartsWith("424D"))
            {
                return "BMP";
            }
            else if (headerCode.StartsWith("474946"))
            {
                return "GIF";
            }
            else if (headerCode.StartsWith("89504E470D0A1A0A"))
            {
                return "PNG";
            }
            else
            {
                return null; //UnKnown
            }
        }

        public static string GetHeaderInfo(byte[] buffer)
        {
            var sb = new StringBuilder();
            foreach (byte b in buffer)
            {
                sb.Append(b.ToString("X2"));
            }
            return sb.ToString().ToUpper();
        }
    }
}