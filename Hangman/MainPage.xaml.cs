namespace Hangman
{
    public partial class MainPage : ContentPage
    {
        //Initialize ViewModel
        private HangmanViewModel viewModel;

        public MainPage()
        {
            InitializeComponent();

            //Create and set ViewModel
            viewModel = new HangmanViewModel();
            BindingContext = viewModel;

            //Create letter buttons
            CreateLetterButtons();
        }

        //Create letter buttons dynamically
        private void CreateLetterButtons()
        {
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            foreach (char letter in alphabet)
            {
                var button = new ImageButton
                {
                    Source = $"{letter.ToString().ToLower()}.png",
                    WidthRequest = 40,
                    HeightRequest = 40,
                    Margin = new Thickness(3),
                    BackgroundColor = Colors.Transparent,
                    CornerRadius = 5,
                    BindingContext = letter
                };

                button.Clicked += OnLetterClicked;
                LettersContainer.Children.Add(button);
            }
        }

        //Handle letter button clicks
        private void OnLetterClicked(object sender, EventArgs e)
        {
            var button = (ImageButton)sender;
            char letter = (char)button.BindingContext;

            if (viewModel.IsLetterGuessed(letter))
                return;

            //Disable the button visually
            button.Opacity = 0.3;
            button.IsEnabled = false;

            // Call ViewModel method
            viewModel.OnLetterClicked(letter);
        }

        //Handle Play Again button click
        private void OnPlayAgainClicked(object sender, EventArgs e)
        {
            // Reset all letter buttons
            foreach (var child in LettersContainer.Children)
            {
                if (child is ImageButton button)
                {
                    button.IsEnabled = true;
                    button.Opacity = 1.0;
                }
            }

            // Call ViewModel method
            viewModel.OnPlayAgain();
        }

        //Handle Quit button click
        private void OnQuitClicked(object sender, EventArgs e)
        {
            viewModel.OnQuit();
        }
    }
}

