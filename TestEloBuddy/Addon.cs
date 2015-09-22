using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using Microsoft.Win32;
using SharpDX;
using System.Threading;
using System.Net;

namespace TestEloBuddy
{
    class Addon
    {
        private Spell.Active Q;
        private Spell.Active W;
        private Spell.Targeted E;
        private Spell.Active R;

        private Spell.Targeted flash;
        private Spell.Targeted smite;

        private Menu myMenu;
        private Menu comboMenu, smiteMenu;

        private int[] smiteDMG = { 390, 410, 430, 450, 480, 510, 540, 570, 600, 640, 680, 720, 760, 800, 850, 900, 950, 1000 };
        private int actualLevel = 1;
        private int smiteDmg = 390;

        public void setupMenu ()
        {
            string currentVersion = "0.1";
            Chat.Print("Simple Xinzao");
            Chat.Print("Checking version..");
            if (new WebClient().DownloadString("http://pastebin.com/raw.php?i=cZm2fW7R") != currentVersion)
                Chat.Print("This is old version");
            else
                Chat.Print("You have the last version of Simple Xinzao.");
            Game.Drop();
            myMenu = MainMenu.AddMenu("Xinzhao", "tittle");
            myMenu.AddLabel("Press Space to combo");
            comboMenu = myMenu.AddSubMenu("Combo settings", "comboSection");
            comboMenu.AddGroupLabel("Configuration");
            comboMenu.AddSeparator();

            comboMenu.Add("combo.Q", new CheckBox("Use Q"));
            comboMenu.Add("combo.W", new CheckBox("Use W"));
            comboMenu.Add("combo.R", new CheckBox("Use R"));
            comboMenu.Add("combo.E", new CheckBox("Use E"));
            comboMenu.Add("combo.smite",new CheckBox("Use Smite"));

            smiteMenu = myMenu.AddSubMenu("Smite settings", "smiteSection");
            smiteMenu.AddSeparator();
            smiteMenu.AddGroupLabel("Configuration");
            smiteMenu.Add("smite.RED", new CheckBox("Smite RED"));
            smiteMenu.Add("smite.BLUE", new CheckBox("Smite BLUE"));
            smiteMenu.Add("smite.DRAGON", new CheckBox("Smite DRAGON"));
            smiteMenu.Add("smite.PINKPENISH", new CheckBox("Smite BARON"));

            //GameObject.OnCreate += castWinWard;
            Game.OnTick += actives;
            Game.OnUpdate += gameUpdate;
            
        }

        public void setupSpell()
        {
            this.Q = new Spell.Active(SpellSlot.Q);
            this.W = new Spell.Active(SpellSlot.W);
            this.E = new Spell.Targeted(SpellSlot.E, 600);
            this.R = new Spell.Active(SpellSlot.R, 480);
            this.smite = new Spell.Targeted(SpellSlot.Summoner2, 500);
            this.flash = new Spell.Targeted(SpellSlot.Summoner1, 400);
        }
        public void start(EventArgs args)
        {
            setupMenu();
            setupSpell();

        }
        public void actives(EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Combo)
            {
                Combo();
            }
            if (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Harass)
            {
                Combo();
            } 
            if (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Flee)
            {
                Combo();
            }
        }
        public string[] MinionNames = 
        {
            "TT_Spiderboss",
            "SRU_Blue",
            "SRU_Red",
            "SRU_Baron"
        };
        public Obj_AI_Minion GetNearest(Vector3 pos)
        {
            var minions = ObjectManager.Get<Obj_AI_Minion>()
                .Where(minion => minion.IsValid && MinionNames.Any(name => minion.Name.StartsWith(name)) && !MinionNames.Any(name => minion.Name.Contains("Mini")) && MinionNames.Any(name => minion.Name.Contains("Spawn")));
            var objAiMinions = minions as Obj_AI_Minion[] ?? minions.ToArray();
            Obj_AI_Minion sMinion = objAiMinions.FirstOrDefault();
            double? nearest = null;
            foreach (Obj_AI_Minion minion in objAiMinions)
            {
                double distance = Vector3.Distance(pos, minion.Position);
                if (nearest == null || nearest > distance)
                {
                    nearest = distance;
                    sMinion = minion;
                }
            }
            return sMinion;
        }
        public void gameUpdate(EventArgs args)
        {

            if (ObjectManager.Player.Level > actualLevel)
            {
                actualLevel = ObjectManager.Player.Level;
                smiteDmg = smiteDMG[actualLevel - 1];
            }

            var mob = GetNearest(ObjectManager.Player.ServerPosition);
            if (mob != null)
            {
                if (smite.IsReady() && smiteDmg >= mob.Health && Vector3.Distance(ObjectManager.Player.ServerPosition, mob.ServerPosition) <= smite.Range)
                {
                    smite.Cast(mob);
                }
            }
        }
        public void Combo()
        {
            Boolean useQ = comboMenu["combo.Q"].Cast<CheckBox>().CurrentValue;
            Boolean useW = comboMenu["combo.W"].Cast<CheckBox>().CurrentValue;
            Boolean useE = comboMenu["combo.E"].Cast<CheckBox>().CurrentValue;
            Boolean useR = comboMenu["combo.R"].Cast<CheckBox>().CurrentValue;
            Boolean usesmite = comboMenu["combo.smite"].Cast<CheckBox>().CurrentValue;

            var target = TargetSelector.GetTarget(600, DamageType.Physical);
            if (usesmite && smite.IsReady())
            {
                smite.Cast(target);
            }
            if(E.IsReady() &&  E.IsInRange(target))
            {
                Q.Cast();
                W.Cast();
                E.Cast(target);
                smite.Cast(target);

            }
            if (useR && R.IsReady())
            {
                R.Cast();
            }
        }
        public void smiteMinion(Obj_AI_Minion minion)
        {
            smite.Cast(minion);
        }
             
    }
}
