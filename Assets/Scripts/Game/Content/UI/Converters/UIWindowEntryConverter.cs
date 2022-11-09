namespace Game.Content.UI
{
    public class UIWindowEntryConverter : EntryConverter<UIWindowSettingsScrobject, UIWindowEntry>
    {
        protected override UIWindowEntry SourceToSettings(UIWindowSettingsScrobject source)
        {
            UIWindowEntry entry = new UIWindowEntry();

            entry.priority = source.priority;
            entry.prefabReference = source.prefabReference;

            entry.useFader = source.useFader;
            entry.faderDuration = source.faderDuration;
            entry.faderDelay = source.faderDelay;

            return entry;
        }
    }
}