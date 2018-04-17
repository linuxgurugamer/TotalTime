
using UnityEngine;
using ToolbarControl_NS;

namespace TotalTime
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class RegisterToolbar : MonoBehaviour
    {
        void Start()
        {
            ToolbarControl.RegisterMod(MainMenuGui.MODID, MainMenuGui.MODNAME);
        }
    }
}