﻿using PangyaAPI.SQL.DATA.TYPE;
using PangyaAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using BlockFlag = PangyaAPI.SQL.DATA.TYPE.BlockFlag;
//using IFF = PangyaAPI.IFF.Models;
//using sIff = PangyaAPI.IFF.GB.Handle.IFFInit;
namespace GameServer.TYPE
{

    internal static class DefineConstants
    {
        public const uint CLEAR_10_DAILY_QUEST_TYPEID = 0x78800001; // Quest 10 clear daily quest
        public const uint ASSIST_ITEM_TYPEID = 0x1BE00016;
        public const uint GRAND_PRIX_TICKET = 0x1A000264;
        public const uint LIMIT_GRAND_PRIX_TICKET = 50; // Limit de Grand Prix Ticket que o player pode ter, chegou nesse limit não drop mais ele do hole
        public const uint MULLIGAN_ROSE_TYPEID = 0x1800000E;
        public const uint DEFAULT_COMET_TYPEID = 0x14000000;
        public const uint AIR_KNIGHT_SET = 0x10000000;
        public const uint CLUB_PATCHER_TYPEID = 0x1A00018F;
        public const uint MILAGE_POINT_TYPEID = 0x1A0002A7;
        public const uint TIKI_POINT_TYPEID = 0x1A0002A6;
        public const uint SPECIAL_SHUFFLE_COURSE_TICKET_TYPEID = 0x1A0000F7;
        public const uint PANG_POUCH_TYPEID = 0x1A000010;
        public const uint EXP_POUCH_TYPEID = 0x1A00015D;
        public const uint CP_POUCH_TYPEID = 0x1A000160;
        public const uint DECREASE_COMBO_VALUE = 3; // No JP é 10, no USA era 3
        public const float MEDIDA_PARA_YARDS = 0.3125f; // Usava 0.31251 Medida uinterna do pangya que no visual é o Yards
        // Icon Player Good(angel), Quiter 1 e 2 
        public const float GOOD_PLAYER_ICON = 3.0f;
        public const float QUITER_ICON_1 = 20.0f;
        public const float QUITER_ICON_2 = 30.0f;

        // Corta com toma, e corta com safety
        public static readonly uint[] active_item_cant_have_2_inveroty = { 402653229u, 402653231u };

        public const uint TROFEL_GM_EVENT_TYPEID = 0x2D0A3B00;

        public const byte cadie_cauldron_Hermes_random_id = 2;
        public const byte cadie_cauldron_Jester_random_id = 3;
        public const byte cadie_cauldron_Twilight_random_id = 4;
        public const int MS_NUM_MAPS = 20;
        public const int INVALID_CHANNEL = ~0;
    }

    public partial class player_info
    {
        public uint uid { get; set; }
        public BlockFlag block_flag { get; set; }
        public short level { get; set; }
        public string id { get; set; }
        public string nickname { get; set; }
        public string pass { get; set; }
        public player_info()
        {
            block_flag = new BlockFlag();
            id = "";
            nickname = "";
            pass = "";
        }
        public PlayerInfo GetInfo()
        {
            return new PlayerInfo(id, block_flag, level, nickname, pass, (int)uid);
        }
    }
    // MemberInfo dados principais do player, tem id, nick, guild, level, exp, e etc)
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 261)]
    public class MemberInfo
    {
        public MemberInfo()
        {
            clear();
        }
        protected virtual void clear()
        {
            oid = uint.MaxValue;
            id = "luismk";
            nick_name = "MK(e7)";
            guild_name = "";
            guild_mark_img = "GUILDMARK";
            state_flag = new uMemberInfoStateFlag();
            capability = new uCapability();
            papel_shop = new PlayerPapelShopInfo();
            unk = "";
            nickNT = "@NTREEV_ID";
            ucUnknown107 = "";
        }
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 22)]

        public string id { get; set; }
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 22)]
        public string nick_name { get; set; }
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string guild_name { get; set; }
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string guild_mark_img { get; set; }
        public uCapability capability;
        public uint ulUnknown2 { get; set; }            // ainda não o que é aqui direito
        public uint oid { get; set; }
        public uint ulUnknown3 { get; set; }            // ainda não o que é aqui direito
        public ulong ullUnknown { get; set; }        // ainda não o que é aqui direito
        public ulong guild_uid { get; set; }
        public uMemberInfoStateFlag state_flag { get; set; }
        public PlayerPapelShopInfo papel_shop { get; set; }
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string unk { get; set; }
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 18)]
        public string nickNT { get; set; }                // ainda não sei direito o que tem aqui
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 110)]
        public string ucUnknown107 { get; set; } // ainda não sei direito o que tem aqui
    }

    // MemberInfoEx extendido tem o uid, limite papel shop e tutorial,
    // so os que nao manda para o pangya no pacote MemberInfo
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class MemberInfoEx : MemberInfo
    {
        public MemberInfoEx()
        {
            clear();
            capability = new uCapability();
            state_flag = new uMemberInfoStateFlagEx();
            papel_shop_last_update = new PangyaTime();
        }
        public new uMemberInfoStateFlagEx state_flag;
        public uint uid;
        public uint guild_point;
        public long guild_pang;
        public uint guild_mark_img_no;
        public short sala_numero = -1;
        public byte sexo;
        public byte level;
        public byte do_tutorial;
        public byte event_1;
        public byte event_2;
        public uint school;
        public uint manner_flag;
        public PangyaTime papel_shop_last_update;
    }

    // Union Capability, que guarda a estrutura de bits do Capability do Player
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 4)]
    public class uCapability
    {
        [field: MarshalAs(UnmanagedType.U4, SizeConst = 4)]
        public uint ulCapability 
        {
                      get;
            set; 
        }

        public _stBit stBit;
        public uCapability(uint ul = 0)
        {
            ulCapability = ul;
            stBit = new _stBit();
            setState();
        }

        public class _stBit
        {
            public uint A_I_MODE;                    // Inteligência Artificial Modo
            public uint un1;                         // Unknwon
            public uint game_master;             // GM(Game Master)
            public uint gm_edit_site;                // GM(Game Master) que pode mexer na parte adm do site
            public uint block_give_item_gm;      // Bloquea o GM de enviar item pelo comando Goldenbell e giveitem
            public uint un2;                         // Unknown
            public uint gm_normal;               // GM player normal, é para poder voltar o GM novamente, isso é para quando o GM quer ficar normal player para poder jogar novamente
            public uint un3;                         // Unknown
            public uint mantle;                  // Player Autorizado ao entrar no matle(Server escondido para usuários normais), server para testes
            public uint un4;                         // Unknown
            public uint premium_user;                // Premium User
            public uint title_gm;           // Title de GM, mostra o Nome GM no lugar no Level ou o Title que o player esteja
        }

        public void setState()
        {
            switch (ulCapability)
            {
                case 4:
                    stBit.game_master = 1;
                    break;
                case 15:
                    stBit.A_I_MODE = 1;
                    break;
                default:
                    break;
            }
        }
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class uMemberInfoStateFlag
    {
        [field: MarshalAs(UnmanagedType.U2, SizeConst = 2)]
        public short ucByte { get; set; }
        public _stBit stFlagBit { get; set; }
        public class _stBit
        {
            public byte channel = 1;          // channel
            public byte visible = 1;          // Visible
            public byte whisper = 1;          // Whisper
            public byte sexo = 1;             // Genero - (ACHO)Já logou mais de uma vez
            public byte azinha = 1;           // Azinha, Quit rate menor que 3%
            public byte icon_angel = 1;       // Angel Wings
            public byte quiter_1 = 1;         // Quit rate maior que 31% e menor que 41%
            public byte quiter_2 = 1;         // Quit rate maior que 41%
        }
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class uMemberInfoStateFlagEx : uMemberInfoStateFlag
    {
        public uMemberInfoStateFlagEx()
        {
            stFlagBit = new _stBit();
        }
      
    }

    // Player Papel Shop Info
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class PlayerPapelShopInfo
    {
        public short remain_count;
        public short current_count;
        public short limit_count;
        public PlayerPapelShopInfo()
        {
            remain_count = -1;
            current_count = -1;
            limit_count = -1;
        }
    }

    // Medal Win
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class uMedalWin
    {
        [field: MarshalAs(UnmanagedType.U1, SizeConst = 1)]
        public byte ucMedal { get; set; }
        public _stMedal stMedal { get; set; }
       public uMedalWin()
        {
            stMedal = new _stMedal();
        }
        public class _stMedal
        {
            public byte lucky = 1;
            public byte speediest = 1;
            public byte best_drive = 1;
            public byte best_chipin = 1;
            public byte best_long_puttin = 1;
            public byte best_recovery = 0;
        }
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class UserInfo
    {
        public UserInfo()
        {
            clear();
        }
        protected void clear()
        {
            best_pang = new long[5];
            best_score = new byte[5];
            medal = new stMedal();
        }
        public void add(UserInfo _ui)
        {

            if (_ui.best_drive > best_drive)
                best_drive = _ui.best_drive;

            if (_ui.best_long_putt > best_long_putt)
                best_long_putt = _ui.best_long_putt;

            if (_ui.best_chip_in > best_chip_in)
                best_chip_in = _ui.best_chip_in;

            // Combo e Todal Combos
            if (_ui.combo < 0)
            {   // Negativo

                // tira só do combo, não de todos os combos que foram feitos
                if (combo <= DefineConstants.DECREASE_COMBO_VALUE)
                    combo = 0;
                else
                    combo += _ui.combo;

            }
            else
            {                   // Positivo

                combo += _ui.combo;

                // Só soma o all combo se combo > que all_combo
                if (combo > all_combo)
                    all_combo += _ui.combo;
            }

            // Event Angel ativado, quitado < 0
            if (_ui.quitado < 0)
            {

                // Se for 0 não subtrai
                if ((quitado + _ui.quitado) <= 0)
                    quitado = 0;
                else
                    quitado += _ui.quitado;

            }
            else // Normal soma o quit do player se ele quitou
                quitado += _ui.quitado;

            // Skin (Pang Battle)
            if ((skin_all_in_count + _ui.skin_all_in_count) >= 5)
            {

                skin_all_in_count = 0;
                skin_pang += 1000; // dá 1000 pangs por que ele jogou 5 Pang Battle

            }
            else
                skin_all_in_count += _ui.skin_all_in_count;

            tacada += _ui.tacada;
            putt += _ui.putt;
            tempo += _ui.tempo;
            tempo_tacada += _ui.tempo_tacada;
            acerto_pangya += _ui.acerto_pangya;
            timeout += _ui.timeout;
            ob += _ui.ob;
            total_distancia += _ui.total_distancia;
            hole += _ui.hole;
            hole_in += (_ui.hole - _ui.hole_in);
            hio += _ui.hio;
            bunker += _ui.bunker;
            fairway += _ui.fairway;
            albatross += _ui.albatross;
            putt_in += _ui.putt_in;
            media_score += _ui.media_score;
            best_score[0] += _ui.best_score[0];
            best_score[1] += _ui.best_score[1];
            best_score[2] += _ui.best_score[2];
            best_score[3] += _ui.best_score[3];
            best_score[4] += _ui.best_score[4];
            best_pang[0] += _ui.best_pang[0];
            best_pang[1] += _ui.best_pang[1];
            best_pang[2] += _ui.best_pang[2];
            best_pang[3] += _ui.best_pang[3];
            best_pang[4] += _ui.best_pang[4];
            sum_pang += _ui.sum_pang;
            event_flag += _ui.event_flag;
            jogado += _ui.jogado;
            team_game += _ui.team_game;
            team_win += _ui.team_win;
            team_hole += _ui.team_hole;
            ladder_point += _ui.ladder_point;
            ladder_hole += _ui.ladder_hole;
            ladder_win += _ui.ladder_win;
            ladder_lose += _ui.ladder_lose;
            ladder_draw += _ui.ladder_draw;
            skin_pang += _ui.skin_pang;
            skin_win += _ui.skin_win;
            skin_lose += _ui.skin_lose;
            skin_run_hole += _ui.skin_run_hole;
            skin_strike_point += _ui.skin_strike_point;
            jogados_disconnect += _ui.jogados_disconnect;
            event_value += _ui.event_value;
            game_count_season += _ui.game_count_season;

            // Medal
            medal.add(_ui.medal);

        }
        public int tacada { get; set; }
        public int putt { get; set; }
        public int tempo { get; set; }
        public int tempo_tacada { get; set; }
        public float best_drive { get; set; }           // Max Distancia
        public int acerto_pangya { get; set; }
        public int timeout { get; set; }
        public int ob { get; set; }
        public int total_distancia { get; set; }
        public int hole { get; set; }
        public int hole_in { get; set; }        // Aqui é os holes que não foram concluídos Ex: Give up, ou no Match o outro player ganho sem precisar do player terminar o hole
        public int hio { get; set; }
        public short bunker { get; set; }
        public int fairway { get; set; }
        public int albatross { get; set; }
        public int mad_conduta { get; set; }    // Aqui é hole in, mas no info não tras ele por que ele já foi salvo no hole alí em cima
        public int putt_in { get; set; }
        public float best_long_putt { get; set; }
        public uint exp { get; set; }
        public byte level { get; set; }
        public ulong pang { get; set; }
        public float best_chip_in { get; set; }
        public int media_score { get; set; }
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x05)]
        public byte[] best_score { get; set; }             // Best Score Por Estrela, mas acho que o pangya nao usa mais isso
        public byte event_flag { get; set; }
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x05)]
        public long[] best_pang { get; set; }           // Best Pang por Estrela, mas acho que o pangya nao usa mais isso
        public long sum_pang { get; set; }               // A soma do pangs das 5 estrela acho
        public int jogado { get; set; }
        public int team_hole { get; set; }
        public int team_win { get; set; }
        public int team_game { get; set; }
        public int ladder_point { get; set; }               // Ladder é o Match acho, de tourneio não sei direito
        public int ladder_hole { get; set; }
        public int ladder_win { get; set; }
        public int ladder_lose { get; set; }
        public int ladder_draw { get; set; }
        public int combo { get; set; }
        public int all_combo { get; set; }
        public int quitado { get; set; }
        public long skin_pang { get; set; }          // Skin é o Pang Battle tem valor negativo ele """##### Ajeitei agora(ACHO)
        public int skin_win { get; set; }
        public int skin_lose { get; set; }
        public int skin_all_in_count { get; set; }
        public int skin_run_hole { get; set; }              // Correu desistiu (ACHO)
        public int skin_strike_point { get; set; }          // Antes era o nao_sei
        public int jogados_disconnect { get; set; }     // Antes era o jogos_nao_sei
        public short event_value { get; set; }//98
        public int game_count_season { get; set; }
        public short _16bit_nao_sei { get; set; }
        [field: MarshalAs(UnmanagedType.Struct, SizeConst = 6)]
        public stMedal medal { get; set; }
        public float getMediaScore()
        {   // AVG SCORE

            // Verifica se é 0, por que não pode dividir 18 por 0 que dá excessão, 
            // por que não pode dividir nenhum número por 0
            if ((hole - hole_in) == 0)
                return 0;

            return (18 / (hole - hole_in)) * media_score + 72;
        }
        public float getPangyaShotRate()
        {

            // Previne divisão por 0
            if (tacada == 0)
                return 0;

            return ((float)acerto_pangya / tacada) * 100;
        }
        public float getFairwayRate()
        {

            // Previne divisão por 0
            if ((hole - hole_in) == 0)
                return 0;

            return ((float)fairway / (hole - hole_in)) * 100;
        }
        public float getPuttRate()
        {

            // Previne divisão por 0
            if (putt == 0)
                return 0;

            return ((float)putt_in / putt) * 100;
        }
        public float getOBRate()
        {

            // Previne divisão por 0
            if ((tacada + putt) == 0)
                return 0;

            return ((float)ob / (tacada + putt)) * 100;
        }
        public float getMatchWinRate()
        {

            // Previne divisão por 0
            if (team_game == 0)
                return 0;

            return ((float)team_win / team_game) * 100;
        }
        public float getShotTimeRate()
        {

            // Previne divisão por 0
            if ((tacada + putt) == 0)
                return 0;

            return ((float)tempo_tacada / (tacada + putt)) * 100;
        }
        public float getQuitRate()
        {

            // Previne divisão por 0
            if (jogado == 0)
                return 0;

            return quitado * 100 / jogado;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class UserInfoEx : UserInfo
    {
        public UserInfoEx()
        {
        }

        public void add(UserInfoEx _ui, uint _game_win_total_pang)
        {

            base.add(_ui);

            //if (_game_win_total_pang > 0)
            //    game_win_total_pang += _game_win_total_pang;

        }
        //public ulong game_win_total_pang { get; set; }
        //public uint disconnect { get; set; }
        //public uint sys_school_serie { get; set; }
    }

    // Medal
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class stMedal
    {
        public void add(stMedal _medal)
        {
            lucky += _medal.lucky;
            fast += _medal.fast;
            best_drive += _medal.best_drive;
            best_chipin += _medal.best_chipin;
            best_puttin += _medal.best_puttin;
            best_recovery += _medal.best_recovery;
        }
        public void add(uMedalWin _medal_win)
        {
            if (_medal_win.stMedal.lucky == 1)
                lucky++;
            else if (_medal_win.stMedal.speediest == 1)
                fast++;
            else if (_medal_win.stMedal.best_drive == 1)
                best_drive++;
            else if (_medal_win.stMedal.best_chipin == 1)
                best_chipin++;
            else if (_medal_win.stMedal.best_long_puttin == 1)
                best_puttin++;
            else if (_medal_win.stMedal.best_recovery == 1)
                best_recovery++;
        }
        public byte lucky { get; set; }
        public byte fast { get; set; }
        public byte best_drive { get; set; }
        public byte best_chipin { get; set; }
        public byte best_puttin { get; set; }
        public byte best_recovery { get; set; }
    }
    /// <summary>
    /// System time public classure based on Windows uinternal SYSTEMTIME public class
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 16)]
    public class PangyaTime
    {
        /// <summary>
        /// Year
        /// </summary>
        public ushort Year { get; set; }

        /// <summary>
        /// Month
        /// </summary>
        public ushort Month { get; set; }

        /// <summary>
        /// Day of Week
        /// </summary>
        public ushort DayOfWeek { get; set; }

        /// <summary>
        /// Day
        /// </summary>
        public ushort Day { get; set; }

        /// <summary>
        /// Hour
        /// </summary>
        public ushort Hour { get; set; }

        /// <summary>
        /// Minute
        /// </summary>
        public ushort Minute { get; set; }

        /// <summary>
        /// Second
        /// </summary>
        public ushort Second { get; set; }

        /// <summary>
        /// Millisecond
        /// </summary>
        public ushort MilliSecond { get; set; }

        public bool TimeActive
        {
            get
            {
                return Year > 0 && Month > 0 && DayOfWeek > 0 && Day > 0 && Hour > 0 && Minute > 0 && Second > 0 && MilliSecond > 0;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return Year == 0 && Month == 0 && DayOfWeek == 0 && Day == 0 && Hour == 0 && Minute == 0 && Second == 0 && MilliSecond > 0;
            }
        }

        public DateTime ConvertTime()
        {
            return new DateTime(Year, Month, Day, Hour, Minute, Second, MilliSecond);
        }

        public void CreateTime(string format)
        {
            var date = DateTime.Parse(format);

            Year = (ushort)date.Year;
            Month = (ushort)date.Month;
            Minute = (ushort)date.Minute;
            Day = (ushort)date.Day;
            Hour = (ushort)date.Hour;
            Second = (ushort)date.Second;
            MilliSecond = (ushort)date.Millisecond;
        }

        public void CreateTime()
        {
            var date = DateTime.Now;

            Year = (ushort)date.Year;
            Month = (ushort)date.Month;
            Minute = (ushort)date.Minute;
            Day = (ushort)date.Day;
            Hour = (ushort)date.Hour;
            Second = (ushort)date.Second;
            MilliSecond = (ushort)date.Millisecond;
        }

        public void CreateTime(DateTime date)
        {
            if (date != DateTime.MinValue)
            {

                Year = (ushort)date.Year;
                Month = (ushort)date.Month;
                Minute = (ushort)date.Minute;
                Day = (ushort)date.Day;
                Hour = (ushort)date.Hour;
                Second = (ushort)date.Second;
                MilliSecond = (ushort)date.Millisecond;

            }
        }
    }

    public class CouponGacha
    {

        public uint partial_ticket;
        public uint normal_ticket;
    }

    // Premium Ticket User
    public class PremiumTicket
    {
        public uint id;
        public uint _typeid;
        public uint unix_sec_date;
        public uint unix_end_date;
    }

    // Request Info
    public class RequestInfo
    {
        public uint uid;
        public byte season;
        public byte show;     // 12 pacotes enviados pode enviar o pacote089
    }

    // Itens Equipado do Player
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class EquipedItem
    {
        public CharacterInfo char_info;
        public CaddieInfoEx cad_info;
        public MascotInfoEx mascot_info;
        public ClubSetInfo csi;
        public WarehouseItem comet;
        public WarehouseItem clubset;

        public EquipedItem()
        { cad_info = new CaddieInfoEx(); csi = new ClubSetInfo(); comet = new WarehouseItem(); clubset = new WarehouseItem(); mascot_info = new MascotInfoEx(); }
    }

    // Itens Equipado do Player
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class UserEquipedItem
    {
        public CharacterInfo char_info;
        public CaddieInfoEx cad_info;
        public MascotInfo mascot_info;
        public ClubSetInfo csi;
        public UserEquipedItem()
        { cad_info = new CaddieInfoEx(); csi = new ClubSetInfo(); }
    }

    // Estado do Character no Lounge
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class StateCharacterLounge
    {
        void clear()
        {
            camera_zoom = 1;
            scale_head = 1;
            walk_speed = 1;
            fUnknown = 1;
        }
        public float camera_zoom;  // Zoom da câmera
        public float scale_head;   // Tamanho da cabeça do character
        public float walk_speed;   // Velocidade que o player anda no lounge
        public float fUnknown;
    }

    // MyRoom Config
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class MyRoomConfig
    {
        public short allow_enter;     // Se pode ou não entrar no My Room
        public byte public_lock;      // Se tem senha ou não
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
        public string pass;//15]                  // Senha
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 90)]
        public byte[] ucUnknown90;//[90]  // Não o que é ainda
        public MyRoomConfig()
        {
            ucUnknown90 = new byte[90];
        }
    }

    // MyRoom Item
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class MyRoomItem
    {
        public uint id;
        public uint _typeid;
        public short number;
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class Location
        {
            public float x;
            public float y;
            public float z;
            public float r;

        }
        public MyRoomItem()
        {
            location = new Location();
        }
        public Location location;
        public byte equiped;     // Equipado ou não, 1 YES, 0 NO
    }

    // Dolfine Locker
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class DolfiniLocker
    {
        public DolfiniLocker()
        {
            v_item = new List<DolfiniLockerItem>();
        }
        void clear()
        {
            pang = 0;

            v_item.Clear();
        }
        public uint isLocker()
        {

            if (pass[0] == '\0')
                return 2;   // Senha não foi criada ainda
            else if (!locker && pass_check)
                return 76;// 1;	// Senha já foi verificada para essa session

            return 76;  // Senha ainda não foi verificada para essa session
        }
        public bool ownerItem(uint _typeid)
        {

            var it = v_item.Where(c => c.item._typeid == _typeid);


            return it.Any();
        }
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
        public string pass;//[7]
        public ulong pang;
        public bool locker;               // Essa opção tem que ser do server para pedir para o player verificar a senha todas vez do locker
        public bool pass_check;  // 1 já foi verificado a senha nessa session, 0 ainda não foi verificada
        public List<DolfiniLockerItem> v_item;
    }
    // Dolfini Locker Item
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class DolfiniLockerItem
    {
        public ulong index; // ID do item no dolfini Locker
        public TradeItem item;

        public DolfiniLockerItem()
        {
            item = new TradeItem();
        }
    }

    // TradeItem
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class TradeItem
    {
        public uint _typeid;
        public uint id;
        public uint qntd;
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] ucUnknown3;//[3]
        public ulong pang;
        public uint upgrade_custo;
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public short[] c;
        public short usUnknown;
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string sd_idx;//[9]
        public short sd_seq;
        public byte sd_status;
        public TradeItem()
        {
            c = new short[5];
        }
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class Card
        {
            [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public uint[] character;
            [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public uint[] caddie;
            [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public uint[] NPC;
            public short character_slot_count;
            public short caddie_slot_count;
            public short NPC_slot_count;
            public Card()
            {
                character = new uint[5];
                caddie = new uint[5];
                NPC = new uint[5];
            }
        }

        public Card card;
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string sd_name;//[41]
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 22)]
        public string sd_copier_nick;//[22]
    }

    /**** Base Item do pacote 0x216 Update Item No Game
	**/
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class stItem216
    {
        public byte type;
        public uint _typeid;
        public uint id;
        public uint flag_time;
        public uint qntd_ant;
        public uint qntd_dep;
        public uint qntd;
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public short[] c; //5
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ucc_idx; //9
        public byte seq;  // ou stats
        public uint card_typeid;
        public byte card_slot;
        public stItem216()
        {
            c = new short[5];
        }
    }

    // Friend Info
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class FriendInfo
    {
        public uint uid;
        public byte sex;  // gender, genero, sexo, 0 masculino, 1 Feminino
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 22)]
        public string id;//[22]
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 22)]
        public string nickname;//[22]
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
        public string apelido;//[15]
    }

    // Daily Quest Info
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class DailyQuestInfo
    {
        public DailyQuestInfo(int _typeid_0, uint _typeid_1, uint _typeid_2, PangyaTime _st)
        {
        }

        public PangyaTime date;            // System Time Windows
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public uint[] _typeid;// [3];    // array[3] Typeid da Quest
    }

    // Daily Quest Info User
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class DailyQuestInfoUser
    {
        public uint now_date;      // Data que a quest está (current quest), do sistema de daily quest
        public uint accept_date;   // Data da ultima quest que foi aceita
        public uint current_date;  // Data que a quest está (current quest), do player
        public uint count;     // count de quest que tem no dia
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public uint[] _typeid;// [3];    // Max 3 quest por dia
    }

    // Remove Daily Quest
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class RemoveDailyQuestUser
    {
        public uint id;
        public uint _typeid;
    }

    // Add DailyQuest
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class AddDailyQuestUser
    {
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;// [64];
        public uint _typeid;
        public uint quest_typeid;
        public uint status;
    }

    // Player Canal Info
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class PlayerCanalInfo
    {
        public PlayerCanalInfo()
        {
            clear();
        }
        public void clear()
        {
            nickname = "";
            nickNT = "";
            sala_numero = -1;
            capability = new uCapability();
            state_flag = new uStateFlag();
            guild_mark_img = "";// [12];
            unknown = "";// [6];
            nickNT = "";// [18];                // Acho
            unknown109 = "";// [109];
        }
        public uint uid;
        public uint oid;
        public short sala_numero;
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 22)]
        public string nickname;// [22];
        public byte level;
        public uCapability capability;
        public uint title;
        public uint team_point;             // Acho que é o team point
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class uStateFlag
        {
            public byte ucByte;
        }
        public uStateFlag state_flag;
        public uint guid_uid;
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public string guild_mark_img;// [12];
        public byte flag_visible_gm;//th é vip
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 6)]
        public string unknown;// [6];
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 18)]
        public string nickNT;// [18];                // Acho
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 109)]
        public string unknown109;// [109];
    }

    // Player Canal Info
    public class PlayerCanalInfoEx : PlayerCanalInfo
    {
        public PlayerCanalInfoEx()
        {
            base.clear();
            state_flag = new uStateFlagEx();
        }
        public new uStateFlagEx state_flag;
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class uStateFlagEx : uStateFlag
        {
            public _stBit stBit;
            public struct _stBit
            {
                public byte away;             // AFK
                public byte sexo;            // Genero
                public byte quiter_1;        // Quit rate maior que 31% e menor que 41%
                public byte quiter_2;         // Quit rate maior que 41%
                public byte azinha;           // Azinha, Quit rate menor que 3%
                public byte icon_angel;       // Angel Wings
                public byte ucUnknown_bit7;   // Unknown Bit 7
                public byte ucUnknown_bit8;   // Unknown Bit 8
            }
        }
    }
    // Player Room Info {
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 341)]
    public class PlayerRoomInfo
    {
        public PlayerRoomInfo()
        {
            clear();
        }
        protected void clear()
        {
            usUnknown_flg = 3490;
            ucUnknown_0A = 0x10;
            ucUnknown3 = 66;
            capability = new uCapability();
            state_flag = new StateFlag();
            skin = new uint[6];
            location = new stLocation();
            shop = new PersonShop();
            flag_item_boost = new uItemBoost();
            nickNT = "@NT";
            ucUnknown105 = new byte[105];
        }
        public uint oid;
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 22)]
        public string nickname;// [22];
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string guild_name;// [17];
        public byte position;
        [field: MarshalAs(UnmanagedType.Struct, SizeConst = 4)]
        public uCapability capability;
        public uint title;
        public uint char_typeid;       // Character Typeid
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public uint[] skin;// [6];
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class StateFlag
        {
            public short usFlag;
        }
        [field: MarshalAs(UnmanagedType.Struct, SizeConst = 2)]
        public StateFlag state_flag;//2 bytes
        public byte level;
        public byte icon_angel;
        public byte ucUnknown_0A;         // Tem o valor 0x0A aqui quase sempre das vezes que vi esse pacote, Pode ser o Place(lugar que o player está) tipo Room = 10(hex:0x0A)
        public uint guild_uid;
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public string guild_mark_img; //[12];
        public uint uid;
        //---------Action
        public uint state_lounge;//animate
        public short usUnknown_flg;//Unknown1	// Acho que seja uma flag tbm
        public uint state;//Posture	// Acho que seja estado de "lugar" pelo que lembro
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class stLocation
        {
            public static stLocation operator +(stLocation a, stLocation _add_location)
            {
                return new stLocation()
                {
                    x = a.x += _add_location.x,
                    z = a.z += _add_location.z,
                    r = a.r += _add_location.r
                };
            }
            public static stLocation operator -(stLocation a, stLocation _add_location)
            {
                return new stLocation()
                {
                    x = a.x -= _add_location.x,
                    z = a.z -= _add_location.z,
                    r = a.r -= _add_location.r
                };
            }
            public float x;
            public float z;
            public float r;
        }
        [field: MarshalAs(UnmanagedType.Struct, SizeConst = 12)]
        public stLocation location;
        //----------
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class PersonShop
        {
            public uint active;
            [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;//64
        }
        [field: MarshalAs(UnmanagedType.Struct, SizeConst = 68)]
        public PersonShop shop;
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class uItemBoost
        {
            public short ulItemBoost;
        }
        public uint mascot_typeid;
        [field: MarshalAs(UnmanagedType.Struct, SizeConst = 2)]
        public uItemBoost flag_item_boost;// Boost EXP, Pang e etc(2 bytes)
        public uint ulUnknown_flg;// Pode ser a flag de teasure do player, ou de drop item
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 22)]
        public string nickNT;//[22] Acho que seja o ID na ntreev do player, a empresa que mantêm as contas, no JP era o gamepot
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 105)]
        public byte[] ucUnknown105;// Unknown 106 bytes que sempre fica com valor 0
        public byte convidado;   // Flag Convidado, [Não sei bem por que os que entra na sala normal tem valor igual aqui, já que é flag de convidado waiting]
        public float avg_score;// Media score "media de holes feito pelo player"
        public float ucUnknown3;// Não sei mas sempre é 0 depois do media score(66 no th)
    }

    // Player Room Info Ex
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class PlayerRoomInfoEx : PlayerRoomInfo
    {
        public PlayerRoomInfoEx()
        {
            clear();
            ci = new CharacterInfo();
        }
        [field: MarshalAs(UnmanagedType.Struct, SizeConst = 513)]
        public CharacterInfo ci { get; set; }
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class PlayerRoomInfoExtensed : PlayerRoomInfoEx
    {
        public PlayerRoomInfoExtensed()
        {
            clear();
            ci = new CharacterInfo();
        }
        [field: MarshalAs(UnmanagedType.Struct, SizeConst = 4)]
        public new uCapability capability;
        [field: MarshalAs(UnmanagedType.Struct, SizeConst = 2)]
        public new StateFlagEx state_flag;
        [field: MarshalAs(UnmanagedType.Struct, SizeConst = 2)]
        public new uItemBoostEx flag_item_boost;
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class StateFlagEx : StateFlag
        {
            public StateFlagEx()
            {

                uFlag = new _UFlag();
            }
            void clear()
            {
                uFlag.stFlagBit.ready = 0; // Unready
            }
            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public class _UFlag
            {
                [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
                public byte[] ucByte;
                [field: MarshalAs(UnmanagedType.Struct, SizeConst = 2)]
                public _stFlagBit stFlagBit;
                public _UFlag()
                {
                    ucByte = new byte[2];
                    stFlagBit = new _stFlagBit();
                }
            }
            public _UFlag uFlag { get; set; }
            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public class _stFlagBit
            {
                public byte team;             // Team que está na sala
                public byte team2;            // Esse é relacionado com o team, mas acho que seja do torneio team ou guild battle
                public byte away;             // AFK
                public byte master;           // Master da sala
                public byte master2;          // Master da sala, tbm esse aqui
                public byte sexo;             // Genero
                public byte quiter_1;         // de 31% a 41% de quit rate
                public byte quiter_2;         // maior que 41% de quit rate
                public byte azinha;           // menor que 3% de quit rate
                public byte ready;            // se está pronto para começar o jogo, 0 Unready, 1 Ready
            }
        }
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class uItemBoostEx : uItemBoost
        {
            public class _stItemBoost
            {
                public byte ucPangMastery;   // Pang Mastery X2
                public byte ucPangNitro;     // Pang Nitro X4
            }
            public _stItemBoost stItemBoost;
        }
    }

    //    // Sala Guild Info(tenho que olhar mais direito se esta correto)
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RoomGuildInfo
    {
        public uint guild_1_uid;
        public uint guild_2_uid;
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string guild_1_nome;// [17];
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string guild_2_nome;// [17];
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public string guild_1_mark;// [12];              // mark string o pangya JP não usa aqui fica 0
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public string guild_2_mark;// [12];              // mark string o pangya JP não usa aqui fica 0
    }

    //    // Sala Grand Prix Info
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RoomGrandPrixInfo
    {
        public uint dados_typeid;
        public uint rank_typeid;
        public uint tempo;
        public uint active;
    }

    // Union Natural e Short Game Flag da sala, que guarda a estrutura de bits da flag da sala de natural e short game
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class uNaturalAndShortGame
    {

        public uint ulNaturalAndShortGame;
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class uNaturalAndShortGameEx : uNaturalAndShortGame
    {

        public _stBit stBit;
        public struct _stBit
        {

            public uint natural;           // Natural Modo
            public uint short_game;    // Short Game Modo
        }
    }

    // SalaInfo
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class RoomInfo
    {
        public enum eCOURSE : byte
        {
            BLUE_LAGOON,
            BLUE_WATER,
            SEPIA_WIND,
            WIND_HILL,
            WIZ_WIZ,
            WEST_WIZ,
            BLUE_MOON,
            SILVIA_CANNON,
            ICE_CANNON,
            WHITE_WIZ,
            SHINNING_SAND,
            PINK_WIND,
            DEEP_INFERNO = 13,
            ICE_SPA,
            LOST_SEAWAY,
            EASTERN_VALLEY,
            CHRONICLE_1_CHAOS,
            ICE_INFERNO,
            WIZ_CITY,
            ABBOT_MINE,
            MYSTIC_RUINS,
            GRAND_ZODIAC = 64,
            RANDOM = 127,
        }
        public enum TIPO : uint
        {
            STROKE,
            MATCH,
            LOUNGE,
            TOURNEY = 4,
            TOURNEY_TEAM,
            GUILD_BATTLE,
            PANG_BATTLE,
            APPROCH = 10,
            GRAND_ZODIAC_INT,
            GRAND_ZODIAC_ADV = 13,
            GRAND_ZODIAC_PRACTICE,
            SPECIAL_SHUFFLE_COURSE = 18,
            PRACTICE,
            GRAND_PRIX,
        }
        public enum MODO : uint
        {
            M_FRONT,
            M_BACK,
            M_RANDOM,
            M_SHUFFLE,
            M_REPEAT,
            M_SHUFFLE_COURSE,
        }
        public enum INFO_CHANGE : uint
        {
            NAME,
            SENHA,
            TIPO,
            COURSE,
            QNTD_HOLE,
            MODO,
            TEMPO_VS,
            MAX_PLAYER,
            TEMPO_30S,
            STATE_FLAG,
            UNKNOWN,
            HOLE_REPEAT,
            FIXED_HOLE,
            ARTEFATO,
            NATURAL,
        }
        public RoomInfo()
        {
            clear();
        }
        protected void clear()
        {

            numero = -1;
            senha_flag = true;
            state = true;
            _30s = 30;

            guilds = new RoomGuildInfo(); // Valores inicias
        }
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string nome;// [64];
        public bool senha_flag;  // Sala sem senha = 1, Sala com senha = 0
        public bool state;       // Sala em espera = 1, Sala em Game = 0
        public byte flag;                 // Sala que pode entrar depois que começou = 1
        public byte max_player;
        public byte num_player;
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
        public byte[] key;
        public byte _30s;                 // Modo Multiplayer do pangya acho, sempre 0x1E (dec: 30) no pangya
        public byte qntd_hole;
        public byte tipo_show;            // esse é o tipo que mostra no pacote, esse pode mudar dependendo do tipo real da sala, fica ou camp, ou VS ou especial, não coloca todos os tipos aqui
        public short numero;
        public byte modo;
        public eCOURSE course;
        public uint time_vs;
        public uint time_30s;
        public uint trofel;
        public short state_flag;          // Quando é sala de 100 player o mais de gm event aqui é 0x100
        public RoomGuildInfo guilds;
        public uint rate_pang;
        public uint rate_exp;
        public uint master;         // Tem valores negativos, por que a sala usa ele para grand prix e etc
        public byte tipo_ex;          // tipo extended, que fala o tipo da sala certinho
        public uint artefato;          // Aqui usa pra GP efeitos especiais do GP
                                       //int natural;			// Aqui usa para Short Game Também
        public uNaturalAndShortGame natural;       // Aqui usa para Short Game Também
        public RoomGrandPrixInfo grand_prix;
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class RoomInfoEx : RoomInfo
    {
        RoomInfoEx()
        {

            base.clear();

            hole_repeat = 0;
            fixed_hole = 0;
            tipo = 0;
            state_afk = false;
            channel_rookie = false;
            angel_event = false;
        }
        public new uNaturalAndShortGameEx natural;
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string senha;// [64];                     // Senha da sala
        public byte tipo;                 // Tipo real da sala
        public byte hole_repeat;          // Número do hole que vai ser repetido
        public uint fixed_hole;            // Aqui é 1 Para Hole(Pin"Beam") Fixo, e 0 para aleatório
        public bool state_afk;   // Estado afk da sala, usar para depois começar a sala, já que o pangya não mostra se a sala está afk
        public bool channel_rookie;   // Flag que guarda, se o channel é rookie ou não, onde a sala foi criada, vem da Flag do channel
        public bool angel_event;      // Flag que guarda se o Angel Event está ligado
        public byte flag_gm;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class RateValue
    {
        public uint pang;
        public uint exp;
        public uint clubset;
        public uint rain;
        public uint treasure;
        public byte persist_rain;
    }


    // Item Pangya Base Para Pacote216
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class ItemPangyaBase
    {

        public byte tipo;
        public uint _typeid;
        public uint id;
        public uint tipo_unidade_add;
        public uint qntd_ant;
        public uint qntd_dep;
        public uint qntd;
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] unknown;// [8];
        public short qntd_time;
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class ItemPangya : ItemPangyaBase
    {
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string sd_idx;// [9];
        public uint sd_status;
        public uint sd_seq;
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public byte[] unknown2;//[5];
    }

    // BuyItem
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class BuyItem
    {
        public uint id;
        public uint _typeid;
        public short time;
        public short usUnknown;
        public uint qntd;
        public uint pang;
        public uint cookie;
    }

    // Email Info
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class EmailInfo
    {
        public EmailInfo()
        {
            clear();
        }

        void clear()
        {

            id = uint.MaxValue;

            lida_yn = 0;

            itens = new List<item>();
        }
        public uint id;
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 22)]
        public string from_id;//[22];
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string gift_date;//[20];
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string msg;//[100];
        public byte lida_yn;
        public class item
        {
            public uint id;
            public uint _typeid;
            public byte flag_time;
            public uint qntd;
            public uint tempo_qntd;
            public ulong pang;
            public ulong cookie;
            public uint gm_id;
            public uint flag_gift;
            [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
            public string ucc_img_mark;//[9];
            [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public byte[] ucUnknown3;//[3];
            public short type;
        }
        public List<item> itens;
    }

    // EmailInfoEx
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class EmailInfoEx : EmailInfo
    {
        EmailInfoEx()
        {
            visit_count = 0;
        }
        public uint visit_count;
    }

    //// Mail Box
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class MailBox
    {
        public uint id;
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 30)]
        public string from_id;// [30];
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string msg;// [80];
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 18)]
        public byte[] unknown2;// [18];
        public uint visit_count;
        public byte lida_yn;
        public uint item_num;           // Número de itens que tem nesse anexado a esse email
        public EmailInfo.item item;
    }

    //// Ticket Report Scroll Info
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class TicketReportScrollInfo
    {

        public void clear()
        {
            id = uint.MaxValue;
            date = new PangyaTime();

            v_players = new List<stPlayerDados>();

        }
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class stPlayerDados
        {
            public stPlayerDados()
            {
                clear();
            }
            public void clear()
            {
                ucUnknown_flg = 2;
            }
            public uint uid;
            public ulong pang;
            public ulong bonus_pang;
            public uint trofel_typeid;
            public uint exp;
            public uint mascot_typeid;
            public byte premium_user;
            public byte item_boost; // [Bit] 1 = Pang Mastery x2, 2 = Pang Nitro x4, 3 = (ACHO) Exp x2
            public uint level;
            public byte score;
            public uMedalWin medalha;
            public byte trofel;
            [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 22)]
            public string id;//[22];
            [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 22)]
            public string nickname;//[22];
            public uint ulUnknown;
            public uint guild_uid;
            public uint mark_index;        // Guild, isso é do JP, que ele nao usa o EMBLEM NUMER
            [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
            public string guild_mark_img;//[12];
            public uint tipo;
            public byte state;
            public byte ucUnknown_flg;    // Não sei mas sempre peguei o valor 2
            public PangyaTime finish_time;
        }
        public uint id;
        public PangyaTime date;
        public List<stPlayerDados> v_players;
    }

    // Estrutura que Guarda as informações dos Convites do Canal
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct InviteChannelInfo
    {
        public short room_number;
        public uint invite_uid;
        public uint invited_uid;
        public PangyaTime time;
    }

    // Command Info, os Comando do Auth Server
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class CommandInfo
    {

        public uint idx;
        public uint id;
        public uint[] arg;// [5];
        public uint target;
        public short flag;
        public byte valid;
        public PangyaTime reserveDate;
    }

    //// Update Item
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class UpdateItem
    {
        public enum UI_TYPE : byte
        {
            CADDIE,
            CADDIE_PARTS,
            MASCOT,
            WAREHOUSE,
        }
        public UI_TYPE type;
        public uint _typeid;
        public uint id;
    }

    //// Grand Prix Clear
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GrandPrixClear
    {
        public uint _typeid;
        public uint position;
    }

    //// Guild Update Activity Info
    //// Guarda os dados das atualizações que os Clubs tem de alterações
    //// Como membro kickado, sair do club e aceito no club
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GuildUpdateActivityInfo
    {
        public enum TYPE_UPDATE : byte
        {
            TU_ACCEPTED_MEMBER,
            TU_EXITED_MEMBER,
            TU_KICKED_MEMBER,
        }

        public ulong index; // ID do update activity
        public uint club_uid;
        public uint owner_uid; // Quem fez a Ação
        public uint player_uid;
        public TYPE_UPDATE type;
        public PangyaTime reg_date;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class ChangePlayerItemRoom
    {
        public enum TYPE_CHANGE : byte
        {
            TC_CADDIE = 1,
            TC_BALL,
            TC_CLUBSET,
            TC_CHARACTER,
            TC_MASCOT,
            TC_ITEM_EFFECT_LOUNGE,  // Hermes x2, Twilight, Jester x2
            TC_ALL,                 // CHARACTER, CADDIE, CLUBSET e BALL essa é a ordem
            TC_UNKNOWN = 255,
        }
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class stItemEffectLounge
        {
            public enum TYPE_EFFECT : uint
            {
                TE_BIG_HEAD = 1,
                TE_FAST_WALK,
                TE_TWILIGHT,
            }
            public uint item_id;   // Aqui ele manda 0 o cliente, não sei por que, deveria mandar o id do item equipado
            public TYPE_EFFECT effect;
        }
        public TYPE_CHANGE type;                   // Type Change
        public uint caddie;                // Caddie ID
        public uint ball;                  // Ball Typeid
        public uint clubset;               // ClubSet ID
        public uint character;         // Character ID
        public uint mascot;                // Mascot ID
        public stItemEffectLounge effect_lounge;   // Item effect lounge
    }

    // Trofel Info
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class TrofelInfo
    {
        public TrofelInfo()
        {
            int i = 0;
            for (i = 0; i < 3; i++)
                ama_6 = new short[3];
            for (i = 0; i < 3; i++)
                ama_5 = new short[3];
            for (i = 0; i < 3; i++)
                ama_4 = new short[3];
            for (i = 0; i < 3; i++)
                ama_3 = new short[3];
            for (i = 0; i < 3; i++)
                ama_2 = new short[3];
            for (i = 0; i < 3; i++)
                ama_1 = new short[3];
            for (i = 0; i < 3; i++)
                pro_1 = new short[3];
            for (i = 0; i < 3; i++)
                pro_2 = new short[3];
            for (i = 0; i < 3; i++)
                pro_3 = new short[3];
            for (i = 0; i < 3; i++)
                pro_4 = new short[3];
            for (i = 0; i < 3; i++)
                pro_5 = new short[3];
            for (i = 0; i < 3; i++)
                pro_6 = new short[3];
            for (i = 0; i < 3; i++)
                pro_7 = new short[3];
        }
        public void update(uint _type, byte _rank)
        {
            // Maior que Pro 7
            if (_type > 12)
            {
                throw new System.Exception("[TrofelInfo::update][Error] _type[VALUE=" + Convert.ToString(_type) + "] is invalid");
            }

            if (_rank == 0u || _rank > 3)
            {
                throw new System.Exception("[TrofelInfo::update][Error] _rank[VALUE=" + Convert.ToString((ushort)_rank) + "] is invalid");
            }

            switch (_type)
            {
                case 0:		// AMA 6
                    ama_6[_rank - 1]++;
                    break;
                case 1:		// AMA 5
                    ama_5[_rank - 1]++;
                    break;
                case 2:		// AMA 4
                    ama_4[_rank - 1]++;
                    break;
                case 3:		// AMA 3
                    ama_3[_rank - 1]++;
                    break;
                case 4:		// AMA 2
                    ama_2[_rank - 1]++;
                    break;
                case 5:		// AMA 1
                    ama_1[_rank - 1]++;
                    break;
                case 6:		// PRO 1
                    pro_1[_rank - 1]++;
                    break;
                case 7:		// PRO 2
                    pro_2[_rank - 1]++;
                    break;
                case 8:		// PRO 3
                    pro_3[_rank - 1]++;
                    break;
                case 9:		// PRO 4
                    pro_4[_rank - 1]++;
                    break;
                case 10:	// PRO 5
                    pro_5[_rank - 1]++;
                    break;
                case 11:	// PRO 6
                    pro_6[_rank - 1]++;
                    break;
                case 12:	// PRO 7
                    pro_7[_rank - 1]++;
                    break;
            }
        }
        public uint getSumGold()
        {

            uint value = 0;


            value += (uint)ama_1[0];
            value += (uint)ama_2[0];
            value += (uint)ama_3[0];
            value += (uint)ama_4[0];
            value += (uint)ama_5[0];
            value += (uint)ama_6[0];
            value += (uint)pro_1[0];
            value += (uint)pro_2[0];
            value += (uint)pro_3[0];
            value += (uint)pro_4[0];
            value += (uint)pro_5[0];
            value += (uint)pro_6[0];
            value += (uint)pro_7[0];

            return value;
        }
        public uint getSumSilver()
        {

            uint value = 0;

            value += (uint)ama_1[1];
            value += (uint)ama_2[1];
            value += (uint)ama_3[1];
            value += (uint)ama_4[1];
            value += (uint)ama_5[1];
            value += (uint)ama_6[1];
            value += (uint)pro_1[1];
            value += (uint)pro_2[1];
            value += (uint)pro_3[1];
            value += (uint)pro_4[1];
            value += (uint)pro_5[1];
            value += (uint)pro_6[1];
            value += (uint)pro_7[1];
            return value;
        }
        public uint getSumBronze()
        {

            uint value = 0;

            value += (uint)ama_1[2];
            value += (uint)ama_2[2];
            value += (uint)ama_3[2];
            value += (uint)ama_4[2];
            value += (uint)ama_5[2];
            value += (uint)ama_6[2];
            value += (uint)pro_1[2];
            value += (uint)pro_2[2];
            value += (uint)pro_3[2];
            value += (uint)pro_4[2];
            value += (uint)pro_5[2];
            value += (uint)pro_6[2];
            value += (uint)pro_7[2];

            return value;
        }
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public short[] ama_6;     // Ouro, Prata e Bronze
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public short[] ama_5;     // Ouro, Prata e Bronze
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public short[] ama_4;     // Ouro, Prata e Bronze
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public short[] ama_3;     // Ouro, Prata e Bronze
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public short[] ama_2;     // Ouro, Prata e Bronze
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public short[] ama_1;     // Ouro, Prata e Bronze
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public short[] pro_1;     // Ouro, Prata e Bronze
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public short[] pro_2;     // Ouro, Prata e Bronze
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public short[] pro_3;     // Ouro, Prata e Bronze
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public short[] pro_4;     // Ouro, Prata e Bronze
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public short[] pro_5;     // Ouro, Prata e Bronze
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public short[] pro_6;     // Ouro, Prata e Bronze
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public short[] pro_7;		// Ouro, Prata e Bronze
    }



    // Trofel Especial Info
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class TrofelEspecialInfo
    {
        public uint id;
        public uint _typeid;
        public uint qntd;
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class CharacterInfoEx : CharacterInfo
    {
        public CharacterInfoEx()
        {
            base.Init();
        }
        public sbyte getSlotOfStatsFromsbyteEquipedPartItem(Stats __stat)
        {   // Get Slot of stats from Character equiped item

            sbyte value = 0;
            // IFF.Part part = null;

            // Invalid Stats type, Unknown type Stats
            if (__stat > Stats.S_CURVE)
                return -1;

            for (var i = 0; i < (Marshal.SizeOf(parts_typeid) / Marshal.SizeOf(parts_typeid[0])); ++i)
            {

                //if (parts_id[i] != 0 && (part = sIff.findPart(parts_typeid[i])) != null)
                //    value += (sbyte)part.Slot[(int)__stat];
            }

            return value;
        }
        public new void initComboDef()
        {   // Initialize o combo de roupas padrões do Character
            Init();
            if (_typeid == 0)
                return;

            uint part_typeid = 0;

            for (var i = 0; i < (Marshal.SizeOf(parts_typeid) / Marshal.SizeOf(parts_typeid[0])); ++i)
            {
                part_typeid = Convert.ToUInt32((((_typeid << 5) | i) << 13) | 0x8000400);

                //if (sIff.findPart(part_typeid) != null)
                //    parts_typeid[i] = part_typeid;
            }
        }
    }
    // Item Equipados
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class UserEquip
    {
        public UserEquip()
        {
            item_slot = new uint[10];
            skin_id = new uint[6];
            skin_typeid = new uint[6];
            poster = new uint[2];
        }
        public uint caddie_id;
        public uint character_id;
        public uint clubset_id;
        public uint ball_typeid;
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public uint[] item_slot;//[10];      // 10 Item slot
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public uint[] skin_id;//[6];     // 6 skin id, tem o title, frame, stick e etc
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public uint[] skin_typeid;//[6]; // 6 skin typeid, tem o title, frame, stick e etc
        public uint mascot_id;
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public uint[] poster;// [2];          // Poster, tem 2 o poster A e poster B
        public uint m_title => skin_typeid[5];// Titulo Typeid
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class MapStatistics
    {
        public MapStatistics()
        {
            clear();
        }
        public void clear(byte _course = 0)
        {
            best_score = 127;
            course = _course;
        }
        public bool isRecorded()
        {

            // Player fez record nesse Course
            return (best_score != 127 ? true : false);
        }
        public byte course;
        public uint tacada;
        public uint putt;
        public uint hole;
        public uint fairway;
        public uint hole_in;
        public uint putt_in;
        public int total_score;
        public byte best_score;
        public ulong best_pang;
        public uint character_typeid;
        public byte event_score;
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    // MapStatisticsEx esse tem o tipo que não vai no pacote que passa pro cliente
    public class MapStatisticsEx : MapStatistics
    {
        public MapStatisticsEx(uint _ul = 0)
        {
            clear();
        }
        public MapStatisticsEx(MapStatistics _cpy)
        {
            tipo = 0;
        }
        public byte tipo;             // Tipo, 0 Normal, 0x32 Natural, 0x33 Grand Prix
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    // Caddie Info
    public class CaddieInfo
    {
        public uint id;
        public uint _typeid;
        public uint parts_typeid;
        public byte level;
        public uint exp;
        public byte rent_flag;
        public short end_date_unix;
        public short parts_end_date_unix;
        public byte purchase;
        public short check_end;
    }

    // Caddie Info Ex
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class CaddieInfoEx : CaddieInfo
    {
        public void updatePartsEndDate()
        {

            ulong diff_end_parts_date = (ulong)(end_parts_date.IsEmpty ? 0 : 0/* getLocalTimeDiffDESC(end_parts_date)*/);

            // Não tem mais o parts _typeid acabou o tempo dela
            if (diff_end_parts_date <= 0)
            {

                parts_end_date_unix = 0;

                // Zera Parts_Typeid
                if (parts_typeid > 0)
                {

                    parts_typeid = 0;

                    need_update = true;   // Precisa Atulizar para o cliente
                }

            }
            else
                parts_end_date_unix = (short)((diff_end_parts_date /= 0) == 0 ? 1 : (short)diff_end_parts_date);

        }
        public void updateEndDate()
        {

            ulong diff_end_date = (ulong)(end_date.IsEmpty ? 0 : 0/*  getLocalTimeDiffDESC(end_date) */);

            if (diff_end_date <= 0)
                end_date_unix = 0;
            else
                end_date_unix = (short)((diff_end_date /= 0) == 0 ? 1 : (short)diff_end_date);

        }
        public CaddieInfo getInfo()
        {

            // Update Timestamp Unix of caddie and caddie Parts

            // End Date Unix Update 
            updateEndDate();

            // Parts End Date Unix Update
            updatePartsEndDate();

            return this;
        }
        public PangyaTime end_date;
        public PangyaTime end_parts_date;
        public bool need_update;         // Precisa Atulizar para o cliente
        public CaddieInfoEx()
        {
            end_date = new PangyaTime();
            end_parts_date = new PangyaTime();
        }
    }

    // Club Set Info
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class ClubSetInfo
    {
        public uint id;
        public uint _typeid;
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public short[] slot_c;// [5];        // Total de slot para upa do stats, força, controle, spin e etc
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public short[] enchant_c;// [5];     // Enchant Club, Força, controle, spin e etc

        public void setValues(uint _uid, uint id_type, short[] value)
        {
            slot_c = value;
            _typeid = id_type;
            id = _uid;
        }
    }

    // Mascot Info
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class MascotInfo
    {
        public uint id;
        public uint _typeid;
        public byte level;
        public uint exp;
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 30)]
        public string message;
        public short tipo;
        public PangyaTime data;
        public byte flag;
        public MascotInfo()
        {
            data = new PangyaTime();
        }
    }

    // Mascot Info Ex, tem o IsCash flag nele
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class MascotInfoEx : MascotInfo
    {
        public bool checkUpdate()
        {

            if (data.IsEmpty)
                need_update = 1;

            return (need_update == 1);
        }
        public byte is_cash;
        public uint price;
        public byte need_update;
    }

    // Item Warehouse
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class WarehouseItem
    {
        public WarehouseItem()
        {
            c = new short[5];
            card = new Card()
            { caddie = new uint[4], character = new uint[4], NPC = new uint[4] };
            clubset_workshop = new ClubsetWorkshop() { c = new short[5] };
            ucc = new UCC();
        }
        public uint id { get; set; }
        public uint _typeid { get; set; }
        public int ano { get; set; }            // acho que seja só tempo que o item ainda tem
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public short[] c { get; set; }      // Stats do item ctrl, força etc, se não usa isso o [0] é a quantidade
        public byte purchase { get; set; }
        public sbyte flag { get; set; }
        public long apply_date { get; set; }
        public long end_date { get; set; }
        public sbyte type { get; set; }
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class UCC
        {
            [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
            public string name { get; set; }
            public sbyte trade { get; set; }     // Aqui pode(acho) ser qntd de sd que foi vendida
            [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
            public string idx { get; set; }             // 8 
            public sbyte status { get; set; }
            public short seq { get; set; }          // aqui é a seq de sd que vendeu
            [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 22)]

            public string copier_nick { get; set; }
            public uint copier { get; set; }                // uid de quem fez a sd
        }
        public UCC ucc;
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class Card
        {
            [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public uint[] character { get; set; }
            [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public uint[] caddie { get; set; }
            [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public uint[] NPC { get; set; }
        }
        public Card card;
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class ClubsetWorkshop
        {
            public short flag { get; set; }
            [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public short[] c { get; set; }
            // Stats do item ctrl, força etc, se não usa isso o [0] é a quantidade			public uint  mastery {get; set;}
            public uint mastery { get; set; }
            public uint recovery_pts { get; set; }
            public int level { get; set; }
            public uint rank { get; set; }          // UP eu chamo esse
            public uint calcRank(short[] _c)
            {
                uint total = (uint)(c[0] + _c[0] + c[1] + _c[1] + c[2] + _c[2] + c[3] + _c[3] + c[4] + _c[4]);

                if (total >= 30 && total < 60)
                    return (total - 30) / 5;

                return uint.MaxValue;
            }
            public uint calcLevel(short[] _c)
            {

                uint total = (uint)(c[0] + _c[0] + c[1] + _c[1] + c[2] + _c[2] + c[3] + _c[3] + c[4] + _c[4]);

                if (total >= 30 && total < 60)
                    return (total - 30) % 5;

                return uint.MaxValue;
            }
            public uint s_calcRank(short[] _c)
            {

                uint total = (uint)(_c[0] + _c[1] + _c[2] + _c[3] + _c[4]);

                if (total >= 30 && total < 60)
                    return (total - 30) / 5;

                return uint.MaxValue;
            }
            public uint s_calcLevel(short[] _c)
            {

                uint total = (uint)(_c[0] + _c[1] + _c[2] + _c[3] + _c[4]);

                if (total >= 30 && total < 60)
                    return (total - 30) % 5;

                return uint.MaxValue;
            }
        }
        public ClubsetWorkshop clubset_workshop;
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class WarehouseItemEx : WarehouseItem
    {
        // Date to Calcule dates
        public uint apply_date_unix_local;
        public uint end_date_unix_local;
    }

    // ClubSet Workshop Last Up Level
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class ClubSetWorkshopLasUpLevel
    {
        public uint clubset_id;
        public uint stat;
    }

    // ClubSet WorkShop Transform ClubSet In Special ClubSet
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class ClubSetWorkshopTransformClubSet
    {
        public uint clubset_id;
        public uint stat;
        public uint transform_typeid;
    }
    // Personal Shop Item
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class PersonalShopItem
    {
        public uint index;     // Index Sequência do item no shop
        public TradeItem item;
    }

    // Tutorial Info
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class TutorialInfo
    {
        public uint getTutoAll()
        {
            return rookie | beginner | advancer;
        }
        public uint rookie;
        public uint beginner;
        public uint advancer;
    }

    // Card Info
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class CardInfo
    {
        public uint id;
        public uint _typeid;
        public uint slot;
        public uint efeito;
        public uint efeito_qntd;
        public uint qntd;
        public PangyaTime use_date;
        public PangyaTime end_date;
        public byte type;
        public byte use_yn;
    }

    // Card Equip Info
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class CardEquipInfo
    {
        public uint id;
        public uint _typeid;
        public uint parts_typeid;
        public uint parts_id;
        public uint efeito;
        public uint efeito_qntd;
        public uint slot;
        public PangyaTime use_date;
        public PangyaTime end_date;
        public uint tipo;
        public byte use_yn;
    }

    // Card Equip Info Ex
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class CardEquipInfoEx : CardEquipInfo
    {
        public ulong index;
    }



    // Message Off
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class MsgOffInfo
    {

        public uint from_uid;
        public short id;
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
        public string nick;//[22];
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public string msg;//[64];
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public string date;// [17];
        public byte Un;
    }

    // Attendence Reward Info
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class AttendanceRewardInfo
    {

        public byte login;
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class item
        {

            public uint _typeid;
            public uint qntd;
        }
        public item now;
        public item after;
        public uint counter;
    }

    // Attendance Reward Info Ex
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class AttendanceRewardInfoEx : AttendanceRewardInfo
    {

        public PangyaTime last_login;   // Data do ultimo login
    }

    // Attendance Reward Item Context
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class AttendanceRewardItemCtx
    {
        public uint _typeid;
        public uint qntd;
        public byte tipo;
    }

    // Last Five Players Played with player
    public class Last5PlayersGame
    {
        public class LastPlayerGame
        {

            public uint sex;   // gender, genero, sexo, 0 masculino, 1 Feminino
            public string nick;
            public string id;
            public uint uid;
            public static bool operator !=(LastPlayerGame a, LastPlayerGame b)
            {
                return a.uid == b.uid && a.id == b.id;
            }
            public static bool operator ==(LastPlayerGame a, LastPlayerGame b)
            {
                return a.uid == b.uid && a.id == b.id;
            }

            public bool Equals(LastPlayerGame obj)
            {
                return (uid == obj.uid
                            && id == obj.id);
            }
        }
        public void add(player_info _pi, uint _sex)
        {

            if (players[0].uid != _pi.uid)
            {

                var it = players.Where(c => c.uid == _pi.uid).First();

                // Put Player Last Position
                if (it != players[5])
                {
                    // Rotate To Right
                    var idx = Array.IndexOf(players, it);
                    Array.Reverse(players, idx + 1, 5);
                }
                //players.SetValue(4, 5);
                // Update
                players[0].uid = _pi.uid;
            }   // já está em primeiro não precisa mexer mais, só atualizar o friend e o nick, que ele pode ter mudado

            players[0].sex = _sex;
            players[0].nick = _pi.nickname;
        }
        LastPlayerGame[] players;  // Last Five Players Played with player
    }

    // Time 32, HighTime, LowTime
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class time32
    {
        void setTime(int time)
        {
            high_time = (short)(time / 0xFFFF);
            low_time = (short)(time % 0xFFFF);
        }
        public uint getTime()
        {
            return (uint)((high_time * 0xFFFF) | low_time);
        }
        private short high_time;
        private short low_time;
    }

    // Item Buff (Exemple: Yam, Bola Arco-iris)
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class ItemBuff
    {
        public enum eTYPE : uint
        {
            NONE,
            YAM_AND_GOLD,
            RAINBOW,
            RED,
            GREEN,
            YELLOW,
        }
        public uint id;
        public uint _typeid;
        public uint parts_typeid;
        public uint parts_id;
        public uint efeito;
        public uint efeito_qntd;
        public uint slot;
        public PangyaTime use_date;
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public byte[] ucUnknown12;
        public time32 tempo;
        public uint tipo;
        public byte use_yn;
    }

    // Item Buff Ex
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class ItemBuffEx : ItemBuff
    {
        public long index;
        public PangyaTime end_date;
        public uint percent;       // Rate, tipo 2 é 0 por que é 100
    }

    // Guild Info
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GuildInfo
    {
        public uint uid;
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string name;
        public uint point;
        public uint pang;
        public uint total_member;
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public string Image;
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 101)]
        public string Notice;
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 101)]
        public string Introducting;
        public uint Position;
        public uint LeaderUID;
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 22)]
        public string LeaderNickname;
    }

    // GuildInfoEx
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GuildInfoEx : GuildInfo
    {
        public PangyaTime create_time;
    }

    // Canal Info
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class ChannelInfo
    {
        public ChannelInfo()
        {
            flag = new uFlag();
        }
        // Union Flag do Canal, que guarda estrutura de bits da flag do Canal
        [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 4)]
        public class uFlag
        {
            [field: MarshalAs(UnmanagedType.U4, SizeConst = 4)]
            public uint ulFlag { get; set; }
            public uFlag(uint ul = 0)
            {
                ulFlag = ul;
                stBit = new _stBit();
            }
            public class _stBit
            {
                public uint all;
                // Unknown
                public uint junior_bellow;             // De Junior A para baixo
                public uint junior_above;                  // De Junior E para cima
                public uint only_rookie;                   // Somente Rookie(Iniciante)
                public uint beginner_between_junior;       // De Beginner a Junior
                public uint junior_between_senior;  // De Junior a Senior
            }
            public _stBit stBit { get; set; }
        }
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name { get; set; }
        [field: MarshalAs(UnmanagedType.U2, SizeConst = 2)]
        public short max_user;
        [field: MarshalAs(UnmanagedType.U2, SizeConst = 2)]
        public short curr_user;
        [field: MarshalAs(UnmanagedType.U1, SizeConst = 1)]
        public sbyte id;
        [field: MarshalAs(UnmanagedType.Struct, SizeConst = 4)]
        public uFlag flag { get; set; }
        public void clear()
        { flag = new uFlag(); name = ""; }
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class ServerInfoEx2 : ServerInfoEx
    {
        public List<ChannelInfo> v_ci;
    }

    public class LoginData
    {
        public string UserName { get; set; }
        public uint UID { get; set; }
        public uint Blank1 { get; set; }
        public ushort CheckUnknow1 { get; set; }
        public string AuthKey_Login { get; set; }
        public string Version { get; set; }
        public uint CheckUnknow2 { get; set; }
        public uint Blank2 { get; set; }
        public string AuthKey_Game { get; set; }
    }
}