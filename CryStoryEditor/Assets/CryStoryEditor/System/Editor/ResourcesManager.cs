/**********************************************************
*Author: wangjiaying
*Date: 2016.6.27
*Func:
**********************************************************/
using UnityEngine;
using System.Collections;

namespace CryStory.Editor
{
    public class ResourcesManager : Singleton<ResourcesManager>
    {

        //Resources
        public Texture2D texInputSlot;
        public Texture2D texInputSlotActive;
        public Texture2D texOutputSlot;
        public Texture2D texOutputSlotActive;
        public Texture2D texGrid;
        public Texture2D texWhiteBorder;
        public Texture2D texBackground;

        public GUISkin skin;

        public GUIStyle StyleBackground { get { return skin.GetStyle("Background"); } }
        public GUIStyle Node { get { return skin.GetStyle("MissionNode"); } }
        public GUIStyle NodeOn { get { return skin.GetStyle("MissionNodeOn"); } }

        public GUIStyle CoreNode { get { return skin.GetStyle("CoreNode"); } }
        public GUIStyle CoreNodeOn { get { return skin.GetStyle("CoreNodeOn"); } }

        public GUIStyle EventNode { get { return skin.GetStyle("EventNode"); } }
        public GUIStyle EventNodeOn { get { return skin.GetStyle("EventNodeOn"); } }
        public GUIStyle ConditionNode { get { return skin.GetStyle("ConditionNode"); } }
        public GUIStyle ConditionNodeOn { get { return skin.GetStyle("ConditionNodeOn"); } }
        public GUIStyle ActionNode { get { return skin.GetStyle("ActionNode"); } }
        public GUIStyle ActionNodeOn { get { return skin.GetStyle("ActionNodeOn"); } }

        private GUIStyle fontStyle = new GUIStyle();
        public GUIStyle GetFontStyle(int fontSize)
        {
            fontStyle.normal.textColor = Color.white;
            fontStyle.fontSize = fontSize;
            return fontStyle;
        }

        public GUIStyle GetFontStyle(int fontSize, Color color)
        {
            fontStyle.normal.textColor = color;
            fontStyle.fontSize = fontSize;
            return fontStyle;
        }

        private TextAsset helpCN;
        private TextAsset helpEN;
        public string HelpCN { get { return helpCN ? helpCN.text : "错误：没有找到帮助文件!"; } }
        public string HelpEN { get { return helpEN ? helpEN.text : "Error：Help file not found!"; } }

        public ResourcesManager()
        {
            if (texInputSlot == null)
                texInputSlot = Resources.Load("CryStory/Image/Slot/input_slot") as Texture2D;
            if (texInputSlotActive == null)
                texInputSlotActive = Resources.Load("CryStory/Image/Slot/input_slot_active") as Texture2D;
            if (texOutputSlot == null)
                texOutputSlot = Resources.Load("CryStory/Image/Slot/output_slot") as Texture2D;
            if (texOutputSlotActive == null)
                texOutputSlotActive = Resources.Load("CryStory/Image/Slot/output_slot_active") as Texture2D;
            if (texGrid == null)
                texGrid = Resources.Load("CryStory/Image/grid") as Texture2D;
            if (texWhiteBorder == null)
                texWhiteBorder = Resources.Load("CryStory/WhiteBorder") as Texture2D;
            if (texBackground == null)
                texBackground = Resources.Load("CryStory/Background") as Texture2D;
            if (skin == null) skin = Resources.Load<GUISkin>("CryStory/Skin");


            if (helpCN == null) helpCN = Resources.Load<TextAsset>("CryStory/Help/Help_CN");
            if (helpEN == null) helpEN = Resources.Load<TextAsset>("CryStory/Help/Help_EN");
        }

    }
}