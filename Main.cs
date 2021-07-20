using DataHelper;
using HeroCameraName;
using Il2CppSystem.Collections.Generic;
using Item;
using MelonLoader;
using System;
using UnityEngine;

namespace GUNINFO
{
    public static class BuildInfo
    {
        public const string Name = "GUNINFO"; // Name of the Mod.  (MUST BE SET)
        public const string Description = "Gunfire Reborn Info"; // Description for the Mod.  (Set as null if none)
        public const string Author = "MT"; // Author of the Mod.  (Set as null if none)
        public const string Company = null; // Company that made the Mod.  (Set as null if none)
        public const string Version = "1.0.0"; // Version of the Mod.  (MUST BE SET)
        public const string DownloadLink = null; // Download Link for the Mod.  (Set as null if none)
    }

    public class GUNINFO : MelonMod
    {



        private static bool shownpc = false;
        private static bool caidan = true;
        private static bool jiangpin = false;
        private static int pinglv = 0;
        private float[] ItemINFOdata;
        private string[] ItemINFOtext;
        private int ItemINFONum = 0;
        private static bool test = false;
        public override void OnApplicationStart() // Runs after Game Initialization.
        {
            MelonLogger.Msg("GUNINFO Loaded");
        }

        public override void OnUpdate() // Runs once per frame.
        {
            try
            {

                if (Input.GetKeyUp(KeyCode.T) || Input.GetKeyUp(KeyCode.I) || Input.GetKeyDown(KeyCode.T))
                {
                    GUNINFO.shownpc = !GUNINFO.shownpc;
                }

                if (Input.GetKeyUp(KeyCode.P))
                {
                    GUNINFO.caidan = !GUNINFO.caidan;
                }

                if (Input.GetKeyUp(KeyCode.U))
                {
                    GUNINFO.jiangpin = !GUNINFO.jiangpin;
                    GUNINFO.pinglv = 0;
                }

                if (Input.GetKeyUp(KeyCode.O))
                {
                    //GUNINFO.test = !GUNINFO.test;


                }

                if (GUNINFO.shownpc)
                {
                    if (!GUNINFO.jiangpin)
                    {
                        int Num = 0;
                        foreach (KeyValuePair<int, NewPlayerObject> keyValuePair in NewPlayerManager.PlayerDict)
                        {
                            NewPlayerObject value = keyValuePair.Value;
                            if (!(value.centerPointTrans == null) && this.ShowObject(value))
                            {
                                Num++;
                            }
                        }
                        ItemINFOdata = new float[Num * 3];
                        ItemINFOtext = new string[Num];
                        ItemINFONum = 0;

                        foreach (KeyValuePair<int, NewPlayerObject> keyValuePair in NewPlayerManager.PlayerDict)
                        {
                            NewPlayerObject value = keyValuePair.Value;
                            if (!(value.centerPointTrans == null) && this.ShowObject(value))
                            {
                                Vector3 vector = CameraManager.MainCameraCom.WorldToScreenPoint(value.centerPointTrans.transform.position);

                                ItemINFOdata[ItemINFONum * 3] = value.centerPointTrans.transform.position.x;
                                ItemINFOdata[ItemINFONum * 3 + 1] = value.centerPointTrans.transform.position.y;
                                ItemINFOdata[ItemINFONum * 3 + 2] = value.centerPointTrans.transform.position.z;
                                ItemINFOtext[ItemINFONum] = this.FightTypeToString(value);
                                ItemINFONum++;

                            }
                        }
                    }
                }

            }
            catch
            {

            }
        }


        public override void OnGUI() // Can run multiple times per frame. Mostly used for Unity's IMGUI.
        {
            try
            {
                if (GUNINFO.caidan)
                {
                    int INFOGUIY = 400;
                    GUI.Label(new Rect((float)(Screen.width - 320), INFOGUIY, 200, 20f), GUNINFO.caidan ? "P 菜单: 开启" : "P 菜单: 关闭");
                    GUI.Label(new Rect((float)(Screen.width - 225), INFOGUIY, 200, 20f), GUNINFO.shownpc ? "I 场景信息: 开启" : "I 场景信息: 关闭");
                    GUI.Label(new Rect((float)(Screen.width - 110), INFOGUIY, 200, 20f), GUNINFO.jiangpin ? "U 降频模式: 开启" : "U 降频模式: 关闭");
                    INFOGUIY += 20;
                    GUI.Label(new Rect((float)(Screen.width - 320), INFOGUIY, 320, 20f), "如果掉帧，按住T显示场景物品，I为单次开关");
                    INFOGUIY += 20;
                    GUI.Label(new Rect((float)(Screen.width - 320), INFOGUIY, 320, 20f), "U开启降频模式,降低刷新频率以提高帧率(已经没什么作用)");
                    INFOGUIY += 20;
                    if (HeroCameraManager.HeroObj != null && HeroCameraManager.HeroObj.BulletPreFormCom != null && HeroCameraManager.HeroObj.BulletPreFormCom.weapondict != null)
                    {
                        GUI.Label(new Rect((float)(Screen.width - 320), INFOGUIY, 500f, 20f), "英雄当前移动速度：" + HeroCameraManager.HeroObj.playerProp.Speed.ToString());
                        INFOGUIY += 20;
                        this.weaponinfoONGUI(INFOGUIY);
                    }
                }
                GUIStyle guistyle = new GUIStyle();
                guistyle.alignment = TextAnchor.MiddleCenter;
                guistyle.normal.background = null;
                if (GUNINFO.shownpc)
                {
                    if (!GUNINFO.jiangpin)
                    {

                        for (int Num = 0; Num < ItemINFONum; Num++)
                        {
                            float X = (float)ItemINFOdata[Num * 3];
                            float Y = (float)ItemINFOdata[Num * 3 + 1];
                            float Z = (float)ItemINFOdata[Num * 3 + 2];

                            Vector3 info_vector = new Vector3(X, Y, Z);
                            Vector3 vector = CameraManager.MainCameraCom.WorldToScreenPoint(info_vector);
                            if (vector.z > 0f)
                            {
                                double distance = (double)Vector3.Distance(HeroMoveManager.HeroObj.centerPointTrans.position, info_vector);
                                if (distance < 2.5) continue;
                                if (distance > 50.0) guistyle.normal.textColor = new Color32(240, 240, 240, 160);
                                else guistyle.normal.textColor = new Color32(240, 240, 240, 220);
                                if (distance > 50.0) guistyle.fontSize = 15;
                                else if (distance < 10.0) guistyle.fontSize = 25;
                                else guistyle.fontSize = (int)(distance * -0.25 + 27.5);

                                string text = ItemINFOtext[Num];
                                text = text + "  " + distance.ToString("0.0");
                                GUI.Label(new Rect(vector.x - 150, Screen.height - vector.y - 30f, 300, 60f), text, guistyle);

                            }
                        }
                    }
                    else if (GUNINFO.jiangpin)
                    {
                        //调用8次刷新一次，尝试增加帧数
                        //已经没用存在的必要

                        if (GUNINFO.pinglv == 0)
                        {
                            int Num = 0;
                            foreach (KeyValuePair<int, NewPlayerObject> keyValuePair in NewPlayerManager.PlayerDict)
                            {
                                NewPlayerObject value = keyValuePair.Value;
                                if (!(value.centerPointTrans == null) && this.ShowObject(value))
                                {
                                    Vector3 vector = CameraManager.MainCameraCom.WorldToScreenPoint(value.centerPointTrans.transform.position);
                                    if (vector.z > 0f)
                                    {
                                        Num++;
                                    }
                                }
                            }
                            ItemINFOdata = new float[Num * 3];
                            ItemINFOtext = new string[Num];
                            ItemINFONum = 0;

                            foreach (KeyValuePair<int, NewPlayerObject> keyValuePair in NewPlayerManager.PlayerDict)
                            {
                                NewPlayerObject value = keyValuePair.Value;
                                if (!(value.centerPointTrans == null) && this.ShowObject(value))
                                {
                                    Vector3 vector = CameraManager.MainCameraCom.WorldToScreenPoint(value.centerPointTrans.transform.position);
                                    if (vector.z > 0f)
                                    {
                                        double distance = (double)Vector3.Distance(HeroMoveManager.HeroObj.centerPointTrans.position, value.centerPointTrans.position);
                                        if (distance < 2.5) continue;
                                        if (distance > 50.0) guistyle.normal.textColor = new Color32(240, 240, 240, 160);
                                        else guistyle.normal.textColor = new Color32(240, 240, 240, 220);
                                        if (distance > 50.0) guistyle.fontSize = 15;
                                        else if (distance < 10.0) guistyle.fontSize = 25;
                                        else guistyle.fontSize = (int)(distance * -0.25 + 27.5);

                                        string text = this.FightTypeToString(value) + "  " + distance.ToString("0.0");
                                        GUI.Label(new Rect(vector.x - 150, Screen.height - vector.y - 30f, 300, 60f), text, guistyle);


                                        ItemINFOdata[ItemINFONum * 3] = value.centerPointTrans.transform.position.x;
                                        ItemINFOdata[ItemINFONum * 3 + 1] = value.centerPointTrans.transform.position.y;
                                        ItemINFOdata[ItemINFONum * 3 + 2] = value.centerPointTrans.transform.position.z;
                                        ItemINFOtext[ItemINFONum] = this.FightTypeToString(value);
                                        ItemINFONum++;
                                    }
                                }
                            }
                        }

                        else if (GUNINFO.pinglv != 0)
                        {
                            for (int Num = 0; Num < ItemINFONum; Num++)
                            {
                                float X = (float)ItemINFOdata[Num * 3];
                                float Y = (float)ItemINFOdata[Num * 3 + 1];
                                float Z = (float)ItemINFOdata[Num * 3 + 2];
                                string text = ItemINFOtext[Num];
                                Vector3 info_vector = new Vector3(X, Y, Z);
                                Vector3 vector = CameraManager.MainCameraCom.WorldToScreenPoint(info_vector);
                                if (vector.z > 0f)
                                {
                                    double distance = (double)Vector3.Distance(HeroMoveManager.HeroObj.centerPointTrans.position, info_vector);
                                    if (distance < 2.5) continue;
                                    if (distance > 50.0) guistyle.normal.textColor = new Color32(240, 240, 240, 160);
                                    else guistyle.normal.textColor = new Color32(240, 240, 240, 220);
                                    if (distance > 50.0) guistyle.fontSize = 15;
                                    else if (distance < 10.0) guistyle.fontSize = 25;
                                    else guistyle.fontSize = (int)(distance * -0.25 + 27.5);

                                    text = text + "  " + distance.ToString("0.0");
                                    GUI.Label(new Rect(vector.x - 150, Screen.height - vector.y - 30f, 300, 60f), text, guistyle);

                                }
                            }
                        }
                    }
                }


                if (GUNINFO.test)
                {

                }

                return;
            }
            catch
            {

            }
        }





        public bool ShowObject(NewPlayerObject obj)
        {
            if (obj.FightType != ServerDefine.FightType.NWARRIOR_DROP_BULLET || obj.FightType != ServerDefine.FightType.NWARRIOR_DROP_CASH)
            {
                switch (obj.FightType)
                {
                    case ServerDefine.FightType.NWARRIOR_DROP_EQUIP: return true;

                    case ServerDefine.FightType.WARRIOR_OBSTACLE_NORMAL:
                        if (obj.Shape == 4406 || obj.Shape == 4419 || obj.Shape == 4427) return true;
                        break;
                    case ServerDefine.FightType.NWARRIOR_NPC_TRANSFER:
                        if (obj.Shape == 4016 || obj.Shape == 4009 || obj.Shape == 4019) return true;
                        break;
                    case ServerDefine.FightType.NWARRIOR_DROP_RELIC: return true;
                    case ServerDefine.FightType.NWARRIOR_NPC_SMITH: return true;
                    case ServerDefine.FightType.NWARRIOR_NPC_SHOP: return true;
                    case ServerDefine.FightType.NWARRIOR_NPC_EVENT: return true;
                    case ServerDefine.FightType.NWARRIOR_NPC_REFRESH: return true;
                    case ServerDefine.FightType.NWARRIOR_NPC_ITEMBOX: return true;
                    case ServerDefine.FightType.NWARRIOR_NPC_GSCASHSHOP: return true;
                    default: return false;
                }
            }

            return false;
        }
        public String FightTypeToString(NewPlayerObject obj)
        {
            switch (obj.FightType)
            {
                case ServerDefine.FightType.NWARRIOR_DROP_EQUIP:
                    return DataMgr.GetWeaponData(obj.Shape).Name + " +" + obj.DropOPCom.WeaponInfo.SIProp.Grade.ToString();
                case ServerDefine.FightType.NWARRIOR_DROP_RELIC:
                    return DataMgr.GetRelicData(obj.DropOPCom.RelicSid).Name;
                case ServerDefine.FightType.NWARRIOR_NPC_SMITH:
                    return "工匠";
                case ServerDefine.FightType.NWARRIOR_NPC_SHOP:
                    return "商人";
                case ServerDefine.FightType.NWARRIOR_NPC_EVENT:
                    return "事件宝箱";
                case ServerDefine.FightType.NWARRIOR_NPC_REFRESH:
                    return "事件宝箱";
                case ServerDefine.FightType.NWARRIOR_NPC_ITEMBOX:
                    return "奖励宝箱";
                case ServerDefine.FightType.WARRIOR_OBSTACLE_NORMAL:
                    return "秘境";
                case ServerDefine.FightType.NWARRIOR_NPC_TRANSFER:
                    return "秘境";
                case ServerDefine.FightType.NWARRIOR_NPC_GSCASHSHOP:
                    return "奇货商";
                default:
                    return "unk";
            }


        }
        public void weaponinfoONGUI(int GUIY)
        {
            int WEPNPOS = 0;
            int WEPNX = Screen.width - 320;
            int WEPNY = GUIY;
            string text = "一号位武器";
            //当前武器ObjectID
            //var mwep = HeroCameraManager.HeroObj.playerProp.CurWeapon[3]; 
            foreach (KeyValuePair<int, WeaponPerformanceObj> weapondict in HeroCameraManager.HeroObj.BulletPreFormCom.weapondict)
            {
                switch (WEPNPOS++)
                {
                    case 0:
                        continue;
                    case 1:
                        text = "一号位武器";
                        break;
                    case 2:
                        text = "二号位武器";
                        WEPNX = Screen.width - 170;
                        WEPNY = GUIY;
                        break;
                }
                GUI.Label(new Rect((float)WEPNX, (float)WEPNY, 150f, 20f), text);
                WEPNY += 20;
                if (weapondict.value.WeaponAttr.ElementType != 2048)
                {
                    if (weapondict.value.WeaponAttr.ElementType == 256)
                    {
                        GUI.Label(new Rect((float)WEPNX, (float)WEPNY, 150f, 20f), "属性:电");
                    }
                    else if (weapondict.value.WeaponAttr.ElementType == 512)
                    {
                        GUI.Label(new Rect((float)WEPNX, (float)WEPNY, 150f, 20f), "属性:毒");
                    }
                    else if (weapondict.value.WeaponAttr.ElementType == 1024)
                    {
                        GUI.Label(new Rect((float)WEPNX, (float)WEPNY, 150f, 20f), "属性:火");
                    }
                    WEPNY += 20;
                    GUI.Label(new Rect((float)WEPNX, (float)WEPNY, 150f, 20f), "属性异常几率:" + weapondict.value.WeaponAttr.DebuffProb / 100 + "%");
                    WEPNY += 20;
                }
                if (weapondict.value.WeaponAttr.Stability.get_Item(0) != 0)
                {
                    GUI.Label(new Rect((float)WEPNX, (float)WEPNY, 150f, 20f), "稳定性加成: " + weapondict.value.WeaponAttr.Stability.get_Item(0));
                    WEPNY += 20;
                }

                if (weapondict.value.WeaponAttr.Accuracy.get_Item(0) != 0)
                {
                    GUI.Label(new Rect((float)WEPNX, (float)WEPNY, 150f, 20f), "精准度加成: " + weapondict.value.WeaponAttr.Accuracy.get_Item(0));
                    WEPNY += 20;
                }
                if (weapondict.value.WeaponAttr.AttSpeed.get_Item(0) != 0)
                {
                    GUI.Label(new Rect((float)WEPNX, (float)WEPNY, 150f, 20f), "射击速度: " + weapondict.value.WeaponAttr.AttSpeed.get_Item(0));
                    WEPNY += 20;
                }
                if (weapondict.value.WeaponAttr.FillTime != 0)
                {
                    GUI.Label(new Rect((float)WEPNX, (float)WEPNY, 150f, 20f), "换弹速度: " + weapondict.value.WeaponAttr.FillTime);
                    WEPNY += 20;
                }
                GUI.Label(new Rect((float)WEPNX, (float)WEPNY, 150f, 20f), "暴击倍率: " + weapondict.value.WeaponAttr.CrazyEff / 10000);
                WEPNY += 20;
                GUI.Label(new Rect((float)WEPNX, (float)WEPNY, 150f, 20f), "子弹速度: " + weapondict.value.WeaponAttr.BulletSpeed);
                WEPNY += 20;
                GUI.Label(new Rect((float)WEPNX, (float)WEPNY, 150f, 20f), "射击距离: " + weapondict.value.WeaponAttr.AttDis);
                WEPNY += 20;
                if (weapondict.value.WeaponAttr.Radius != 0f)
                {
                    GUI.Label(new Rect((float)WEPNX, (float)WEPNY, 150f, 20f), "爆炸范围: " + weapondict.value.WeaponAttr.Radius);
                    WEPNY += 20;
                }
                if (weapondict.value.WeaponAttr.LuckyHit != 0)
                {
                    GUI.Label(new Rect((float)WEPNX, (float)WEPNY, 150f, 20f), "幸运一击加成: " + weapondict.value.WeaponAttr.LuckyHit);
                    WEPNY += 20;
                }
            }
        }

    }
}