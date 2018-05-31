using System.Text;

namespace LiveArt.Design.PostProcessing
{
    internal class ZipFileNameEncoder: UTF8Encoding
    {
        public override byte[] GetBytes(string s)
        {
            s = s.Replace("\\", "/");
            return base.GetBytes(s);
        }
    }
}
