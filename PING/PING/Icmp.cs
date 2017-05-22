using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PING
{
    /// <summary>
    /// ICMP报文结构
    /// </summary>
    public class Icmp
    {
        //ICMP类型,占一个字节
        private Byte my_type;
        //ICMP代码,占一个字节
        private Byte my_subCode;
        //ICMP校验和，两个字节
        private UInt16 my_checkSum;
        //标示符，两个字节
        private UInt16 my_identifier;
        //序列码，两个字节
        private UInt16 my_sequenceCode;

        private Byte[] data;

        /// <summary>
        /// 新建Icmp数据报
        /// </summary>
        /// <param name="type"></param>
        /// <param name="code"></param>
        /// <param name="checkSum"></param>
        /// <param name="identifier"></param>
        /// <param name="sequence"></param>
        /// <param name="dataLength"></param>
        public Icmp(Byte type, Byte code, UInt16 checkSum, UInt16 identifier, UInt16 sequence, int dataLength)
        {
            this.my_type = type;
            this.my_subCode = code;
            this.my_checkSum = checkSum;
            this.my_identifier = identifier;
            this.my_sequenceCode = sequence;
            data = new Byte[dataLength];
            for (int i = 0; i < dataLength; i++)
            {
                data[i] = (Byte)'k';
            }
        }

        public int CountByte(Byte[] buffer)
        {
            Byte[] b_type = new Byte[1] { my_type };
            Byte[] b_subCode = new Byte[1] { my_subCode };
            Byte[] b_checkSum = BitConverter.GetBytes(my_checkSum);
            Byte[] b_identifier = BitConverter.GetBytes(my_identifier);
            Byte[] b_sequenceNumnber = BitConverter.GetBytes(my_sequenceCode);
            int count = 0;
            Array.Copy(b_type, 0, buffer, count, b_type.Length);
            count += b_type.Length;
            Array.Copy(b_subCode, 0, buffer, count, b_subCode.Length);
            count += b_subCode.Length;
            Array.Copy(b_checkSum, 0, buffer, count, b_checkSum.Length);
            count += b_checkSum.Length;
            Array.Copy(b_identifier, 0, buffer, count,b_identifier.Length);
            count += b_identifier.Length;
            Array.Copy(b_sequenceNumnber, 0, buffer, count, b_sequenceNumnber.Length);
            count += b_sequenceNumnber.Length;
            return count;
        }

        /// <summary>
        /// ICMP的校验算法
        /// </summary>
        /// <param name="buffer">报文首部</param>
        /// <returns></returns>
        public static UInt16 SumOfCheck(UInt16[] buffer)
        {
            int checkSum = 0;
            for (int i = 0; i < buffer.Length; i++)
            {
                checkSum += (int)buffer[i];
            }
            checkSum = (checkSum >> 16) + (checkSum & 0xffff);
            checkSum += (checkSum >> 16);
            return (UInt16)(~checkSum);
        }


    }
}
