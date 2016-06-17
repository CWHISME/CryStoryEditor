/**********************************************************
*Author: wangjiaying
*Date: 2016.6.17
*Func:
**********************************************************/

namespace CryStory
{
    public class Singleton<T> where T : class, new()
    {
        private static T _instance;
        public static T GetInstance { get { if (_instance == null) _instance = new T(); return _instance; } }
    }
}
