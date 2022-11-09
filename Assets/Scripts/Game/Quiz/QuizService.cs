using System;
using System.Collections.Generic;
using Game.Content.Main;
using Random = UnityEngine.Random;

namespace Game.Quiz
{
    public class QuizService
    {
        private QuizData _data;
        private MainSettingsEntry _entry;

        private Dictionary<int, string> _wordsDatabase;

        public Action wordGenerated;
        public Action<string> wordCompleted;

        public Action<char> letterUsed;
        
        public Action<bool> quizEnded;

        public Action<QuizData> dataChanged;

        public QuizService(string settingsId)
        {
            _data = new QuizData();

            _entry = ContentManager.GetEntry<MainSettingsEntry>(settingsId);
            
            CreateWordsDatabase();
            
            Reset();
        }

        public void Reset()
        {
            _data.Reset(_entry);
            dataChanged?.Invoke(_data);
            
            TryGenerateWord();
        }

        public QuizData GetData()
        {
            return _data;
        }

        public string GetWord(int wordHash)
        {
            if (_wordsDatabase.TryGetValue(wordHash, out var word))
                return word;
            
            return string.Empty;
        }

        public void UseLetter(char letter)
        {
            if (_data.usedLetters.Contains(letter))
                return;

            _data.usedLetters.Add(letter);
            
            letterUsed?.Invoke(letter);

            if (!IsWordContainsLetter(_data.currentWordHash, letter))
            {
                _data.attempts--;
                dataChanged?.Invoke(_data);
                
                if (_data.attempts < 0)
                {
                    quizEnded?.Invoke(false);
                    return;
                }
            }
            
            dataChanged?.Invoke(_data);

            if (IsWordCompleted(_data.currentWordHash))
            {
                CompleteWord(_data.currentWordHash);
            }
        }

        private bool TryGenerateWord()
        {
            if (_wordsDatabase == null || _wordsDatabase.Count == 0)
                return false;

            var availableWords = new List<int>();
            foreach (var wordHash in _wordsDatabase.Keys)
            {
                if (_data.usedWordsHash.Contains(wordHash))
                    continue;
                
                availableWords.Add(wordHash);
            }

            if (availableWords.Count == 0)
                return false;

            var index = Random.Range(0, availableWords.Count);
            var generatedWordHash = availableWords[index];
            
            _data.currentWordHash = generatedWordHash;
            _data.usedWordsHash.Add(generatedWordHash);
            
            dataChanged?.Invoke(_data);
            
            wordGenerated?.Invoke();

            return true;
        }

        public bool IsWordContainsLetter(int wordHash, char letter)
        {
            if (_wordsDatabase.TryGetValue(wordHash, out var word))
            {
                for (int i = 0; i < word.Length; i++)
                {
                    if (word[i] == letter)
                        return true;
                }
            }

            return false;
        }

        private bool IsWordCompleted(int wordHash)
        {
            if (_wordsDatabase.TryGetValue(wordHash, out var word))
            {
                for (int i = 0; i < word.Length; i++)
                {
                    var letter = word[i];
                    if (!_data.usedLetters.Contains(letter))
                        return false;
                }

                return true;
            }

            return false;
        }

        private void CompleteWord(int wordHash)
        {
            if (!_wordsDatabase.TryGetValue(wordHash, out var word))
                return;

            _data.score += _data.attempts;
            _data.attempts = _entry.defaultAttemptsCount;
            _data.usedLetters.Clear();
            
            wordCompleted?.Invoke(word);
            dataChanged?.Invoke(_data);

            if (!TryGenerateWord())
            {
                quizEnded?.Invoke(true);
            }
        }

        private void CreateWordsDatabase()
        {
            _wordsDatabase = new Dictionary<int, string>();

            foreach (var word in _entry.wordsDatabase)
            {
                _wordsDatabase.Add(StringUtility.StringToHash(word), word);
            }
        }
    }
}