/**********************************************************
*Author: wangjiaying
*Date: 2016.7.12
*Func:
**********************************************************/

using System.IO;

namespace CryStory.Runtime
{
    public interface ISerialize
    {
        void Serialize(BinaryWriter w);
        void Deserialize(BinaryReader r);
    }
}