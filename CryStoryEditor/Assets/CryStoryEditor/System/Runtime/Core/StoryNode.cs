/**********************************************************
*Author: wangjiaying
*Date: 2016.6.16
*Func:
**********************************************************/

namespace CryStory.Runtime
{

    abstract public class StoryNode : UpdateNode
    {
        protected Mission GetMission { get { return GetContentNode() as Mission; } }
        protected Story GetStory
        {
            get
            {
                Mission mi = GetMission;
                if (mi != null)
                    return mi.GetContentNode() as Story;
                return null;
            }
        }

        public virtual string ToDescription() { return ""; }
    }
}