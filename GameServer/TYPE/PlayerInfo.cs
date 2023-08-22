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
            throw new NotImplementedException();
        }

        public override void addCookie(int _uid, ulong _cookie)
        {
            throw new NotImplementedException();
        }

        public override int addExp(int _exp)
        {
            throw new NotImplementedException();
        }

        public override void addGrandZodiacPontos(ulong _pontos)
        {
            throw new NotImplementedException();
        }

        public override void addMoeda(ulong _pang, ulong _cookie)
        {
            throw new NotImplementedException();
        }

        public override void addPang(ulong _pang)
        {
            throw new NotImplementedException();
        }

        public override void addPang(int _uid, ulong _pang)
        {
            throw new NotImplementedException();
        }

        public override void addUserInfo(UserInfoEx _ui, ulong _total_pang_win_game = 0)
        {
            throw new NotImplementedException();
        }

        public override bool checkAlterationCookieOnDB()
        {
            throw new NotImplementedException();
        }

        public override bool checkAlterationPangOnDB()
        {
            throw new NotImplementedException();
        }

        public override bool checkEquipedItem(int _typeid)
        {
            throw new NotImplementedException();
        }

        public override PlayerRoomInfo.uItemBoost checkEquipedItemBoost()
        {
            throw new NotImplementedException();
        }

        public override void consomeCookie(ulong _cookie)
        {
            throw new NotImplementedException();
        }

        public override void consomeMoeda(ulong _pang, ulong _cookie)
        {
            throw new NotImplementedException();
        }

        public override void consomePang(ulong _pang)
        {
            throw new NotImplementedException();
        }

        public override CaddieInfoEx findCaddieById(int _id)
        {
            throw new NotImplementedException();
        }

        public override CaddieInfoEx findCaddieByTypeid(int _typeid)
        {
            throw new NotImplementedException();
        }

        public override CaddieInfoEx findCaddieByTypeidAndId(int _typeid, int _id)
        {
            throw new NotImplementedException();
        }

        public override CardInfo findCardById(int _id)
        {
            throw new NotImplementedException();
        }

        public override CardInfo findCardByTypeid(int _typeid)
        {
            throw new NotImplementedException();
        }

        public override CardEquipInfoEx findCardEquipedById(int _id, int _char_typeid, int _slot)
        {
            throw new NotImplementedException();
        }

        public override CardEquipInfoEx findCardEquipedByTypeid(int _typeid, int _char_typeid = 0, int _slot = 0, int _tipo = 0, int _efeito = 0)
        {
            throw new NotImplementedException();
        }

        public override CharacterInfo findCharacterById(int _id)
        {
            throw new NotImplementedException();
        }

        public override CharacterInfo findCharacterByTypeid(int _typeid)
        {
            throw new NotImplementedException();
        }

        public override CharacterInfo findCharacterByTypeidAndId(int _typeid, int _id)
        {
            throw new NotImplementedException();
        }

        public override FriendInfo findFriendInfoById(string _id)
        {
            throw new NotImplementedException();
        }

        public override FriendInfo findFriendInfoByNickname(string _nickname)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public override MascotInfoEx findMascotById(int _id)
        {
            throw new NotImplementedException();
        }

        public override MascotInfoEx findMascotByTypeid(int _typeid)
        {
            throw new NotImplementedException();
        }

        public override MascotInfoEx findMascotByTypeidAndId(int _typeid, int _id)
        {
            throw new NotImplementedException();
        }

        public override MyRoomItem findMyRoomItemById(int _id)
        {
            throw new NotImplementedException();
        }

        public override MyRoomItem findMyRoomItemByTypeid(int _typeid)
        {
            throw new NotImplementedException();
        }

        public override TrofelEspecialInfo findTrofelEspecialById(int _id)
        {
            throw new NotImplementedException();
        }

        public override TrofelEspecialInfo findTrofelEspecialByTypeid(int _typeid)
        {
            throw new NotImplementedException();
        }

        public override TrofelEspecialInfo findTrofelEspecialByTypeidAndId(int _typeid, int _id)
        {
            throw new NotImplementedException();
        }

        public override TrofelEspecialInfo findTrofelGrandPrixById(int _id)
        {
            throw new NotImplementedException();
        }

        public override TrofelEspecialInfo findTrofelGrandPrixByTypeid(int _typeid)
        {
            throw new NotImplementedException();
        }

        public override TrofelEspecialInfo findTrofelGrandPrixByTypeidAndId(int _typeid, int _id)
        {
            throw new NotImplementedException();
        }

        public override WarehouseItemEx findWarehouseItemById(int _id)
        {
         return  mp_wi.GetValues((uint)_id).First();
        }

        public override WarehouseItemEx findWarehouseItemByTypeid(int _typeid)
        {
            throw new NotImplementedException();
        }

        public override WarehouseItemEx findWarehouseItemByTypeidAndId(int _typeid, int _id)
        {
            throw new NotImplementedException();
        }

        public override int getCharacterMaxSlot(CharacterInfo.Stats _stats)
        {
            throw new NotImplementedException();
        }

        public override int getClubSetMaxSlot(CharacterInfo.Stats _stats)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public override int getSumRecordGrandPrix()
        {
            throw new NotImplementedException();
        }

        public override bool isAuxPartEquiped(int _typeid)
        {
            throw new NotImplementedException();
        }

        public override bool isFriend(int _uid)
        {
            throw new NotImplementedException();
        }

        public override bool isMasterCourse()
        {
            throw new NotImplementedException();
        }

        public override bool isPartEquiped(int _typeid, int _id)
        {
            throw new NotImplementedException();
        }

        public override bool ownerCaddieItem(int _typeid)
        {
            throw new NotImplementedException();
        }

        public override bool ownerHairStyle(int _typeid)
        {
            throw new NotImplementedException();
        }

        public override bool ownerItem(int _typeid, int option = 0)
        {
            throw new NotImplementedException();
        }

        public override bool ownerMailBoxItem(int _typeid)
        {
            throw new NotImplementedException();
        }

        public override bool ownerSetItem(int _typeid)
        {
            throw new NotImplementedException();
        }

        public override void updateCookie()
        {
            throw new NotImplementedException();
        }

        public override bool updateGrandPrixClear(int _typeid, int _position)
        {
            throw new NotImplementedException();
        }

        public override void updateLocationDB()
        {
        }

        public override void updateMedal(uMedalWin _medal_win)
        {
            throw new NotImplementedException();
        }

        public override void updateMedal(int _uid, uMedalWin _medal_win)
        {
            throw new NotImplementedException();
        }

        public override void updateMoeda()
        {
            throw new NotImplementedException();
        }

        public override void updatePang()
        {
            throw new NotImplementedException();
        }

        public override void updateTrofelInfo(int _trofel_typeid, bool _trofel_rank)
        {
            throw new NotImplementedException();
        }

        public override void updateTrofelInfo(int _uid, int _trofel_typeid, bool _trofel_rank)
        {
            throw new NotImplementedException();
        }

        public override void updateUserInfo()
        {
           // snmdb.NormalManagerDB.add(3, new CmdUpdateUserInfo(uid, ui), SQLDBResponse, this);
        }

        public override void updateUserInfo(int _uid, UserInfoEx _ui)
        {
            throw new NotImplementedException();
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
