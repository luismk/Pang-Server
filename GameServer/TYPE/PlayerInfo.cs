using PangyaAPI.SQL.DATA.TYPE;
using PangyaAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _smp = PangyaAPI.Utilities.Log;
using snmdb = PangyaAPI.SQL.Manager;
namespace GameServer.TYPE
{
    public class PlayerInfo : PlayerInfoBase
    {
        public PlayerInfo()
        {
        }

        public PlayerInfo(string _id, BlockFlag _block_flag, short _level, string nick, string pwd, int _uid) : base(_id, _block_flag, _level, nick, pwd, _uid)
        {
        }


        public override void addCookie(ulong _cookie)
        {
        }

        public override void addCookie(int _uid, ulong _cookie)
        {
        }

        public override int addExp(int _exp)
        {
            return 1;
        }

        public override void addGrandZodiacPontos(ulong _pontos)
        {
        }

        public override void addMoeda(ulong _pang, ulong _cookie)
        {
        }

        public override void addPang(ulong _pang)
        {
        }

        public override void addPang(int _uid, ulong _pang)
        {
        }

        public override void addUserInfo(UserInfoEx _ui, ulong _total_pang_win_game = 0)
        {
        }

        public override bool checkAlterationCookieOnDB()
        {
            return false;
        }

        public override bool checkAlterationPangOnDB()
        {
            return false;
        }

        public override bool checkEquipedItem(int _typeid)
        {
            return false;
        }

        public override PlayerRoomInfo.uItemBoost checkEquipedItemBoost()
        {
            return null;
        }

        public override void consomeCookie(ulong _cookie)
        {
        }

        public override void consomeMoeda(ulong _pang, ulong _cookie)
        {
        }

        public override void consomePang(ulong _pang)
        {
        }

        public override CaddieInfoEx findCaddieById(int _id)
        {
            return null;
        }

        public override CaddieInfoEx findCaddieByTypeid(int _typeid)
        {
            return null;
        }

        public override CaddieInfoEx findCaddieByTypeidAndId(int _typeid, int _id)
        {
            return null;
        }

        public override CardInfo findCardById(int _id)
        {
            return null;
        }

        public override CardInfo findCardByTypeid(int _typeid)
        {
            return null;
        }

        public override CardEquipInfoEx findCardEquipedById(int _id, int _char_typeid, int _slot)
        {
            return null;
        }

        public override CardEquipInfoEx findCardEquipedByTypeid(int _typeid, int _char_typeid = 0, int _slot = 0, int _tipo = 0, int _efeito = 0)
        {
            return null;
        }

        public override CharacterInfo findCharacterById(int _id)
        {
            return null;
        }

        public override CharacterInfo findCharacterByTypeid(int _typeid)
        {
            return null;
        }

        public override CharacterInfo findCharacterByTypeidAndId(int _typeid, int _id)
        {
            return null;
        }

        public override FriendInfo findFriendInfoById(string _id)
        {
            return null;
        }

        public override FriendInfo findFriendInfoByNickname(string _nickname)
        {
            return null;
        }

        public override FriendInfo findFriendInfoByUID(int _uid)
        {
            if (_uid == 0u)
            {

                return null;
            }

            var it = mp_fi.Where(c=> c.Key == _uid);

            return it.Any() ? it.First().Value : null;
        }

        public override GrandPrixClear findGrandPrixClear(int _typeid)
        {
            return null;
        }

        public override MascotInfoEx findMascotById(int _id)
        {
            return null;
        }

        public override MascotInfoEx findMascotByTypeid(int _typeid)
        {
            return null;
        }

        public override MascotInfoEx findMascotByTypeidAndId(int _typeid, int _id)
        {
            return null;
        }

        public override MyRoomItem findMyRoomItemById(int _id)
        {
            return null;
        }

        public override MyRoomItem findMyRoomItemByTypeid(int _typeid)
        {
            return new MyRoomItem();
        }

        public override TrofelEspecialInfo findTrofelEspecialById(int _id)
        {
            return new TrofelEspecialInfo();
        }

        public override TrofelEspecialInfo findTrofelEspecialByTypeid(int _typeid)
        {
            return new TrofelEspecialInfo();
        }

        public override TrofelEspecialInfo findTrofelEspecialByTypeidAndId(int _typeid, int _id)
        {
            return new TrofelEspecialInfo();
        }

        public override TrofelEspecialInfo findTrofelGrandPrixById(int _id)
        {
            return new TrofelEspecialInfo();
        }

        public override TrofelEspecialInfo findTrofelGrandPrixByTypeid(int _typeid)
        {
            return new TrofelEspecialInfo();
        }

        public override TrofelEspecialInfo findTrofelGrandPrixByTypeidAndId(int _typeid, int _id)
        {
            return new TrofelEspecialInfo();
        }

        public override WarehouseItemEx findWarehouseItemById(int _id)
        {
         return  mp_wi.GetValues((uint)_id).First();
        }

        public override WarehouseItemEx findWarehouseItemByTypeid(int _typeid)
        {
            return new WarehouseItemEx();
        }

        public override WarehouseItemEx findWarehouseItemByTypeidAndId(int _typeid, int _id)
        {
            return new WarehouseItemEx();
        }

        public override int getCharacterMaxSlot(CharacterInfo.Stats _stats)
        {
            return 1;
        }

        public override int getClubSetMaxSlot(CharacterInfo.Stats _stats)
        {
            return 1;
        }

        public override int getSizeCupGrandZodiac()
        {
            int size_cup = 1;

            if (grand_zodiac_pontos < 300)
                size_cup = 9;
            else if (grand_zodiac_pontos < 600)
                size_cup = 8;
            else if (grand_zodiac_pontos < 1200)
                size_cup = 7;
            else if (grand_zodiac_pontos < 1800)
                size_cup = 6;
            else if (grand_zodiac_pontos < 4000)
                size_cup = 5;
            else if (grand_zodiac_pontos < 5200)
                size_cup = 4;
            else if (grand_zodiac_pontos < 7600)
                size_cup = 3;
            else if (grand_zodiac_pontos < 10000)
                size_cup = 2;

            return size_cup;
        }

        public override int getSlotPower()
        {
            return 1;
        }

        public override int getSumRecordGrandPrix()
        {
            return 1;
        }

        public override bool isAuxPartEquiped(int _typeid)
        {
            return false;
        }

        public override bool isFriend(int _uid)
        {
            return false;
        }

        public override bool isMasterCourse()
        {
            return false;
        }

        public override bool isPartEquiped(int _typeid, int _id)
        {
            return false;
        }

        public override bool ownerCaddieItem(int _typeid)
        {
            return false;
        }

        public override bool ownerHairStyle(int _typeid)
        {
            return false;
        }

        public override bool ownerItem(int _typeid, int option = 0)
        {
            return false;
        }

        public override bool ownerMailBoxItem(int _typeid)
        {
            return false;
        }

        public override bool ownerSetItem(int _typeid)
        {
            return false;
        }

        public override void updateCookie()
        {
        }

        public override bool updateGrandPrixClear(int _typeid, int _position)
        {
            return false;
        }

        public override void updateLocationDB()
        {
        }

        public override void updateMedal(uMedalWin _medal_win)
        {
        }

        public override void updateMedal(int _uid, uMedalWin _medal_win)
        {
        }

        public override void updateMoeda()
        {
        }

        public override void updatePang()
        {
        }

        public override void updateTrofelInfo(int _trofel_typeid, bool _trofel_rank)
        {
        }

        public override void updateTrofelInfo(int _uid, int _trofel_typeid, bool _trofel_rank)
        {
        }

        public override void updateUserInfo()
        {
           // snmdb.NormalManagerDB.add(3, new CmdUpdateUserInfo(uid, ui), SQLDBResponse, this);
        }

        public override void updateUserInfo(int _uid, UserInfoEx _ui)
        {
        }

        public multimap<uint/*ID*/, WarehouseItemEx> findWarehouseItemItByTypeid(uint _typeid)
        {
            multimap<uint/*ID*/, WarehouseItemEx> HasSet = new multimap<uint, WarehouseItemEx>();
            foreach (var item in mp_wi)
            {
               var result = item.Value.FirstOrDefault(c => c._typeid == _typeid);
                if (result!=null)
                {
                    HasSet.Add(result.id,result);
                }
            }
            return HasSet;
        }

        public multimap<uint/*ID*/, WarehouseItemEx> findWarehouseItemItByTypeid(uint _typeid, uint _id)
        {
            multimap<uint/*ID*/, WarehouseItemEx> HasSet = new multimap<uint, WarehouseItemEx>();

            var it = mp_wi.Find(_id);
            foreach (var item in it)
            {
                if (item._typeid != _typeid)
                {
                    HasSet.Add(item.id, item);
                }
            }
            return HasSet;
        }

        void SQLDBResponse(int _msg_id, PangyaAPI.SQL.Pangya_DB _pangya_db, object _arg)
        {
            if (_arg == null)
            {
                _smp.Message_Pool.push("[PlayerInfo::SQLDBResponse][WARNING] _arg is nullptr na msg_id = " + (_msg_id));
                return;
            }

            try
            {
                var pi = (PlayerInfo)_arg;


                //// Por Hora só sai, depois faço outro tipo de tratamento se precisar
                //if (_pangya_db.getException().getCodeError() != 0)
                //{

                //    // Trata alguns tipo aqui, que são necessários
                //    switch (_msg_id)
                //    {
                //        case 1: // Update Pang
                //            {
                //                // Error at update on DB
                //                pi.m_update_pang_db.errorUpdateOnDB();

                //                break;
                //            }
                //        case 2: // Update Cookie
                //            {
                //                // Error at update on DB
                //                pi.m_update_cookie_db.errorUpdateOnDB();

                //                break;
                //            }
                //        case 5: // Update Location Player on DB
                //            {
                //                // Error at update on DB
                //                pi.m_pl.errorUpdateOnDB();

                //                break;
                //            }
                //    }

                //    _smp::Message_Pool.push("[PlayerInfo::SQLDBResponse][Error] " + _pangya_db.getException().getFullMessageError());

                //    return;
                //}

                //switch (_msg_id)
                //{
                //    case 1: // UPDATE pang
                //        {

                //            // Success update on DB
                //            pi.m_update_pang_db.confirmUpdadeOnDB();

                //            // Não tem retorno então não precisa reinterpretar o pangya_db
                //            //var cmd_up = ( CmdUpdatePang)(_pangya_db);
                //            break;
                //        }
                //    case 2: // UPDATE cookie
                //        {

                //            // Success update on DB
                //            pi.m_update_cookie_db.confirmUpdadeOnDB();

                //            // Não tem retorno então não precisa reinterpretar o pangya_db
                //            //var cmd_uc = ( CmdUpdateCookie)(_pangya_db);
                //            break;
                //        }
                //    case 3: // UPDATE USER INFO
                //        {
                //            // Não tem retorno então não precisa reinterpretar o pangya_db
                //            // var cmd_uui = ( CmdUpdateUserInfo)(_pangya_db);
                //            break;
                //        }
                //    case 4: // Update Normal Trofel Info
                //        {
                //            break;
                //        }
                //    case 5: // Update Location Player on DB
                //        {
                //            // Success update on DB
                //            pi.m_pl.confirmUpdadeOnDB();

                //            break;
                //        }
                //    case 6: // Insert Grand Prix Clear
                //        {

                //            var cmd_igpc = (CmdInsertGrandPrixClear)(_pangya_db);

                //            break;
                //        }
                //    case 7: // Update Grand Prix Clear
                //        {
                //            var cmd_ugpc = (CmdUpdateGrandPrixClear)(_pangya_db);

                //            break;
                //        }
                //    case 8: // Update Grand Zodiac Pontos
                //        {
                //            var cmd_gzp = (CmdGrandZodiacPontos)(_pangya_db);

                //            break;
                //        }
                //    case 0:
                //    default:
                //        break;
                //}

            }
            catch (Exception)
            {

                throw;
            }
        }
    }

}
