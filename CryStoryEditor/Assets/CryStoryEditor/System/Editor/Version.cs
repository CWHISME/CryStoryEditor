/**********************************************************
*Author: wangjiaying
*Date: 2016.6.23
*Func:
**********************************************************/

namespace CryStory.Editor
{
    public class Version
    {
        public const int MainVersion = 0;
        public const int Subversion = 3;
        public const string VersionName = "Alpha";
        public static string FullVersion { get { return VersionName + " " + MainVersion + "." + Subversion; } }
    }
}
