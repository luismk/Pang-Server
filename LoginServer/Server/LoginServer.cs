using PangyaAPI.Utilities;
using System;
using _smp = PangyaAPI.Utilities.Log;
using LoginServer.TYPE;
using PangyaAPI.SQL.DATA.Cmd;
using LoginServer.Cmd;
using PangyaAPI.SuperSocket.SocketBase;
using LoginServer.Session;
using PangyaAPI.SuperSocket.Engine;
using LoginServer.PacketFunc;

namespace LoginServer.ServerTcp
{
    public class LoginServerTcp : PangyaServer<Player>
    {
        public int m_access_flag { get; private set; }
        public int m_create_user_flag { get; private set; }
        public int m_same_id_login_flag { get; private set; }
        public LoginServerTcp() 
        {
            addPacketCall(0x01, new Action<ParamDispatch>(packet_func_ls.packet001));
            addPacketCall(0x03, new Action<ParamDispatch>(packet_func_ls.packet003));
            addPacketCall(0x04, new Action<ParamDispatch>(packet_func_ls.packet004));
            addPacketCall(0x06, new Action<ParamDispatch>(packet_func_ls.packet006));
            addPacketCall(0x07, new Action<ParamDispatch>(packet_func_ls.packet007));
            addPacketCall(0x08, new Action<ParamDispatch>(packet_func_ls.packet008));
            addPacketCall(0x0B, new Action<ParamDispatch>(packet_func_ls.packet00B));
        }
        protected override void OnStarted()
        {
            _smp.Message_Pool.push("[Server.OnStarted][Log]: Server starting...", _smp.type_msg.CL_FILE_LOG_AND_CONSOLE);
            base.OnStarted();
        }

        public override void ConfigInit()
        {
            base.ConfigInit();
            // Server Tipo
            m_si.Tipo = 0/*Login Server*/;
            m_access_flag = Ini.ReadInt32("OPTION", "ACCESSFLAG", 0);
            m_create_user_flag = Ini.ReadInt32("OPTION", "CREATEUSER", 0);

            try
            {
                m_same_id_login_flag = Ini.ReadInt32("OPTION", "SAME_ID_LOGIN", 0);
            }
            catch (Exception e)
            {
                // Não precisa printar mensagem por que essa opção é de desenvolvimento
            }

            //if (ConnectToAuthServer(AuthServerConstructor()) == false)
            //{
            //    Console.WriteLine("[ERROR_START_AUTH]: Não foi possível se conectar ao AuthServer");
            //    Console.ReadKey();
            //    Environment.Exit(1);
            //}
        }



        public bool getAccessFlag()
        {
            return m_access_flag == 1;
        }

        public bool getCreateUserFlag()
        {
            return m_create_user_flag == 1;
        }

        public bool canSameIDLogin()
        {
            return m_same_id_login_flag == 1;
        }
        public void requestLogin(Packet p, Player _session)
        {

            /// Pacote01 Option 0x0F(15) é manutenção

            try
            {


                // Ler dados do packet de login
                var result = new LoginData();
                
                p.ReadObject(@result);

                //  Verify Id is valid
                if (result.id.size() < 2 || System.Text.RegularExpressions.Regex.Match(result.id, (".*[\\^$&,\\?`´~\\|\"@#¨'%*!\\\\].*")).Success)
                    throw new exception("[login_server::requestLogin][Error] ID(" + result.id
                            + ") invalid, less then 2 characters or invalid character include in id.", STDA_ERROR_TYPE.LOGIN_SERVER);

                // Password to MD5
                var pass_md5 = result.password;

                try
                {

                    pass_md5 = result.password.size() < 32 ? Tools.MD5Hash(result.password) : result.password;

                }
                catch (exception e)
                {

                    _smp.Message_Pool.push("[login_server::requestLogin][ErrorSystem] " + e.getFullMessageError(), _smp.type_msg.CL_FILE_LOG_AND_CONSOLE);

                    // Relança
                    throw;
                }

#if _RELEASE

                // Log
                _smp.Message_Pool.push("ID : " + result.id, _smp.type_msg.CL_FILE_LOG_AND_CONSOLE);
                _smp.Message_Pool.push("Senha: " + pass_md5, _smp.type_msg.CL_FILE_LOG_AND_CONSOLE);

                _smp.Message_Pool.push("Option Count : " + (result.opt_count), _smp.type_msg.CL_FILE_LOG_AND_CONSOLE);

                foreach (var el in result.v_opt_unkn)
                    _smp.Message_Pool.push("Option Unknown 8 Bytes : 0x" + el.ToString(), _smp.type_msg.CL_FILE_LOG_AND_CONSOLE);

                _smp.Message_Pool.push("Mac Address : " + result.mac_address, _smp.type_msg.CL_FILE_LOG_AND_CONSOLE);
                _smp.Message_Pool.push("ID : " + result.id, _smp.type_msg.CL_ONLY_FILE_LOG);
                _smp.Message_Pool.push("Senha: " + pass_md5, _smp.type_msg.CL_ONLY_FILE_LOG);

#else
                //gerar outro log aqui
#endif
                if (!haveBanList(_session.m_ip, result.mac_address, !result.mac_address.empty()))
                {   // Verifica se está na list de ips banidos

                    var cmd_verifyId = new CmdVerifyID(result.id); // ID

                    cmd_verifyId.waitEvent();

                    if (cmd_verifyId.getException().getCodeError() != 0)
                        throw cmd_verifyId.getException();

                    if (cmd_verifyId.getUID() > 0)
                    {   // Verifica se o ID existe

                        var cmd_verifyPass = new CmdVerifyPass(cmd_verifyId.getUID(), pass_md5); // PASSWORD

                        cmd_verifyPass.waitEvent();

                        if (cmd_verifyPass.getException().getCodeError() != 0)
                            throw cmd_verifyPass.getException();

                        if (cmd_verifyPass.getLastVerify())
                        {   // Verifica se a senha bate com a do banco de dados

                            var cmd_pi = new CmdPlayerInfo(cmd_verifyId.getUID());


                            cmd_pi.waitEvent();

                            if (cmd_pi.getException().getCodeError() != 0)
                                throw cmd_pi.getException();

                            _session.m_pi =cmd_pi.getInfo();
                            var pi = _session.m_pi;

                            var cmd_lc = new CmdLogonCheck(pi.uid);
                            var cmd_flc = new CmdFirstLoginCheck(pi.uid);
                            var cmd_fsc = new CmdFirstSetCheck(pi.uid);

                            cmd_lc.waitEvent();

                            if (cmd_lc.getException().getCodeError() != 0)
                                throw cmd_lc.getException();

                            cmd_flc.waitEvent();

                            if (cmd_flc.getException().getCodeError() != 0)
                                throw cmd_flc.getException();

                            cmd_fsc.waitEvent();

                            if (cmd_fsc.getException().getCodeError() != 0)
                                throw cmd_fsc.getException();

                            // Verifica se tem o mesmo player logado com outro socket
                            Player player_logado = HasLoggedWithOuterSocket(_session);

                            if (!canSameIDLogin() && player_logado != null)
                            {   // Verifica se ja nao esta logado

                                p.init_plain(0x01);

                                p.AddByte(0xE2);
                                p.AddInt32(5100107);

                                packet_func_ls.session_send(p, _session, 0);

                                _smp.Message_Pool.push("[login_server::requestLogin][Log] player[UID="
                                        + (pi.uid) + ", ID=" + (pi.id) + ", IP=" + _session.GetAdress + "] ja tem outro Player conectado[UID=" + (player_logado.GetUID())
                                        + ", OID=" + (player_logado.m_oid) + ", IP=" + player_logado.GetAdress + "]", _smp.type_msg.CL_FILE_LOG_AND_CONSOLE);

                            }
                            else if (pi.m_state == 1)
                            {   // Verifica se já pediu para logar

                                p.init_plain((short)0x01);

                                p.AddUInt8(0xE2);
                                p.AddInt32(500010); // Já esta logado, ja enviei o pacote de logar

                                packet_func_ls.session_send(p, _session, 0);

                                if (pi.m_state++ >= 3)  // Ataque, derruba a conexão maliciosa
                                    _smp.Message_Pool.push("[login_server::requestLogin][Log] Player ja esta logado, o pacote de logar ja foi enviado, player[UID="
                                            + (pi.uid) + ", ID=" + (pi.id) + "]", _smp.type_msg.CL_FILE_LOG_AND_CONSOLE);

                            }
                            else
                            {

                                var cmd_vi = new CmdVerifyIP(pi.uid, _session.m_ip);


                                cmd_vi.waitEvent();

                                if (cmd_vi.getException().getCodeError() != 0)
                                    throw cmd_vi.getException();

                                if (!Convert.ToBoolean(pi.m_cap & 4) && getAccessFlag() && !cmd_vi.getLastVerify())
                                {   // Verifica se tem permição para acessar

                                    p.init_plain((short)0x01);

                                    p.AddUInt8(0xE2);
                                    p.AddInt32(500015); // Acesso restrito

                                    packet_func_ls.session_send(p, _session, 0);

                                    _smp.Message_Pool.push("[login_server::requestLogin][Log] acesso restrito para o player [UID=" + (pi.uid)
                                            + ", ID=" + (pi.id) + "]", _smp.type_msg.CL_FILE_LOG_AND_CONSOLE);

                                }
                                else if (pi.block_flag.m_id_state.id_state.ull_IDState != 0)
                                {   // Verifica se está bloqueado

                                    if (pi.block_flag.m_id_state.id_state.st_IDState.L_BLOCK_TEMPORARY && (pi.block_flag.m_id_state.block_time == -1 || pi.block_flag.m_id_state.block_time > 0))
                                    {

                                        var tempo = pi.block_flag.m_id_state.block_time / 60 / 60/*Hora*/; // Hora

                                        p.init_plain((short)0x01);

                                        p.AddUInt8(7);
                                        p.AddInt32(pi.block_flag.m_id_state.block_time == -1 || tempo == 0 ? 1/*Menos de uma hora*/ : tempo);   // Block Por Tempo

                                        // Aqui pode ter uma  com mensagem que o pangya exibe
                                        //p.AddString("ola");

                                        packet_func_ls.session_send(p, _session, 0);

                                        _smp.Message_Pool.push("[login_server::requestLogin][Log] Bloqueado por tempo[Time="
                                                + (pi.block_flag.m_id_state.block_time == -1 ? ("indeterminado") : ((pi.block_flag.m_id_state.block_time / 60)
                                                + "min " + (pi.block_flag.m_id_state.block_time % 60) + "sec"))
                                                + "]. player [UID=" + (pi.uid) + ", ID=" + (pi.id) + "]", _smp.type_msg.CL_FILE_LOG_AND_CONSOLE);

                                    }
                                    else if (pi.block_flag.m_id_state.id_state.st_IDState.L_BLOCK_FOREVER)
                                    {

                                        p.init_plain((short)0x01);

                                        p.AddUInt8(0x0c);       // Acho que seja block permanente, que fala de email
                                                                //p.AddInt32(500012);	// Block Permanente

                                        packet_func_ls.session_send(p, _session, 0);

                                        _smp.Message_Pool.push("[login_server::requestLogin][Log] Bloqueado permanente. player [UID=" + (pi.uid)
                                                + ", ID=" + (pi.id) + "]", _smp.type_msg.CL_FILE_LOG_AND_CONSOLE);

                                    }
                                    else if (pi.block_flag.m_id_state.id_state.st_IDState.L_BLOCK_ALL_IP)
                                    {

                                        // Bloquea todos os IP que o player logar e da error de que a area dele foi bloqueada

                                        // Add o ip do player para a lista de ip banidos
                                        new CmdInsertBlockIp(_session.m_ip, "255.255.255.255").waitEvent();

                                        // Resposta
                                        p.init_plain((short)0x01);

                                        p.AddUInt8(16);
                                        p.AddInt32(500012);     // Ban por Região;

                                        packet_func_ls.session_send(p, _session, 0);
                                        _smp.Message_Pool.push("[login_server::requestLogin][Log] Player[UID=" + (_session.m_pi.uid)
                                                + ", IP=" + (_session.m_ip) + "] Block ALL IP que o player fizer login.", _smp.type_msg.CL_FILE_LOG_AND_CONSOLE);

                                    }
                                    else if (pi.block_flag.m_id_state.id_state.st_IDState.L_BLOCK_MAC_ADDRESS)
                                    {

                                        // Bloquea o MAC Address que o player logar e da error de que a area dele foi bloqueada

                                        // Add o MAC Address do player para a lista de MAC Address banidos
                                        var mac = new CmdInsertBlockMac(result.mac_address);

                                        mac.waitEvent();
                                        // Resposta
                                        p.init_plain((short)0x01);

                                        p.AddUInt8(16);
                                        p.AddInt32(500012);     // Ban por Região;

                                        packet_func_ls.session_send(p, _session, 0);

                                        _smp.Message_Pool.push("[login_server::requestLogin][Log] Player[UID=" + (_session.m_pi.uid)
                                                + ", IP=" + (_session.m_ip) + ", MAC=" + result.mac_address + "] Block MAC Address que o player fizer login.", _smp.type_msg.CL_FILE_LOG_AND_CONSOLE);

                                    }
                                    else if (!cmd_flc.getLastCheck())
                                    {   // Verifica se fez o primeiro login

                                        // Authorized a ficar online no server por tempo indeterminado
                                        _session.m_is_authorized = 1;

                                        FIRST_LOGIN(_session);

                                        _smp.Message_Pool.push("[login_server::requestLogin][Log] Primeira vez que o player loga. player[UID=" + (pi.uid)
                                                + ", ID=" + (pi.id) + "]", _smp.type_msg.CL_FILE_LOG_AND_CONSOLE);

                                    }
                                    else if (!cmd_fsc.getLastCheck())
                                    {   // Verifica se fez o primeiro set do character

                                        // Authorized a ficar online no server por tempo indeterminado
                                        _session.m_is_authorized = 1;

                                        FIRST_SET(_session);

                                        _smp.Message_Pool.push("[login_server::requestLogin][Log] Primeira vez que o player escolhe um character padrao. player[UID="
                                                + (pi.uid) + ", ID=" + (pi.id) + "]", _smp.type_msg.CL_FILE_LOG_AND_CONSOLE);

                                    }
                                    else if (cmd_lc.getLastCheck())
                                    {   // Verifica se já esta logado no game server

                                        // Pega o Server UID para usar depois no packet004, para derrubar do server
                                        _session.m_pi.m_server_uid = cmd_lc.getServerUID();

                                        // Já está varrizado a ficar online, o login server só vai derrubar o outro que está online no game server
                                        // Authorized a ficar online no server por tempo indeterminado
                                        _session.m_is_authorized = 1;

                                        p.init_plain((short)0x01);

                                        p.AddUInt8(4);

                                        packet_func_ls.session_send(p, _session, 0);

                                        _smp.Message_Pool.push("[login_server::requestLogin][Log] Player ja esta logado no game server. player[UID="
                                                + (pi.uid) + ", ID=" + (pi.id) + "]", _smp.type_msg.CL_FILE_LOG_AND_CONSOLE);

                                    }
                                    else if (Convert.ToBoolean(pi.m_cap & 4))
                                    {   // Acesso permtido

                                        // Authorized a ficar online no server por tempo indeterminado
                                        _session.m_is_authorized = 1;

                                        SUCCESS_LOGIN("requestLogin", _session);

                                        _smp.Message_Pool.push("[login_server::requestLogin][Log] GM logou[UID=" + (pi.uid)
                                                + ", ID=" + (pi.id) + "]", _smp.type_msg.CL_FILE_LOG_AND_CONSOLE);

                                    }
                                    else
                                    {

                                        // Authorized a ficar online no server por tempo indeterminado
                                        _session.m_is_authorized = 1;

                                        SUCCESS_LOGIN("requestLogin", _session);
                                    }

                                }
                                else if (!cmd_flc.getLastCheck())
                                {   // Verifica se fez o primeiro login

                                    // Authorized a ficar online no server por tempo indeterminado
                                    _session.m_is_authorized = 1;

                                    FIRST_LOGIN(_session);

                                    _smp.Message_Pool.push("[login_server::requestLogin][Log] Primeira vez que o player loga. player[UID=" + (pi.uid)
                                            + ", ID=" + (pi.id) + "]", _smp.type_msg.CL_FILE_LOG_AND_CONSOLE);

                                }
                                else if (!cmd_fsc.getLastCheck())
                                {   // Verifica se fez o primeiro set do character

                                    // Authorized a ficar online no server por tempo indeterminado
                                    _session.m_is_authorized = 1;

                                    FIRST_SET(_session);

                                    _smp.Message_Pool.push("[login_server::requestLogin][Log] Primeira vez que o player escolhe um character padrao. player[UID="
                                            + (pi.uid) + ", ID=" + (pi.id) + "]", _smp.type_msg.CL_FILE_LOG_AND_CONSOLE);

                                }
                                else if (cmd_lc.getLastCheck())
                                {   // Verifica se já esta logado no game server

                                    // Pega o Server UID para usar depois no packet004, para derrubar do server
                                    _session.m_pi.m_server_uid = cmd_lc.getServerUID();

                                    // Já está varrizado a ficar online, o login server só vai derrubar o outro que está online no game server
                                    // Authorized a ficar online no server por tempo indeterminado
                                    _session.m_is_authorized = 1;

                                    p.init_plain((short)0x01);

                                    p.AddUInt8(4);

                                    packet_func_ls.session_send(p, _session, 0);

                                    _smp.Message_Pool.push("[login_server::requestLogin][Log] Player ja esta logado no game server. player[UID="
                                            + (pi.uid) + ", ID=" + (pi.id) + "]", _smp.type_msg.CL_FILE_LOG_AND_CONSOLE);

                                }
                                else if (Convert.ToBoolean(pi.m_cap & 4))
                                {   // Acesso permtido

                                    // Authorized a ficar online no server por tempo indeterminado
                                    _session.m_is_authorized = 1;

                                    SUCCESS_LOGIN("requestLogin", _session);

                                    _smp.Message_Pool.push("[login_server::requestLogin][Log] GM logou[UID=" + (pi.uid)
                                            + ", ID=" + (pi.id) + "]", _smp.type_msg.CL_FILE_LOG_AND_CONSOLE);

                                }
                                else
                                {

                                    // Authorized a ficar online no server por tempo indeterminado
                                    _session.m_is_authorized = 1;

                                    SUCCESS_LOGIN("requestLogin", _session);
                                }
                            }

                        }
                        else
                        {

                            p = packet_func_ls.pacote001(_session, 6/* ID ou PW errado*/);
                            packet_func_ls.session_send(p, _session, 1); // Erro pass


                            _smp.Message_Pool.push("[login_server::requestLogin][Log] senha errada. ID: " + cmd_verifyId.getID()
                                    + "  senha: " + pass_md5/*cmd_verifyPass.getPass()*/, _smp.type_msg.CL_FILE_LOG_AND_CONSOLE);
                        }

                    }

                    else if (!getAccessFlag() && getCreateUserFlag())
                    {

                        //// Authorized a ficar online no server por tempo indeterminado
                        _session.m_is_authorized = 1;

                        _smp.Message_Pool.push("[login_server::requestLogin][Log] Criando um novo usuario[ID=" + cmd_verifyId.getID()
                                + ", PASSWORD=" + pass_md5/*pass*/ + "]", _smp.type_msg.CL_FILE_LOG_AND_CONSOLE);

                        var ip = _session.GetAdress;

                        var cmd_cu = new CmdCreateUser(cmd_verifyId.getID(), result.password, ip, m_si.UID);


                        cmd_cu.waitEvent();

                        if (cmd_cu.getException().getCodeError() != 0)
                            throw cmd_cu.getException();

                        var pi = _session.m_pi;

                        pi.uid = cmd_cu.getUID();

                        var cmd_pi = new CmdPlayerInfo(pi.uid);

                        cmd_pi.waitEvent();

                        if (cmd_pi.getException().getCodeError() != 0)
                            throw cmd_pi.getException();

                        pi = cmd_pi.getInfo().GetInfo();

                        FIRST_LOGIN(_session);

                        // Log
                        _smp.Message_Pool.push("[login_server::requestLogin][Log] Conta Criada com sucesso. Player[UID=" + (pi.uid)
                                + ", ID=" + pi.id + ", PASSWORD=" + pass_md5/*pi.pass*/ + "]", _smp.type_msg.CL_FILE_LOG_AND_CONSOLE);

                    }
                    else
                    {

                        p = packet_func_ls.pacote001(_session, 6/*ID é 2, 6 é o ID ou pw errado*/);
                        packet_func_ls.session_send(p, _session, 1);
                        _session.m_pi.id = result.id;
                        _smp.Message_Pool.push("[login_server::requestLogin][Log] ID nao existe, ID: " + cmd_verifyId.getID(), _smp.type_msg.CL_FILE_LOG_AND_CONSOLE);
                        OnSessionClosed(_session);
                    }

                }
                else
                {   // Ban IP/MAC por região

                    p.init_plain((short)0x01);

                    p.AddUInt8(16);
                    p.AddInt32(500012);     // Ban por Região;

                    packet_func_ls.session_send(p, _session, 0);
                    _smp.Message_Pool.push("[login_server::requestLogin][Log] Block por Regiao o IP/MAC: " + (_session.m_ip) + "/" + result.mac_address, _smp.type_msg.CL_FILE_LOG_AND_CONSOLE);
                }

            }

            catch (exception e)
            {

                _smp.Message_Pool.push("[login_server::requestLogin][ErrorSystem] " + e.getFullMessageError(), _smp.type_msg.CL_FILE_LOG_AND_CONSOLE);

                if (e.getCodeError() == STDA_ERROR_TYPE.LOGIN_SERVER)
                {

                    // Invalid ID
                    p = packet_func_ls.pacote001(_session, 2/*Invlid ID*/);
                    packet_func_ls.session_send(p, _session, 1);

                }
                else
                {

                    // Unknown Error (System Fail)
                    p.init_plain((short)0x01);

                    p.AddUInt8(0xE2);
                    p.AddInt32(500050);     // System Error

                    packet_func_ls.session_send(p, _session, 0);
                }

            }
        }
        public void SUCCESS_LOGIN(string _from, Player  _session)
        {
            (_session).m_pi.m_state = 1;
            _smp.Message_Pool.push("[login_server::" + ((_from)) + "][Log] Player logou. [ID=" + ((_session).m_pi.id) + ", UID=" + ((_session).m_pi.uid) + "]");
            packet_func_ls.succes_login((this), (_session));
        }
        protected void FIRST_SET(Player  _session)
        {
            (_session).m_pi.m_state = 3;
            var p = new Packet();
            packet_func_ls.pacote00F(ref p, (_session), 1);
            packet_func_ls.session_send(p, (_session), 1);
            p = packet_func_ls.pacote001((_session), 0xD9);
            packet_func_ls.session_send(p, (_session), 1);
        }
        protected void FIRST_LOGIN(Player  _session)
        {
            _session.m_pi.m_state = 2;
            var p = new Packet();
            packet_func_ls.pacote00F(ref p, (_session));
            packet_func_ls.session_send(p, (_session), 1);
            p = packet_func_ls.pacote001((_session), 0xD8);
            packet_func_ls.session_send(p, (_session), 1);
        }

        public void requestDownPlayerOnGameServer(Player  _session, Packet p)
        {
            try
            {

                // Verifica se session está autorizada para executar esse ação, 
                // se ele não fez o login com o Server ele não pode fazer nada até que ele faça o login
                //CHECK_SESSION_IS_AUTHORIZED("DownPlayerOnGameServer");

                // Derruba o player que está logado no game server
                // Se o Auth Server Estiver ligado manda por ele, se não tira pelo banco de dados mesmo
                //if (m_unit_connect.isLive())
                //{

                //    // [Auth Server] . Game Server UID = _session.m_pi.m_server_uid;
                //    m_unit_connect.sendDisconnectPlayer(_session.m_pi.m_server_uid, _session.m_pi.uid);

                //}
                //else
                //{

                // Auth Server não está online, resolver por aqui mesmo
                var cmd_rl = new CmdRegisterLogon(_session.m_pi.uid, 1);

                cmd_rl.waitEvent();

                if (cmd_rl.getException().getCodeError() != 0)
                    throw cmd_rl.getException();

                // Loga com sucesso
                SUCCESS_LOGIN("login_server", _session);

                _smp.Message_Pool.push("[login_server::requestDownPlayerOnGameServer][Log] Player[UID=" + (_session.m_pi.uid)
                        + ", ID=" + (_session.m_pi.id) + "] derrubou o outro do game server[UID="
                        + (_session.m_pi.m_server_uid) + "] com sucesso.");
                //}

            }
            catch (exception e)
            {

                _smp.Message_Pool.push("[login_server::requestDownPlayerOnGame][ErrorSystem] " + e.getFullMessageError());

                // Fail Login
                packet_func_ls.pacote00E(ref p, _session, "", 12, (e.getCodeError() == STDA_ERROR_TYPE.LOGIN_SERVER ? (uint)e.getCodeError() : 500053));

                packet_func_ls.session_send(p, _session, 1);
            }
        }

        internal void requestReLogin(Player  _session, Packet _packet)
        {
            var p = _packet;

            try
            {

               string id = _packet.ReadString();
               _packet.ReadInt32(out int server_uid);
                string auth_key_login = _packet.ReadString();

# if RELEASE
               _smp.Message_Pool.push("[login_server::requestReLogin][Log] ID: " + id, _smp.type_msg.CL_FILE_LOG_AND_CONSOLE);
               _smp.Message_Pool.push("[login_server::requestReLogin][Log] UID: " + (server_uid), _smp.type_msg.CL_FILE_LOG_AND_CONSOLE);
               _smp.Message_Pool.push("[login_server::requestReLogin][Log] Auth Key Login: " + auth_key_login, _smp.type_msg.CL_FILE_LOG_AND_CONSOLE);
#else
               _smp.Message_Pool.push("[login_server::requestReLogin][Log] ID: " + id, _smp.type_msg.CL_ONLY_FILE_LOG);
               _smp.Message_Pool.push("[login_server::requestReLogin][Log] UID: " + (server_uid), _smp.type_msg.CL_ONLY_FILE_LOG);
                _smp.Message_Pool.push("[login_server::requestReLogin][Log] Auth Key Login: " + auth_key_login, _smp.type_msg.CL_ONLY_FILE_LOG);
#endif

                var cmd_verifyId = new CmdVerifyID(id); // ID

                //snmdb::NormalManagerDB::getInstance().Add(0, &cmd_verifyId, nullptr, nullptr);

                cmd_verifyId.waitEvent();

                if (cmd_verifyId.getException().getCodeError() != 0)
                    throw cmd_verifyId.getException();

                if (cmd_verifyId.getUID() <= 0) // Verifica se o ID existe
                    throw new exception("[login_server::requestReLogin][Error] Player[ID=" + id + "] not found. Hacker ou Bug",STDA_ERROR_TYPE.LOGIN_SERVER);

              var cmd_pi= new  CmdPlayerInfo(cmd_verifyId.getUID());

                //snmdb::NormalManagerDB::getInstance().Add(0, &cmd_pi, nullptr, nullptr);

                cmd_pi.waitEvent();

                if (cmd_pi.getException().getCodeError() != 0)
                    throw cmd_pi.getException();

                _session.m_pi = cmd_pi.getInfo().GetInfo();

                if (id.CompareTo(_session.m_pi.id) != 0)
                    throw new exception("[login_server::requestReLogin][Error] id nao eh igual ao da session[PlayerUID: " + (_session.m_pi.uid) + "] { SESSION_ID="
                            + (_session.m_pi.id) + ", REQUEST_ID=" + id + " } no match", STDA_ERROR_TYPE.LOGIN_SERVER);

               var cmd_akli = new CmdAuthKeyLoginInfo(_session.m_pi.uid);

                //snmdb::NormalManagerDB::getInstance().Add(0, &cmd_akli, nullptr, nullptr);

                cmd_akli.waitEvent();

                if (cmd_akli.getException().getCodeError() != 0)
                    throw cmd_akli.getException();

                var akli = cmd_akli.getInfo();

                if (auth_key_login.CompareTo(akli.key) != 0)
                    throw new exception("[login_server::requestReLogin][Error] auth login server nao eh igual a do banco de dados da session[PlayerUID: "
                            + (_session.m_pi.uid) + "] AuthKeyLogin: " + (akli.key) + " != "
                            + auth_key_login, STDA_ERROR_TYPE.LOGIN_SERVER);

                // Verifica se ele pode logar de novo, verifica as flag do login server
                if (haveBanList(_session.m_ip, "", false/*Não verifica o MAC Address*/))    // Verifica se está na list de ips banidos
                    throw new exception("[login_server::requestReLogin][Error] auth login server, o player[UID="
                            + (_session.m_pi.uid) + "] esta na lista de ip banidos.", STDA_ERROR_TYPE.LOGIN_SERVER);

                var cmd_vi = new CmdVerifyIP(_session.m_pi.uid, _session.m_ip);

                //snmdb::NormalManagerDB::getInstance().Add(0, &cmd_vi, nullptr, nullptr);

                cmd_vi.waitEvent();

                if (cmd_vi.getException().getCodeError() != 0)
                    throw cmd_vi.getException();

                if (!Convert.ToBoolean(_session.m_pi.m_cap & 4) && getAccessFlag() && !cmd_vi.getLastVerify())
                {   // Verifica se tem permição para acessar

                    throw new exception("[login_server::requestReLogin][Log] acesso restrito para o player [UID=" + (_session.m_pi.uid)
                            + ", ID=" + (_session.m_pi.id) + "]",STDA_ERROR_TYPE.LOGIN_SERVER);

                }
                else if (_session.m_pi.block_flag.m_id_state.id_state.ull_IDState != 0)
                {   // Verifica se está bloqueado

                    if (_session.m_pi.block_flag.m_id_state.id_state.st_IDState.L_BLOCK_TEMPORARY && (_session.m_pi.block_flag.m_id_state.block_time == -1 || _session.m_pi.block_flag.m_id_state.block_time > 0))
                    {

                        throw new exception("[login_server::requestReLogin][Log] Bloqueado por tempo[Time="
                                + (_session.m_pi.block_flag.m_id_state.block_time == -1 ? ("indeterminado") : ((_session.m_pi.block_flag.m_id_state.block_time / 60)
                                + "min " + (_session.m_pi.block_flag.m_id_state.block_time % 60) + "sec"))
                                + "]. player [UID=" + (_session.m_pi.uid) + ", ID=" + (_session.m_pi.id) + "]", STDA_ERROR_TYPE.LOGIN_SERVER);

                    }
                    else if (_session.m_pi.block_flag.m_id_state.id_state.st_IDState.L_BLOCK_FOREVER)
                    {

                        throw new exception("[login_server::requestReLogin][Log] Bloqueado permanente. player [UID=" + (_session.m_pi.uid)
                                + ", ID=" + (_session.m_pi.id) + "]", STDA_ERROR_TYPE.LOGIN_SERVER);

                    }
                    else if (_session.m_pi.block_flag.m_id_state.id_state.st_IDState.L_BLOCK_ALL_IP)
                    {

                        // Bloquea todos os IP que o player logar e da error de que a area dele foi bloqueada

                        // Add o ip do player para a lista de ip banidos
                        //snmdb::NormalManagerDB::getInstance().Add(1, new CmdInsertBlockIP(_session.m_ip, "255.255.255.255"), login_server::SQLDBResponse, this);

                        // Resposta
                        throw new exception("[login_server::requestReLogin][Log] Player[UID=" + (_session.m_pi.uid)
                                + ", IP=" + (_session.m_ip) + "] Block ALL IP que o player fizer login.", STDA_ERROR_TYPE.LOGIN_SERVER);

                    }
                    else if (_session.m_pi.block_flag.m_id_state.id_state.st_IDState.L_BLOCK_MAC_ADDRESS)
                    {

                        // Bloquea o MAC Address que o player logar e da error de que a area dele foi bloqueada

                        // Aqui só da error por que não tem como bloquear o MAC Address por que o cliente não fornece o MAC Address nesse pacote
                        throw new exception("[login_server::requestReLogin][Log] Player[UID=" + (_session.m_pi.uid)
                                + ", IP=" + (_session.m_ip) + ", MAC=UNKNOWN] (Esse pacote o cliente nao fornece o MAC Address) Block MAC Address que o player fizer login.",
                               STDA_ERROR_TYPE.LOGIN_SERVER);

                    }

                }

                // Passou da verificação com sucesso
               _smp.Message_Pool.push("[login_server::requestReLogin][Log] player[UID=" + (_session.m_pi.uid) + ", ID="
                        + (_session.m_pi.id) + "] relogou com sucesso", _smp.type_msg.CL_FILE_LOG_AND_CONSOLE);

                // Authorized a ficar online no server por tempo indeterminado
                _session.m_is_authorized = 1;

                packet_func_ls.succes_login(this, _session, 1/*só passa auth Key Login, Server List, Msn Server List*/);

            }
            catch (exception e) {

                // Erro do sistema
                packet_func_ls.pacote00E(ref p, _session, "", 12, 500052);
                packet_func_ls.session_send(p, _session, 1);


               _smp.Message_Pool.push("[login_server::requestReLogin][ErrorSystem] " + e.getFullMessageError());
            }
            }

        

        public override void onHeartBeat()
        {
            try
            {
                // Server ainda não está totalmente iniciado
                if (this.State != ServerState.NotInitialized)
                    return;

                // Tirei o list IP/MAC block daqui e coloquei no monitor no server, por que agora eles são da classe server
            }
            catch (exception e)
            {
                _smp.Message_Pool.push("[login_server::onHeartBeat][ErrorSystem] " + e.getFullMessageError(), _smp.type_msg.CL_FILE_LOG_AND_CONSOLE);
            }
        }

        protected override bool checkPacket(Player session, Packet packet)
        {
            return true;
        }

        protected override void onAcceptCompleted(Player _session)
        {
            try
            {
                var packet = new Packet(0x0B00);
                packet.AddInt32(05);
                packet.AddInt32(10101);
                packet.MakeRaw();
                var mb = packet.GetMakedBuf();
                _session.Send(mb.Buffer, 0, (int)mb.Length);
            }
            catch (Exception ex)
            {

            }

        }

    }
}
