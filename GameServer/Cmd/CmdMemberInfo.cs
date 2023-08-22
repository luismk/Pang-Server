﻿using GameServer.TYPE;
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
    public class CmdMemberInfo : Pangya_DB
    {
        readonly uint m_uid = uint.MaxValue;
        MemberInfoEx m_mi = new MemberInfoEx();
        protected override string _getName { get; set; } = "CmdMemberInfo";

        public CmdMemberInfo(uint _uid) : this(true)
        {
            m_uid = _uid;
        }

        public CmdMemberInfo(bool wait = false) : base(wait)
        {
        }

        protected override void lineResult(ctx_res _result, uint _index_result)
        {
            checkColumnNumber(28);
            try
            {
                // Aqui faz as coisas
                if (_result.IsNotNull(0))
                   (m_mi.id) = (string)_result.data[0];

                m_mi.uid = Convert.ToUInt32(_result.data[1]);
                m_mi.sexo = Convert.ToByte(_result.data[2]);
                m_mi.do_tutorial = Convert.ToByte(_result.data[3]);

                if (_result.IsNotNull(4))
                   m_mi.nick_name = Convert.ToString(_result.data[4]);

                m_mi.nickNT = "@" + m_mi.nick_name;
                m_mi.school = Convert.ToUInt32(_result.data[5]);
                m_mi.capability.ulCapability = Convert.ToUInt32(_result.data[6]);
                m_mi.capability.setState();
                m_mi.manner_flag = Convert.ToUInt32(_result.data[9]);

                if (_result.IsNotNull(11))
                  m_mi.guild_name = Convert.ToString(_result.data[11]);

                m_mi.guild_uid = Convert.ToUInt64(_result.data[12]);
                m_mi.guild_pang = Convert.ToInt64(_result.data[13]);
                m_mi.guild_point = Convert.ToUInt32(_result.data[14]);
                m_mi.guild_mark_img_no = Convert.ToUInt32(_result.data[15]); // Guild Idx é o ultilizado no PangYa JP
                m_mi.event_1 = Convert.ToByte(_result.data[16]);
                m_mi.event_2 = Convert.ToByte(_result.data[17]);

                // 1 Player loga primeira vezes, 2 é o um player que já logou mais de 1x
                //m_mi.flag_login_time = 2u;

                // Sexo do player
                m_mi.state_flag.stFlagBit.sexo = m_mi.sexo;

                m_mi.papel_shop.limit_count = Convert.ToInt16(_result.data[18]);
                m_mi.papel_shop.current_count = Convert.ToInt16(_result.data[22]);
                m_mi.papel_shop.remain_count = Convert.ToInt16(_result.data[23]);

                if (_result.data[24] != null)
                    m_mi.papel_shop_last_update.CreateTime(_result.data[24].ToString());

                m_mi.level = Convert.ToByte(_result.data[25]);

                if (_result.IsNotNull(26))
                    m_mi.guild_mark_img = Convert.ToString(_result.data[26]);


                if (m_mi.uid != m_uid)
                    throw new Exception("[CmdMemberInfo::lineResult][Error] UID do member info do player nao e igual ao requisitado. UID Req: " + (m_uid) + " != " + (m_mi.uid));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

        public MemberInfoEx getInfo()
        {
            return m_mi;
        }

        public MemberInfo getMemberInfo()
        {
            return m_mi;
        }

        protected override Response prepareConsulta(database input_db)
        {
            var r = procedure(_db, "pangya.ProcGetUserInfo", new string[] { "@IDUSER" }, new type_SqlDbType[] { type_SqlDbType.Int }, new string[] { m_uid.ToString() }, ParameterDirection.Input);
            checkResponse(r, "nao conseguiu pegar o member info do player: " + (m_uid));
            return r;
        }
    }
}
