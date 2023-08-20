using LoginServer.Cmd;
using PangyaAPI.SQL.DATA.Cmd;
using _smp = PangyaAPI.Utilities.Log;
using System;
using PangyaAPI.Utilities;
using PangyaAPI.SQL.DATA.TYPE;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using LoginServer.ServerTcp;
using LoginServer.Defines;
using PangyaAPI.SuperSocket.SocketBase;
using LoginServer.Session;

namespace LoginServer.PacketFunc
{

    public static class packet_func_ls
    {
        #region Call Packet

        public static void packet001(ParamDispatch _arg1)
        {
            try
            {
                Program.AppServer.requestLogin(_arg1._packet, _arg1._session as Player);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static void packet003(ParamDispatch _arg1)
        {
            string auth_key_game = "";
            var pd = (Player)_arg1._session;
            var _session = (Player)_arg1._session;
            var p = _arg1._packet;
            try
            {

                p.ReadInt32(out int server_uid);

                _smp.Message_Pool.push(("[packet_func::packet003][Log] Server UID: " + (server_uid)));


                // Registra o logon no server_uid do player_uid
                var cmd_rls = new CmdRegisterLogonServer(_session.m_pi.uid, server_uid);

                cmd_rls.waitEvent();

                if (cmd_rls.getException().getCodeError() != 0)
                    throw cmd_rls.getException();

                var cmd_auth_key_game = new CmdAuthKeyGame(_session.m_pi.uid, server_uid);

                cmd_auth_key_game.waitEvent();

                if (cmd_auth_key_game.getException().getCodeError() != 0)
                    throw cmd_auth_key_game.getException();

                auth_key_game = cmd_auth_key_game.getAuthKey();

                _smp.Message_Pool.push(("[packet_func::packet003][Log] AuthKeyGame: " + auth_key_game
                        + ", do player: " + (_session.m_pi.uid)));

            }
            catch (exception e)
            {

                _smp.Message_Pool.push(("[packet_func::packet003][ErrorSystem] " + e.getFullMessageError()));

                if (!(e.getCodeError() == STDA_ERROR_TYPE.EXEC_QUERY))
                    throw;
            }

            pacote003(ref p, _session, auth_key_game);
            session_send(p, _session, 1);
        }

        internal static void packet00B(ParamDispatch _arg1)
        {
            var ls = Program.AppServer;
            var _session = (Player)_arg1._session;
            try
            {

                ls.requestReLogin(_session, _arg1._packet);

            }
            catch (exception e)
            {

                _smp.Message_Pool.push("[packet_func::packet00B][ErrorSystem] " + e.getFullMessageError());

                if (e.getCodeError() != STDA_ERROR_TYPE.LOGIN_SERVER)
                    throw;
            }

        }

        internal static void packet008(ParamDispatch _arg1)
        {
            var pd = (Player)_arg1._session;
            var _session = (Player)_arg1._session;
            var p = _arg1._packet;
            try
            {

                uint _typeid = p.ReadUInt32();
                short default_hair = p.ReadUInt8();
                short default_shirts = p.ReadUInt8();

# if _DEBUG
                _smp.Message_Pool.push("[packet_func::packet008][Log] Character Type: " + (_typeid), _smp.type_msg.CL_FILE_LOG_AND_CONSOLE);
                _smp.Message_Pool.push("[packet_func::packet008][Log] Default hair: " + (default_hair), _smp.type_msg.CL_FILE_LOG_AND_CONSOLE);
                _smp.Message_Pool.push("[packet_func::packet008][Log] Default shirts: " + (default_shirts), _smp.type_msg.CL_FILE_LOG_AND_CONSOLE);
#else
                _smp.Message_Pool.push("[packet_func::packet008][Log] Character Type: " + (_typeid), _smp.type_msg.CL_ONLY_FILE_LOG);
                _smp.Message_Pool.push("[packet_func::packet008][Log] Default hair: " + (default_hair), _smp.type_msg.CL_ONLY_FILE_LOG);
                _smp.Message_Pool.push("[packet_func::packet008][Log] Default shirts: " + (default_shirts), _smp.type_msg.CL_ONLY_FILE_LOG);
#endif

                // Verifica se session está autorizada para executar esse ação, 
                // se ele não fez o login com o Server ele não pode fazer nada até que ele faça o login
                //CHECK_SESSION_IS_AUTHORIZED("packet008");

                //if (sIff::getInstance().findCharacter(_typeid) == null)
                //    throw new exception("[packet_func::packet008][Error] typeid character: " + (_typeid) + " is worng.", STDA_MAKE_ERROR(STDA_ERROR_TYPE::PACKET_FUNC_LS, 21, 0));

                if (default_hair > 9)
                    throw new exception("[packet_func::packet008][Error] default_hair: " + (default_hair) + " is wrong. character: " + (_typeid), STDA_ERROR_TYPE.PACKET_FUNC_SV);

                if (default_shirts != 0)
                    throw new exception("[packet_func::packet008][Error] default_shirts: " + (default_shirts) + " is wrong. character: " + (_typeid), STDA_ERROR_TYPE.PACKET_FUNC_LS);

                CharacterInfo ci = new CharacterInfo
                {
                    id = 0,
                    _typeid = _typeid,
                    default_hair = (byte)default_hair,
                    default_shirts = (byte)default_shirts
                };

                // Default Parts
                ci.initComboDef();

                var cmd_ac = new CmdAddCharacter(_session.m_pi.uid, ci, 0, 1);
                var cmd_afs = new CmdAddFirstSet(_session.m_pi.uid);


                cmd_ac.waitEvent();

                if (cmd_ac.getException().getCodeError() != 0)
                    throw cmd_ac.getException();

                // Info Character Add com o Id gerado no banco de dados
                ci = cmd_ac.getInfo();

                cmd_afs.waitEvent();

                if (cmd_afs.getException().getCodeError() != 0)
                    throw cmd_afs.getException();

                // Update Character Equipado no banco de dados
                var cmd_uce = new CmdUpdateCharacterEquip(_session.m_pi.uid, (int)ci.id);

                cmd_uce.waitEvent();

                if (cmd_uce.getException().getCodeError() != 0)
                    throw cmd_uce.getException();

                // concerta o character :)
                var cmd_ucf = new CmdFuncPartsCharacter(_session.m_pi.uid, (int)ci._typeid);

                cmd_ucf.waitEvent();

                if (cmd_ucf.getException().getCodeError() != 0)
                    throw cmd_ucf.getException();

                _smp.Message_Pool.push("[packet_func::packet008][Log] First Character Set with success! to player: " + (_session.m_pi.uid), _smp.type_msg.CL_FILE_LOG_AND_CONSOLE);

                // Ok
                pacote011(ref p, _session);
                session_send(p, _session, 1);

                // Success Login
                Program.AppServer.SUCCESS_LOGIN("packet008", _session);

            }
            catch (exception e)
            {
                // Erro na hora de salvar o character
                pacote011(ref p, _session, 1);
                session_send(p, _session, 1);
                pacote00E(ref p, _session, "", 12, 500051);
                session_send(p, _session, 1);

                _smp.Message_Pool.push("[packet_func::packet008][ErrorSystem] " + e.getFullMessageError(), _smp.type_msg.CL_FILE_LOG_AND_CONSOLE);
            }
        }

        internal static void packet007(ParamDispatch _arg1)
        {
            NICK_CHECK nc = NICK_CHECK.SUCCESS;
            uint error_info = 0;
            var p = _arg1._packet;
            var _session = (Player)_arg1._session;
            string wnick = "";
            try
            {

                wnick = p.ReadString();

                _smp.Message_Pool.push("[packet_func::packet007][Log] Check Nickname: " + wnick);

                // Verifica se session está autorizada para executar esse ação, 
                // se ele não fez o login com o Server ele não pode fazer nada até que ele faça o login

                if (_session.m_pi.id == wnick)
                {
                    nc = NICK_CHECK.SAME_NICK_USED;        // NICK igual ao ID, nao pode

                    _smp.Message_Pool.push("[packet_func::packet007][Error] O nick igual ao ID nao pode. nick: "
                            + wnick + " Player: " + (_session.m_pi.uid));
                }

                // Pavras que não pode usar
                if (nc == NICK_CHECK.SUCCESS && !Convert.ToBoolean(_session.m_pi.m_cap & 4) && System.Text.RegularExpressions.Regex.Match(wnick, "(.*GM.*)|(.*ADM.*)", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Success)
                {
                    nc = NICK_CHECK.HAVE_BAD_WORD;

                    _smp.Message_Pool.push("[packet_func::packet007][Error] O nick contem palavras inapropriadas: "
                            + wnick + " Player: " + (_session.m_pi.uid));
                }

                if (nc == NICK_CHECK.SUCCESS && System.Text.RegularExpressions.Regex.Match(wnick, ".*[ ].*").Success)
                {
                    nc = NICK_CHECK.EMPETY_ERROR;

                    _smp.Message_Pool.push("[packet_func::packet007][Error] O nick contem espaco em branco: "
                            + wnick + " Player: " + (_session.m_pi.uid));
                }

                if (nc == NICK_CHECK.SUCCESS && wnick.size() < 4 || System.Text.RegularExpressions.Regex.Match(wnick, ".*[\\^$&,\\?`´~\\|\"@#¨'%*!\\\\].*").Success)
                {
                    nc = NICK_CHECK.INCORRECT_NICK;

                    _smp.Message_Pool.push("[packet_func::packet007][Error] O nick eh menor que 4 letras ou tem caracteres que nao pode: "
                            + wnick + " Player: " + (_session.m_pi.uid));
                }

                if (nc == NICK_CHECK.SUCCESS)
                {
                    var cmd_vn = new CmdVerifyNick(wnick);

                    cmd_vn.waitEvent();

                    if (cmd_vn.getException().getCodeError() != 0)
                        throw cmd_vn.getException();

                    if (cmd_vn.getLastCheck())
                    {
                        nc = NICK_CHECK.NICK_IN_USE;

                        _smp.Message_Pool.push("[packet_func::packet007][Error] O nick ja esta em uso: "
                                + wnick + " Player: " + (_session.m_pi.uid));
                    }
                }

            }
            catch (exception e)
            {

                _smp.Message_Pool.push("[packet_func::packet007][ErrorSystem] " + e.getFullMessageError());

                if ((e.getCodeError()) == STDA_ERROR_TYPE.PANGYA_DB)
                    nc = NICK_CHECK.ERROR_DB;
                else
                    nc = NICK_CHECK.UNKNOWN_ERROR;

            }
            catch (Exception e)
            {

                _smp.Message_Pool.push("[packet_func::packet007][ErrorSystem] " + e.Message);

                nc = NICK_CHECK.UNKNOWN_ERROR;
            }

            pacote00E(ref p, _session, wnick, (int)nc, error_info);
            session_send(p, _session, 1);
        }

        internal static void packet006(ParamDispatch _arg1)
        {
            var wnick = "";
            var p = _arg1._packet;
            var _session = (Player)_arg1._session;
            var ls = Program.AppServer;
            try
            {
                wnick = p.ReadString();

                _smp.Message_Pool.push("[packet_func::packet006][Log] Save Nickname: " + wnick);

                // Verifica se session está autorizada para executar esse ação, 
                // se ele não fez o login com o Server ele não pode fazer nada até que ele faça o login
                //CHECK_SESSION_IS_AUTHORIZED("packet006");

                var cmd_sn = new CmdSaveNick(_session.m_pi.uid, wnick);

                cmd_sn.waitEvent();

                if (cmd_sn.getException().getCodeError() != 0)
                    throw cmd_sn.getException();

                var cmd_afl = new CmdAddFirstLogin(_session.m_pi.uid, 1);

                cmd_afl.waitEvent();

                if (cmd_afl.getException().getCodeError() != 0)
                    throw cmd_afl.getException();

                _smp.Message_Pool.push("[packet_func::packet006][Log] salvou o nick: " + wnick + ", do player: "
                        + _session.m_pi.uid + " com sucesso.");

                // Aqui colocar para verificar se ele já fez o first set, se não envia o pacote do first set, se não success_login
                var cmd_fsc = new CmdFirstSetCheck(_session.m_pi.uid, true/*Waiter*/);

                cmd_fsc.waitEvent();

                if (cmd_fsc.getException().getCodeError() != 0)
                    throw cmd_fsc.getException();

                if (!cmd_fsc.getLastCheck())
                {   // Verifica se fez o primeiro set do character

                    // FIRST_SET
                    p = pacote001(_session, 0xD9);
                    session_send(p, _session, 1);

                    _smp.Message_Pool.push("[packet_func::packet006][Log] Primeira vez que o player escolhe um character padrao. player[UID="
                            + _session.m_pi.uid + ", ID=" + _session.m_pi.id + "]");

                }
                else
                    ls.SUCCESS_LOGIN("packet006", _session);

            }
            catch (exception e)
            {

                pacote00E(ref p, _session, wnick, 1/*UNKNOWN ERROR*/);
                session_send(p, _session, 1);

                _smp.Message_Pool.push("[packet_func::packet006][ErrorSystem] " + e.getFullMessageError());
            }

        }

        public static void packet004(ParamDispatch _arg1)
        {
            var ls = Program.AppServer;
            var pd = (Player)_arg1._session;
            var _session = (Player)_arg1._session;


            try
            {
                ls.requestDownPlayerOnGameServer(_session, _arg1._packet);

            }
            catch (exception e)
            {

                var p = _arg1._packet;
                pacote00E(ref p, _session, "", 12, 500053);
                session_send(p, _session, 1);

                _smp.Message_Pool.push(("[packet_func::packet004][Error] " + e.getFullMessageError()));
            }

        }


        #endregion

        #region Response Packet
        public static Packet pacote001(Player _session, byte option = 0)
        {
            Packet p = new Packet();

            p.init_plain(0x001);

            p.AddByte(option);  // OPTION 1 SENHA OU ID ERRADO

            if (option == 0)
            {
                p.AddString(_session.m_pi.id);
                p.AddInt32(_session.m_pi.uid);
                p.AddInt32(_session.m_pi.m_cap);
                p.AddInt16(_session.m_pi.level);           // 1 level, 1 pc bang(ACHO), com base no S4
                p.AddInt32(0);                              // valor 0 Unknown
                p.AddInt32(5);                              // valor 5 Unknown
                p.AddBuffer(new _SYSTEMTIME(true), 19);	// Time Build Login Server (ACHO)							- JP S9 ler mais ignora ele
                p.AddZeroByte(3);   // Time Build Login Server (ACHO)							- JP S9 ler mais ignora ele
                p.AddString("302540");                      // Alguma AuthKey aleatória para minha conta que eu não sei - JP S9 ler mais ignora ele
                p.AddInt32(0);                             // Unknown valor - JP S9 ler mais ignora ele
                p.AddInt32(0);                             // Unknown valor - JP S9 ler mais ignora ele
                p.AddString(_session.m_pi.nickname);
                p.AddInt16(0);
            }
            else if (option == 1)
                p.AddInt32(0);  // add 4 bytes vazios
            else if (option == 0xD8)
            {       // First Login
                p.AddInt32(-1);
                p.AddInt16(0);
            }
            else if (option == 0xD9)        // First Set
                p.AddInt16(0);

            return p;
        }

        public static void session_send(Packet p, Player s, int _debug)
        {
            if (s == null)
                throw new exception("[packet_func::session_send][Error] session *s is nullptr.", STDA_ERROR_TYPE.PACKET_FUNC_LS);

            var mb = p.GetPlainBuf();

            s.Send(mb.Buffer,0, (int)mb.Length);

            #if _RELEASE
            if(_debug == 1)
            {
                //_smp.Message_Pool.push($"[SEND_PACKET_LOG]: PacketSize({p.GetBytes.Length}) \t\n{p.GetBytes.HexDump()}");
            }
            #endif
        }
        public static void pacote00F(ref Packet p, Player _session, int option = 1)
        {
            p.init_plain((short)0x0F);

            p.AddByte((byte)option);

            p.AddString(_session.m_pi.id);

            p.AddInt32(0);                             // valor 0 Unknown
            p.AddInt32(5);                             // valor 5 Unknown
            p.AddBuffer(new _SYSTEMTIME(true), 19);   // Time Build Login Server (ACHO)								- JP S9 ler mais ignora ele
            p.AddZeroByte(3);   // Time Build Login Server (ACHO)							- JP S9 ler mais ignora ele
            p.AddString("302540");                      // Alguma AuthKey aleatória para minha conta que eu não sei		- JP S9 ler mais ignora ele
        }

        public static void succes_login(LoginServerTcp ls, Player _session, int option = 0)
        {

            List<ServerInfo> sis = new List<ServerInfo>();
            List<ServerInfo> msns = new List<ServerInfo>();
            chat_macro_user _cmu = new chat_macro_user().Init();
            string auth_key_login = "";

            /* OPTION
			*  0 PRIMEIRO LOGIN
			*  1 RELOGA DEPOIS QUE CAIU DO GAME SERVER, COM A AUTH KEY
			*/

            try
            {

                var cmd_server_list = new CmdServerList(TYPE_SERVER.GAME);


                cmd_server_list.waitEvent();

                if (cmd_server_list.getException().getCodeError() != 0)
                    throw cmd_server_list.getException();

                sis = cmd_server_list.getServerList();

                cmd_server_list = new CmdServerList(TYPE_SERVER.MSN);

                var cmd_auth_key_login = new CmdAuthKeyLogin(_session.m_pi.uid);

                cmd_server_list.waitEvent();

                if (cmd_server_list.getException().getCodeError() != 0)
                    throw cmd_server_list.getException();

                msns = cmd_server_list.getServerList();

                cmd_auth_key_login.waitEvent();

                if (cmd_auth_key_login.getException().getCodeError() != 0)
                    throw cmd_auth_key_login.getException();

                auth_key_login = cmd_auth_key_login.getAuthKey();

                _smp.Message_Pool.push("[packet_func::succes_login][Log] AuthKeyLogin: " + auth_key_login
                        + ", do player: " + (_session.m_pi.uid));

                if (option == 0)
                {
                    var cmd_macro_user = new CmdChatMacroUser(_session.m_pi.uid);


                    cmd_macro_user.waitEvent();

                    if (cmd_macro_user.getException().getCodeError() != 0)
                        throw cmd_macro_user.getException();

                    _cmu = cmd_macro_user.getMacroUser();
                }

                var ip = (_session.m_ip);

                // RegisterLogin do Player
                var cmd_rpl = new CmdRegisterPlayerLogin(_session.m_pi.uid, ip, ls.m_si.UID);


                cmd_rpl.waitEvent();

                if (cmd_rpl.getException().getCodeError() != 0)
                    throw cmd_rpl.getException();

            }
            catch (exception e)
            {

                _smp.Message_Pool.push("[packet_func::succes_login][ErrorSystem] " + e.getFullMessageError());

                if ((e.getCodeError()) == STDA_ERROR_TYPE.EXEC_QUERY)
                {
                    if ((e.getCodeError()) != (STDA_ERROR_TYPE)7/*getServerList*/ && (e.getCodeError()) != (STDA_ERROR_TYPE)9/*MacroUser*/
                        && (e.getCodeError()) != (STDA_ERROR_TYPE)8/*getMsnList*/ && (e.getCodeError()) != (STDA_ERROR_TYPE)5/*AuthKey*/)
                        throw;
                }
                else
                    throw;
            }
            var p = new Packet();
            pacote010(ref p, _session, auth_key_login);
            session_send(p, _session, 1);

            if (option == 0)
            {
                p = pacote001(_session);
                session_send(p, _session, 1);
            }

            pacote002(ref p, _session, sis);
            session_send(p, _session, 1);

            pacote009(ref p, _session, msns);
            session_send(p, _session, 1);

            if (option == 0)
            {
                pacote006(ref p, _session, _cmu);
                session_send(p, _session, 1);
            }
        }

        private static void pacote006(ref Packet p, Player session, chat_macro_user _mu)
        {
            p.init_plain((short)0x006);
            for (int i = 0; i < _mu.macro.Length; i++)
            {
                p.AddFixedString(_mu.macro[i], 64);
            }
        }

        private static void pacote009(ref Packet p, Player session, List<ServerInfo> v_element)
        {
            p.init_plain(0x009);

            p.AddByte((byte)(v_element.Count & 0xFF)); // nenhum Msn Server on

            for (var i = 0; i < v_element.Count; i++)
                p.AddBuffer(v_element[i], Marshal.SizeOf(new ServerInfo()));
        }

        private static void pacote002(ref Packet p, Player session, List<ServerInfo> v_element)
        {
            p.init_plain((short)0x002);

            p.AddByte((byte)(v_element.Count & 0xFF)); // nenhum GS Server on

            for (var i = 0; i < v_element.Count; i++)
                p.AddBuffer(v_element[i]);
        }

        private static void pacote010(ref Packet p, Player session, string AuthKey)
        {
            p.init_plain(0x10);

            p.AddString(AuthKey);
        }

        private static void pacote003(ref Packet p, Player session, string AuthKeyLogin, int option = 0)
        {
            p.init_plain(0x003);
            p.AddInt32(option);

            p.AddString(AuthKeyLogin);
        }

        private static void pacote011(ref Packet p, Player session, short option = 0)
        {
            p.init_plain(0x11);
            p.AddInt16(option);
        }

        public static void pacote00E(ref Packet p, Player session, string nick, int option, uint error = 0)
        {
            p.init_plain(0x00E);

            p.AddInt32(option);

            if (option == 0)
                p.AddString(nick);
            else if (option == 12)
                p.AddUInt32(error);
        }

        #endregion

    }
}
