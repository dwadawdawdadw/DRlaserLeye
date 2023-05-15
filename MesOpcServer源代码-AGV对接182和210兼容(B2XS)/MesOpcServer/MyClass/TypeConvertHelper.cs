using System;
using System.IO;
using System.Text;
namespace TypeConvertHelper
{
    class TypeConvert
    {
        /// <summary> Convert a string of hex digits (ex: E4 CA B2) to a byte array. </summary>
        /// <param name="s"> The string containing the hex digits (with or without spaces). </param>
        /// <returns> Returns an array of bytes. </returns>
        public static byte[] HexStringToByteArray(string s)
        {
            s = s.Replace(" ", "");
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
            {
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
            }

            return buffer;
        }

        /// <summary> Converts an array of bytes into a formatted string of hex digits (ex: E4 CA B2)</summary>
        /// <param name="data"> The array of bytes to be translated into a string of hex digits. </param>
        /// <returns> Returns a well formatted string of hex digits with spacing. </returns>
        public static string ByteArrayToHexString(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            foreach (byte b in data)
            {
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0').PadRight(3, ' '));
            }

            return sb.ToString().Trim().ToUpper();
        }

        /// <summary>
        /// 将一条十六进制字符串转换为ASCII
        /// </summary>
        /// <param name="hexstring">一条十六进制字符串</param>
        /// <returns>返回一条ASCII码</returns>
        public static string HexStringToASCII(string hexstring)
        {
            byte[] bt = HexStringToBinary(hexstring);
            string lin = "";
            for (int i = 0; i < bt.Length; i++)
            {
                lin = lin + bt[i] + " ";
            }


            string[] ss = lin.Trim().Split(new char[] { ' ' });
            char[] c = new char[ss.Length];
            int a;
            for (int i = 0; i < c.Length; i++)
            {
                a = Convert.ToInt32(ss[i]);
                c[i] = Convert.ToChar(a);
            }

            string b = new string(c);
            return b;
        }


        /**/
        /// <summary>
        /// 16进制字符串转换为二进制数组
        /// </summary>
        /// <param name="hexstring">用空格切割字符串</param>
        /// <returns>返回一个二进制字符串</returns>
        public static byte[] HexStringToBinary(string hexstring)
        {

            string[] tmpary = hexstring.Trim().Split(' ');
            byte[] buff = new byte[tmpary.Length];
            for (int i = 0; i < buff.Length; i++)
            {
                buff[i] = Convert.ToByte(tmpary[i], 16);
            }
            return buff;
        }


        /// <summary>
        /// 将byte型转换为字符串
        /// </summary>
        /// <param name="arrInput">byte型数组</param>
        /// <returns>目标字符串</returns>
        public static string ByteArrayToString(byte[] arrInput,int startIndex,int Length)
        {
            int i;
            StringBuilder sOutput = new StringBuilder(Length);
            for (i = startIndex; i < startIndex + Length; i++)
            {
               
                sOutput.Append(Convert.ToChar(arrInput[i]));
            }
            //将此实例的值转换为System.String
            return sOutput.ToString();
        }

        public static UInt16 Bytes2UInt16(byte[] bytes, int startIndex)
        {
            UInt16 num = Convert.ToUInt16(Convert.ToUInt16(bytes[startIndex+1]) & 0xFF);           
            num |= Convert.ToUInt16((Convert.ToUInt16(bytes[startIndex]) << 8) & 0xFF00);

            return num;
        }


        public static UInt32 Bytes2UInt32(byte[] bytes, int startIndex)
        {
            UInt32 num = Convert.ToUInt32(Convert.ToUInt32(bytes[startIndex+3]) & 0xFF);
            num |= Convert.ToUInt32((Convert.ToUInt32(bytes[startIndex + 2]) << 8) & 0xFF00);
            num |= Convert.ToUInt32((Convert.ToUInt32(bytes[startIndex + 1]) << 16) & 0xFF0000);
            num |= Convert.ToUInt32((Convert.ToUInt32(bytes[startIndex]) << 24) & 0xFF000000);

            return num;
        }

        //public static UInt64 Bytes2UInt64(byte[] bytes, int startIndex)
        //{
        //    UInt64 num = Convert.ToUInt64(Convert.ToUInt64(bytes[startIndex + 7]) & 0xFF);

        //    num |= Convert.ToUInt64((Convert.ToUInt64(bytes[startIndex + 6]) << 8) & 0xFF00);
        //    num |= Convert.ToUInt64((Convert.ToUInt64(bytes[startIndex + 5]) << 16) & 0xFF0000);
        //    num |= Convert.ToUInt64((Convert.ToUInt64(bytes[startIndex + 4]) << 24) & 0xFF000000);
        //    num |= Convert.ToUInt64((Convert.ToUInt64(bytes[startIndex + 3]) << 32) & 0xFF00000000);

        //    num |= Convert.ToUInt64((Convert.ToUInt64(bytes[startIndex + 2]) << 40) & 0xFF0000000000);
        //    num |= Convert.ToUInt64((Convert.ToUInt64(bytes[startIndex + 1]) << 48) & 0xFF000000000000);

        //    num |= Convert.ToUInt64((Convert.ToUInt64(bytes[startIndex]) << 56) & 0xFF00000000000000);

        //    return num;
        //}

        public static UInt64 Bytes2UInt64(byte[] bytes, int startIndex)
        {
            UInt64 num = Convert.ToUInt64(Convert.ToUInt64(bytes[startIndex + 6]) & 0xFF);

            num |= Convert.ToUInt64((Convert.ToUInt64(bytes[startIndex + 5]) << 8) & 0xFF00);
            num |= Convert.ToUInt64((Convert.ToUInt64(bytes[startIndex + 4]) << 16) & 0xFF0000);
            num |= Convert.ToUInt64((Convert.ToUInt64(bytes[startIndex + 3]) << 24) & 0xFF000000);
            num |= Convert.ToUInt64((Convert.ToUInt64(bytes[startIndex + 2]) << 32) & 0xFF00000000);

            num |= Convert.ToUInt64((Convert.ToUInt64(bytes[startIndex + 1]) << 40) & 0xFF0000000000);
            num |= Convert.ToUInt64((Convert.ToUInt64(bytes[startIndex]) << 48) & 0xFF000000000000);

            //num |= Convert.ToUInt64((Convert.ToUInt64(0x00) << 56) & 0xFF00000000000000);

            return num;
        }





        /// <summary>
        /// 对接收到的数据进行解包（将接收到的byte型数组解包为Unicode字符串）
        /// </summary>
        /// <param name="recbytes">byte型数组</param>
        /// <returns>Unicode编码的字符串</returns>
        public static string disPackage(byte[] recbytes)
        {
            string temp = "";
            foreach (byte b in recbytes)
                temp += b.ToString("X2") + " ";//ToString("X2") 为C#中的字符串格式控制符
            return temp;
        }

        /**
    * int转byte[]
    * 该方法将一个int类型的数据转换为byte[]形式，因为int为32bit，而byte为8bit所以在进行类型转换时，知会获取低8位，
    * 丢弃高24位。通过位移的方式，将32bit的数据转换成4个8bit的数据。注意 &0xff，在这当中，&0xff简单理解为一把剪刀，
    * 将想要获取的8位数据截取出来。
    * @param i 一个int数字
    * @return byte[]
    */
        public static byte[] Int16ToByteArray(UInt16 i)
        {
            byte[] result = new byte[2];
            result[0] = (byte)((i >> 8) & 0xFF);
            result[1] = (byte)(i & 0xFF);
          
            return result;
        }

        public static byte[] Int32ToByteArray(UInt32 i)
        {
            byte[] result = new byte[4];
            result[0] = (byte)((i >> 24) & 0xFF);
            result[1] = (byte)((i >> 16) & 0xFF);
            result[2] = (byte)((i >> 8) & 0xFF);
            result[3] = (byte)(i & 0xFF);
            return result;
        }

        public static byte[] Int64ToByteArray(UInt64 i)
        {
            byte[] result = new byte[7];  //new byte[8];
           // result[0] = (byte)((i >> 56) & 0xFF);
            result[0] = (byte)((i >> 48) & 0xFF);
            result[1] = (byte)((i >> 40) & 0xFF);
            result[2] = (byte)((i >> 32) & 0xFF);
            result[3] = (byte)((i >> 24) & 0xFF);
            result[4] = (byte)((i >> 16) & 0xFF);
            result[5] = (byte)((i >> 8) & 0xFF);
            result[6] = (byte)(i & 0xFF);
            return result;
        }

        public static string ByteToCmdString(byte i)
        {
            byte[] result = new byte[1];           
            result[0] = i;         

            string str = Encoding.ASCII.GetString(result);
            return str;
        }

        public static string Int16ToCmdString(UInt16 i)
        {
            byte[] result = new byte[2];
            result[0] = (byte)((i >> 8) & 0xFF);
            result[1] = (byte)(i & 0xFF);

            string str = Encoding.ASCII.GetString(result);           
            return str;

           
        }

        public static string Int32ToCmdString(UInt32 i)
        {
            byte[] result = new byte[4];
            result[0] = (byte)((i >> 24) & 0xFF);
            result[1] = (byte)((i >> 16) & 0xFF);
            result[2] = (byte)((i >> 8) & 0xFF);
            result[3] = (byte)(i & 0xFF);

            string str = Encoding.ASCII.GetString(result);
            return str;

           
        }

        public static string Int64ToCmdString(UInt64 i)
        {
            byte[] result = new byte[7];  //new byte[8];
            // result[0] = (byte)((i >> 56) & 0xFF);
            result[0] = (byte)((i >> 48) & 0xFF);
            result[1] = (byte)((i >> 40) & 0xFF);
            result[2] = (byte)((i >> 32) & 0xFF);
            result[3] = (byte)((i >> 24) & 0xFF);
            result[4] = (byte)((i >> 16) & 0xFF);
            result[5] = (byte)((i >> 8) & 0xFF);
            result[6] = (byte)(i & 0xFF);

            string str = Encoding.ASCII.GetString(result);
            return str;
        }

        public static string Int2String(int str)
        {
            string S = Convert.ToString(str);
            return S;
        }

        public static int String2Int32(string str)
        {
            int a;
            int.TryParse(str, out a);
            int a1 = Convert.ToInt32(str);
            return a1;
        }


        /*  将int转为低字节在后，高字节在前的byte数组
          
        */
        public static byte[] IntToByteArray2(int value)
        {
            byte[] src = new byte[4];
            src[0] = (byte)((value >> 24) & 0xFF);
            src[1] = (byte)((value >> 16) & 0xFF);
            src[2] = (byte)((value >> 8) & 0xFF);
            src[3] = (byte)(value & 0xFF);
            return src;
        }
        //将高字节在前转为int，低字节在后的byte数组(与IntToByteArray2相对应)
        public static int ByteArrayToInt2(byte[] bArr)
        {
            if (bArr.Length != 4)
            {
                return -1;
            }
            return (int)((((bArr[0] & 0xff) << 24)
                       | ((bArr[1] & 0xff) << 16)
                       | ((bArr[2] & 0xff) << 8)
                       | ((bArr[3] & 0xff) << 0)));
        }

        public static string StringToHexArray(string input)
        {
            char[] values = input.ToCharArray();
            StringBuilder sb = new StringBuilder(input.Length * 3);
            foreach (char letter in values)
            {
                // Get the integral value of the character.
                int value = Convert.ToInt32(letter);
                // Convert the decimal value to a hexadecimal value in string form.
                string hexOutput = String.Format("{0:X}", value);
                sb.Append(Convert.ToString(value, 16).PadLeft(2, '0').PadRight(3, ' '));
            }

            return sb.ToString().ToUpper();



        }

    }
}
