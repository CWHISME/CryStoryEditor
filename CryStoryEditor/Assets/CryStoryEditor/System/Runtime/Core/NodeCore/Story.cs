/**********************************************************
*Author: wangjiaying
*Date: 2016.6.16
*Func:
**********************************************************/

using System.Collections.Generic;
using System.IO;

namespace CryStory.Runtime
{
    public class Story : DragModifier
    {
        public string _missionDescriptionType = "CryStory.Runtime.MissionDescriptionSimpleRPG";

        protected override void OnAddedContentNode(NodeModifier node)
        {
            base.OnAddedContentNode(node);
            Mission mission = node as Mission;
            if (mission != null)
            {
                if (mission._missionDescription == null || mission._missionDescription.GetType().FullName != _missionDescriptionType)
                    mission._missionDescription = ReflectionHelper.CreateInstance<MissionDescription>(_missionDescriptionType);
            }
        }

        //public override void Serialize(BinaryWriter w)
        //{
        //    w.Write(_missionDescriptionType);
        //}

        //public override void Deserialize(BinaryReader r)
        //{
        //    _missionDescriptionType = r.ReadString();
        //}
    }
}