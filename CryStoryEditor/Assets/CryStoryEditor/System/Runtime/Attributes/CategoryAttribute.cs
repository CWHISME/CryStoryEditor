/**********************************************************
*Author: wangjiaying
*Date: 2016.7.15
*Func:
**********************************************************/

namespace CryStory.Runtime
{
    public class CategoryAttribute : System.Attribute
    {

        private string _category;
        public string Category { get { return _category; } }

        public CategoryAttribute(string category)
        {
            _category = category;
        }
    }
}