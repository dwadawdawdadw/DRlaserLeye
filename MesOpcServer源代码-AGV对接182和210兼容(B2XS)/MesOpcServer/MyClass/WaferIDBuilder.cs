using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IniHelper;

namespace MesOpcServer
{
    class WaferIDBuilder
    {
        public static Configure _cfg = new Configure("D:\\DRLaser\\MES\\MES.ini");

        public static string GenerateWaferID()
        {
            string EqpNum = _cfg.ReadConfig("MES","EqpNum","01").PadLeft(2,'0');
            string ProcessNum = _cfg.ReadConfig("MES", "ProcessNum", "60");

            int Sequence = _cfg.ReadConfig("MES", "Sequence", 0);

            if (Sequence >= 999999)
            {
                _cfg.WriteConfig("MES", "Sequence", 1);
                Sequence = 1;
                return EqpNum + ProcessNum + Sequence.ToString().PadLeft(6, '0');
            }
            else
            
            {
                _cfg.WriteConfig("MES", "Sequence", ++Sequence);
                return EqpNum + ProcessNum + Sequence.ToString().PadLeft(6, '0');
                
            }

            

        }



    }
}
