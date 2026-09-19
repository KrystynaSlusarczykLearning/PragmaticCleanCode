Console.WriteLine("Let's play Hangman! Enter a word to guess:");
string word = Console.ReadLine();
Console.Clear();
HangmanGame hangmanGame = new HangmanGame(word);
hangmanGame.Play();
Console.ReadKey();

class HangmanGame
{
    private string _wordToGuess;
    private HashSet<char> _guessedLetters = new HashSet<char>();
    private const int MaxAttempts = 6;

    public HangmanGame(string word)
    {
        _wordToGuess = word.ToLower();
    }

    public void Play()
    {
        var usedAttempts = 0;

        while (usedAttempts < MaxAttempts && !IsWordGuessed())
        {
            usedAttempts = AttemptToGuessLetter(usedAttempts);
        }

        PrintGameResult();
    }

    private void PrintGameResult()
    {
        PrintWord();

        if (IsWordGuessed())
        {
            Console.WriteLine(
                "Congratulations! You've won the game!");
        }
        else
        {
            Console.WriteLine(
                $"Game over :( The word was: {_wordToGuess}.");
        }

        Console.WriteLine("Press any key to close.");
    }

    private int AttemptToGuessLetter(int usedAttempts)
    {
        PrintGameStatus(usedAttempts);
        char currentGuess = GuessLetter();

        if (_guessedLetters.Contains(currentGuess))
        {
            Console.WriteLine("You've already guessed that letter. Try again.");
        }
        else
        {
            _guessedLetters.Add(currentGuess);
            if (!_wordToGuess.Contains(currentGuess))
            {
                usedAttempts++;
                Console.WriteLine("Wrong guess!");
            }
        }

        Console.WriteLine();
        return usedAttempts;
    }

    private void PrintGameStatus(int usedAttempts)
    {
        PrintWord();
        Console.WriteLine($"Attempts remaining: {MaxAttempts - usedAttempts}");
    }

    private static char GuessLetter()
    {
        Console.Write("Guess a letter: ");
        char currentGuess = ReadSingleChar();
        Console.WriteLine();
        return currentGuess;
    }

    private void PrintWord()
    {
        foreach (char letter in _wordToGuess)
        {
            if (_guessedLetters.Contains(letter))
            {
                Console.Write(letter + " ");
            }
            else
            {
                Console.Write("_ ");
            }
        }

        Console.WriteLine();
    }

    private bool IsWordGuessed()
    {
        foreach (char letter in _wordToGuess)
        {
            if (!_guessedLetters.Contains(letter))
            {
                return false;
            }
        }
        return true;
    }

    private static char ReadSingleChar()
    {
        return Console.ReadKey().KeyChar.ToString().ToLower()[0];
    }
}