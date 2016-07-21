/**********************************************************
*Author: wangjiaying
*Date: 2016.7.21
*Func:
**********************************************************/

namespace CryStory.Runtime
{
    public class HelpAttribute : System.Attribute
    {

        private string _help;
        public string Help { get { return _help; } }

        public HelpAttribute(string help)
        {
            _help = help;
        }
    }
}