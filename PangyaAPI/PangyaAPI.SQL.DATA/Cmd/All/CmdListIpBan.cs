﻿
using PangyaAPI.SQL.DATA.TYPE;
using PangyaAPI.SQL;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace PangyaAPI.SQL.DATA.Cmd
{
    public class CmdListIpBan: Pangya_DB
    {
        List<IPBan> v_list_ip_ban;
        protected override string _getName { get; set; } = "CmdListIpBan";

        public CmdListIpBan()
        {
            v_list_ip_ban = new List<IPBan>();
        }

        protected override void lineResult(ctx_res _result, uint _index_result)
        {
            checkColumnNumber(2);
            try
            {
                IPBan pb = new IPBan();
                string ip = "", mask = "";
                int offset = -1;
                bool error = false;

                if (_result.IsNotNull(0))
                {
                    ip = _result.data[0].ToString();
                }
                if (_result.IsNotNull(1))
                {
                    mask = _result.data[1].ToString();
                }

                // Verifica se é um IP RANGE
                if ((offset = ip.IndexOf('/')) == -1 && ip.Length > 15)
                {
                    return;
                }
                else
                {
                    // Range IP
                    if (offset != -1)
                    {
                        pb.Type = IPBanType.IPBlockRange;

                        mask = ip.Substring(offset + 1);
                        ip = ip.Substring(0, offset);

                        error = IPAddress.TryParse(ip, out IPAddress ipAddr);
                        pb.IP = (uint)IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ipAddr.GetAddressBytes(), 0));

                        error = IPAddress.TryParse(mask, out IPAddress maskAddr);
                        pb.Mask = (uint)IPAddress.NetworkToHostOrder(BitConverter.ToInt32(maskAddr.GetAddressBytes(), 0));
                    }
                    else
                    {
                        // IP Normal
                        error = IPAddress.TryParse(ip, out IPAddress ipAddr);
                        pb.IP = (uint)IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ipAddr.GetAddressBytes(), 0));

                        error = IPAddress.TryParse(mask, out IPAddress maskAddr);
                        pb.Mask = (uint)IPAddress.NetworkToHostOrder(BitConverter.ToInt32(maskAddr.GetAddressBytes(), 0));
                    }
                }

                // Error 1 Success tem um ip e mask válida
                if (error)
                {
                    v_list_ip_ban.Add(pb);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

        protected override Response prepareConsulta()
        {
            v_list_ip_ban.Clear();

            var r = consulta("SELECT ip, mask FROM pangya.pangya_ip_table");

            checkResponse(r, "nao conseguiu recuperar a lista de MAC Address");
            return r;
        }


        public List<IPBan> getListIPBan()
        {
            return v_list_ip_ban;
        }

    }
}
