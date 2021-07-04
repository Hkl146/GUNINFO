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

        public static bool shownpc = false;
        public static bool caidan = true;
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
                    GUI.Label(new Rect((float)(Screen.width - 320), 340f, 200, 20f), GUNINFO.caidan ? "P 菜单: 开启" : "P 菜单: 关闭");
                    GUI.Label(new Rect((float)(Screen.width - 170), 340f, 200, 20f), GUNINFO.shownpc ? "I 场景信息: 开启" : "I 场景信息: 关闭");
                    GUI.Label(new Rect((float)(Screen.width - 320), 360f, 200, 20f), "按住T显示场景物品，I为单次开关");
                    if (HeroCameraManager.HeroObj != null && HeroCameraManager.HeroObj.BulletPreFormCom != null && HeroCameraManager.HeroObj.BulletPreFormCom.weapondict != null)
                    {
                        GUI.Label(new Rect((float)(Screen.width - 320), 380f, 500f, 20f), "英雄当前移动速度：" + HeroCameraManager.HeroObj.playerProp.Speed.ToString());
                        this.weaponinfoONGUI();
                    }
                }
                GUIStyle guistyle = new GUIStyle();
                guistyle.alignment = TextAnchor.MiddleCenter;
                guistyle.normal.background = null;
                if (GUNINFO.shownpc)
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
                                if (distance > 50.0)
                                {
                                    guistyle.normal.textColor = new Color32(240, 240, 240, 160);
                                }
                                else
                                {
                                    guistyle.normal.textColor = new Color32(240, 240, 240, 220);
                                }
                                if (distance > 50.0)
                                {
                                    guistyle.fontSize = 15;
                                }
                                else if (distance < 10.0)
                                {
                                    guistyle.fontSize = 25;
                                }
                                else
                                {
                                    guistyle.fontSize = (int)(distance * -0.25 + 27.5);
                                }
                                string text = distance.ToString("0");
                                text = this.FightTypeToString(value) + "  " + text;
                                GUI.Label(new Rect(vector.x - 200f, (float)Screen.height - vector.y - 30f, 400f, 60f), text, guistyle);
                            }
                        }
                    }
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
            else if ((obj.FightType == ServerDefine.FightType.WARRIOR_OBSTACLE_NORMAL && (obj.Shape == 4406||obj.Shape == 4419||obj.Shape == 4427)) )return true;
            else if ((obj.FightType == ServerDefine.FightType.NWARRIOR_NPC_TRANSFER && (obj.Shape == 4016 || obj.Shape == 4009 || obj.Shape == 4019))) return true;
            else if (obj.FightType == ServerDefine.FightType.NWARRIOR_NPC_EVENT) return true; 
            else if (obj.FightType == ServerDefine.FightType.NWARRIOR_NPC_ITEMBOX) return true;
            else if  ( obj.FightType == ServerDefine.FightType.NWARRIOR_NPC_GSCASHSHOP) return true;
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
        public void weaponinfoONGUI()
        {
            int WEPNPOS = 0;
            int WEPNY = 400;
            int WEPNX = Screen.width - 320;
            string text = "一号位武器";
            foreach (KeyValuePair<int, WeaponPerformanceObj> keyValuePair in HeroCameraManager.HeroObj.BulletPreFormCom.weapondict)
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
                        WEPNY = 400;
                        break;
                }
                GUI.Label(new Rect((float)WEPNX, (float)WEPNY, 150f, 20f), text);
                WEPNY += 20;
                if (keyValuePair.value.WeaponAttr.ElementType != 2048)
                {
                    if (keyValuePair.value.WeaponAttr.ElementType == 256)
                    {
                        GUI.Label(new Rect((float)WEPNX, (float)WEPNY, 150f, 20f), "属性:电");
                    }
                    else if (keyValuePair.value.WeaponAttr.ElementType == 512)
                    {
                        GUI.Label(new Rect((float)WEPNX, (float)WEPNY, 150f, 20f), "属性:毒");
                    }
                    else if (keyValuePair.value.WeaponAttr.ElementType == 1024)
                    {
                        GUI.Label(new Rect((float)WEPNX, (float)WEPNY, 150f, 20f), "属性:火");
                    }
                    WEPNY += 20;
                    GUI.Label(new Rect((float)WEPNX, (float)WEPNY, 150f, 20f), "属性异常几率:" + keyValuePair.value.WeaponAttr.DebuffProb / 100 + "%");
                    WEPNY += 20;
                }
                if (keyValuePair.value.WeaponAttr.Stability[0] != 0)
                {
                    GUI.Label(new Rect((float)WEPNX, (float)WEPNY, 150f, 20f), "稳定性加成: " + keyValuePair.value.WeaponAttr.Stability[0]);
                    WEPNY += 20;
                }
                if (keyValuePair.value.WeaponAttr.Accuracy[0] != 0)
                {
                    GUI.Label(new Rect((float)WEPNX, (float)WEPNY, 150f, 20f), "精准度加成: " + keyValuePair.value.WeaponAttr.Accuracy[0]);
                    WEPNY += 20;
                }
                if (keyValuePair.value.WeaponAttr.AttSpeed[0] != 0)
                {
                    GUI.Label(new Rect((float)WEPNX, (float)WEPNY, 150f, 20f), "射击速度: " + keyValuePair.value.WeaponAttr.AttSpeed[0]);
                    WEPNY += 20;
                }
                if (keyValuePair.value.WeaponAttr.FillTime != 0)
                {
                    GUI.Label(new Rect((float)WEPNX, (float)WEPNY, 150f, 20f), "换弹速度: " + keyValuePair.value.WeaponAttr.FillTime);
                    WEPNY += 20;
                }
                GUI.Label(new Rect((float)WEPNX, (float)WEPNY, 150f, 20f), "暴击倍率: " + keyValuePair.value.WeaponAttr.CrazyEff / 10000);
                WEPNY += 20;
                GUI.Label(new Rect((float)WEPNX, (float)WEPNY, 150f, 20f), "子弹速度: " + keyValuePair.value.WeaponAttr.BulletSpeed);
                WEPNY += 20;
                GUI.Label(new Rect((float)WEPNX, (float)WEPNY, 150f, 20f), "射击距离: " + keyValuePair.value.WeaponAttr.AttDis);
                WEPNY += 20;
                if (keyValuePair.value.WeaponAttr.Radius != 0f)
                {
                    GUI.Label(new Rect((float)WEPNX, (float)WEPNY, 150f, 20f), "爆炸范围: " + keyValuePair.value.WeaponAttr.Radius);
                    WEPNY += 20;
                }
                if (keyValuePair.value.WeaponAttr.LuckyHit != 0)
                {
                    GUI.Label(new Rect((float)WEPNX, (float)WEPNY, 150f, 20f), "幸运一击加成: " + keyValuePair.value.WeaponAttr.LuckyHit);
                    WEPNY += 20;
                }
            }
        }


    }
}