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
        /// <summary>
        /// 描述模板类，存储任务详细描述信息
        /// </summary>
        public string _missionDescriptionType = "CryStory.Runtime.MissionDescriptionSimpleRPG";

        /// <summary>
        /// 当添加至Story中时，将作为MIssion节点的任务描述类，修改为上述设置类型
        /// 若新类型与旧类型不一致，将导致数据丢失
        /// </summary>
        /// <param name="node"></param>
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

    }
}