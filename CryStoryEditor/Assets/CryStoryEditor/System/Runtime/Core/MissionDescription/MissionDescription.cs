/**********************************************************
*Author: wangjiaying
*Date: 2016.7.19
*Func:
**********************************************************/

using System;
using System.IO;

namespace CryStory.Runtime
{

    /// <summary>
    /// 包含一个任务中的各项信息，用于事先设置，然后运行时使用。
    /// </summary>
    public class MissionDescription : ISerialize
    {
        public virtual void Serialize(BinaryWriter w) { }

        public virtual void Deserialize(BinaryReader r) { }
    }
}