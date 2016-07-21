/**********************************************************
*Author: wangjiaying
*Date: 2016.7.21
*Func:
**********************************************************/

namespace CryStory.Runtime
{
    /// <summary>
    /// 该属性针对Enum及Bool类型的自定义变量处理
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public class ValueKeyReferenceAttribute : System.Attribute
    {

        public string _keyRef;
        private ValueScope _scope;

        public ValueKeyReferenceAttribute(string keyRefernce, ValueScope scope)
        {
            _keyRef = keyRefernce;
            _scope = scope;
        }

        public Value GetValue(Mission mission, string key)
        {
            Value value = null;
            if (_scope == ValueScope.Mission)
                mission._valueContainer.TryGetValue(key, out value);
            else if (_scope == ValueScope.Story)
            {
                Story story = mission.GetContentNode() as Story;
                if (story != null) story._valueContainer.TryGetValue(key, out value);
            }

            return value;
        }
    }

}