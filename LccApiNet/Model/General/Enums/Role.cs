using Ardalis.SmartEnum;
using System;
using System.Collections.Generic;
using System.Text;

namespace LccApiNet.Model.General.Enums
{
    /// <summary>
    /// Enumeration of all available position team players 
    /// could be placed on during the game
    /// </summary>
    public class Role : SmartEnum<Role> {
        
        private Role(string name, int value) : base(name, value) { }

        /// <summary>
        /// Role of the top lane
        /// </summary>
        public static readonly Role Top = new Role("Top", 1);
       
        /// <summary>
        /// Role of the jungle
        /// </summary>
        public static readonly Role Jungle = new Role("Jungle", 2);
    
        /// <summary>
        /// Role of the mid lane
        /// </summary>
        public static readonly Role Mid = new Role("Mid", 3);
    
        /// <summary>
        /// Role of the bot lane
        /// </summary>
        public static readonly Role Bot = new Role("Bot", 4);
    
        /// <summary>
        /// Role of the support
        /// </summary>
        public static readonly Role Support = new Role("Support", 5);
    }
}
