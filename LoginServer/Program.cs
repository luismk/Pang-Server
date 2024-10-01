using System;
using LoginServer.ServerTcp;
using LoginServer.Session;
namespace LoginServer
{
    internal class Program
    {
        public static LoginServerTcp AppServer;
        static void Main(string[] args)
        {
            AppServer = new LoginServerTcp();//chama a class com servidor imbutido
            if (AppServer.StartingServer())           //inicializa o server
            {
                AppServer.NewSessionConnected += Handle_NewSessionConnected;
            }
            //faz um laço para o servidor fica sempre correndo
            while (true)
            {
                System.Threading.Thread.Sleep(50);
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadLine();
                    if (key == "server stop")
                    {
                        Console.WriteLine("Server Terminate ~~~");
                        AppServer.Stop();//faço o servidor parar de rodar ou simplesmente não ira mais receber conexao!
                        break;
                    }
                    if (key == "server restart")
                    {
                        Console.WriteLine("Server Restart ~~~");
                        AppServer.Restart();//faço o servidor parar de rodar ou simplesmente não ira mais receber conexao!
                        break;

                    }
                    if (key == "server on")
                    {
                        Console.WriteLine("Server Accept players ~~~");
                        AppServer.setIsUnderMaintenance(true);//faço o servidor parar de rodar ou simplesmente não ira mais receber conexao!
                        break;
                    }
                    if (key == "server off")
                    {
                        Console.WriteLine("Server close players ~~~");
                        AppServer.setIsUnderMaintenance(false);//faço o servidor parar de rodar ou simplesmente não ira mais receber conexao!
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// metodo que ira lidar com conexao do player recebida
        /// </summary>
        /// <param name="session">Jogador ou conexao</param>
        /// <exception cref="NotImplementedException">@! não implementado(ainda)</exception>
        private static void Handle_NewSessionConnected(Player session)
        {
            //faça uma implementação aqui
        }
    }
}
