﻿using System;
using System.Data;
using System.Globalization;
using System.Diagnostics;
using System.Data.Common;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic.ApplicationServices;

namespace PangyaAPI.SQL
{
    public class ctx_res
    {
        public object[] data;
        public uint cols = new uint();
        public ctx_res next;
        public bool IsNotNull(int colum)
        {
            if (data == null)
            {
                return false;
            }
            return data[colum] != null;
        }
        public bool GetBoolean(int colum)
        {                           
            return Convert.ToBoolean(data[colum]);
        }

        public int GetInt32(int colum)
        {
            return Convert.ToInt32(data[colum]);
        }

        public uint GetUInt32(int colum)
        {
            return Convert.ToUInt32(data[colum]);
        }
    }

    public class Result_Set : System.IDisposable
	{
        public enum STATE_TYPE : uint
        {
            HAVE_DATA,
            _NO_DATA,
            _UPDATE_OR_DELETE,
            _ERROR
        }           
        public Result_Set(uint _state)
        {
            this.m_state = (STATE_TYPE)_state;
            this.m_lines_affected = -1;
            this.m_data = null;
            this.m_curr_data = null;
            this.m_lines = 0;
            this.m_cols = 0;
        }

        public Result_Set(uint _state,
            uint _cols,
            int _lines_affected)
        {
            this.m_state = (STATE_TYPE)_state;
            this.m_lines_affected = _lines_affected;
            this.m_data = null;
            this.m_curr_data = null;
            this.m_lines = 0;
            this.m_cols = _cols;
        }

        public void Dispose()
        {
            destroy();
        }

        public void destroy()
        {
            if (m_data != null)
            {
                ctx_res pNext = null;

                while (m_data != null)
                {
                    pNext = m_data.next; // Armazena o próximo nó antes de modificar m_data

                    if (m_data.data != null)
                    {
                        for (uint i = 0; i < m_data.cols; ++i)
                        {
                            m_data.data[i] = new string[] { "0" }; // Definindo para o caractere nulo
                        }

                        m_data.data = null; // Libera o array de dados
                    }

                    m_data = pNext; // Avança para o próximo
                }
            }

            m_data = null; // Garante que m_data seja nulo no final
        }

        public uint reserve_cols(int _cols)
        {
            if (_cols > 0)
            {
                addLineData();

                m_cols = m_curr_data.cols = (uint)_cols;

                if (m_curr_data.data == null)
                {
                    m_curr_data.data = new string[_cols][]; // Preenchido com 0 por padrão
                }
            }

            return (uint)_cols;
        }

        public ctx_res getFirstLine()
        {
            return m_data;
        }

        public void setLinesAffected(int _lines_affected)
        {
            m_lines_affected = _lines_affected;
        }

        public object getColAt(uint _index)
        {
            if (m_curr_data == null)
            {
                throw new Exception("Nao tem nenhum dados reservado, reserve primeiro.");
            }

            if ((int)_index < 0 || _index >= m_curr_data.cols)
            {
                throw new Exception("Index out of range.");
            }

            return m_curr_data.data[_index];
        }

        public uint getNumLines()
        {
            return  m_lines;
        }

        public uint getNumCols()
        {
            return m_cols;
        }

        public void setState(uint _state)
        {
            m_state = (STATE_TYPE)_state;
        }

        public uint getState()
        {
            return (uint)m_state;
        }

        public void addLine()
        {
            if (m_state == STATE_TYPE.HAVE_DATA)
            {
                reserve_cols((int)m_cols);
            }
        }

        protected ctx_res addLineData()
        {                   
            if (m_data == null)
            {
                m_data = new ctx_res();

                m_curr_data = m_data;

                // Init Dados
                m_curr_data.next = null;
                m_curr_data.cols = 0;
                m_curr_data.data = null;
            }
            else
            {
                m_curr_data.next = new ctx_res();

                m_curr_data = m_curr_data.next;

                // Init Dados
                m_curr_data.next = null;
                m_curr_data.cols = 0;
                m_curr_data.data = null;
            }

            ++m_lines;

            return m_curr_data;
        }

        internal void setRow(DataRow dataRow)
        {
            m_data.data = dataRow.ItemArray;
        }

        protected STATE_TYPE m_state = 0;
        protected int m_lines_affected = 0;

        protected uint m_lines =0;
        protected uint m_cols = 0;
        protected ctx_res m_data;
        protected ctx_res m_curr_data;
    }
}

