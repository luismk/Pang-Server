using GameServer.TYPE;
using PangyaAPI.SQL.DATA.TYPE;
using PangyaAPI.SQL;
using PangyaAPI.SQL.TYPE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Cmd
{
    public class CmdCouponGacha : Pangya_DB
    {
        readonly uint m_uid = uint.MaxValue;
        CouponGacha m_cg = new CouponGacha();
        protected override string _getName { get; set; } = "CmdCouponGacha";

        public CmdCouponGacha(uint _uid) 
        {
            m_uid = _uid;
        }

        protected override void lineResult(ctx_res _result, uint _index_result)
        {
            checkColumnNumber(1, _result.cols);
            try
            {

                //if (_index_result == 0)
                //    m_cg.normal_ticket = Convert.ToUInt32(_result.data[0]);
                //else if (_index_result == 1)
                //    m_cg.partial_ticket = Convert.ToUInt32(_result.data[0]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

        protected override Response prepareConsulta(database _db)
        {
            var m_szConsulta  = new string[] { "SELECT c0 FROM pangya.pangya_item_warehouse WHERE typeid = 436207744 AND uid = ", "SELECT c0 FROM pangya.pangya_item_warehouse WHERE typeid = 436207747 AND uid = " };

            var r = consulta(_db,m_szConsulta[0] + (m_uid) + "; " + m_szConsulta[1] + (m_uid));
            checkResponse(r, "nao conseguiu pegar o(s) coupon(s) gacha do player: " + (m_uid));
            return r;
        }


        public CouponGacha getCouponGacha()
        {
            return m_cg;
        }

        public void getCouponGacha(CouponGacha cg)
        {
            m_cg = cg;
        }
    }
}
