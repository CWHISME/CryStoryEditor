/**********************************************************
*Author: wangjiaying
*Date: 2016.6.16
*Func:
**********************************************************/
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CryStory.Runtime
{

    public class Mission : DragModifier
    {

        /// <summary>
        /// 存储任务详情
        /// </summary>
        public MissionDescription _missionDescription;


        /// <summary>
        /// 保存数据，将会把返回保存后的二进制数据
        /// </summary>
        /// <returns></returns>
        public byte[] Save()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter w = new BinaryWriter(ms))
                {
                    w.Write(JsonUtility.ToJson(this));
                    Serialize(w);

                    w.Write(_missionDescription.GetType().FullName);
                    _missionDescription.Serialize(w);

                }
                //Save
                return ms.GetBuffer();
            }
        }

        /// <summary>
        /// 从已保存的二进制数据中加载
        /// </summary>
        /// <param name="data"></param>
        public void Load(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader r = new BinaryReader(ms))
                {
                    JsonUtility.FromJsonOverwrite(r.ReadString(), this);
                    Deserialize(r);

                    string name = r.ReadString();
                    _missionDescription = ReflectionHelper.CreateInstance<MissionDescription>(name);
                    _missionDescription.Deserialize(r);
                }
            }
        }


        public void OnAbort()
        {

        }

    }
}