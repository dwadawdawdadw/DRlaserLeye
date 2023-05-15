using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceManeger
{
    public class EquipmentSimpleFactory
    {
        public static readonly EquipmentSimpleFactory ins = new EquipmentSimpleFactory();
        public Dictionary<string, IEquipments> strDevDic = new Dictionary<string, IEquipments>();

        //public IEquipments CreateEquipment(string equistr)
        //{
        //    IEquipments equipments = null;
        //    //Equipments
        //    EquipmentsSection lstEqup = ConfigurationManager.GetSection("Equipments") as EquipmentsSection;
        //    if (lstEqup == null)
        //    {
        //        return null;
        //    }
        //    //Equipment
        //    EquipmentSection equSection = lstEqup[equistr] as EquipmentSection;
        //    if (equSection == null)
        //    {
        //        return null;
        //    }
        //    //class name
        //    string className = equSection.value;

        //    try
        //    {
        //        //反射创建类
        //        Type t = Type.GetType(className);
        //        object instance = Activator.CreateInstance(t);
        //        equipments = instance as IEquipments;

        //        //save
        //        strDevDic[equistr] = equipments;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.ToString());
        //    }

        //    //switch (equistr)
        //    //         {
        //    //             case "PLC":
        //    //                 {
        //    //                     //equipments = new PLCLinker();
        //    //                 }
        //    //                 break;
        //    //             case "Num":
        //    //                 {
        //    //                    // equipments = new NumMachineLinker();
        //    //                 }
        //    //                 break;
        //    //             case "Laser":
        //    //                 {
        //    //                    // equipments = new LaserLinker();
        //    //                 }
        //    //                 break;
        //    //             default:
        //    //                 {
        //    //                     break;
        //    //                 }
        //    //         }
        //    return equipments;
        //}












    }
}
