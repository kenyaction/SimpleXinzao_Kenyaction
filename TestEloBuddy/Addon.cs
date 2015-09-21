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
        private Menu myMenu;
        private Menu comboMenu, smiteMenu;
        public void setupMenu ()
        {
            //string currentVersion = "1.0";
            Chat.Print("Test Menu loaded");
            Chat.Print("Checking version..");
            myMenu = MainMenu.AddMenu("Test Menu", "tittle");
            myMenu.AddLabel("Press nothing to see it");

            comboMenu = myMenu.AddSubMenu("Combo settings", "comboSection");
            comboMenu.AddGroupLabel("Cai dat");
            comboMenu.AddSeparator();

            comboMenu.Add("combo.Q", new CheckBox("Use Q"));
            comboMenu.Add("combo.W", new CheckBox("Use W"));
            comboMenu.Add("combo.R", new CheckBox("Use R"));
            comboMenu.Add("combo.E", new CheckBox("Use E"));

            smiteMenu = myMenu.AddSubMenu("Smite settings", "smiteSection");
            smiteMenu.AddSeparator();
            comboMenu.Add("smite.RED", new CheckBox("Smite RED"));
            comboMenu.Add("smite.BLUE", new CheckBox("smite BLUE"));
            comboMenu.Add("smite.DRAGON", new CheckBox("smite DRAGON"));
            comboMenu.Add("smite.PINKPENISH", new CheckBox("smite BARON"));

            //GameObject.OnCreate += castWinWard;
            //Game.OnTick += actives;
            //Game.OnUpdate += gameUpdate;
            
        }
        public void start(EventArgs args)
        {
            setupMenu();
            //setUp_spells();

        }
    }
}
