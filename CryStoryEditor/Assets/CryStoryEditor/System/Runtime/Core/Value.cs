/**********************************************************
*Author: wangjiaying
*Date: 2016.7.20
*Func:
**********************************************************/

using System;
using System.IO;

namespace CryStory.Runtime
{
    public class Value : ISerialize
    {
        private object _value;
        private VarType _varType;

        public VarType ValueType { get { return _varType; } }

        public Value(int v)
        {
            _value = v;
            _varType = VarType.INT;
        }

        public Value(bool v)
        {
            _value = v;
            _varType = VarType.BOOL;
        }

        public Value(float v)
        {
            _value = v;
            _varType = VarType.FLOAT;
        }

        public Value(string v)
        {
            _value = v;
            _varType = VarType.STRING;
        }

        public Value(string v, VarType type)
        {
            _value = v;
            _varType = type;
        }

        public int IntValue
        {
            get
            {
                if (_value == null) return 0;
                if (_varType == VarType.INT || _varType == VarType.FLOAT) return (int)_value;
                return 0;
            }
            set
            {
                _value = value;
            }
        }

        public float FloatValue
        {
            get
            {
                if (_value == null) return 0;
                if (_varType == VarType.INT || _varType == VarType.FLOAT) return (float)_value;
                return 0;
            }
            set
            {
                _value = value;
            }
        }

        public bool BoolValue
        {
            get
            {
                if (_value == null) return false;
                if (_varType == VarType.BOOL) return (bool)_value;
                return false;
            }
            set
            {
                _value = value;
            }
        }

        public string StringValue
        {
            get
            {
                if (_value == null) return "";
                if (_varType == VarType.STRING) return ((string)_value);
                return "";
            }
            set
            {
                _value = value;
            }
        }

        public void OperationValue(ValueFunctor func, string val)
        {
            switch (ValueType)
            {
                case VarType.INT:
                    OperationValue(func, Convert.ToInt32(val));
                    break;
                case VarType.FLOAT:
                    OperationValue(func, Convert.ToSingle(val));
                    break;
                case VarType.BOOL:
                    OperationValue(func, Convert.ToBoolean(val));
                    break;
                case VarType.STRING:
                    if (func == ValueFunctor.Set) _value = val;
                    break;
            }
        }

        public void OperationValue(ValueFunctor func, bool val)
        {
            if (func == ValueFunctor.Set) _value = val;
        }

        public void OperationValue(ValueFunctor func, int val)
        {
            if (ValueType != VarType.INT) return;

            int realVal = IntValue;
            switch (func)
            {
                case ValueFunctor.Set:
                    realVal = val;
                    break;
                case ValueFunctor.Add:
                    realVal += val;
                    break;
                case ValueFunctor.Reduce:
                    realVal -= val;
                    break;
                case ValueFunctor.Multiply:
                    realVal *= val;
                    break;
                case ValueFunctor.Divide:
                    realVal /= val;
                    break;
            }
            _value = realVal;
        }

        public void OperationValue(ValueFunctor func, float val)
        {
            if (ValueType != VarType.FLOAT) return;

            float realVal = IntValue;
            switch (func)
            {
                case ValueFunctor.Set:
                    realVal = val;
                    break;
                case ValueFunctor.Add:
                    realVal += val;
                    break;
                case ValueFunctor.Reduce:
                    realVal -= val;
                    break;
                case ValueFunctor.Multiply:
                    realVal *= val;
                    break;
                case ValueFunctor.Divide:
                    realVal /= val;
                    break;
            }
            _value = realVal;
        }

        public bool Compare(string v, ValueCompare compare)
        {
            switch (ValueType)
            {
                case VarType.INT:
                    Compare(Convert.ToInt32(v), compare);
                    break;
                case VarType.FLOAT:
                    Compare(Convert.ToSingle(v), compare);
                    break;
                case VarType.BOOL:
                    Compare(Convert.ToBoolean(v), compare);
                    break;
                case VarType.STRING:
                    return StringValue == v;
            }

            return false;
        }

        public bool Compare(bool val, ValueCompare compare)
        {
            switch (compare)
            {
                case ValueCompare.Equal:
                    return val == BoolValue;
                case ValueCompare.NotEqual:
                    return val != BoolValue;
            }
            return false;
        }

        public bool Compare(int val, ValueCompare compare)
        {
            switch (compare)
            {
                case ValueCompare.Equal:
                    return IntValue == val;
                case ValueCompare.NotEqual:
                    return IntValue != val;
                case ValueCompare.Less:
                    return IntValue < val;
                case ValueCompare.Greater:
                    return IntValue > val;
                case ValueCompare.LessEqual:
                    return IntValue <= val;
                case ValueCompare.GreaterEqual:
                    return IntValue >= val;
            }
            return false;
        }

        public bool Compare(float val, ValueCompare compare)
        {
            switch (compare)
            {
                case ValueCompare.Equal:
                    return FloatValue == val;
                case ValueCompare.NotEqual:
                    return FloatValue != val;
                case ValueCompare.Less:
                    return FloatValue < val;
                case ValueCompare.Greater:
                    return FloatValue > val;
                case ValueCompare.LessEqual:
                    return FloatValue <= val;
                case ValueCompare.GreaterEqual:
                    return FloatValue >= val;
            }
            return false;
        }

        public void ConverToRealType()
        {
            switch (_varType)
            {
                case VarType.INT:
                    _value = Convert.ToInt32(_value);
                    break;
                case VarType.FLOAT:
                    _value = Convert.ToSingle(_value);
                    break;
                case VarType.BOOL:
                    _value = Convert.ToBoolean(_value);
                    break;
            }
        }

        public void Serialize(BinaryWriter w)
        {
            w.Write((int)_varType);
            w.Write(_value.ToString());
        }

        public void Deserialize(BinaryReader r)
        {
            int t = r.ReadInt32();
            _varType = (VarType)t;
            _value = r.ReadString();

            ConverToRealType();
        }
    }

    [Flags]
    public enum VarType
    {
        INT = 1,
        FLOAT = 4,
        BOOL = 8,
        STRING = 16
    }
}