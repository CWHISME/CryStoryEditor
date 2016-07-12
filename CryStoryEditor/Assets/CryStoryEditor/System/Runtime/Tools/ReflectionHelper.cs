/**********************************************************
*Author: wangjiaying
*Date: 2016.7.12
*Func:
**********************************************************/

using System.Reflection;

namespace CryStory
{
    public class ReflectionHelper
    {
        private static Assembly _asm;
        public static Assembly Asm
        {
            get
            {
                if (_asm == null)
                    _asm = typeof(CryStory.Runtime.Story).Assembly;
                return _asm;
            }
        }

        public static object CreateInstance(string fullName)
        {
            return Asm.CreateInstance(fullName);
        }

        public static T CreateInstance<T>(string fullName) where T : class, new()
        {
            return CreateInstance(fullName) as T;
        }

    }
}