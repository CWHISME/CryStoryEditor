/**********************************************************
*Author: wangjiaying
*Date: 2016.7.7
*Func:
**********************************************************/
using UnityEngine;
using System.Collections;
using CryStory.Runtime;

namespace CryStory.Editor
{
    public class MissionEditor : NodeContentEditor<MissionEditor>
    {
        protected override void InternalOnGUI()
        {
            base.InternalOnGUI();
            CheckReturnStoryEditor();
        }

        private void CheckReturnStoryEditor()
        {
            if (IsDoubleClick() && !Tools.IsValidMouseAABB(_currentNodeRect))
            {
                _window._editMission = null;
            }
        }
    }
}