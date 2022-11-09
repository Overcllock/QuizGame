namespace Game.Content.Main
{
    public class MainSettingsConverter : EntryConverter<MainSettingsScrobject, MainSettingsEntry>
    {
        protected override MainSettingsEntry SourceToSettings(MainSettingsScrobject source)
        {
            var entry = new MainSettingsEntry();

            entry.defaultAttemptsCount = source.defaultAttemptsCount;

            entry.minWordLength = source.minWordLength;
            entry.maxWordLength = source.maxWordLength;

            entry.wordsDatabase = source.wordsDatabase.ToArray();

            return entry;
        }
    }
}