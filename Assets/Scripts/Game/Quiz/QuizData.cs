using System.Collections.Generic;
using Game.Content.Main;

namespace Game.Quiz
{
    public class QuizData
    {
        public int score;
        public int attempts;

        public int currentWordHash;
        public List<int> usedWordsHash = new List<int>();
        public List<char> usedLetters = new List<char>();

        public void Reset(MainSettingsEntry entry)
        {
            score = 0;
            attempts = entry.defaultAttemptsCount;
            
            currentWordHash = -1;
            usedWordsHash.Clear();
            
            usedLetters.Clear();
        }
    }
}