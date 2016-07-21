/**********************************************************
*Author: wangjiaying
*Date: 2016.7.12
*Func:
**********************************************************/

using System.IO;

namespace CryStory.Runtime
{
    public static class ExtensionMethod
    {
        public static EnumResult ToEnumResult(this bool b)
        {
            return b ? EnumResult.Success : EnumResult.Failed;
        }
    }
}