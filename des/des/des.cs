using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace des
{
    class des
    {
        private string input="";                //输入
        private string key="";                  //秘钥
        private string output="";               //输出
        private UInt32[] li=new UInt32[16];     //待加密左边 32 位
        private UInt32[] ri = new UInt32[16];   //待加密右边 32 位
        private UInt32[] k = new UInt32[16];    //密钥，秘钥(yao)，密钥(yue)
        private int[] IP ={   //明文初始置换IP
                              58,50,42,34,26,18,10,2,
                              60,52,44,36,28,20,12,4,
                              62,54,46,38,30,22,14,6,
                              64,56,48,40,32,24,16,8,
                              57,49,41,33,25,17,9,1,
                              59,51,43,35,27,19,11,3,
                              61,53,45,37,29,21,13,5,
                              63,55,47,39,31,23,15,7
                         };
        private int[] IP_1 ={   //明文初始置换逆IP表
                                40,8,48,16,56,24,64,32,
                                39,7,47,15,55,23,63,31,
                                38,6,46,14,54,22,62,30,
                                37,5,45,13,53,21,61,29,
                                36,4,44,12,52,20,60,28,
                                35,3,43,11,51,19,59,27,
                                34,2,42,10,50,18,58,26,
                                33,1,41,9,49,17,57,25
                           };
        private int[] E ={      //比特选择表，将 32bit 扩展到48bit
                            32,1,2,3,4,5,
                            4,5,6,7,8,9,
                            8,9,10,11,12,13,
                            12,13,14,15,16,17,
                            16,17,18,19,20,21,
                            20,21,22,23,24,25,
                            24,25,26,27,28,29,
                            28,29,30,31,32,1
                        };

        private int[] PC1={   //对密钥进行初始置换
                              57,49,41,33,25,17,9,
                              1,58,50,42,34,26,18,
                              10,2,59,51,43,35,27,
                              19,11,3,60,52,44,36,
                              63,55,47,39,31,23,15,
                              7,62,54,46,38,30,22,
                              14,6,61,53,45,37,29,
                              21,13,5,28,20,12,4
                            };

        private int[] PC2 ={  //对密钥进行置换，每次置换+运算之后产生一个子密钥
                              14,17,11,24,1,5,
                              3,28,15,6,21,10,
                              23,19,12,4,26,8,
                              16,7,27,20,13,2,
                              41,52,31,37,47,55,
                              30,40,51,45,33,48,
                              44,49,39,56,34,53,
                              46,42,30,36,29,32
                          };

        public des(string input,string key)             //构造函数
        {
            this.input = input;
            this.key = key;
            this.output=this.encrypt();
        }

        //加密函数
        private string encrypt()
        {
            UInt64 keyH = 0x0000000000000000;            //定义一个8字节的整型数，以16进制的方式存储秘钥
            UInt64 inputH = 0x0000000000000000;          //定义一个8字节的整形数，以16进制的方式存储输入字符串串

            foreach (char ch in this.input)              //将秘钥中的字符转化为 16 进制数
            {
                keyH <<= 8;
                keyH |= ch;
            }

            foreach (char ch in this.input)             //将输入的字符串装化为 16 进制数
            {
                inputH <<= 8;
                inputH |= ch;
            }

            spilitInt64(keyH, ref li[0], ref ri[0]);    //将装置后的明文分成 32 位的两个部分

            for (int i = 0; i < 16; i++)
            {
                /****
                 * 
                 * 核心算法
                 * 
                 * ***/
            }

            keyH = this.exchangePC1_64_56(0x0101010101010101);   //0x8000 0000 0000 0000 -> 0x0100 0000 0000 0000       0x0101 0101 0101 0101 -> 0x000 0000 0000 0000

            //keyH = 0x0000000000000001;
            //exchange(ref keyH, ip);
            //exchange(ref keyH, ip_1);

            

            //spilitInt64(keyH,ref  li[0],ref ri[0]);
            //this.output = String.Format("{0:X}", li[0]);
            //this.output = String.Format("{0:X}", ri[0]);
            this.output = String.Format("{0:X}", keyH);


            return this.output;
        }


        //将 64 位数据变成左右 2 个 32 位的部分
        private void spilitInt64(UInt64 input, ref UInt32 li,ref UInt32 ri)
        {
            li = 0;
            ri = 0;
            li |= ((UInt32)(input >> 32));
            ri |= (UInt32)(input&0x00000000ffffffff);
        }

        //解密函数
        private string decipher()
        {
            string output = "";
            return output;
        }

        public string getInput()            //获取输入数据
        {
            return this.output;
        }

        public string getOutput()           //获取输出数据
        {
            return this.output;
        }

        //置换函数
        private void exchangeIP(ref UInt64 dt, int[] ip)       
        {
            UInt64 temp = 0x8000000000000000;
            UInt64 result = 0;
            for (int i = 0; i < 64; i++)
            {
                cycleShift(ref dt, ip[i] - 1-i, true);
                result |= (temp & dt);
                cycleShift(ref dt, ip[i] - 1-i, false);     //15921065532
                temp >>= 1;
            }
            dt = result;
        }

        //循环移位函数,长度为 64 位，函数重载
        private void cycleShift(ref UInt64 str,int i,bool isLeft=true)        
        {
            UInt64 temp = str;              // 64 位整型数
            //temp |= str;                  //临时存储str的值
            i %= 64;
            if (isLeft)                     //向左移位
            {
                str <<= i;                  //输入数据向左移动 i 位
                temp >>= (64 - i);          //temp 向右移动 64 - i 位
                str |= temp;                //将两个移位后的变量合并
            }
            else
            {
                str >>= i;                  //输入数据向左移动 i 位
                temp <<= (64 - i);          //temp 向右移动 64 - i 位
                str |= temp;                //将两个移位后的变量合并
            }
        }

        //循环移位函数,长度为 32 位，重载
        private void cycleShift(ref UInt32 str, int i, bool isLeft = true)
        {
            UInt32 temp = str;              // 32 位整型数
            //temp |= str;                  //临时存储str的值
            i %= 32;
            if (isLeft)                     //向左移位
            {
                str <<= i;                  //输入数据向左移动 i 位
                temp >>= (32 - i);          //temp 向右移动 32 - i 位
                str |= temp;                //将两个移位后的变量合并
            }
            else
            {
                str >>= i;                  //输入数据向左移动 i 位
                temp <<= (32 - i);          //temp 向右移动 32 - i 位
                str |= temp;                //将两个移位后的变量合并
            }
        }

        //循环移位函数,长度为 16 位，重载
        private void cycleShift(ref UInt16 str, int i, bool isLeft = true)
        {
            UInt16 temp = str;              // 16 位整型数
            i %= 16;
            if (isLeft)                     //向左移位
            {
                str <<= i;                  //输入数据向左移动 i 位
                temp >>= (16 - i);          //temp 向右移动 16 - i 位
                str |= temp;                //将两个移位后的变量合并
            }
            else
            {
                str >>= i;                  //输入数据向左移动 i 位
                temp <<= (16 - i);          //temp 向右移动 16 - i 位
                str |= temp;                //将两个移位后的变量合并
            }
        }

        //将 32 bit的数据扩展到48位，取 64bit的前 48 bit,返回一个 UInt64的一个数
        // 测试数据：
        // 0x0000 0001 -> 0x8000 0000 0002 0000       
        // 0x8000 0001 -> 0xC000 0000 0003 0000       
        // 0x0001 0000 -> 0x0000 0280 0000 0000
        private UInt64 exchangeEA32_48(UInt32 A)
        {
            UInt64 result = 0;
            UInt64 temp = 0x8000000000000000;
            UInt64 A_ = 0;
            A_ = ((A_ | A) << 32);
            for (int i = 0; i < 48; i++)
            {
                cycleShift(ref A_,E[i]-1-i,true);
                result |= (temp & A_);
                cycleShift(ref A_, E[i]-1-i, false);
                temp >>= 1;
            }
            return result;
        }

        //计算子密钥
        private void produceSonKey()
        {
            ;
        }



        //把 64 位的密钥变成 56 位并转置，数据存放在左边 56 位
        //测试数据：
        //0x8000 0000 0000 0000 -> 0x0100 0000 0000 0000       
        //0x0101 0101 0101 0101 -> 0x000 0000 0000 0000
        private UInt64 exchangePC1_64_56(UInt64 input)
        {
            UInt64 result = 0;
            UInt64 temp = 0x8000000000000000;
            for (int i = 0; i < 56; i++)
            {
                cycleShift(ref input,PC1[i]-1-i,true);
                result |= (temp & input);
                cycleShift(ref input,PC1[i]-1-i,false);
                temp >>= 1;
            }
            return result;
        }

        //将 56 位的数据通过通过 PC2 转变成 48 bit 的数据
        //测试数据：未测试
        private UInt64 exchangePC2_56_48(UInt64 input)
        {
            UInt64 result = 0;
            UInt64 temp = 0x8000000000000000;
            for (int i = 0; i < 48; i++)
            {
                cycleShift(ref input, PC1[i] - 1 - i, true);
                result |= (temp & input);
                cycleShift(ref input, PC1[i] - 1 - i, false);
                temp >>= 1;
            }
            return result;
        }

    }
}
