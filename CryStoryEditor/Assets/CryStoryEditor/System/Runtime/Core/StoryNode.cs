/**********************************************************
*Author: wangjiaying
*Date: 2016.6.16
*Func:
**********************************************************/

using System.IO;

namespace CryStory.Runtime
{

    abstract public class StoryNode : UpdateNode
    {
        protected Mission GetMission { get { return GetContent() as Mission; } }
        protected Story GetStory
        {
            get
            {
                Mission mi = GetMission;
                if (mi != null)
                    return mi.GetContent() as Story;
                return null;
            }
        }

        public virtual string ToDescription() { return ""; }

        public sealed override void Serialize(BinaryWriter w)
        {
            base.Serialize(w);
        }

        public sealed override void Deserialize(BinaryReader r)
        {
            base.Deserialize(r);
        }

    }
}