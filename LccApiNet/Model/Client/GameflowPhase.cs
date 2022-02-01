using Ardalis.SmartEnum;

namespace LccApiNet.Model.Client
{
    /// <summary>
    /// Indicates phase of the game flow in league client.
    /// </summary>
    public class GameflowPhase : SmartEnum<GameflowPhase>
    {
        public GameflowPhase(string name, int value): base(name, value) {}

        /// <summary>
        /// User wandering around client.
        /// </summary>
        public static readonly GameflowPhase None = new GameflowPhase("None", 1);

        /// <summary>
        /// User in the lobby.
        /// </summary>
        public static readonly GameflowPhase Lobby = new GameflowPhase("Lobby", 2);

        /// <summary>
        /// Matchmaking in progress.
        /// </summary>
        public static readonly GameflowPhase Matchmaking = new GameflowPhase("Matchmaking", 3);

        /// <summary>
        /// 
        /// </summary>
        public static readonly GameflowPhase CheckedIntoTournament = new GameflowPhase("CheckedIntoTournament", 4);

        /// <summary>
        /// User should accept or decline match.
        /// </summary>
        public static readonly GameflowPhase ReadyCheck = new GameflowPhase("ReadyCheck", 5);

        /// <summary>
        /// User selecting and/or banning champions.
        /// </summary>
        public static readonly GameflowPhase ChampSelect = new GameflowPhase("ChampSelect", 6);

        /// <summary>
        /// 
        /// </summary>
        public static readonly GameflowPhase GameStart = new GameflowPhase("GameStart", 7);
        
        /// <summary>
        /// 
        /// </summary>
        public static readonly GameflowPhase FailedToLaunch = new GameflowPhase("FailedToLaunch", 8);

        /// <summary>
        /// Game in progress.
        /// </summary>
        public static readonly GameflowPhase InProgress = new GameflowPhase("InProgress", 9);

        /// <summary>
        /// 
        /// </summary>
        public static readonly GameflowPhase Reconnect = new GameflowPhase("Reconnect", 10);

        /// <summary>
        /// Waiting for post game stats.
        /// </summary>
        public static readonly GameflowPhase WaitingForStats = new GameflowPhase("WaitingForStats", 11);

        /// <summary>
        /// When game is ended but user has not returned to the lobby yet.
        /// </summary>
        public static readonly GameflowPhase PreEndOfGame = new GameflowPhase("PreEndOfGame", 12);

        /// <summary>
        /// Game is ended.
        /// </summary>
        public static readonly GameflowPhase EndOfGame = new GameflowPhase("EndOfGame", 13);

        /// <summary>
        /// 
        /// </summary>
        public static readonly GameflowPhase TerminatedInError = new GameflowPhase("TerminatedInError ", 14);
    }
}
