#nullable enable
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Ardalis.SmartEnum;


namespace LarcApiNet.Model {
    
    
    /// <summary>
    /// Indicates type of action that is available
    /// for an ally of an opponent during champ select phase.
    /// </summary>
    public class ActionType : SmartEnum<ActionType> {
        
        /// <summary>
        /// 
        /// </summary>
        public static ActionType Pick = new ActionType("Pick", 1);
        
        /// <summary>
        /// 
        /// </summary>
        public static ActionType Ban = new ActionType("Ban", 2);
        
        public ActionType(string name, int value) : 
                base(name, value) {
        }
    }
}

#nullable restore