using System;
using System.Collections.Generic;

namespace Game.Content.Main
{
    public class MainSettingsMap : ContentMap
    {
        public EntryMap<MainSettingsEntry> settings = new EntryMap<MainSettingsEntry>();
        
        protected override Dictionary<Type, EntryMap> GetMaps()
        {
            return new Dictionary<Type, EntryMap>
            {
                [typeof(MainSettingsEntry)] = settings
            };
        }
    }
}