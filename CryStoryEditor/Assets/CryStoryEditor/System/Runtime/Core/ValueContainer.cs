/**********************************************************
*Author: wangjiaying
*Date: 2016.7.20
*Func:
**********************************************************/

using System.Collections.Generic;
using System.IO;

namespace CryStory.Runtime
{
    public class ValueContainer : NodeContent
    {

        public Dictionary<string, Value> _valueContainer = new Dictionary<string, Value>();

        public bool AddValue(string key, Value v)
        {
            if (HaveKey(key)) return false;
            _valueContainer[key] = v;
            return true;
        }

        public bool AddValue(string key, VarType type, string v)
        {
            if (HaveKey(key)) return false;
            Value val = new Value(v, type);
            val.ConverToRealType();
            _valueContainer[key] = val;
            return true;
        }

        public Value GetValue(string key)
        {
            if (HaveKey(key)) return _valueContainer[key];
            return null;
        }

        public bool HaveKey(string key)
        {
            if (_valueContainer.ContainsKey(key))
                return true;
            return false;
        }

        protected override void OnSaved(BinaryWriter w)
        {
            base.OnSaved(w);

            SaveValue(w);
        }

        protected override void OnLoaded(BinaryReader r)
        {
            base.OnLoaded(r);

            LoadValue(r);
        }

        private void SaveValue(BinaryWriter w)
        {
            w.Write(_valueContainer.Count);
            foreach (var item in _valueContainer)
            {
                w.Write(item.Key);
                item.Value.Serialize(w);
            }
        }

        private void LoadValue(BinaryReader r)
        {
            int count = r.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                string key = r.ReadString();
                Value v = new Value(0);
                v.Deserialize(r);
                _valueContainer[key] = v;
                //AddValue(key, v);
            }
        }

#if UNITY_EDITOR
        public virtual void SaveInEditor(BinaryWriter w)
        {
            SaveValue(w);
        }

        public virtual void LoadInEditor(BinaryReader r)
        {
            LoadValue(r);
        }
#endif
    }
}