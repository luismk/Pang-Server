using GameServer.ServerTcp;

namespace GameServer
{
    class Program
	{
		public static GameServerTcp server;
		static void Main(string[] args)
		{
			server = new GameServerTcp();
			// Start the server
			if (server.StartingServer())
			{
			}
           
            for (; ; ) { server.checkCommand(new string[] {}); }
		}
	}
}
