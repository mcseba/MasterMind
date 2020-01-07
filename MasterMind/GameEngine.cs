using System;
using System.Collections.Generic;
using System.Text;

namespace MasterMind
{
    class GameEngine
    {
        /// <summary>
        /// This field holds the code.
        /// </summary>
        private char[] code = new char[4];

        /// <summary>
        /// This field keeps player guess.
        /// </summary>
        public string playerGuess;

        /// <summary>
        /// '_liczbaRuchow' holds the value for number of moves, default number of moves that player can make is 9.
        /// </summary>
        private int _liczbaRuchow = 9;

        /// <summary>
        /// 'black' field holds the number of colors player guessed correctly.
        /// </summary>
        private int black;

        /// <summary>
        /// 'white' field holds the number of colors that appear in code but are not in the correct position.
        /// </summary>
        private int white;

        public int liczbaRuchow
        {
            get => _liczbaRuchow;

            set => _liczbaRuchow = value;
        }

        /// <summary>
        /// wTrakcie - Status when game is in progress.
        /// Zakonczona - Games has ended.
        /// Poddana - Player surrendered the game.
        /// </summary>
        public enum StatusGry
        {
            wTrakcie,
            Zakonczona,
            Poddana
        }

        public StatusGry Status
        {
            get;
            private set;
        }

        public DateTime CzasRozpoczecia { get; private set; }
        public DateTime CzasZakonczenia { get; private set; }

        /// <summary>
        /// Timespan between game start and end.
        /// </summary>
        public TimeSpan CzasGry => CzasZakonczenia - CzasRozpoczecia;

        /// <summary>
        /// This list stores information about the game.
        /// </summary>
        public List<Ruch> listaRuchow = new List<Ruch>();

        /// <summary>
        /// Method for generating random code with given length of code and colors
        /// </summary>
        /// <param name="codelength">Length of the code</param>
        /// <param name="colors">Given colors</param>
        public void GenerateCode(int codelength, string colors)
        {
            Random rnd = new Random();

            for (int i = 0; i < codelength; i++)
            {
                int index = rnd.Next(colors.Length);
                code[i] = colors[index];
            }

            CzasRozpoczecia = DateTime.Now;
            Status = StatusGry.wTrakcie;
        }

        /// <summary>
        /// This method sets the number of moves that player can make.
        /// </summary>
        /// <param name="number">Represent number of moves that player can make in a single game.</param>
        public void MoveNumber(int number)
        {
            liczbaRuchow = number;
        }

        public void PlayerGuess (string odpowiedz)
        {
            if (odpowiedz.Length != this.code.Length)
                throw new ArgumentOutOfRangeException(odpowiedz, "Podany kod jest za krótki.");

            odpowiedz = odpowiedz.ToLower().Trim();

            playerGuess = odpowiedz;
        }

        /// <summary>
        /// Method returns the secret code but only when game has ended.
        /// </summary>
        public void ShowCode()
        {
            if (this.Status != StatusGry.wTrakcie)
            {
                foreach (var el in this.code)
                {
                    Console.Write(el);
                }
            }
            else
                Console.WriteLine("Nie możesz jeszcze podejrzeć kodu! Gra jest w trakcie rozgrywki.");
        }

        /// <summary>
        /// GetBlackPins returns how many black pins player could guess and stores it in 'black' field.
        /// </summary>
        public void GetBlackPins()
        {
            black = 0;

            for (int i = 0; i < code.Length; i++)
            {
                if (playerGuess[i] == this.code[i])
                {
                    black++;
                }
            }

            if (black == code.Length)
            {
                Status = StatusGry.Zakonczona;
                CzasZakonczenia = DateTime.Now;
                listaRuchow.Add(new Ruch(white, black, StatusGry.Zakonczona, playerGuess));
            }

            Console.WriteLine($"Black pins: {black}");
        }

        /// <summary>
        /// GetWhitePins method returns White pins that player could guess and stores it in 'white' field.
        /// </summary>
        public void GetWhitePins()
        {
            white = 0;

            char[] pomocniczyKod = new char[code.Length];
            char[] pomocniczyPlayerGuess = new char[code.Length];

            for (int i = 0; i < code.Length; i++)
            {
                pomocniczyKod[i] = code[i];
                pomocniczyPlayerGuess[i] = playerGuess[i];
            }

            for (int i = 0; i < code.Length; i++)
            {
                for (int j = 0; j < code.Length; j++)
                {
                    if (playerGuess[i] == pomocniczyKod[j])
                    {
                        pomocniczyKod[j] = '0';                 // Zapobiega ponownemu policzeniu danego koloru
                        pomocniczyPlayerGuess[i] = '0';         //
                        white++;
                    }
                }
            }
            white -= black;

            Console.WriteLine($"White pins: {white}");
            listaRuchow.Add(new Ruch(white, black, StatusGry.wTrakcie, playerGuess));
        }

        /// <summary>
        /// This method Stops the game if needed.
        /// </summary>
        public void StopGame()
        {
            if (Status == StatusGry.wTrakcie)
            {
                Status = StatusGry.Poddana;
                CzasZakonczenia = DateTime.Now;
                listaRuchow.Add(new Ruch(white, black, StatusGry.Poddana, null));
            }

            Console.Write($"Twój kod do odgadnięcia to: ");
            Console.ForegroundColor = ConsoleColor.Red;
            ShowCode();
            Console.ResetColor();
        }

        /// <summary>
        /// This class represents the way to store information about the game in the List<Ruch>.
        /// </summary>
        public class Ruch
        {
            public int whitePin { get; }
            public int blackPin { get; }
            public StatusGry status { get; }
            public string playerguess { get; }
            public DateTime time { get; }

            public Ruch(int whitepin, int blackpin, StatusGry status, string playerguess)
            {
                this.whitePin = whitepin;
                this.blackPin = blackpin;
                this.status = status;
                this.playerguess = playerguess;
                this.time = DateTime.Now;
            }

            public override string ToString()
            {
                return $"{playerguess}, black pin: {blackPin}, white pin: {whitePin}, {status}, {time}";
            }
        }

    }
}
