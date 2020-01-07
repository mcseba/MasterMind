using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MasterMind
{
    class GameInterface
    {
        public GameEngine gra = new GameEngine();

        public void UruchomRozgrywke()
        {
            OpisGry();

            Thread.Sleep(2000);

            Console.WriteLine("\n");

            Console.WriteLine("Komputer wygeneruje kod do odgadnięcia.");

            bool correct = false;

            while (!correct)
            {
                try
                {
                    Console.WriteLine("Podaj liczbę z ilu kolorów powinien się składać kod (min. 4 - poziom łatwy).");
                    Console.WriteLine("Długość kodu: ");
                    int length = Convert.ToInt32(Console.ReadLine());
                    Thread.Sleep(2000);

                    Console.WriteLine("Podaj pierwsze litery kolorów z jakich może składać się kod (min. 4), np: \"rygb\" (kolejno - red yellow green black).");
                    Console.WriteLine("Wprowadź kolory: ");
                    string kod = Console.ReadLine();

                    gra.GenerateCode(length, kod);
                    correct = true;

                    Console.WriteLine("Domyślna ilość Twoich ruchów w grze to 9. Czy chcesz to zmienić? (t/n)");
                    string s = Console.ReadLine();
                    if (s == "t")
                    {
                        ZmienLiczbeRuchow();
                    }

                    Console.WriteLine("Wygenerowano kod. Czy chcesz rozpocząć grę? (t/n)");
                    var wybor = Console.ReadLine();

                    if (wybor == "t")
                    {
                        Console.WriteLine();
                        Console.WriteLine("--------------------------------------");
                        Console.WriteLine();
                        Rozgrywka();
                    }
                    else
                    {
                        gra.StopGame();
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Podana wartość nie jest poprawna! Wprowadź prawidłowe dane.");
                    continue;
                }
                catch (Exception)
                {
                    Console.WriteLine("Wystąpił nieoczekiwany błąd! Spróbuj ponownie.");
                    continue;
                }
            }

        }
        
        public void Rozgrywka()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Czas na grę!");
            Console.WriteLine("Podaj kod o poprawnej długości, a komputer oceni jego poprawność.");


            while (gra.Status != GameEngine.StatusGry.Zakonczona)
            {
                if (gra.liczbaRuchow == 0)
                {
                    Przegrana();
                }

                Console.WriteLine();
                Console.WriteLine($"Zostało Ci {gra.liczbaRuchow} ruchów.");
                string odpowiedz = null;
                bool wartosc = false;

                while (!wartosc)
                {
                    try
                    {
                        Console.WriteLine("Twoja odpowiedz: ");
                        odpowiedz = Console.ReadLine();

                        if (odpowiedz == "Poddana")
                            Poddana();
                        wartosc = true;
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Wprowadzono złe dane!");
                        continue;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        Console.WriteLine("Coś poszło nie tak! Wprowadź poprawny kod.");
                        continue;
                    }
                    catch (StackOverflowException)
                    {
                        Console.WriteLine("Przesadziłeś! Spróbuj ponownie.");
                        continue;
                    }
                    catch(Exception)
                    {
                        Console.WriteLine("Wystąpił nieoczekiwany błąd! Spróbuj ponownie.");
                        continue;
                    }
                }

                gra.PlayerGuess(odpowiedz);

                gra.GetBlackPins();
                gra.GetWhitePins();

                gra.liczbaRuchow--;
            }

            Wygrana();
        }

        public void OpisGry()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("-----------------------------------GRA MASTERMIND-----------------------------------");
            Console.ResetColor();

            Console.WriteLine("\n");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Opis gry : Komputer losuje sekretny kod składający się z pierwszych liter kolorów (np. r - red, y - yellow, b - black)." + Environment.NewLine
                + "Twoim zadaniem jest odgadnąć ten kod. Masz ograniczoną liczbę ruchów." + Environment.NewLine
                + "Black pins - oznacza liczbę poprawnie trafionych kolorów w odpowiednim miejscu w sekretnym kodzie." + Environment.NewLine
                + "White pins - oznacza liczbę poprawnie trafionych kolorów ale kolory te nie są umieszczone w poprawnym miejscu." + Environment.NewLine
                + "Przykład: " + Environment.NewLine
                + "> komputer losuje sekretny kod 'brrg' (black red red green) z puli kolorów bwrg" + Environment.NewLine
                + "> Gracz wpisuje 'rbrw', co daje mu 1 Black Pin (poprawnie wpisana pozycja 3 w kodzie) i 2 White Pins (za kolory rb które są w złych miejscach)"
                + Environment.NewLine
                + "Możesz poddać grę w trakcie gry wpisując w odpowiedzi 'Poddana'.");
            
        }

        public void Wygrana()
        {
            Console.WriteLine("Brawo! Udało Ci się odgadnąć kod!");
            Console.WriteLine($"Czas trwania gry: {gra.CzasGry}");
            Console.WriteLine("Lista Twoich ruchów:");
            HistoriaGry();

            Console.WriteLine("Czy chcesz rozpocząć nową grę? (t/n)");
            string s = Console.ReadLine();

            switch (s)
            {
                case "t":
                    Console.Clear();
                    Thread.Sleep(1000);
                    UruchomRozgrywke();
                    break;
                case "n":
                    Console.WriteLine("DZIĘKUJĘ ZA GRĘ!");
                    Thread.Sleep(3000);
                    Environment.Exit(0);
                    break;
                default:
                    Environment.Exit(0);
                    break;
            }
        }

        public void Przegrana()
        {
            Console.WriteLine("Skończyły Ci się ruchy, przegrałeś.");
            gra.StopGame();
            HistoriaGry();
            Console.WriteLine("Czy chcesz rozpocząć nową grę? (t/n)");
            string s = Console.ReadLine();

            switch (s)
            {
                case "t":
                    Console.Clear();
                    Thread.Sleep(1000);
                    UruchomRozgrywke();
                    break;
                case "n":
                    Console.WriteLine("DZIĘKUJĘ ZA GRĘ!");
                    Thread.Sleep(3000);
                    Environment.Exit(0);
                    break;
                default:
                    Environment.Exit(0);
                    break;
            }
        }

        public void Poddana()
        {
            Console.WriteLine("Poddałeś grę.");
            gra.StopGame();
            HistoriaGry();
            Console.WriteLine("Czy chcesz rozpocząć nową grę? (t/n)");
            string s = Console.ReadLine();

            switch (s)
            {
                case "t":
                    Console.Clear();
                    Thread.Sleep(1000);
                    UruchomRozgrywke();
                    break;
                case "n":
                    Console.WriteLine("DZIĘKUJĘ ZA GRĘ!");
                    Thread.Sleep(3000);
                    Environment.Exit(0);
                    break;
                default:
                    Environment.Exit(0);
                    break;
            }
        }

        public void ZmienLiczbeRuchow()
        {
            Console.WriteLine("Wprowadz liczbe ruchow w grze: ");
            int moves = Convert.ToInt32(Console.ReadLine());
            
            gra.MoveNumber(moves);
        }

        public void HistoriaGry()
        {
            Console.WriteLine();
            Console.WriteLine("=============================================");
            Console.WriteLine("NR   Odp   Black    White    Czas");
            Console.WriteLine("=============================================");
            int i = 1;
            foreach (var ruch in gra.listaRuchow)
            {
                Console.WriteLine($"{i}     {ruch.playerguess}      {ruch.blackPin}  {ruch.whitePin}   {ruch.time}");
                i++;
            }
        }
    }
}
