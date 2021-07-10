using DataHelper;
using HeroCameraName;
using Il2CppSystem.Collections.Generic;
using Item;
using MelonLoader;
using System;
using System.Linq;
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
    /*public class ShowItemLIB
    {
        double[][] data;
        int size;
        List<string> text;

        public ShowItemLIB()
        {
            data = new double[10][];
            size = 0;
            text = new List<string>();
        }
        public void add(double x, double y, double distant, string text)
        {
            MelonLogger.Msg(size);
            if (size + 1 >= data.Length)
            {
                double[][] arr = data;
                data = new double[size + 10][];
                data = arr;
                MelonLogger.Msg("6");
            }
            MelonLogger.Msg("2");
            data[size][0] = x;
            MelonLogger.Msg("3");
            data[size][1] = y;
            MelonLogger.Msg("4");
            data[size][2] = distant;
            MelonLogger.Msg("5");
            this.text.Add(text);
            MelonLogger.Msg(x + "  " + y + "  " + distant + "  " + text);
            size++;
        }
        public double GetX(int size)
        {
            return data[size][0];
        }
        public double GetY(int size)
        {
            return data[size][1];
        }
        public double GetDistant(int size)
        {
            return data[size][2];
        }
        public string GetText(int size)
        {
            return text[size];
        }
        public int GetSiza()
        {
            return size;
        }

    }

    public class GUNINFOItemEntry
    {
        public float x;
        public float y;
        public double distant;
        public string text;

        
        public GUNINFOItemEntry(float x, float y, double distant, string text)
        {
            MelonLogger.Msg("2");
            this.x = x;
            MelonLogger.Msg("3");
            this.y = y;
            MelonLogger.Msg("4");
            this.distant = distant;
            MelonLogger.Msg("5");
            this.text = text;
            MelonLogger.Msg(x + "  " + y + "  " + distant + "  " + text);
        }

        public float GetX()
        {
            return x;
        }
        public float GetY()
        {
            return y;
        }
        public double GetDistant()
        {
            return distant;
        }
        public string GetText()
        {
            return text;
        }

    }*/
    public class GUNINFO : MelonMod
    {

        public static bool shownpc = false;
        public static bool caidan = true;
        public static bool jiangpin = false;
        public static int pinglv = 0;
        public static List<double> ItemINFOdata = new List<double>();
        public static List<string> ItemINFOtext = new List<string>();
        private static int ItemINFONum = 0;
        public static bool test = false;
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
                }
                if (Input.GetKeyUp(KeyCode.O))
                {
                    // GUNINFO.test = !GUNINFO.test;
                }
            }
            catch
            {

            }
        }

        public override void OnFixedUpdate() // Can run multiple times per frame. Mostly used for Physics.
        {
        }

        public override void OnLateUpdate() // Runs once per frame after OnUpdate and OnFixedUpdate have finished.
        {
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
                    GUI.Label(new Rect((float)(Screen.width - 320), INFOGUIY, 320, 20f), "U开启降频模式,降低刷新频率以提高帧率(勉强有点用)");
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
                    //调用8次刷新一次，尝试增加帧数
                    if (GUNINFO.pinglv == 0)
                    {

                        foreach (KeyValuePair<int, NewPlayerObject> keyValuePair in NewPlayerManager.PlayerDict)
                        {
                            NewPlayerObject value = keyValuePair.Value;
                            if (!(value.centerPointTrans == null) && this.ShowObject(value))
                            {
                                Vector3 vector = CameraManager.MainCameraCom.WorldToScreenPoint(value.centerPointTrans.transform.position);
                                if (vector.z > 0f)
                                {
                                    double distance = (double)Vector3.Distance(HeroMoveManager.HeroObj.centerPointTrans.position, value.centerPointTrans.position);

                                    if (distance > 50.0) guistyle.normal.textColor = new Color32(240, 240, 240, 160);
                                    else guistyle.normal.textColor = new Color32(240, 240, 240, 220);
                                    if (distance > 50.0) guistyle.fontSize = 15;
                                    else if (distance < 10.0) guistyle.fontSize = 25;
                                    else guistyle.fontSize = (int)(distance * -0.25 + 27.5);

                                    string text = distance.ToString("0");
                                    text = this.FightTypeToString(value) + "  " + text;
                                    GUI.Label(new Rect(vector.x - 200f, Screen.height - vector.y - 30f, 400f, 60f), text, guistyle);

                                    //调用8次刷新一次，尝试增加帧数
                                    if (!GUNINFO.jiangpin) continue;
                                    ItemINFOdata.Add(vector.x - 200f);
                                    ItemINFOdata.Add(Screen.height - vector.y - 30f);
                                    ItemINFOdata.Add(distance);
                                    ItemINFOtext.Add(text);
                                    ItemINFONum++;
                                }
                            }
                        }
                    }

                    //调用8次刷新一次，尝试增加帧数
                    else if (GUNINFO.pinglv != 0)
                    {
                        for (int Num = 0; Num < ItemINFONum; Num++)
                        {
                            float X = (float)ItemINFOdata[Num * 3];
                            float Y = (float)ItemINFOdata[Num * 3 + 1];
                            double distance = ItemINFOdata[Num * 3 + 2];
                            string text = ItemINFOtext[Num];

                            if (distance > 50.0) guistyle.normal.textColor = new Color32(240, 240, 240, 160);
                            else guistyle.normal.textColor = new Color32(240, 240, 240, 220);
                            if (distance > 50.0) guistyle.fontSize = 15;
                            else if (distance < 10.0) guistyle.fontSize = 25;
                            else guistyle.fontSize = (int)(distance * -0.25 + 27.5);

                            GUI.Label(new Rect(X, Y, 400f, 60f), text, guistyle);
                        }


                        if (!GUNINFO.jiangpin)
                        {
                            GUNINFO.pinglv = 0;
                            ItemINFOdata = new List<double>();
                            ItemINFOtext = new List<string>();
                            ItemINFONum = 0;
                        }
                    }
                    if (GUNINFO.jiangpin)
                    {
                        GUNINFO.pinglv = (GUNINFO.pinglv + 1) % 8;//调用8次刷新一次，尝试增加帧数
                        if (GUNINFO.pinglv == 0)
                        {
                            ItemINFOdata = new List<double>();
                            ItemINFOtext = new List<string>();
                            ItemINFONum = 0;
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
            if (obj.FightType == ServerDefine.FightType.NWARRIOR_DROP_EQUIP) return true;

            else if (obj.FightType == ServerDefine.FightType.NWARRIOR_DROP_RELIC) return true;
            else if (obj.FightType == ServerDefine.FightType.NWARRIOR_NPC_SMITH) return true;
            else if (obj.FightType == ServerDefine.FightType.NWARRIOR_NPC_SHOP) return true;
            else if ((obj.FightType == ServerDefine.FightType.WARRIOR_OBSTACLE_NORMAL && (obj.Shape == 4406 || obj.Shape == 4419 || obj.Shape == 4427))) return true;
            else if ((obj.FightType == ServerDefine.FightType.NWARRIOR_NPC_TRANSFER && (obj.Shape == 4016 || obj.Shape == 4009 || obj.Shape == 4019))) return true;
            else if (obj.FightType == ServerDefine.FightType.NWARRIOR_NPC_EVENT) return true;
            else if (obj.FightType == ServerDefine.FightType.NWARRIOR_NPC_ITEMBOX) return true;
            else if (obj.FightType == ServerDefine.FightType.NWARRIOR_NPC_GSCASHSHOP) return true;
            return false;
        }
        public String FightTypeToString(NewPlayerObject obj)
        {
            if (obj.FightType == ServerDefine.FightType.NWARRIOR_DROP_EQUIP)
            {
                return DataMgr.GetWeaponData(obj.Shape).Name + " +" + obj.DropOPCom.WeaponInfo.SIProp.Grade.ToString();
            }
            if (obj.FightType == ServerDefine.FightType.NWARRIOR_DROP_RELIC)
            {
                return DataMgr.GetRelicData(obj.DropOPCom.RelicSid).Name;
            }
            if (obj.FightType == ServerDefine.FightType.NWARRIOR_NPC_SMITH)
            {
                return "工匠";
            }
            if (obj.FightType == ServerDefine.FightType.NWARRIOR_NPC_SHOP)
            {
                return "商人";
            }
            if (obj.FightType == ServerDefine.FightType.NWARRIOR_NPC_EVENT)
            {
                return "事件宝箱";
            }
            if (obj.FightType == ServerDefine.FightType.NWARRIOR_NPC_ITEMBOX)
            {
                return "奖励宝箱";
            }
            if (obj.FightType == ServerDefine.FightType.WARRIOR_OBSTACLE_NORMAL || obj.FightType == ServerDefine.FightType.NWARRIOR_NPC_TRANSFER)
            {
                return "秘境";
            }
            if (obj.FightType == ServerDefine.FightType.NWARRIOR_NPC_GSCASHSHOP)
            {
                return "奇货商";
            }
            return "unk";
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
                if (weapondict.value.WeaponAttr.Stability[0] != 0)
                {
                    GUI.Label(new Rect((float)WEPNX, (float)WEPNY, 150f, 20f), "稳定性加成: " + weapondict.value.WeaponAttr.Stability[0]);
                    WEPNY += 20;
                }
                if (weapondict.value.WeaponAttr.Accuracy[0] != 0)
                {
                    GUI.Label(new Rect((float)WEPNX, (float)WEPNY, 150f, 20f), "精准度加成: " + weapondict.value.WeaponAttr.Accuracy[0]);
                    WEPNY += 20;
                }
                if (weapondict.value.WeaponAttr.AttSpeed[0] != 0)
                {
                    GUI.Label(new Rect((float)WEPNX, (float)WEPNY, 150f, 20f), "射击速度: " + weapondict.value.WeaponAttr.AttSpeed[0]);
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