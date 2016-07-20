/**********************************************************
*Author: wangjiaying
*Date: 2016.7.20
*Func:
**********************************************************/

using System.Collections.Generic;

namespace CryStory.Runtime
{
    public class ValueNameSelectAttribute : System.Attribute
    {

        private ValueScope _valueScope;
        public ValueNameSelectAttribute(ValueScope scope)
        {
            _valueScope = scope;
        }

        public string[] GetValueNameList(Mission mission)
        {
            List<string> names = new List<string>(5);
            if (_valueScope == ValueScope.Mission)
            {
                foreach (var item in mission._valueContainer)
                {
                    names.Add(item.Key);
                }
            }
            else if (_valueScope == ValueScope.Story)
            {
                ValueContainer con = mission.GetContentNode() as ValueContainer;
                if (con != null)
                    foreach (var item in con._valueContainer)
                    {
                        names.Add(item.Key);
                    }
            }

            return names.ToArray();
        }

    }

}