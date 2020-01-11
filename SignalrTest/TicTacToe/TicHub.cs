using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace TicTacToe
{
    public class TicHub : Hub
    {
        static Queue<string> signs = new Queue<string>();
        static string[,] GameBoard = new string[3, 3];
        static int Counter = 0;

        static List<Game> hostedGames = new List<Game>();

        static TicHub()
        {
            signs.Enqueue("X");
            signs.Enqueue("O");
        }
        public void PlayMove(int x, int y, string sign)
        {
            var gameOver = true;
            Counter++;
            GameBoard[x, y] = sign;
            var message = "";
            if (HasWon())
            {
                Clients.Caller.DrawPlayMove(x, y, sign, "You won..!", gameOver);
                Clients.AllExcept(Context.ConnectionId).DrawPlayMove(x, y, sign, "You lost..!", gameOver);
            }
            else if (Counter == 9)
            {
                Clients.All.DrawPlayMove(x, y, sign, "It's a draw..!", gameOver);
            }
            else
            {
                Clients.AllExcept(Context.ConnectionId).DrawPlayMove(x, y, sign, message);
            }
        }

        internal static void AddHost()
        {
            hostedGames.Add(new Game() { PlayerName = "Vinit" });
        }

        internal static List<Game> GetAvaialbleHosts()
        {
            if (hostedGames.Count == 0)
            {
                hostedGames.Add(new Game() {PlayerName = "Vinit"});
                hostedGames.Add(new Game() {PlayerName = "David"});
            }
            var retVal = hostedGames.Where(x => x.ConnectionId2 == null).ToList();
            return retVal;
        }
        public override System.Threading.Tasks.Task OnConnected()
        {
            ResetGame();//todo remove
            var sign = signs.Dequeue();
            signs.Enqueue(sign);
            Clients.Caller.Init(sign);
            return base.OnConnected();
        }
        public void ResetGame()
        {
            Counter = 0;
            for (int i = 0, k=0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++,k++)
                {
                    GameBoard[i, j] = k.ToString();
                }
            }
        }
        private bool HasWon()
        {
            for (int i = 0; i < 3; i++)
            {
                if (GameBoard[i, 0] == GameBoard[i, 1] && GameBoard[i, 1] == GameBoard[i, 2])
                    return true;
            }
            for (int i = 0; i < 3; i++)
            {
                if (GameBoard[0,i] == GameBoard[1,i] && GameBoard[1,i] == GameBoard[2,i])
                    return true;
            }
            if (GameBoard[0, 0] == GameBoard[1, 1] && GameBoard[1, 1] == GameBoard[2, 2])
                return true;
            if (GameBoard[0, 2] == GameBoard[1, 1] && GameBoard[1, 1] == GameBoard[2, 0])
                return true;

            return false;
        }
    }
    public class Game
    {
        public Game()
        {
            _gameId = Guid.NewGuid().ToString();
        }
        public string GameId { get { return _gameId; }}
        public string PlayerName { get; set; }
        public string ConnectionId1{ get; set; }
        public string ConnectionId2 { get; set; }
        public string PlaySign1 { get; set; }
        public string PlaySign2 { get; set; }
        public string[,] GameBoard = new string[3,3];
        private readonly string _gameId;
    }
}