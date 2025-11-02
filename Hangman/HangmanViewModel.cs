using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Hangman
{
    public class HangmanViewModel : INotifyPropertyChanged
    {
        //INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        //Game State
        private string currentWord;
        private string currentCategory;
        private List<char> guessedLetters;
        private int wrongGuesses;
        private const int maxWrongGuesses = 4;

        //Word Categories
        private Dictionary<string, List<string>> wordCategories = new Dictionary<string, List<string>>()
        {
            { "ANIMALS", new List<string> { "ELEPHANT", "GIRAFFE", "DOLPHIN", "PENGUIN", "CHEETAH", "KANGAROO" } },
            { "FRUITS", new List<string> { "BANANA", "ORANGE", "PINEAPPLE", "STRAWBERRY", "WATERMELON" } },
            { "COUNTRIES", new List<string> { "CANADA", "BRAZIL", "AUSTRALIA", "JAPAN", "FRANCE", "ITALY" } },
            { "SPORTS", new List<string> { "BASKETBALL", "FOOTBALL", "TENNIS", "SWIMMING", "BASEBALL" } }
        };

        // Bindable Properties
        private string categoryText;
        public string CategoryText
        {
            get => categoryText;
            set
            {
                categoryText = value;
                OnPropertyChanged();
            }
        }

        //Displayed word with underscores
        private string wordDisplay;
        public string WordDisplay
        {
            get => wordDisplay;
            set
            {
                wordDisplay = value;
                OnPropertyChanged();
            }
        }

        //Hangman image source
        private string hangmanImageSource;
        public string HangmanImageSource
        {
            get => hangmanImageSource;
            set
            {
                hangmanImageSource = value;
                OnPropertyChanged();
            }
        }

        //Control hangman.gif animation
        private bool isHangmanAnimationPlaying;
        public bool IsHangmanAnimationPlaying
        {
            get => isHangmanAnimationPlaying;
            set
            {
                isHangmanAnimationPlaying = value;
                OnPropertyChanged();
            }
        }

        //Stage indicators
        private string stage1Source;
        public string Stage1Source
        {
            get => stage1Source;
            set
            {
                stage1Source = value;
                OnPropertyChanged();
            }
        }

        //Stage indicators
        private string stage2Source;
        public string Stage2Source
        {
            get => stage2Source;
            set
            {
                stage2Source = value;
                OnPropertyChanged();
            }
        }

        //Stage indicators
        private string stage3Source;
        public string Stage3Source
        {
            get => stage3Source;
            set
            {
                stage3Source = value;
                OnPropertyChanged();
            }
        }

        //Stage indicators
        private string stage4Source;
        public string Stage4Source
        {
            get => stage4Source;
            set
            {
                stage4Source = value;
                OnPropertyChanged();
            }
        }

        //Win overlays
        private bool isWinOverlayVisible;
        public bool IsWinOverlayVisible
        {
            get => isWinOverlayVisible;
            set
            {
                isWinOverlayVisible = value;
                OnPropertyChanged();
            }
        }

        //Lose overlays
        private bool isLoseOverlayVisible;
        public bool IsLoseOverlayVisible
        {
            get => isLoseOverlayVisible;
            set
            {
                isLoseOverlayVisible = value;
                OnPropertyChanged();
            }
        }

        //Win message
        private string winMessage;
        public string WinMessage
        {
            get => winMessage;
            set
            {
                winMessage = value;
                OnPropertyChanged();
            }
        }

        //Correct word label for lose overlay
        private string correctWordLabel;
        public string CorrectWordLabel
        {
            get => correctWordLabel;
            set
            {
                correctWordLabel = value;
                OnPropertyChanged();
            }
        }

        //Constructor
        public HangmanViewModel()
        {
            StartNewGame();
        }

        //Start a new game
        public void StartNewGame()
        {
            guessedLetters = new List<char>();
            wrongGuesses = 0;

            //Select random category and word
            var random = new Random();
            var categoryIndex = random.Next(wordCategories.Count);
            var category = wordCategories.Keys.ElementAt(categoryIndex);
            currentCategory = category;

            var words = wordCategories[category];
            currentWord = words[random.Next(words.Count)];

            //Update UI
            CategoryText = currentCategory;
            UpdateWordDisplay();
            UpdateHangmanImage();
            UpdateStageDisplay();

            //Stop animation at start
            IsHangmanAnimationPlaying = false;

            //Hide overlays
            IsWinOverlayVisible = false;
            IsLoseOverlayVisible = false;
        }

        //Handle letter clicks
        public void OnLetterClicked(char letter)
        {
            if (guessedLetters.Contains(letter))
                return;

            guessedLetters.Add(letter);

            if (currentWord.Contains(letter))
            {
                //Correct guess
                UpdateWordDisplay();

                if (IsWordComplete())
                {
                    ShowWinMessage();
                }
            }
            else
            {
                //Wrong guess
                wrongGuesses++;
                UpdateHangmanImage();
                UpdateStageDisplay();

                if (wrongGuesses >= maxWrongGuesses)
                {
                    ShowLoseMessage();
                }
            }
        }

        //Update the displayed word with guessed letters
        private void UpdateWordDisplay()
        {
            string display = "";
            foreach (char letter in currentWord)
            {
                if (guessedLetters.Contains(letter))
                {
                    display += letter + " ";
                }
                else
                {
                    display += "_ ";
                }
            }
            WordDisplay = display.Trim();
        }

        //Update hangman image based on wrong guesses
        private void UpdateHangmanImage()
        {
            //hang.gif is always displayed
            HangmanImageSource = "hang.gif";
        }

        //Update stage indicators
        private void UpdateStageDisplay()
        {
            Stage1Source = wrongGuesses > 0 ? "wrong.png" : "nowrong.png";
            Stage2Source = wrongGuesses > 1 ? "wrong.png" : "nowrong.png";
            Stage3Source = wrongGuesses > 2 ? "wrong.png" : "nowrong.png";
            Stage4Source = wrongGuesses > 3 ? "wrong.png" : "nowrong.png";
        }

        //Check if the word is completely guessed
        private bool IsWordComplete()
        {
            return currentWord.All(letter => guessedLetters.Contains(letter));
        }

        //Check if a letter has been guessed
        public bool IsLetterGuessed(char letter)
        {
            return guessedLetters.Contains(letter);
        }

        //Show win message
        private void ShowWinMessage()
        {
            WinMessage = $"You guessed the word: {currentWord}!";
            IsWinOverlayVisible = true;
        }

        //Show lose message
        private async void ShowLoseMessage()
        {
            //Start the hang.gif animation
            IsHangmanAnimationPlaying = true;

            //Wait for animation to play (2 seconds)
            await Task.Delay(2000);

            //Stop the hang.gif animation
            IsHangmanAnimationPlaying = false;

            //Show lose overlay
            CorrectWordLabel = currentWord;
            IsLoseOverlayVisible = true;
        }

        //Handle Play Again button
        public void OnPlayAgain()
        {
            StartNewGame();
        }

        //Handle Quit button
        public void OnQuit()
        {
            Application.Current.Quit();
        }

        //Notify property changed
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
