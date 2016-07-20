/**********************************************************
*Author: wangjiaying
*Date: 2016.7.20
*Func:
**********************************************************/

namespace CryStory.Runtime
{
    public class ValueFuncAttribute : System.Attribute
    {

        public VarType _varType;

        public ValueFuncAttribute(VarType type)
        {
            _varType = type;
        }

    }

}