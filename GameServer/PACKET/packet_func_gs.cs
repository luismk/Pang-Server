using GameServer.ServerTcp;
using System;
using System.Collections.Generic;
using System.Linq;
using _smp = PangyaAPI.Utilities.Log;
using packet = PangyaAPI.SuperSocket.SocketBase.Packet;
using PangyaAPI.SQL.DATA.TYPE;
using GameServer.TYPE;
using System.Runtime.InteropServices;
using GameServer.Game;
using PangyaAPI.Utilities;
using static GameServer.TYPE.DefineConstants;
using MemberInfo = GameServer.TYPE.MemberInfo;
using GameServer.Session;
using PangyaAPI.SuperSocket.SocketBase;

namespace GameServer.PACKET
{
    public static class packet_func
    {
        static int MAX_BUFFER_PACKET = 1000;
        static int total;
        static int por_packet;
        static packet m_p;
        static multimap<uint, WarehouseItemEx> m_element;
        static int elements;
        static int m_element_size;
        static int max_packet = MAX_BUFFER_PACKET;
        #region Call Packet
        static GameServerTcp gs = Program.server;
        public static void packet002(ParamDispatch _arg1)
        {
            try
            {
                gs.requestLogin(_arg1._packet, (Player)_arg1._session);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static void packet003(ParamDispatch pd)
        {

            try
            {

                gs.requestChat((Player)pd._session, pd._packet);

            }
            catch
            {

                _smp.Message_Pool.push("[packet_func::packet003][ErrorSystem] ");


            }
        }

        public static void packet004(ParamDispatch pd)
        {
            try
            {
                // Enter Channel, channel ID
                gs.requestEnterChannel((Player)pd._session, pd._packet);
            }
            catch
            {
                _smp.Message_Pool.push("[packet_func::packet004][ErrorSystem] ");
            }
        }


        public static void packet081(ParamDispatch pd)
        {
            var player = ((Player)pd._session);
            try
            {
                var c = gs.findChannel(player.m_pi.channel);

                if (c != null)
                    c.requestEnterLobby(player, pd._packet);
            }
            catch (exception)
            {
            }
        }
        public static void packet082(ParamDispatch pd)
        {
            var player = ((Player)pd._session);

            try
            {
                var c = gs.findChannel(player.m_pi.channel);

                if (c != null)
                    c.requestExitLobby(player, pd._packet);
            }
            catch (exception)
            {
            }
        }
        #endregion

        #region Response Packet
        public static void principal(ref packet p, PlayerInfo pi, ServerInfoEx _si)
        {
            if (pi == null)
                throw new Exception("Erro PlayerInfo *pi is null. packet_func::principal()");

            int st_i = 0;
            PangyaTime si = new PangyaTime();

            // Clinte Versao
            p.AddString(_si.Version_Client);
            p.AddString(_si.Version);
            // member info
            p.AddInt16(pi.mi.sala_numero);
            p.AddBuffer(pi.mi, Marshal.SizeOf(new MemberInfo()));

            // User Info
            p.AddUInt32(pi.uid);
            p.AddBuffer(pi.ui, Marshal.SizeOf(new UserInfo()));

            // Trofel Info
            p.AddBuffer(pi.ti_current_season, Marshal.SizeOf(new TrofelInfo()));

            // User Equip
            p.AddBuffer(pi.ue, Marshal.SizeOf(new UserEquip()));

            // Map Statistics Normal
            for (st_i = 0; st_i < MS_NUM_MAPS; st_i++)
                p.AddBuffer(pi.a_ms_normal[st_i], Marshal.SizeOf(new MapStatistics()));

            // Map Statistics Natural
            for (st_i = 0; st_i < MS_NUM_MAPS; st_i++)
                p.AddBuffer(pi.a_ms_natural[st_i], Marshal.SizeOf(new MapStatistics()));

            for (var j = 0; j < 9/*season's*/; j++)
                // Map Statistics Normal
                for (st_i = 0; st_i < MS_NUM_MAPS; st_i++)
                    p.AddBuffer(pi.aa_ms_normal_todas_season[j, st_i], Marshal.SizeOf(new MapStatistics()));

            //Character Info(CharEquip)
            if (pi.ei.char_info != null)
                p.AddBuffer(pi.ei.char_info, Marshal.SizeOf(new CharacterInfo()));
            else
                p.AddZeroByte(Marshal.SizeOf(new CharacterInfo()));

            // Caddie Info
            if (pi.ei.cad_info != null)
                p.AddBuffer(pi.ei.cad_info.getInfo(), Marshal.SizeOf(new CaddieInfo()));
            else
                p.AddZeroByte(Marshal.SizeOf(new CaddieInfo()));

            // Club Set Info
            p.AddBuffer(pi.ei.csi, Marshal.SizeOf(new ClubSetInfo()));

            // Mascot Info
            if (pi.ei.mascot_info != null)
                p.AddBuffer(pi.ei.mascot_info, Marshal.SizeOf(new MascotInfo()));
            else
                p.AddZeroByte(Marshal.SizeOf(new MascotInfo()));

            // Date Atual
            si.CreateTime();  // Local

            p.AddBuffer(si);

            // Config do Server
            p.AddInt16(0);             // Esse é o valor 2 que o JP passa, 1 primeira vez que loga, 2 ele já logou uma ou mais vezes
            p.AddBuffer(pi.mi.papel_shop);
            p.AddInt32(0); // estava 2 aqui, mas com nova conta é 0 no JP							// Novo no JP, tbm não sei o que é
            p.AddInt16(0); // estava 2 aqui, mas com nova conta é 0 no JP							// Novo no JP, tbm não sei o que é
            p.AddInt64(0/*_si.flag.ullFlag |pi.block_flag.m_flag.ullFlag*/);           // Flag do server, de block os sistemas
            p.AddInt32(363);                       // Aqui é a quantidade de vezes que logou
            p.AddInt32(0);  // Property do server
            p.AddBuffer(pi.gi, Marshal.SizeOf(new GuildInfo()));
        }
        public static void pacote046(ref packet p, Player _session, List<PlayerCanalInfo> v_element, int option)
        {

            var elements = v_element.Count();

            if (elements * Marshal.SizeOf(new PlayerCanalInfo()) < (MAX_BUFFER_PACKET - 100))
            {
                p.init_plain(0x46);
                p.AddByte((byte)option);
                p.AddByte((byte)elements);

                for (int i = 0; i < v_element.Count(); i++)
                    p.AddBuffer(v_element[i], Marshal.SizeOf(new PlayerCanalInfo()));
            }
            else
            {
                //MAKE_BEGIN_SPLIT_PACKET(0x46, _session, MAX_BUFFER_PACKET);

                //p.AddByte((byte)option);
                //p.AddByte((byte)((total > por_packet) ? por_packet : total));

                //MAKE_MID_SPLIT_PACKET_VECTOR();

                //MAKE_END_SPLIT_PACKET(1);
            }

        }
        internal static void pacote11F(ref packet p, Player Player, PlayerInfo pi, short tipo)
        {
            if (pi == null)
                throw new Exception("Erro PlayerInfo *pi is nullptr. packet_func::pacote11F()");

            p.init_plain(0x11F);

            p.AddInt16(tipo);

            p.AddBuffer(pi.TutoInfo, Marshal.SizeOf<TutorialInfo>());
        }

        internal static void pacote1A9(ref packet p, Player Player, int ttl_milliseconds/*time to live*/, int option = 0)
        {
            p.init_plain(0x1A9);

            p.AddByte((byte)option);

            p.AddInt32(ttl_milliseconds);
        }

        internal static void pacote095(ref packet p, Player session, short sub_tipo, int option = 0, PlayerInfo pi = null)
        {
            p.init_plain(0x95);

            p.AddInt16(sub_tipo);

            if (sub_tipo == 0x102)
                p.AddByte((byte)option);

            else if (sub_tipo == 0x111)
            {
                p.AddInt32(option);

                if (pi == null)
                {
                    //delete p;

                    throw new Exception("Erro PlayerInfo *pi is nullptr. packet_func::pacote095()");
                }

                p.AddQWord(pi.ui.pang);
            }
        }

        public static void pacote25D(ref packet p, Player m_session, List<TrofelEspecialInfo> v_tgp_current_season, int v)
        {
            // throw new NotImplementedException();
        }

        public static void pacote158(ref packet p, Player m_session, uint _uid, UserInfoEx _ui, byte season)
        {
            p.init_plain(0x158);

            p.AddByte((byte)season);

            p.AddUInt32(_uid);

            p.AddBuffer(_ui, Marshal.SizeOf(new UserInfo()));
        }

        public static void pacote096(ref packet p, Player m_session, PlayerInfo pi)
        {
            if (pi == null)
                throw new Exception("Erro PlayerInfo *pi is nullptr. packet_func::pacote096()");

            p.init_plain(0x96);

            p.AddQWord(pi.cookie);

            // throw new NotImplementedException();
        }

        public static void pacote181(ref packet p, Player m_session, List<ItemBuffEx> v_ib)
        {
            // throw new NotImplementedException();
        }

        public static void pacote13F(ref packet p, Player m_session)
        {
            // throw new NotImplementedException();
        }

        public static void pacote137(ref packet p, Player m_session, List<CardEquipInfoEx> v_cei)
        {
            // throw new NotImplementedException();
        }

        public static void pacote136(ref packet p, Player m_session)
        {
            // throw new NotImplementedException();
        }

        public static void pacote138(ref packet p, Player m_session, List<CardInfo> v_card_info)
        {
            // throw new NotImplementedException();
        }

        public static void pacote135(ref packet p, Player m_session)
        {
            // throw new NotImplementedException();
        }

        public static void pacote131(packet p)
        {
            // throw new NotImplementedException();
        }

        public static void pacote072(ref packet p, Player m_session, UserEquip ue)
        {
            p.init_plain(0x72);

            p.AddBuffer(ue, Marshal.SizeOf(new UserEquip()));
        }

        public static void pacote0E1(ref packet p, Player m_session, SortedList<uint, MascotInfoEx> v_element, int option = 0)
        {
            p.init_plain(0xE1);

            p.AddByte((byte)(v_element.Count & 0xFF));

            foreach (var item in v_element.Values)
                p.AddBuffer(item, Marshal.SizeOf(new MascotInfo()));
        }

        public static void pacote073(ref packet p, Player m_session, multimap<uint, WarehouseItemEx> v_element, int option = 0)
        {
            var elements = v_element.Count();
            if (elements * Marshal.SizeOf(new WarehouseItem()) < (MAX_BUFFER_PACKET - 100))
            {
                p.init_plain(0x73);
                p.AddInt16((short)elements);
                p.AddInt16((short)elements);

                foreach (var item in v_element.GetValues())
                {
                    p.AddBuffer(item, new WarehouseItem());
                }
            }
            else
            {
                p.init_plain(0x73);
                p.AddInt16((short)elements);
                p.AddInt16((short)elements);

                foreach (var item in v_element.GetValues())
                {
                    p.AddBuffer(item, new WarehouseItem());
                }

            }
        }

        public static void pacote071(ref packet p, Player m_session, SortedList<uint, CaddieInfoEx> v_element, int option = 0)
        {
            var elements = v_element.Count();

            if (elements * Marshal.SizeOf(new CharacterInfo()) < (MAX_BUFFER_PACKET - 100))
            {
                p.init_plain(0x71);
                p.AddInt16((short)elements);
                p.AddInt16((short)elements);

                foreach (var item in v_element.Values)
                {
                    p.AddBuffer(item.getInfo());
                }
            }
            else
            {
                p.init_plain(0x71);
                p.AddInt16((short)elements);
                p.AddInt16((short)elements);

                foreach (var item in v_element.Values)
                {
                    p.AddBuffer(item.getInfo());
                }
            }
        }

        public static void pacote070(ref packet p, Player m_session, SortedList<uint, CharacterInfoEx> v_element, int option = 0)
        {
            var elements = v_element.Count();

            if (elements * Marshal.SizeOf(new CharacterInfo()) < (MAX_BUFFER_PACKET - 100))
            {
                p.init_plain(0x70);
                p.AddInt16((short)elements);
                p.AddInt16((short)elements);

                foreach (var item in v_element.Values)
                {
                    p.AddBuffer(item);
                }
            }
            else
            {
                p.init_plain(0x70);
                p.AddInt16((short)elements);
                p.AddInt16((short)elements);

                foreach (var item in v_element.Values)
                {
                    p.AddBuffer(item);
                }
            }
        }


        public static void pacote04D(ref packet p, Player m_session, List<Channel> v_element, int option = 0)
        {

            p.init_plain(0x4D);

            p.AddByte((byte)v_element.Count);

            for (var i = 0; i < v_element.Count; ++i)
                p.AddBuffer(v_element[i].getInfo(), Marshal.SizeOf<ChannelInfo>());

        }

        public static void pacote04E(ref packet p, Player session, int option, int _codeErrorInfo = 0)
        {
            /* Option Values
                * 1 Sucesso
                * 2 Channel Full
                * 3 Nao encontrou canal
                * 4 Nao conseguiu pegar informções do canal
                * 6 ErrorCode Info
                */

            p.init_plain(0x4E);

            p.AddByte((byte)option);

            if (_codeErrorInfo != 0)
                p.AddInt32(_codeErrorInfo);
        }

        public static void pacote044(ref packet p, Player _session, ServerInfoEx _si, int option, PlayerInfo pi = null, int valor = 0)
        {
            if (option == 0 && pi == null)
                throw new Exception("Erro PlayerInfo *pi is nullptr. packet_func::pacote044()");

            p.init_plain(0x44);

            p.AddByte((byte)(option & 0xFF));   // Option

            if (option == 0)
                principal(ref p, pi, _si);
            else if (option == 1)
                p.AddByte(0);
            else if (option == 0xD3)
                p.AddByte(0);
            else if (option == 0xD2)
                p.AddInt32(valor);
        }



        public static void session_send(ref packet p, Player s, int _debug)
        {
            if (s == null)
                throw new Exception("[packet_func::session_send][Error] session *s is nullptr.");

            s.Send(p.GetPlainBuf().Buffin, true);
#if _RELEASE
            if(_debug == 1)
            {
                _smp.Message_Pool.push($"[SEND_PACKET_LOG]: PacketSize({p.GetBytes.Length}) \t\n{p.GetBytes.HexDump()}");
            }
#endif
            p.Clear();//@! pode esta causando falha aqui
        }


        // Metôdos de auxílio de criação de pacotes
        public static void channel_broadcast(Channel _channel, ref packet p, int _debug)
        {

            vector<Player> channel_session = _channel.getSessions();  //gs->getSessionPool().getChannelSessions(s->m_channel);

            for (var i = 0; i < channel_session.Count(); ++i)
            {
                channel_session[i].Send(p.GetBuffer());//@!errado
            }

            p.Clear();
        }

      public static  void lobby_broadcast(Channel _channel, packet p, byte _debug)
        {

           vector<Player> channel_session = _channel.getSessions();  //gs->getSessionPool().getChannelSessions(s->m_channel);

            for (var i = 0; i < channel_session.Count(); ++i)
            {
                if (channel_session[i].m_pi.mi.sala_numero == -1)
                {  
                }
            }

            //delete p;
        }

        #endregion

    }
}
