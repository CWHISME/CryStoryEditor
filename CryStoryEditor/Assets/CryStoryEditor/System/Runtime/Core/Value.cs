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

        public VarType VarlueType { get { return _varType; } }

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
                if (_varType == VarType.INT || _varType == VarType.FLOAT) return int.Parse((string)_value);
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
                if (_varType == VarType.INT || _varType == VarType.FLOAT) return float.Parse((string)_value);
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
                if (_varType == VarType.BOOL) return bool.Parse((string)_value);
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
            w.Write((string)_value);
        }

        public void Deserialize(BinaryReader r)
        {
            int t = r.ReadInt32();
            _varType = (VarType)t;
            _value = r.ReadString();
        }
    }

    [Flags]
    public enum VarType
    {
        INT = 1,
        FLOAT = 4,
        BOOL = 16,
        STRING = 32
    }
}