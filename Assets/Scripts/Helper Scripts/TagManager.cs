using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TagManager
{
    public static string HORIZONTAL_AXIS = "Horizontal";
    public static string DISPLAY_BACKGROUND = "DisplayBackground";
    public static string SHOWROOM_SCENE_NAME = "Showroom";
    public static string MAIN_MENU_SCENE_NAME = "MainShowroomGUI";
    public static string OUTPUT_PANEL_NAME = "OutputPanel";
    public const string NAN = "NaN";

    public static int PANEL_TEMPLATE_START_POSITION_R = 0; //For resize panels
    public static int PANEL_TEMPLATE_START_POSITION = -360;
    public static int DISPLAY_TEMPLATE_START_POSITION = -720;
    public static int NEXT_POS_DISPLAY = 480;
    public static int NEXT_POS_PANEL = 240;
    public static int Y_RESOLUTION = 1080;
    public static int X_RESOLUTION = 1920;
    public static int IMAGE_PREFAB_INDEX = 0;
    public static int VIDEO_PREFAB_INDEX = 1;
    public static int W_NIGHT_PREFAB_INDEX = 2;
    public static int W_DAY_PREFAB_INDEX = 3;
    public static int JUMP_PREFAB_PICKER = 4;
    public static int NIGHT_TIME = 8;
}
