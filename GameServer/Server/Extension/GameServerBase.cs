using System;
using System.Collections.Generic;
using GameServer.Cmd;
using GameServer.TYPE;
using PangyaAPI.SQL;
using PangyaAPI.SuperSocket.Engine;
using PangyaAPI.SQL.DATA.Cmd;
using PangyaAPI.SQL.DATA.TYPE;
using PangyaAPI.Utilities;
using GameServer.Session;
using _smp = PangyaAPI.Utilities.Log;
using snmdb = PangyaAPI.SQL.Manager;
using packet_func_gs = GameServer.PACKET.packet_func;
using packet = PangyaAPI.SuperSocket.SocketBase.Packet;
using static PangyaAPI.Utilities.Tools;
using GameServer.Game;
using PangyaAPI.SuperSocket.Interface;
using PangyaAPI.SuperSocket.SocketBase;

namespace GameServer.Server.Extension
{
    public class GameServerBase : PangyaServer<Player>
	{
		private int m_GameGuardAuth;

		public int m_access_flag { get; private set; }


		public int m_create_user_flag { get; private set; }
		public int m_same_id_login_flag { get; private set; }

		DailyQuestInfo m_dqi;

		LoginManager m_login_manager;
		protected List<Channel> v_channel;
		//BroadcastList m_notice;
		//BroadcastList m_ticker;
		public GameServerBase()
		{
			v_channel = new List<Channel>();
			m_login_manager = new LoginManager();
			init_packets();
			init_load_channels();
		}

	
		protected override void OnStarted()
		{

			_smp.Message_Pool.push("[Server.OnStarted][Log]: Server starting...", _smp.type_msg.CL_FILE_LOG_AND_CONSOLE);
			base.OnStarted();
		}


		public override void ConfigInit()
		{
			base.ConfigInit();
			m_player_manager = new player_manager(m_si.MaxUser);
			// Server Tipo
			m_si.Tipo = 1;

			m_si.ImgNo = Ini.ReadInt16("SERVERINFO", "ICONINDEX");
			m_si.Rate.exp = (short)Ini.readInt("SERVERINFO", "EXPRATE");
			m_si.Rate.scratchy = (short)Ini.readInt("SERVERINFO", "SCRATCHY_RATE");
			m_si.Rate.pang = (short)Ini.readInt("SERVERINFO", "PANGRATE");
			m_si.Rate.club_mastery = (short)Ini.readInt("SERVERINFO", "CLUBMASTERYRATE");
			m_si.Rate.papel_shop_rare_item = (short)Ini.readInt("SERVERINFO", "PAPEL_Rate_RATE"); ;
			m_si.Rate.papel_shop_cookie_item = (short)Ini.readInt("SERVERINFO", "PAPEL_COOKIE_ITEM_RATE"); ;
			m_si.Rate.treasure = (short)Ini.readInt("SERVERINFO", "TREASURE_RATE"); ;
			m_si.Rate.memorial_shop = (short)Ini.readInt("SERVERINFO", "MEMORIAL_RATE");
			m_si.Rate.chuva = (short)Ini.readInt("SERVERINFO", "CHUVA_RATE");
			m_si.Rate.grand_zodiac_event_time = (short)(Ini.readInt("SERVERINFO", "GZ_EVENT") >= 1 ? 1 : 0);// Ativo por padrão
			m_si.Rate.grand_prix_event = (short)(Ini.readInt("SERVERINFO", "GP_EVENT") >= 1 ? 1 : 0);// Ativo por padrão
			m_si.Rate.golden_time_event = ((short)(Ini.readInt("SERVERINFO", "GOLDEN_TIME_EVENT") >= 1 ? 1 : 0));// Ativo por padrão
			m_si.Rate.login_reward_event = ((short)(Ini.readInt("SERVERINFO", "LOGIN_REWARD") >= 1 ? 1 : 0));// Ativo por padrão
			m_si.Rate.bot_gm_event = ((short)(Ini.readInt("SERVERINFO", "BOT_GM_EVENT") >= 1 ? 1 : 0));// Ativo por padrão
			m_si.Rate.smart_calculator = (/*Ini.readInt("SERVERINFO", "SMART_CALC") >= 1 ? true :*/ 0);// Atibo por padrão
			m_si.Rate.angel_event = ((short)(Ini.readInt("SERVERINFO", "ANGEL_EVENT") >= 1 ? 1 : 0));// Atibo por padrão
			try
			{

				m_si.flag.ullFlag = Ini.ReadUInt64("SERVERINFO", "FLAG");

			}
			catch (exception e)
			{

				_smp.Message_Pool.push(("[game_server::config_init][ErrorSystem] Config.FLAG" + e.getFullMessageError()));
			}

			// Game Guard Auth
			try
			{

				m_GameGuardAuth = (Ini.readInt("SERVERINFO", "GAMEGUARDAUTH") >= 1 ? 1 : 0);

			}
			catch (exception e)
			{

				_smp.Message_Pool.push(("[game_server::config_init][ErrorSystem] Config.GAMEGUARDAUTH. " + e.getFullMessageError()));
			}


			// Recupera Valores de Rate do server do banco de dados
			var cmd_rci = new CmdRateConfigInfo(m_si.UID);  // Waiter

			cmd_rci.waitEvent();

			if (cmd_rci.getException().getCodeError() != 0 || cmd_rci.isError()/*Deu erro na consulta não tinha o Rate config info para esse server, pode ser novo*/)
			{

				if (cmd_rci.getException().getCodeError() != 0)
					_smp.Message_Pool.push(("[game_server::config_init][ErrorSystem] " + cmd_rci.getException().getFullMessageError()));


				setAngelEvent(m_si.Rate.angel_event);
				setRatePang(m_si.Rate.pang);
				setRateExp(m_si.Rate.exp);
				setRateClubMastery(m_si.Rate.club_mastery);
			}
			else
			{   // Conseguiu recuperar com sucesso os valores do server

				setAngelEvent(m_si.Rate.angel_event);
				setRatePang(m_si.Rate.pang);
				setRateExp(m_si.Rate.exp);
				setRateClubMastery(m_si.Rate.club_mastery);
			}
			m_si.AppRate = 100;    // Esse aqui nunca usei, deixei por que no DB do s4 tinha só cópiei
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
		private void setAngelEvent(short _angel_event)
		{// Evento para reduzir o quit rate, diminui 1 quit a cada jogo concluído
			m_si.EventFlag.stBit.angel_wing = _angel_event > 0;

			// Update Event Angel
			m_si.Rate.angel_event = (short)_angel_event;
		}
		private void setRatePang(short _pang)
		{
			// Update Flag Event
			m_si.EventFlag.stBit.pang_x_plus = (_pang >= 200) ? true : false;

			// Update rate Pang
			m_si.Rate.pang = (short)_pang;
		}
		private void setRateExp(short _exp)
		{// Reseta flag antes de atualizar ela 
			m_si.EventFlag.stBit.exp_x2 = m_si.EventFlag.stBit.exp_x_plus = true;

			// Update Flag Event
			if (_exp > 200)
				m_si.EventFlag.stBit.exp_x_plus = true;
			else if (_exp == 200)
				m_si.EventFlag.stBit.exp_x2 = true;
			else
				m_si.EventFlag.stBit.exp_x2 = m_si.EventFlag.stBit.exp_x_plus = false;

			// Update rate Experiência
			m_si.Rate.exp = _exp;
		}
		private void setRateClubMastery(short _club_mastery)
		{
			// Update Flag Event
			m_si.EventFlag.stBit.club_mastery_x_plus = (_club_mastery >= 200) ? true : false;

			// Update rate Club Mastery
			m_si.Rate.club_mastery = (short)_club_mastery;
		}
				
		public override void OnHeartBeat()
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

		public void requestLogin(packet _packet, Player _session)
		{
			packet p;

			try
			{

				uint ii = 0, packet_version = 0;

				KeysOfLogin kol = new KeysOfLogin();
				AuthKeyInfo akli = new AuthKeyInfo();
				AuthKeyGameInfo akgi = new AuthKeyGameInfo();

				string client_version;

				// Temp que vai guarda os dados que o cliente enviou para fazer o login com o server
				PlayerInfo _pi = new PlayerInfo();

				// Player info da session e vai guardar os valores recuperados do banco de dados
				PlayerInfo pi = (_session.m_pi);

                //////////// ----------------------- Começa a ler o packet que o cliente enviou ------------------------- \\\\\\\\\\\
                // Read Packet Client request
                _pi.id =_packet.ReadString();
                _pi.uid = _packet.ReadUInt32();
               var ntKey = _packet.ReadUInt32(); // ntKey
               var Command = _packet.ReadUInt16();
                kol.keys[0] = _packet.ReadString();
                client_version = _packet.ReadString();
                packet_version = _packet.ReadUInt32();
                string mac_address = _packet.ReadString();
                kol.keys[1] = _packet.ReadString();
                // -------------- Finished reading the packet sent by the client ---------------


                ////////////----------------------- Terminou a leitura do packet que o cliente enviou -------------------------\\\\\\\\\\\/

                // Verifica aqui se o IP/MAC ADDRESS do player está bloqueado
                if (haveBanList(_session.getIP(), mac_address, !mac_address.empty()))
					throw new exception("[game_server::requestLogin][Error] Player[UID=" + (_pi.uid) + ", IP="
							+ _session.getIP() + ", MAC=" + mac_address + "] esta bloqueado por regiao IP/MAC Addrress.");

				// Aqui verifica se recebeu os dados corretos
				if (_pi.id[0] == '\0')
					throw new exception("[game_server::requestLogin][Error] Player[UID=" + (_pi.uid)
							+ ", IP=" + _session.getIP() + "] id que o player enviou eh invalido. id: " + (_pi.id));

				// Verifica se o server está mantle, se tiver verifica se o player tem capacidade para entrar
				var cmd_pi = new CmdPlayerInfo(_pi.uid); // Waiter

				snmdb.NormalManagerDB.add(0, cmd_pi, null, null);

				cmd_pi.waitEvent();

				if (cmd_pi.getException().getCodeError() != 0)
					throw cmd_pi.getException();

				pi = cmd_pi.getInfo();
				_session.m_pi = _pi;

				if (pi.uid == ~0)
					throw new exception("[game_server::requestLogin][Error] player[UID=" + (_pi.uid) + "] nao existe no banco de dados");

				// UID de outro player ou enviou o ID errado mesmo (essa parte é anti-hack ou bot)
				if (string.Compare(pi.id, _pi.id) != 0)
					throw new exception("[game_server::requestLogin][Error] Player[UID=" + (pi.uid) + ", REQ_UID="
							+ (_pi.uid) + "] Player ID nao bate : client send ID : " + (_pi.id) + "\t player DB ID : "
							+ (pi.id));
				// Verifica aqui se a conta do player está bloqueada
				if (pi.block_flag.m_id_state.id_state.ull_IDState != 0)
				{

					if (pi.block_flag.m_id_state.id_state.st_IDState.L_BLOCK_TEMPORARY && (pi.block_flag.m_id_state.block_time == -1 || pi.block_flag.m_id_state.block_time > 0))
					{

						throw new exception("[game_server::requestLogin][Log] Bloqueado por tempo[Time="
								+ (pi.block_flag.m_id_state.block_time == -1 ? ("indeterminado") : ((pi.block_flag.m_id_state.block_time / 60)
								+ "min " + (pi.block_flag.m_id_state.block_time % 60) + "sec"))
								+ "]. player [UID=" + (pi.uid) + ", ID=" + (pi.id) + "]");

					}
					else if (pi.block_flag.m_id_state.id_state.st_IDState.L_BLOCK_FOREVER)
					{

						throw new exception("[game_server::requestLogin][Log] Bloqueado permanente. player [UID=" + (pi.uid)
								+ ", ID=" + (pi.id) + "]");
					}
					else if (pi.block_flag.m_id_state.id_state.st_IDState.L_BLOCK_ALL_IP)
					{

						// Bloquea todos os IP que o player logar e da error de que a area dele foi bloqueada

						// Add o ip do player para a lista de ip banidos
						snmdb.NormalManagerDB.add(9, new CmdInsertBlockIp(_session.getIP(), "255.255.255.255"), SQLDBResponse, this);

						// Resposta
						throw new exception("[game_server::requestLogin][Log] Player[UID=" + (pi.uid) + ", IP=" + (_session.getIP())
								+ "] Block ALL IP que o player fizer login.");
					}
					else if (pi.block_flag.m_id_state.id_state.st_IDState.L_BLOCK_MAC_ADDRESS)
					{

						// Bloquea o MAC Address que o player logar e da error de que a area dele foi bloqueada

						// Add o MAC Address do player para a lista de MAC Address banidos
						snmdb.NormalManagerDB.add(10, new CmdInsertBlockMac(mac_address), SQLDBResponse, this);

						// Resposta
						throw new exception("[game_server::requestLogin][Log] Player[UID=" + (pi.uid)
								+ ", IP=" + (_session.getIP()) + ", MAC=" + mac_address + "] Block MAC Address que o player fizer login.");

					}
				}

				// Check packet version
				_packet.Version_Decrypt(@packet_version);


				//// Se a flag do canSameIDLogin estiver ativo, não verifica packet
				//if (!m_login_manager.canSameIDLogin() && packet_version != m_si.packet_version)
				//{
				//	// Error no login, set falso o autoriza o player a continuar conectado com o Game Server
				//	_session.m_is_authorized = 0;

				//	// Error Sistema
				//	packet p((unsigned short)0x44);

				//	// Pronto agora sim, mostra o erro que eu quero
				//	p.addInt32(0x0B);

				//	packet_func.session_send(p,_session, 1);

				//	// Disconnect

				//}
				// Verifica o Auth Key do player
				var cmd_akli = new CmdAuthKeyLoginInfo((int)pi.uid); // Waiter

				snmdb.NormalManagerDB.add(0, cmd_akli, null, null);

				cmd_akli.waitEvent();

				if (cmd_akli.getException().getCodeError() != 0)
					throw cmd_akli.getException();
				//false  = true, true = false
				//	// ### Isso aqui é uma falha de segurança faltal, muito grande nunca posso deixar isso ligado depois que colocar ele online
				//	if (!m_login_manager.canSameIDLogin() && !cmd_akli.getInfo().valid)
				//		throw exception("[game_server::requestLogin][Error] Player[UID=" + (pi.uid) + "].\tAuthKey ja foi utilizada antes.", STDA_MAKE_ERROR(STDA_ERROR_TYPE::GAME_SERVER, 1056, 0));

				//	// ### Isso aqui é uma falha de segurança faltal, muito grande nunca posso deixar isso ligado depois que colocar ele online
				//	if (!m_login_manager.canSameIDLogin() &&
				//string.Compare(kol.keys[0], cmd_akli.getInfo().key) != 0
				//	)
				//		throw new exception("[game_server::requestLogin][Error] Player[UID=" + (pi.uid) + "].\tAuthKey no bate(no match).", STDA_MAKE_ERROR(STDA_ERROR_TYPE::GAME_SERVER, 1057, 0));

				//	ClientVersion cv_side_sv = ClientVersion::make_version(const_cast <  &> (m_login_manager.getClientVersionSideServer()));
				//	auto cv_side_c = ClientVersion::make_version(client_version);

				//	if (cv_side_c.flag == ClientVersion::COMPLETE_VERSION && strcmp(cv_side_c.region, cv_side_sv.region) == 0
				//			&& strcmp(cv_side_c.season, cv_side_sv.season) == 0)
				//	{

				//		if (cv_side_c.high != cv_side_sv.high || cv_side_c.low < cv_side_sv.low)
				//		{
				//			_smp.Message_Pool.push(("[game_server::requestLogin][WARNING] Player[UID=" + (pi.uid) + "].\tClient Version not match. Server: "
				//					+ (m_login_manager.getClientVersionSideServer()) + " == Client: " + cv_side_c.toString(), CL_ONLY_FILE_LOG));

				//			pi.block_flag.m_flag.stBit.all_game = 1;// |= BLOCK_PLAY_ALL;
				//		}

				//	}
				//	else if (cv_side_c.high != cv_side_sv.high || cv_side_c.low < cv_side_sv.low)
				//	{

				//		_smp.Message_Pool.push(("[game_server::requestLogin][WARNING] Player[UID=" + (pi.uid) + "].\tClient Version not match. Server: "
				//				+ (m_login_manager.getClientVersionSideServer()) + " == Client: " + cv_side_c.toString(), CL_ONLY_FILE_LOG));

				//		pi.block_flag.m_flag.stBit.all_game = 1;// |= BLOCK_PLAY_ALL;
				//	}

				// Member Info
				var cmd_mi = new CmdMemberInfo(pi.uid);    // Waiter

				snmdb.NormalManagerDB.add(0, cmd_mi, null, null);

				cmd_mi.waitEvent();

				if (cmd_mi.getException().getCodeError() != 0)
					throw cmd_mi.getException();

				_session.m_pi.mi = cmd_mi.getInfo();
				// Passa o Online ID para a estrutura MemberInfo, para não da erro depois
				pi.mi.oid = _session.m_oid;
				pi.mi.state_flag.stFlagBit.visible = 1;
				pi.mi.state_flag.stFlagBit.whisper = pi.whisper;
				pi.mi.state_flag.stFlagBit.channel = (byte)~pi.whisper;

				if (pi.m_cap.stBit.game_master.IsTrue())
				{
					_session.m_gi.setGMUID(pi.uid);    // Set o UID do GM dados

					pi.mi.state_flag.stFlagBit.visible = _session.m_gi.visible;
					pi.mi.state_flag.stFlagBit.whisper = _session.m_gi.whisper;
					pi.mi.state_flag.stFlagBit.channel = _session.m_gi.channel;
				}

				// Verifica se o player tem a capacidade e level para entrar no server
				if (m_si.Property.stBit.only_rookie.IsTrue() && pi.level >= 6/*Beginner E maior*/)
					throw new exception("[game_server::requestLogin][Error] Player[UID=" + (pi.uid) + ", LEVEL="
							+ ((short)pi.level) + "] nao pode entrar no server por que o server eh so para rookie.");
				/*Nega ele não pode ser nenhum para lançar o erro*/
				if (m_si.Property.stBit.mantle.IsTrue() && !(pi.m_cap.stBit.mantle.IsTrue() || pi.m_cap.stBit.game_master.IsTrue()))
					throw new exception("[game_server::requestLogin][Error] Player[UID=" + (pi.uid) + ", CAP=" + (pi.m_cap.ulCapability)
							+ "] nao tem a capacidade para entrar no server mantle.");
				// Verifica se o Player já está logado
				var player_logado = HasLoggedWithOuterSocket(_session);

				if (player_logado != null)
				{
					//if (!m_login_manager.canSameIDLogin())
					//{
					//	_smp.Message_Pool.push(("[game_server::requestLogin][Log] Player[UID=" + (_pi.uid) + ", OID="
					//		+ (_session.m_oid) + ", IP=" + _session.getIP() + "] que esta logando agora, ja tem uma outra session com o mesmo UID logado, desloga o outro Player[UID="
					//		+ (player_logado.getUID()) + ", OID=" + (player_logado.m_oid) + ", IP=" + player_logado.getIP() + "]", CL_FILE_LOG_AND_CONSOLE));

					////	if (!DisconnectSession(player_logado))
					////		throw new exception("[game_server::requestLogin][Error] Nao conseguiu disconnectar o player[UID=" + (player_logado.getUID())
					////			+ ", OID=" + (player_logado.m_oid) + ", IP=" + player_logado.getIP() + "], ele pode esta com o bug do oid bloqueado, ou Session::UsaCtx bloqueado.");
					//}
				}

				// Junta Flag de block do server, ao do player
				pi.block_flag.m_flag.ullFlag |= m_si.flag.ullFlag;

				// Authorized a ficar online no server por tempo indeterminado
				_session.m_is_authorized = 1;

				// Registra no Banco de dados que o player está logado no Game Server
				snmdb.NormalManagerDB.add(5, new CmdRegisterLogon((int)pi.uid, 0/*Logou*/), SQLDBResponse, this);

				// Resgistra o Login do Player no server
				snmdb.NormalManagerDB.add(7, new CmdRegisterLogonServer(pi.uid, m_si.UID), SQLDBResponse, this);

				_smp.Message_Pool.push("[game_server::requestLogin][Log] Player[OID=" + (_session.m_oid) + ", UID=" + (pi.uid) + ", NICKNAME="
						+ (pi.nickname) + "] Autenticou com sucesso.");

				//// Verifica se o papel tem limite por dia, se não anula o papel shop do player
				//sPapelShopSystem.init_player_papel_shop_info(_session);

				//snmdb.NormalManagerDB.add(11, new CmdFirstAnniversary(), SQLDBResponse, this);


				// Cria o login manager para carregar o cache das informações e itens completo do player
				m_login_manager.createTask(ref _session, kol, _pi/*esses valores não vai usar mais se ficar tudo bem aqui no game_server*/, this);


				// Entra com sucesso
				p = new packet();

				packet_func_gs.pacote044(ref p, _session, m_si, 0xD3);

				// Entra com sucesso
				packet_func_gs.session_send(ref p, _session, 0);

			}
			catch (exception e)
			{

				// Error no login, set falso o autoriza o player a continuar conectado com o Game Server
				_session.m_is_authorized = 0;

				// Error Sistema
				p = new packet(0x044);

				// Pronto agora sim, mostra o erro que eu quero
				p.AddInt32(300);

				//packet_func.session_send(p,_session, 1);

				// Disconnect

				this.OnSessionClosed(_session);
			}
		}
        protected override void OnSessionClosed(Player _player, CloseReason reason = CloseReason.ClientClosing)
		{
			if (_player == null)
				throw new exception("[game_server::onDisconnected][Error] _session is nullptr");

			_smp.Message_Pool.push("[game_server::onDisconnected][Log] Player Desconectou. ID: " + (_player.m_pi.id) + "  UID: " + (_player.m_pi.uid));

			/// Novo
			var _channel = findChannel(_player.m_pi.channel);
			try
			{

				if (_channel != null)
					_channel.leaveChannel(_player);
			}
			catch (exception e) {

				_smp.Message_Pool.push("[game_server::onDisconnect][Error] " + e.getFullMessageError());
			}

			// Register Player Logon ON DB, 0 Login, 1 Logout
			snmdb::NormalManagerDB.add(5, new CmdRegisterLogon((int)_player.m_pi.uid, 1/*Logout*/), SQLDBResponse, this);
			base.OnSessionClosed(_player, reason);
		}
		protected override bool checkPacket(Player session, packet packet)
		{
			return true;
		}

		public override void SQLDBResponse(int _msg_id, Pangya_DB _pangya_db, object _arg)
		{
			if (_arg == null)
			{
				_smp.Message_Pool.push("[GameServer.SQLDBResponse][Error] _arg is null na msg_id = " + (_msg_id));
				return;
			}

			// Por Hora só sai, depois faço outro tipo de tratamento se precisar
			if (_pangya_db.getException().getCodeError() != 0)
				throw new exception("[GameServer.SQLDBResponse][Error] " + _pangya_db.getException().getFullMessageError());

			switch (_msg_id)
			{
				case 0: // Info Player
					{

						break;
					}
				case 1: // Key Login
					{


						break;
					}
				case 2: // Member Info - User Equip
					{
						break;
					}
				case 3: // User Equip - Desativa
					{
						break;
					}
				case 4: // Premium Ticket
					{

						break;
					}
				case 5: // Tutorial Info
					{
						break;
					}
			}

		
	}


        public virtual void destroyRoom(byte _channel_owner, short _number) { }


		public virtual void clear() { }

		public virtual Channel enterChannel(Player _session, byte _channel) { return null; }

		public virtual void sendChannelListToSession(Player _session) { }
		public virtual void sendServerListAndChannelListToSession(Player _session) { }
		public virtual void sendDateTimeToSession(Player _session) { }

		public virtual void sendRankServer(Player _session) { }

		public virtual Channel findChannel(sbyte _channel) { return v_channel.Find(c=> c.getId() == _channel); }

		public virtual Player findPlayer(uint _uid, bool _oid = false) { return null; }

		// find All GM Online
		public virtual List<Player> findAllGM() { return null; }

		public virtual void blockOID(uint _oid) { }
		public virtual void unblockOID(uint _oid) { }

		DailyQuestInfo getDailyQuestInfo() { return m_dqi; }

		public virtual LoginManager getLoginManager() { return m_login_manager; }


		// Login
		public virtual void requestLogin(Player _session, packet _packet) { }

		// Channel
		public virtual void requestEnterChannel(Player _session, packet _packet) { }

		public virtual void requestEnterOtherChannelAndLobby(Player _session, packet _packet) { }

		// Change Server
		public virtual void requestChangeServer(Player _session, packet _packet) { }

		// UCC::Self Design System [Info, Save, Web Key]
		public virtual void requestUCCWebKey(Player _session, packet _packet) { }
		public virtual void requestUCCSystem(Player _session, packet _packet) { }

		// Chat
		public virtual void requestChat(Player _session, packet _packet) { }

		// Chat Macro
		public virtual void requestChangeChatMacroUser(Player _session, packet _packet) { }

		// Request Player Info
		public virtual void requestPlayerInfo(Player _session, packet _packet) { }

		// Private Message
		public virtual void requestPrivateMessage(Player _session, packet _packet) { }
		public virtual void requestChangeWhisperState(Player _session, packet _packet) { }
		public virtual void requestNotifyNotDisplayPrivateMessageNow(Player _session, packet _packet) { }

		// Command GM
		public virtual void requestCommonCmdGM(Player _session, packet _packet) { }
		public virtual void requestCommandNoticeGM(Player _session, packet _packet) { }

		// Request translate Sub Packet
		public virtual void requestTranslateSubPacket(Player _session, packet _packet) { }

		// Ticker
		public virtual void requestSendNotice(string notice) { }

		public virtual void requestSendTicker(Player _session, packet _packet) { }
		public virtual void requestQueueTicker(Player _session, packet _packet) { }

		// Exception Client Message
		public virtual void requestExceptionClientMessage(Player _session, packet _packet) { }

		// Game Guard Auth
		public virtual void requestCheckGameGuardAuthAnswer(Player _session, packet _packet) { }

		// Set Rate Server

		// Set Event Server
		public virtual void setAngelEvent(uint _angel_event) { }

		// Update Daily Quest Info
		public virtual void updateDailyQuest(DailyQuestInfo _dqi) { }

		// send Update Room Info, find room nos canais e atualiza o info
	//	public virtual void sendUpdateRoomInfo(room _r, int _option) { }


		player_manager m_player_manager;


		public virtual bool checkCommand(string[] _command) { return true; }

			public virtual void reload_files() { }

		public virtual void init_systems() { }
		public virtual void init_packets()
		{
			this.addPacketCall(0x02, packet_func_gs.packet002);
			this.addPacketCall(0x03, packet_func_gs.packet003);
			this.addPacketCall(0x04, packet_func_gs.packet004);
            this.addPacketCall(0x81, packet_func_gs.packet081);
        }
        public virtual void init_load_channels() {
			ChannelInfo ci = new ChannelInfo();
			int num_channel = Ini.readInt("CHANNELINFO", "NUM_CHANNEL");

			for (sbyte i = 0; i < num_channel; ++i)
			{
				ci.id = i;
				ci.name = Ini.ReadString("CHANNEL" + (i + 1), "NAME");
				ci.max_user = Ini.ReadInt16("CHANNEL" + (i + 1), "MAXUSER");

				try
				{
					ci.flag.ulFlag = Ini.ReadUInt32("CHANNEL" +(i + 1), "FLAG");
				}
				catch (Exception e) {

					_smp.Message_Pool.push("[game_server::init_load_channels][ErrorSystem] " + e.Message);
			}

			v_channel.Add(new Channel(ci, m_si.Property.ulProperty));
		}
	}
		public virtual void reload_systems() { }
		public virtual void reloadGlobalSystem(uint _tipo) { }

		// Update Rate e Event of Server
		public virtual void updateRateAndEvent(uint _tipo, uint _qntd) { }

		// Shutdown With Time


		// Check Player Itens

		public virtual void check_player() { }

		// Make Grand Zodiac Event Room
		public virtual void makeGrandZodiacEventRoom() { }

		// Make List of Players to Golden Time Event
		public virtual void makeListOfPlayersToGoldenTime() { }

		// Make Bot GM Event Room
		public virtual void makeBotGMEventRoom() { }

        protected override void onAcceptCompleted(Player _session)
        {
            try
            {
                var packet = new packet(0x3F);	// Tipo Packet Game Server initial packet no compress e no crypt

                packet.AddByte(1);	// OPTION 1
                packet.AddByte(1);	// OPTION 2
                packet.AddByte(_session.m_key);	// Key
                packet.MakeRaw();
                var mb = packet.GetMakedBuf().Buffin;
                _session.Send(mb);
            }
            catch (Exception ex)
            {
                _smp.Message_Pool.push(ex.Message, "AppServer::onAcceptCompleted", 808);
            }
        }
    }
}
