using System.Collections.Specialized;

namespace GameOfLife
{
    using System.Text;
    using System.Timers;

    internal class Program
    {
        static bool[,] board;
        public const int RIGHE = 20;
        public const int COLONNE = 40;

        static void Main(string[] args)
        {
            string gliderGun = @"
-------------------------X----------
----------------------XXXX----X-----
-------------X-------XXXX-----X-----
------------X-X------X--X---------XX
-----------X---XX----XXXX---------XX
XX---------X---XX-----XXXX----------
XX---------X---XX--------X----------
------------X-X---------------------
-------------X----------------------";
            board = Convert(gliderGun);
            Draw(board);

            //while (true)
            //{
            //    System.Threading.Thread.Sleep(300)
            //    board = Calculate(board);
            //    Draw(board);
            //}

            // System.Timers;
            var timer = new Timer(50);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
            Console.ReadLine();
        }

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            board = Calculate(board);
            Draw(board);
        }

        static void Draw(bool[,] list) // Funzione per stampare a schermo spazi vuoti (morte) e carattere pieno (vive) 
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(0, 0);
            StringBuilder sb = new StringBuilder(); //ad inserimento finito la stringa sb risulta essere --> sb["█"," "]
            sb.Append("█");
            sb.Append(" ");
            sb.AppendLine();

            // Stampare la lista, se true sb[0], dove false sb[1]
            for (int righe = 0; righe < RIGHE; righe++) // Controllo ogni colonna di ogni riga
            {
                for (int colonne = 0; colonne < COLONNE; colonne++)
                {
                    if (list[righe, colonne]) // se cella puntata è true (viva)
                    {
                        Console.Write(sb[0]); // inserisco carattere pieno (sb[0])
                    }
                    else // se cella puntata false (morta)
                    {
                        Console.Write(sb[1]); // inserisco spazio vuoto (sb[1])
                    }
                }

                // mando a capo quando si raggiunge la fine della riga
                Console.Write("\n");
            }
        } 

        static bool[,] Calculate(bool[,] list) // Funzione per calcolare la successiva generazione in base alle 4 regole 
        {
            var newBoard = new bool[list.GetLength(0), list.GetLength(1)];
            
            // REGOLA 1: Qualsiasi cella viva con meno di due celle vive adiacenti muore, come per effetto d'isolamento;
            // REGOLA 2: Qualsiasi cella viva con due o tre celle vive adiacenti sopravvive alla generazione successiva;
            // REGOLA 3: Qualsiasi cella viva con più di tre celle vive adiacenti muore, come per effetto di sovrappopolazione;
            // REGOLA 4: Qualsiasi cella morta con esattamente tre celle vive adiacenti diventa una cella viva, come per effetto di riproduzione.
            
            for (int righe = 0; righe < RIGHE; righe++)
            {
                for (int colonne = 0; colonne < COLONNE; colonne++)
                {
                    // controllare celle senza andare fuori dall'array
                    // conto numero caselle vive adiacenti
                    int count = 0;
                    
                    // terminati i 2 for so quante caselle a true ho vicino
                    for (int r = -1; r <=1 ; r++)
                    {
                        for (int c = -1; c <= 1; c++)
                        {
                            if (r == 0 && c == 0)
                            {
                                continue; //se guardo la casella di partenza skippo
                            }
                            // controllare valore della cella e che sia dentro ai limiti
                            if ((righe + r) != -1 && colonne + c != -1 && (righe + r) < 20 && (colonne + c) < 40)
                            {
                                if (list[righe + r, colonne + c])
                                {
                                    count++;
                                }
                            }
                        }
                    }
                    //dentro count ho numero caselle true vicine a quella in cui mi trovo
                    
                    // implementazione delle 4 regole del gioco sulle quasi si basa la modifica delle caselle
                    if (list[righe, colonne]) // verifico che la cella puntata sia viva
                    {
                        if (count < 2) // se meno di due celle vive vicine: cella muore per isolamento
                        {
                            newBoard[righe, colonne] = false;
                        }
                        else if (count == 2 || count == 3) // se 2 o 3 celle vive adiacenti: cella sopravvive a generazione
                        {
                            newBoard[righe, colonne] = true;
                        }
                        else if (count > 3) // se più di 3 celle vive vicine: cella muore per sovrappopolazione
                        {
                            newBoard[righe, colonne] = false;
                        }
                    }
                    else // caso in cui la cella sia morta
                    {
                        if (count == 3) // se più di 3 celle vive adiacenti: cella viva per riproduzione
                        {
                            newBoard[righe, colonne] = true;
                        }
                    }
                }
            }

            return newBoard;
        } 

        static bool[,] Convert(string pattern) // Funzione per impostare a vive o morte le celle in base al carattere in input 
        {
            var newBoard = new bool[RIGHE, COLONNE];
            string[] lines = pattern.Split('\n');

            int righe = 0;
            int colonne = 0;


            foreach (Char c in pattern)
            {
                if (c == '-')
                {
                    newBoard[colonne, righe++] = false;
                }
                else if (c == 'X')
                {
                    newBoard[colonne, righe++] = true;
                }
                else if (c == '\n')
                {
                    colonne++;
                    righe = 0;
                }
            }


            return newBoard;
        } 
    }
}