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
            if (func == ValueFunctor.Set) _value = val;
        }

        public void OperationValue(ValueFunctor func, bool val)
        {
            if (func == ValueFunctor.Set) _value = val;
        }

        public void OperationValue(ValueFunctor func, Int32 val)
        {
            switch (func)
            {
                case ValueFunctor.Set:

                    break;
                case ValueFunctor.Add:
                    break;
                case ValueFunctor.Reduce:
                    break;
                case ValueFunctor.Multiply:
                    break;
                case ValueFunctor.Divide:
                    break;
                default:
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