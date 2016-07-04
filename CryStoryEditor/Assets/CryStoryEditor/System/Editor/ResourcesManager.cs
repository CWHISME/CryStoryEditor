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
        public Texture2D texLogo;
        public Texture2D texInputSlot;
        public Texture2D texInputSlotActive;
        public Texture2D texOutputSlot;
        public Texture2D texOutputSlotActive;
        public Texture2D texTargetArea;
        public Texture2D texSave;
        public Texture2D texGrid;
        public Texture2D texWhiteBorder;
        public Texture2D texBackground;

        public GUISkin skin;

        public GUIStyle StyleBackground { get { return skin.GetStyle("Background"); } }
        public GUIStyle MissionNode { get { return skin.GetStyle("MissionNode"); } }
        public GUIStyle MissionNodeOn { get { return skin.GetStyle("MissionNodeOn"); } }

        public ResourcesManager()
        {
            if (texLogo == null)
                texLogo = Resources.Load("cf_logo") as Texture2D;
            if (texInputSlot == null)
                texInputSlot = Resources.Load("cf_input_slot") as Texture2D;
            if (texInputSlotActive == null)
                texInputSlotActive = Resources.Load("cf_input_slot_active") as Texture2D;
            if (texOutputSlot == null)
                texOutputSlot = Resources.Load("cf_output_slot") as Texture2D;
            if (texOutputSlotActive == null)
                texOutputSlotActive = Resources.Load("cf_output_slot_active") as Texture2D;
            if (texTargetArea == null)
                texTargetArea = Resources.Load("cf_target_area") as Texture2D;
            if (texSave == null)
                texSave = Resources.Load("cf_save") as Texture2D;
            if (texGrid == null)
                texGrid = Resources.Load("cf_grid") as Texture2D;
            if (texWhiteBorder == null)
                texWhiteBorder = Resources.Load("WhiteBorder") as Texture2D;
            if (texBackground == null)
                texBackground = Resources.Load("Background") as Texture2D;
            if (skin == null) skin = Resources.Load<GUISkin>("Skin");
        }

    }
}