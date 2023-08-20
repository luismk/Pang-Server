﻿using PangyaAPI.SuperSocket.Interface;
using System;

namespace PangyaAPI.SuperSocket.SocketBase
{
    /// <summary>
    /// Implementação de um filtro de recebimento usando a interface IRequestInfo.
    /// </summary>
    /// <typeparam name="TRequestInfo">O tipo de informação de requisição.</typeparam>
    public class PangyaReceiveFilter : IReceiveFilter<PangyaRequestInfo>
    {

        /// <summary>
        /// implemente a leitura do pacote aqui:
        /// </summary>
        /// <param name="readBuffer"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <param name="toBeCopied"></param>
        /// <param name="rest"></param>
        /// <returns></returns>
        public PangyaRequestInfo Filter(byte[] readBuffer, int offset, int length, bool toBeCopied, out int rest)
        {
            rest = 0;

            try
            {
                if (length < 5)//packet not decrypt
                {
                    _state = FilterState.Error;
                }
                var body = new byte[length];
                Buffer.BlockCopy(readBuffer, offset, body, 0, length);
                var deviceRequest = new PangyaRequestInfo();
                return deviceRequest;
            }
            catch
            {
                return null;
            }
        }
        public PangyaRequestInfo Filter(byte _key, byte[] readBuffer, int offset, int length)
        {
            try
            {
                if (length < 5)//packet not decrypt
                {
                    _state = FilterState.Error;
                }
                var body = new byte[length];
                Buffer.BlockCopy(readBuffer, offset, body, 0, length);
                var deviceRequest = new PangyaRequestInfo().Initialize(_key, body, length);
                return deviceRequest;
            }
            catch
            {
                return null;
            }
        }

        public void Reset()
        {
            // Implemente a reinicialização do filtro, se necessário
        }

        public int LeftBufferSize { get; }

        public FilterState State => _state;
        protected FilterState _state
        {
            get; set;
        }
    }
}
