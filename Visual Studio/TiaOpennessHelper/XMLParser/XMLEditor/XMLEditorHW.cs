using System;
using System.Collections.Generic;
using System.IO;
using Siemens.Engineering;
using Siemens.Engineering.HW;
using Siemens.Engineering.HW.Features;
using Siemens.Engineering.Library;
using Siemens.Engineering.Library.MasterCopies;

namespace TiaOpennessHelper
{
    public partial class OpennessHelper
    {
        /// <summary>
        /// Handles the Executed event of the GenerateRobotCommand control.
        /// </summary>
        /// <param name="devName"></param>
        /// <param name="IPAddr"></param>
        /// <param name="startAddr"></param>
        /// <param name="tiaPortal"></param>
        public static void InsertHWRobot(string devName, string IPAddr, string startAddr, Project tiaPortal)
        {
            Device deviceName = tiaPortal.Devices.CreateWithItem("GSD:GSDML-V2.33-KUKA-KRC4-PROFINET_4.1-20170630.XML/DAP/DIM 1", devName , devName);

            foreach (DeviceItem device in deviceName.DeviceItems)
            {
                if (device.GetAttribute("PositionNumber").ToString() == "1")
                {
                    DeviceItemAssociation itemAss = device.Items;
                    foreach (DeviceItem deviceItem in itemAss)
                    {
                        AddressComposition AddrAss = deviceItem.Addresses;
                        foreach (Address address in AddrAss)
                        {
                            address.SetAttribute("StartAddress", int.Parse(startAddr));
                        }
                    }
                }
                if (device.GetAttribute("PositionNumber").ToString() == "2")
                {
                    HardwareObject bj = device.Container;
                    device.Delete();
                    DeviceItem deviceSubMod = bj.PlugNew("GSD:GSDML-V2.33-KUKA-KRC4-PROFINET_4.1-20170630.XML/M/14", "1024 digitale Ein- und Ausgänge_1", 2);
                    DeviceItemAssociation itemAss = deviceSubMod.Items;
                    foreach (DeviceItem deviceItem in itemAss)
                    {
                        AddressComposition AddrAss = deviceItem.Addresses;
                        foreach (Address address in AddrAss)
                        {
                            address.SetAttribute("StartAddress", int.Parse(startAddr) + 12);
                        }
                    }
                }
            }
            NetworkInterface itf = ((IEngineeringServiceProvider)deviceName.Items[0].Items[0].Items[0]).GetService<NetworkInterface>();
            Subnet net = tiaPortal.Subnets.Find("PN/IE_1");
            Node node = itf.Nodes[0];
            IoConnector ioConn = itf.IoConnectors[0];
            node.SetAttribute("Address", IPAddr);
            node.ConnectToSubnet(net);
            ioConn.ConnectToIoSystem(net.IoSystems[0]);
            ioConn.SetAttribute("PnDeviceNumber", int.Parse(IPAddr.Split('.')[3]));
        }
       
        /// <summary>
        /// Handles the Executed event of the GenerateRobotCommand control.
        /// </summary>
        /// <param name="devName"></param>
        /// <param name="IPAddr"></param>
        /// <param name="tiaPortal"></param>
        public static void InsertHWRobotScalance(string devName, string IPAddr, Project tiaPortal)
        {
            if (tiaPortal.Devices.Find(devName) != null) return;

            Device deviceName = tiaPortal.Devices.CreateWithItem("OrderNumber:6GK5 208-0BA00-2AC2/V3.0", devName, devName);
            NetworkInterface itf = ((IEngineeringServiceProvider)deviceName.Items[0].Items[0].Items[1]).GetService<NetworkInterface>();
            Subnet net = tiaPortal.Subnets.Find("PN/IE_1");
            Node node = itf.Nodes[0];
            IoConnector ioConn = itf.IoConnectors[0];
            node.SetAttribute("Address", IPAddr);
            node.ConnectToSubnet(net);
            ioConn.ConnectToIoSystem(net.IoSystems[0]);
            ioConn.SetAttribute("PnDeviceNumber", int.Parse(IPAddr.Split('.')[3]));
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="devName"></param>
        /// <param name="IPAddr"></param>
        /// <param name="startAddr"></param>
        /// <param name="pneuList"></param>
        /// <param name="tiaPortal"></param>
        public static void InsertHWPneumatic(string devName, string IPAddr, string startAddr, List<int> pneuList, Project tiaPortal)
        {
            Device device = tiaPortal.Devices.Find(devName);
            int slotNr = 2;

            if (device == null)
            {
                Device deviceName = tiaPortal.Devices.CreateWithItem("GSD:GSDML-V2.31-FESTO-CPX-20161019.XML/DAP/DAP CU R30", devName, devName);
                HardwareObject obj = deviceName.DeviceItems[1].Container;

                if (obj.CanPlugNew("GSD:GSDML-V2.31-FESTO-CPX-20161019.XML/M/CPX_216_000", "FB34 PNIO Modul_1", 1))
                    obj.PlugNew("GSD:GSDML-V2.31-FESTO-CPX-20161019.XML/M/CPX_216_000", "FB34 PNIO Modul_1", 1);

                for (int i = 1; i <= pneuList[0]; i++) //16DI
                {
                    if (obj.CanPlugNew("GSD:GSDML-V2.31-FESTO-CPX-20161019.XML/M/CPX_016_000", "16DI-D [16DE]_" + i.ToString(), slotNr))
                    {
                        DeviceItem deviceSubMod = obj.PlugNew("GSD:GSDML-V2.31-FESTO-CPX-20161019.XML/M/CPX_016_000", "16DI-D [16DE]_" + i.ToString(), slotNr);
                        slotNr++;
                        DeviceItemAssociation itemAss = deviceSubMod.Items;
                        foreach (DeviceItem deviceItem in itemAss)
                        {
                            AddressComposition AddrAss = deviceItem.Addresses;
                            foreach (Address address in AddrAss)
                            {
                                address.SetAttribute("StartAddress", int.Parse(startAddr));
                                startAddr = (int.Parse(startAddr) + 2).ToString();
                            }
                        }
                    }
                }
                for (int i = 1; i <= pneuList[1]; i++) //DO-H
                {
                    if (obj.CanPlugNew("GSD:GSDML-V2.31-FESTO-CPX-20161019.XML/M/CPX_017_000", "8DO-H [8DO]_" + i.ToString(), slotNr))
                    {
                        DeviceItem deviceSubMod = obj.PlugNew("GSD:GSDML-V2.31-FESTO-CPX-20161019.XML/M/CPX_017_000", "8DO-H [8DO]_" + i.ToString(), slotNr);
                        slotNr++;
                        DeviceItemAssociation itemAss = deviceSubMod.Items;
                        foreach (DeviceItem deviceItem in itemAss)
                        {
                            AddressComposition AddrAss = deviceItem.Addresses;
                            foreach (Address address in AddrAss)
                            {
                                address.SetAttribute("StartAddress", int.Parse(startAddr));
                                startAddr = (int.Parse(startAddr) + 1).ToString();
                            }
                        }
                    }
                }
                for (int i = 1; i <= pneuList[2]; i++) //FDI
                {
                    if (obj.CanPlugNew("GSD:GSDML-V2.31-FESTO-CPX-20161019.XML/M/CPX_028_001", "F8DI-P Word [8DE-F]_" + i.ToString(), slotNr))
                    {
                        DeviceItem deviceSubMod = obj.PlugNew("GSD:GSDML-V2.31-FESTO-CPX-20161019.XML/M/CPX_028_001", "F8DI-P Word [8DE-F]_" + i.ToString(), slotNr);
                        slotNr++;
                        DeviceItemAssociation itemAss = deviceSubMod.Items;
                        foreach (DeviceItem deviceItem in itemAss)
                        {
                            AddressComposition AddrAss = deviceItem.Addresses;
                            foreach (Address address in AddrAss)
                            {
                                address.SetAttribute("StartAddress", int.Parse(startAddr));
                            }
                            startAddr = (int.Parse(startAddr) + 7).ToString();
                        }
                    }
                }
                for (int i = 1; i <= pneuList[3]; i++) //FDO
                {
                    if (obj.CanPlugNew("GSD:GSDML-V2.31-FESTO-CPX-20161019.XML/M/CPX_193_008", "FVDO-P2 [3DA-F]_" + i.ToString(), slotNr))
                    {
                        DeviceItem deviceSubMod = obj.PlugNew("GSD:GSDML-V2.31-FESTO-CPX-20161019.XML/M/CPX_193_008", "FVDO-P2 [3DA-F]_" + i.ToString(), slotNr);
                        slotNr++;
                        DeviceItemAssociation itemAss = deviceSubMod.Items;
                        foreach (DeviceItem deviceItem in itemAss)
                        {
                            AddressComposition AddrAss = deviceItem.Addresses;
                            foreach (Address address in AddrAss)
                            {
                                address.SetAttribute("StartAddress", int.Parse(startAddr));
                            }
                            startAddr = (int.Parse(startAddr) + 6).ToString();
                        }
                    }
                }

                if (obj.CanPlugNew("GSD:GSDML-V2.31-FESTO-CPX-20161019.XML/M/CPX_069_000_32V", "VTSA DIL 4 [32DA]_1", slotNr))
                {
                    DeviceItem deviceSubMod = obj.PlugNew("GSD:GSDML-V2.31-FESTO-CPX-20161019.XML/M/CPX_069_000_32V", "VTSA DIL 4 [32DA]_1", slotNr);
                    slotNr++;
                    DeviceItemAssociation itemAss = deviceSubMod.Items;
                    foreach (DeviceItem deviceItem in itemAss)
                    {
                        AddressComposition AddrAss = deviceItem.Addresses;
                        foreach (Address address in AddrAss)
                        {
                            address.SetAttribute("StartAddress", int.Parse(startAddr));
                        }
                    }
                }
                NetworkInterface itf = ((IEngineeringServiceProvider)deviceName.Items[0].Items[0].Items[0]).GetService<NetworkInterface>();
                Subnet net = tiaPortal.Subnets.Find("PN/IE_1");
                Node node = itf.Nodes[0];
                IoConnector ioConn = itf.IoConnectors[0];
                node.SetAttribute("Address", IPAddr);
                node.ConnectToSubnet(net);
                ioConn.ConnectToIoSystem(net.IoSystems[0]);
                ioConn.SetAttribute("PnDeviceNumber", int.Parse(IPAddr.Split('.')[3]));
            }
        }
      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="devName"></param>
        /// <param name="IPAddr"></param>
        /// <param name="startAddr"></param>
        /// <param name="leftDoor"></param>
        /// <param name="tiaPortal"></param>
        /// <param name="library"></param>
        public static void InsertHWEuchner(string devName, string IPAddr, string startAddr, bool leftDoor, Project tiaPortal, MasterCopySystemFolder library)
        {
            DeviceItemAssociation itemAss;
            Device deviceTIA = tiaPortal.Devices.Find(devName);
            if (deviceTIA == null)
            {
                Device deviceName;
                if (leftDoor)
                {
                    MasterCopy HWmasterCopy = SearchLibraryFolder(library, "EUCHENER-L");
                    if (HWmasterCopy == null) return;

                    deviceName = tiaPortal.Devices.CreateFrom(HWmasterCopy);
                    OpennessHelper.RenameDevice(deviceName, devName);
                    //deviceName = tiaPortal.Devices.CreateWithItem("GSD:GSDML-V2.3-EUCHNER-MGB_PN_D_110025-20150410.XML/DAP/DAP 100", devName, devName);
                }

                else
                {
                    MasterCopy HWmasterCopy = SearchLibraryFolder(library, "EUCHENER-R");
                    if (HWmasterCopy == null) return;
                    deviceName = tiaPortal.Devices.CreateFrom(HWmasterCopy);
                    OpennessHelper.RenameDevice(deviceName, devName);
                    //deviceName = tiaPortal.Devices.CreateWithItem("GSD:GSDML-V2.3-EUCHNER-MGB_PN_D_110025-20150410.XML/DAP/DAP 99", devName, devName);
                }


                foreach (DeviceItem device in deviceName.DeviceItems)
                {
                    switch (device.GetAttribute("PositionNumber").ToString())
                    {
                        case "1":
                            itemAss = device.Items;
                            foreach (DeviceItem deviceItem in itemAss)
                            {
                                AddressComposition AddrAss = deviceItem.Addresses;
                                foreach (Address address in AddrAss)
                                {
                                    address.SetAttribute("StartAddress", int.Parse(startAddr));
                                }
                            }
                            break;
                        case "2":
                            itemAss = device.Items;
                            foreach (DeviceItem deviceItem in itemAss)
                            {
                                AddressComposition AddrAss = deviceItem.Addresses;
                                foreach (Address address in AddrAss)
                                {
                                    address.SetAttribute("StartAddress", int.Parse(startAddr) + 1);
                                }
                            }
                            break;
                        case "3":
                            itemAss = device.Items;
                            foreach (DeviceItem deviceItem in itemAss)
                            {
                                AddressComposition AddrAss = deviceItem.Addresses;
                                foreach (Address address in AddrAss)
                                {
                                    address.SetAttribute("StartAddress", int.Parse(startAddr) + 2);
                                }
                            }
                            break;
                        case "4":
                            itemAss = device.Items;
                            foreach (DeviceItem deviceItem in itemAss)
                            {
                                AddressComposition AddrAss = deviceItem.Addresses;
                                foreach (Address address in AddrAss)
                                {
                                    address.SetAttribute("StartAddress", int.Parse(startAddr) + 3);
                                }
                            }
                            break;
                        case "5":
                            itemAss = device.Items;
                            foreach (DeviceItem deviceItem in itemAss)
                            {
                                AddressComposition AddrAss = deviceItem.Addresses;
                                foreach (Address address in AddrAss)
                                {
                                    address.SetAttribute("StartAddress", int.Parse(startAddr) + 4);
                                }
                            }
                            break;
                        case "6":
                            itemAss = device.Items;
                            foreach (DeviceItem deviceItem in itemAss)
                            {
                                AddressComposition AddrAss = deviceItem.Addresses;
                                foreach (Address address in AddrAss)
                                {
                                    address.SetAttribute("StartAddress", int.Parse(startAddr) + 6);
                                }
                            }
                            break;
                        default: break;
                    }
                }
                NetworkInterface itf = ((IEngineeringServiceProvider)deviceName.Items[0].Items[0].Items[0]).GetService<NetworkInterface>();
                Subnet net = tiaPortal.Subnets.Find("PN/IE_1");
                Node node = itf.Nodes[0];
                IoConnector ioConn = itf.IoConnectors[0];
                node.SetAttribute("Address", IPAddr);
                node.ConnectToSubnet(net);
                ioConn.ConnectToIoSystem(net.IoSystems[0]);
                ioConn.SetAttribute("PnDeviceNumber", int.Parse(IPAddr.Split('.')[3]));
            }
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="devName"></param>
        /// <param name="IPAddr"></param>
        /// <param name="startAddr"></param>
        /// <param name="tiaPortal"></param>
        /// <param name="SickS3000"></param>
        /// <param name="library"></param>
        public static void InsertHwPLS(string devName, string IPAddr, string startAddr, Project tiaPortal, bool SickS3000, MasterCopySystemFolder library)
        {
            Device deviceTIA = tiaPortal.Devices.Find(devName);
            MasterCopy HWmasterCopy;
            if (deviceTIA == null)
            {
                //Device deviceName = tiaPortal.Devices.CreateWithItem("GSD:GSDML-V2.25-SICK-S3000-20130101.XML/DAP/DIM 1", devName, devName);
                if (SickS3000)
                    HWmasterCopy = SearchLibraryFolder(library, "SICK-S3000");
                else
                    HWmasterCopy = SearchLibraryFolder(library, "KEYENCE SZ-V");

                if (HWmasterCopy == null) return;

                Device deviceName = tiaPortal.Devices.CreateFrom(HWmasterCopy);
                OpennessHelper.RenameDevice(deviceName, devName);
                

                foreach (DeviceItem device in deviceName.DeviceItems)
                {
                    if (device.GetAttribute("PositionNumber").ToString() == "1")
                    {
                        DeviceItemAssociation itemAss = device.Items;
                        foreach (DeviceItem deviceItem in itemAss)
                        {
                            AddressComposition AddrAss = deviceItem.Addresses;
                            foreach (Address address in AddrAss)
                            {
                                //address.SetAttribute("StartAddress", int.Parse(startAddr));
                                address.StartAddress = int.Parse(startAddr);
                            }
                        }
                    }
                }
                NetworkInterface itf = ((IEngineeringServiceProvider)deviceName.Items[0].Items[0].Items[0]).GetService<NetworkInterface>();
                Subnet net = tiaPortal.Subnets.Find("PN/IE_1");
                Node node = itf.Nodes[0];
                IoConnector ioConn = itf.IoConnectors[0];
                node.SetAttribute("Address", IPAddr);
                node.ConnectToSubnet(net);
                ioConn.ConnectToIoSystem(net.IoSystems[0]);
                ioConn.SetAttribute("PnDeviceNumber", int.Parse(IPAddr.Split('.')[3]));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="devName"></param>
        /// <param name="IPAddr"></param>
        /// <param name="startAddr"></param>
        /// <param name="tiaPortal"></param>
        /// <param name="library"></param>
        public static void InsertHWKP32F(string devName, string IPAddr, string startAddr, Project tiaPortal, MasterCopySystemFolder library)
        {
            DeviceItemAssociation itemAss;
            Device deviceTIA = tiaPortal.Devices.Find(devName);
            if (deviceTIA == null)
            {
                Device deviceName = tiaPortal.Devices.CreateWithItem("OrderNumber:6AV3 688-3EH47-0AX0/V01.00.00", devName, devName);
                //MasterCopy HWmasterCopy = library.Folders[0].Folders[1].MasterCopies.Find("KP32F");
                //Device deviceName = tiaPortal.Devices.CreateFrom(HWmasterCopy);
                //OpennessHelper.RenameDevice(deviceName, devName);
                foreach (DeviceItem device in deviceName.DeviceItems)
                {
                    switch (device.GetAttribute("PositionNumber").ToString())
                    {
                        case "1":
                            itemAss = device.Items;
                            foreach (DeviceItem deviceItem in itemAss)
                            {
                                AddressComposition AddrAss = deviceItem.Addresses;
                                foreach (Address address in AddrAss)
                                {
                                    address.SetAttribute("StartAddress", int.Parse(startAddr));
                                }
                            }
                            break;
                        case "2":
                            itemAss = device.Items;
                            foreach (DeviceItem deviceItem in itemAss)
                            {
                                AddressComposition AddrAss = deviceItem.Addresses;
                                foreach (Address address in AddrAss)
                                {
                                    address.SetAttribute("StartAddress", int.Parse(startAddr) + 6);
                                }
                            }
                            break;
                        case "3":
                            itemAss = device.Items;
                            foreach (DeviceItem deviceItem in itemAss)
                            {
                                AddressComposition AddrAss = deviceItem.Addresses;
                                foreach (Address address in AddrAss)
                                {
                                    address.SetAttribute("StartAddress", int.Parse(startAddr) + 12);
                                }
                            }
                            break;
                        case "4":
                            itemAss = device.Items;
                            foreach (DeviceItem deviceItem in itemAss)
                            {
                                AddressComposition AddrAss = deviceItem.Addresses;
                                foreach (Address address in AddrAss)
                                {
                                    address.SetAttribute("StartAddress", int.Parse(startAddr) + 14);
                                }
                            }
                            break;
                        case "5":
                            itemAss = device.Items;
                            foreach (DeviceItem deviceItem in itemAss)
                            {
                                AddressComposition AddrAss = deviceItem.Addresses;
                                foreach (Address address in AddrAss)
                                {
                                    address.SetAttribute("StartAddress", int.Parse(startAddr) + 16);
                                }
                            }
                            break;
                    }
                }
                //NetworkInterface itf = ((IEngineeringServiceProvider)deviceName.Items[0].Items[0].Items[0]).GetService<NetworkInterface>(); //FROM MasterCopies
                NetworkInterface itf = ((IEngineeringServiceProvider)deviceName.Items[0].Items[4].Items[0]).GetService<NetworkInterface>();
                Subnet net = tiaPortal.Subnets.Find("PN/IE_1");
                Node node = itf.Nodes[0];
                IoConnector ioConn = itf.IoConnectors[0];
                node.SetAttribute("Address", IPAddr);
                node.ConnectToSubnet(net);
                ioConn.ConnectToIoSystem(net.IoSystems[0]);
                ioConn.SetAttribute("PnDeviceNumber", int.Parse(IPAddr.Split('.')[3]));
            }
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="devName"></param>
        /// <param name="IPAddr"></param>
        /// <param name="tiaPortal"></param>
        /// <param name="library"></param>
        public static void InsertHWScalanceKP32F(string devName, string IPAddr, Project tiaPortal, MasterCopySystemFolder library)
        {
            Device deviceTIA = tiaPortal.Devices.Find(devName);
            if (deviceTIA == null)
            {
                Device deviceName = tiaPortal.Devices.CreateWithItem("OrderNumber:6GK5 204-2BC00-2AF2/V5.2", devName, devName);
                //MasterCopy HWmasterCopy = library.MasterCopies.Find("HW");
                //Device deviceName = tiaPortal.Devices.CreateFrom(HWmasterCopy);
                NetworkInterface itf = ((IEngineeringServiceProvider)deviceName.Items[0].Items[0].Items[1]).GetService<NetworkInterface>();
                Subnet net = tiaPortal.Subnets.Find("PN/IE_1");
                Node node = itf.Nodes[0];
                IoConnector ioConn = itf.IoConnectors[0];
                node.SetAttribute("Address", IPAddr);
                node.ConnectToSubnet(net);
                ioConn.ConnectToIoSystem(net.IoSystems[0]);
                ioConn.SetAttribute("PnDeviceNumber", int.Parse(IPAddr.Split('.')[3]));
            }
        }

        /// <summary>
        /// Get libraries names from TIA Portal 
        /// </summary>
        /// <param name="tiaPortal"></param>
        /// <returns>List containing libraries names</returns>
        public static List<string> GetLibrariesNamesFromTIA(TiaPortal tiaPortal)
        {
            List<string> list = new List<string>();

            IList<GlobalLibraryInfo> libraryInfos = tiaPortal.GlobalLibraries.GetGlobalLibraryInfos();
            foreach (GlobalLibraryInfo libraryInfo in libraryInfos)
            {
                list.Add(libraryInfo.Name);
            }

            return list;
        }

        /// <summary>
        /// Get hardware master copies from tiaPortal with a specific name
        /// </summary>
        /// <param name="tiaPortal"></param>
        /// <param name="lib"></param>
        /// <param name="fromPath"></param>
        /// <returns></returns>
        public static MasterCopySystemFolder GetHWMasterCopies(TiaPortal tiaPortal, string lib, bool fromPath)
        {
            if (!fromPath)
            {
                IList<GlobalLibraryInfo> libraryInfos = tiaPortal.GlobalLibraries.GetGlobalLibraryInfos();
                foreach (GlobalLibraryInfo libraryInfo in libraryInfos)
                {
                    if (libraryInfo.Name.Equals(lib))
                    {
                        GlobalLibrary library = tiaPortal.GlobalLibraries.Open(libraryInfo);
                        return library.MasterCopyFolder;
                    }
                }
            }
            else
            {
                FileInfo fi = new FileInfo(lib);
                UserGlobalLibrary userLib = tiaPortal.GlobalLibraries.Open(fi, OpenMode.ReadWrite);
                return userLib.MasterCopyFolder;
            }

            return null;
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="devName"></param>
        /// <param name="IPAddr"></param>
        /// <param name="startAddr"></param>
        /// <param name="tiaPortal"></param>
        /// <param name="library"></param>
        public static void InsertHWOperatorDoor(string devName, string IPAddr, string startAddr, Project tiaPortal, MasterCopySystemFolder library)
        {
            DeviceItemAssociation itemAss;
            Device deviceTIA = tiaPortal.Devices.Find(devName);

            if (deviceTIA == null)
            {
                MasterCopy HWfolder = SearchLibraryFolder(library, "ALBANY");
                if (HWfolder == null) return;

                Device deviceName = tiaPortal.Devices.CreateFrom(HWfolder);

                foreach (DeviceItem device in deviceName.DeviceItems[1].DeviceItems)
                {
                        switch (device.GetAttribute("PositionNumber").ToString())
                        {
                            case "1":
                                itemAss = device.Items;
                                foreach (DeviceItem deviceItem in itemAss)
                                {
                                    AddressComposition AddrAss = deviceItem.Addresses;
                                    foreach (Address address in AddrAss)
                                    {
                                        //SAFETY.. ATTRIBUTE PROPERTIES ARE READ ONLY ...
                                        address.SetAttribute("StartAddress", Int32.Parse(startAddr));
                                    }
                                }
                                break;
                            case "2":
                                itemAss = device.Items;
                                foreach (DeviceItem deviceItem in itemAss)
                                {
                                    AddressComposition AddrAss = deviceItem.Addresses;
                                    foreach (Address address in AddrAss)
                                    {
                                        address.SetAttribute("StartAddress", int.Parse(startAddr) + 7);
                                    }
                                }
                                break;
                            case "3":
                                itemAss = device.Items;
                                foreach (DeviceItem deviceItem in itemAss)
                                {
                                    AddressComposition AddrAss = deviceItem.Addresses;
                                    foreach (Address address in AddrAss)
                                    {
                                        address.SetAttribute("StartAddress", int.Parse(startAddr) + 9);
                                    }
                                }
                                break;
                            case "4":
                                itemAss = device.Items;
                                foreach (DeviceItem deviceItem in itemAss)
                                {
                                    AddressComposition AddrAss = deviceItem.Addresses;
                                    foreach (Address address in AddrAss)
                                    {
                                        address.SetAttribute("StartAddress", int.Parse(startAddr) + 7);
                                    }
                                }
                                break;
                            case "5":
                                itemAss = device.Items;
                                foreach (DeviceItem deviceItem in itemAss)
                                {
                                    AddressComposition AddrAss = deviceItem.Addresses;
                                    foreach (Address address in AddrAss)
                                    {
                                        address.SetAttribute("StartAddress", int.Parse(startAddr) + 9);
                                    }
                                }
                                break;
                            default: break;
                        }
                }

                NetworkInterface itf = ((IEngineeringServiceProvider)deviceName.Items[0].Items[0].Items[0]).GetService<NetworkInterface>();
                Subnet net = tiaPortal.Subnets.Find("PN/IE_1");
                Node node = itf.Nodes[0];
                IoConnector ioConn = itf.IoConnectors[0];
                node.SetAttribute("Address", IPAddr);
                node.ConnectToSubnet(net);
                ioConn.ConnectToIoSystem(net.IoSystems[0]);
                ioConn.SetAttribute("PnDeviceNumber", int.Parse(IPAddr.Split('.')[3]));
            }
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="device"></param>
        /// <param name="devName"></param>
        public static void RenameDevice(Device device, string devName)
        {
            device.SetAttribute("Name", devName);
            device.DeviceItems[1].Name = devName;
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="devName"></param>
        /// <param name="IPAddr"></param>
        /// <param name="startAddr"></param>
        /// <param name="tiaPortal"></param>
        public static void InsertHWMURRFDI8FDO4MVK(string devName, string IPAddr, string startAddr, Project tiaPortal)
        {
            Device deviceTIA = tiaPortal.Devices.Find(devName);
            if (deviceTIA == null)
            {
                Device deviceName = tiaPortal.Devices.CreateWithItem("GSD:GSDML-V2.31-MURRELEKTRONIK-MVK_MPNIO_F-20150903.XML/DAP/55557", devName, devName);
                HardwareObject obj = deviceName.DeviceItems[1].Container;

                if (obj.CanPlugNew("GSD:GSDML-V2.31-MURRELEKTRONIK-MVK_MPNIO_F-20150903.XML/M/4", "FS Data_1", 2))
                {
                    DeviceItem deviceSubMod = obj.PlugNew("GSD:GSDML-V2.31-MURRELEKTRONIK-MVK_MPNIO_F-20150903.XML/M/4", "FS Data 1", 2);
                    DeviceItemAssociation itemAss = deviceSubMod.Items;
                    foreach (DeviceItem deviceItem in itemAss)
                    {
                        AddressComposition AddrAss = deviceItem.Addresses;
                        foreach (Address address in AddrAss)
                        {
                            address.SetAttribute("StartAddress", int.Parse(startAddr));
                        }
                    }
                }

                NetworkInterface itf = ((IEngineeringServiceProvider)deviceName.Items[0].Items[1].Items[0]).GetService<NetworkInterface>();
                Subnet net = tiaPortal.Subnets.Find("PN/IE_1");
                Node node = itf.Nodes[0];
                IoConnector ioConn = itf.IoConnectors[0];
                node.SetAttribute("Address", IPAddr);
                node.ConnectToSubnet(net);
                ioConn.ConnectToIoSystem(net.IoSystems[0]);
                ioConn.SetAttribute("PnDeviceNumber", int.Parse(IPAddr.Split('.')[3]));
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="devName"></param>
        /// <param name="murrDi6List"></param>
        /// <param name="IPAddr"></param>
        /// <param name="startAddr"></param>
        /// <param name="tiaPortal"></param>
        public static void InsertHWMURRDI6DO6MVK(string devName, List<int> murrDi6List, string IPAddr, string startAddr, Project tiaPortal)
        {
            Device deviceTIA = tiaPortal.Devices.Find(devName);
            if (deviceTIA == null)
            {
                Device deviceName = tiaPortal.Devices.CreateWithItem("GSD:GSDML-V2.32-MURRELEKTRONIK-MVK_MPNIO_0125-20180305-000000.XML/DAP/55516", devName, devName);
                HardwareObject obj = deviceName.Items[0].Items[1];

                if(murrDi6List[0] == 0)
                {
                    if (obj.CanPlugNew("GSD:GSDML-V2.32-MURRELEKTRONIK-MVK_MPNIO_0125-20180305-000000.XML/SM/10202", "IOL_I/O_ 2/ 2 Byte", 0))
                    {
                        DeviceItem deviceSubMod = deviceName.Items[0].Items[1].PlugNew("GSD:GSDML-V2.32-MURRELEKTRONIK-MVK_MPNIO_0125-20180305-000000.XML/SM/10202", "IOL_I/O_ 2/ 2 Byte", 0);
                        DeviceItemAssociation itemAss = deviceSubMod.Items;
                        foreach (DeviceItem deviceItem in itemAss)
                        {
                            AddressComposition AddrAss = deviceItem.Addresses;
                            foreach (Address address in AddrAss)
                            {
                                address.SetAttribute("StartAddress", int.Parse(startAddr));
                            }
                        }
                    }
                    if (obj.CanPlugNew("GSD:GSDML-V2.32-MURRELEKTRONIK-MVK_MPNIO_0125-20180305-000000.XML/SM/10000", "Deactivated", 1))
                    {
                        DeviceItem deviceSubMod = deviceName.Items[0].Items[1].PlugNew("GSD:GSDML-V2.32-MURRELEKTRONIK-MVK_MPNIO_0125-20180305-000000.XML/SM/10000", "Deactivated", 1);
                        DeviceItemAssociation itemAss = deviceSubMod.Items;
                        foreach (DeviceItem deviceItem in itemAss)
                        {
                            AddressComposition AddrAss = deviceItem.Addresses;
                            foreach (Address address in AddrAss)
                            {
                                address.SetAttribute("StartAddress", int.Parse(startAddr));
                            }
                        }
                    }
                }
                else
                {
                    if (obj.CanPlugNew("GSD:GSDML-V2.32-MURRELEKTRONIK-MVK_MPNIO_0125-20180305-000000.XML/SM/10002", "IOL_O_ 2 Byte", 0))
                    {
                        DeviceItem deviceSubMod = deviceName.Items[0].Items[1].PlugNew("GSD:GSDML-V2.32-MURRELEKTRONIK-MVK_MPNIO_0125-20180305-000000.XML/SM/10002", "IOL_O_ 2 Byte", 0);
                        DeviceItemAssociation itemAss = deviceSubMod.Items;
                        foreach (DeviceItem deviceItem in itemAss)
                        {
                            AddressComposition AddrAss = deviceItem.Addresses;
                            foreach (Address address in AddrAss)
                            {
                                address.SetAttribute("StartAddress", int.Parse(startAddr));
                            }
                        }
                    }
                    if (obj.CanPlugNew("GSD:GSDML-V2.32-MURRELEKTRONIK-MVK_MPNIO_0125-20180305-000000.XML/SM/10202", "IOL_I/O_ 2/ 2 Byte", 1))
                    {
                        DeviceItem deviceSubMod = deviceName.Items[0].Items[1].PlugNew("GSD:GSDML-V2.32-MURRELEKTRONIK-MVK_MPNIO_0125-20180305-000000.XML/SM/10202", "IOL_I/O_ 2/ 2 Byte", 1);
                        DeviceItemAssociation itemAss = deviceSubMod.Items;
                        foreach (DeviceItem deviceItem in itemAss)
                        {
                            AddressComposition AddrAss = deviceItem.Addresses;
                            foreach (Address address in AddrAss)
                            {
                                address.SetAttribute("StartAddress", int.Parse(startAddr));
                            }
                        }
                    }
                }

                NetworkInterface itf = ((IEngineeringServiceProvider)deviceName.Items[0].Items[2].Items[0]).GetService<NetworkInterface>();
                Subnet net = tiaPortal.Subnets.Find("PN/IE_1");
                Node node = itf.Nodes[0];
                IoConnector ioConn = itf.IoConnectors[0];
                node.SetAttribute("Address", IPAddr);
                node.ConnectToSubnet(net);
                ioConn.ConnectToIoSystem(net.IoSystems[0]);
                ioConn.SetAttribute("PnDeviceNumber", int.Parse(IPAddr.Split('.')[3]));
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="devName"></param>
        /// <param name="IPAddr"></param>
        /// <param name="startAddr"></param>
        /// <param name="T200List"></param>
        /// <param name="tiaPortal"></param>
        public static void InsertHWT200(string devName, string IPAddr, string startAddr, List<int> T200List, Project tiaPortal)
        {
            Device device = tiaPortal.Devices.Find(devName);
            int slotNr = 1;

            if (device == null)
            {
                Device deviceName = tiaPortal.Devices.CreateWithItem("OrderNumber:6ES7 155-6AU00-0CN0/V3.3", devName, devName);
                HardwareObject obj = deviceName.DeviceItems[1].Container;

                for (int i = 1; i <= T200List[0]; i++) //8DI
                {
                    if (obj.CanPlugNew("OrderNumber:6ES7 131-6BF01-0BA0/V0.0", "DI 8x24VDC ST_" + i.ToString(), slotNr))
                    {
                        DeviceItem deviceSubMod = obj.PlugNew("OrderNumber:6ES7 131-6BF01-0BA0/V0.0", "DI 8x24VDC ST_" + i.ToString(), slotNr);
                        slotNr++;
                        DeviceItemAssociation itemAss = deviceSubMod.Items;
                        foreach (DeviceItem deviceItem in itemAss)
                        {
                            AddressComposition AddrAss = deviceItem.Addresses;
                            foreach (Address address in AddrAss)
                            {
                                address.SetAttribute("StartAddress", int.Parse(startAddr));
                                startAddr = (int.Parse(startAddr) + 2).ToString();
                            }
                        }
                    }
                }
                for (int i = 1; i <= T200List[1]; i++) //16DI
                {
                    if (obj.CanPlugNew("OrderNumber:6ES7 131-6BH01-0BA0/V0.0", "DI 16x24VDC ST_" + i.ToString(), slotNr))
                    {
                        DeviceItem deviceSubMod = obj.PlugNew("OrderNumber:6ES7 131-6BH01-0BA0/V0.0", "DI 16x24VDC ST_" + i.ToString(), slotNr);
                        slotNr++;
                        DeviceItemAssociation itemAss = deviceSubMod.Items;
                        foreach (DeviceItem deviceItem in itemAss)
                        {
                            AddressComposition AddrAss = deviceItem.Addresses;
                            foreach (Address address in AddrAss)
                            {
                                address.SetAttribute("StartAddress", int.Parse(startAddr));
                                startAddr = (int.Parse(startAddr) + 1).ToString();
                            }
                        }
                    }
                }
                for (int i = 1; i <= T200List[2]; i++) //8DQ
                {
                    if (obj.CanPlugNew("OrderNumber:6ES7 132-6BF01-0BA0/V0.0", "DQ 8x24VDC/0.5A ST_" + i.ToString(), slotNr))
                    {
                        DeviceItem deviceSubMod = obj.PlugNew("OrderNumber:6ES7 132-6BF01-0BA0/V0.0", "DQ 8x24VDC/0.5A ST_" + i.ToString(), slotNr);
                        slotNr++;
                        DeviceItemAssociation itemAss = deviceSubMod.Items;
                        foreach (DeviceItem deviceItem in itemAss)
                        {
                            AddressComposition AddrAss = deviceItem.Addresses;
                            foreach (Address address in AddrAss)
                            {
                                address.SetAttribute("StartAddress", int.Parse(startAddr));
                            }
                            startAddr = (int.Parse(startAddr) + 7).ToString();
                        }
                    }
                }
                for (int i = 1; i <= T200List[3]; i++) //16DQ
                {
                    if (obj.CanPlugNew("OrderNumber:6ES7 132-6BH01-0BA0/V0.0", "DQ 16x24VDC/0.5A ST_" + i.ToString(), slotNr))
                    {
                        DeviceItem deviceSubMod = obj.PlugNew("OrderNumber:6ES7 132-6BH01-0BA0/V0.0", "DQ 16x24VDC/0.5A ST_" + i.ToString(), slotNr);
                        slotNr++;
                        DeviceItemAssociation itemAss = deviceSubMod.Items;
                        foreach (DeviceItem deviceItem in itemAss)
                        {
                            AddressComposition AddrAss = deviceItem.Addresses;
                            foreach (Address address in AddrAss)
                            {
                                address.SetAttribute("StartAddress", int.Parse(startAddr));
                            }
                            startAddr = (int.Parse(startAddr) + 6).ToString();
                        }
                    }
                }

                // Safe DI
                if (obj.CanPlugNew("OrderNumber:6ES7 136-6BA00-0CA0/V1.0", "F-DI 8x24VDC HF_1", slotNr))
                {
                    DeviceItem deviceSubMod = obj.PlugNew("OrderNumber:6ES7 136-6BA00-0CA0/V1.0", "F-DI 8x24VDC HF_1", slotNr);
                    slotNr++;
                    DeviceItemAssociation itemAss = deviceSubMod.Items;
                    foreach (DeviceItem deviceItem in itemAss)
                    {
                        AddressComposition AddrAss = deviceItem.Addresses;
                        foreach (Address address in AddrAss)
                        {
                            address.SetAttribute("StartAddress", int.Parse(startAddr));
                        }
                    }
                }

                // Safe DQ
                if (obj.CanPlugNew("OrderNumber:6ES7 136-6DC00-0CA0/V1.0", "F-DQ 8x24VDC/0.5A PP HF_1", slotNr))
                {
                    DeviceItem deviceSubMod = obj.PlugNew("OrderNumber:6ES7 136-6DC00-0CA0/V1.0", "F-DQ 8x24VDC/0.5A PP HF_1", slotNr);
                    slotNr++;
                    DeviceItemAssociation itemAss = deviceSubMod.Items;
                    foreach (DeviceItem deviceItem in itemAss)
                    {
                        AddressComposition AddrAss = deviceItem.Addresses;
                        foreach (Address address in AddrAss)
                        {
                            address.SetAttribute("StartAddress", int.Parse(startAddr));
                        }
                    }
                }

                // Server Module
                if (obj.CanPlugNew("OrderNumber:6ES7 193-6PA00-0AA0/V1.1", "Server module_1", slotNr))
                {
                    DeviceItem deviceSubMod = obj.PlugNew("OrderNumber:6ES7 193-6PA00-0AA0/V1.1", "Server module_1", slotNr);
                    slotNr++;
                    DeviceItemAssociation itemAss = deviceSubMod.Items;
                    foreach (DeviceItem deviceItem in itemAss)
                    {
                        AddressComposition AddrAss = deviceItem.Addresses;
                        foreach (Address address in AddrAss)
                        {
                            address.SetAttribute("StartAddress", int.Parse(startAddr));
                        }
                    }
                }

                NetworkInterface itf = ((IEngineeringServiceProvider)deviceName.Items[0].Items[0].Items[1]).GetService<NetworkInterface>();
                Subnet net = tiaPortal.Subnets.Find("PN/IE_1");
                Node node = itf.Nodes[0];
                IoConnector ioConn = itf.IoConnectors[0];
                node.SetAttribute("Address", IPAddr);
                node.ConnectToSubnet(net);
                ioConn.ConnectToIoSystem(net.IoSystems[0]);
                ioConn.SetAttribute("PnDeviceNumber", int.Parse(IPAddr.Split('.')[3]));
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="devName"></param>
        /// <param name="IPAddr"></param>
        /// <param name="startAddr"></param>
        /// <param name="motorsList"></param>
        /// <param name="tiaPortal"></param>
        /// <param name="type"></param>
        public static void InsertHWLenze(string devName, string IPAddr, string startAddr, List<int> motorsList, Project tiaPortal, string type)
        {
            string typeIdentifierLENZE = "", typeIdentifierModule = "", typeIdentifierSafetyModule = "", name = "";
            int pzdW = 0; //Number of PZD W

            switch (type)
            {
                case "RF_":
                    pzdW = 6;
                    typeIdentifierModule = "GSD:GSDML-V2.32-LENZE-8420PN120-20161213.XML/M/IDM_MODULE_46";
                    typeIdentifierLENZE = "GSD:GSDML-V2.32-LENZE-8420PN120-20161213.XML/DAP/ID_DAP";
                    typeIdentifierSafetyModule = "GSD:GSDML-V2.32-LENZE-8420PN120-20161213.XML/M/IDM_MODULE_84";
                    name = "PCD(  " + pzdW + "W ) AR kons._";
                    break;

                case "HER":
                case "HE_":
                case "HTS":
                    pzdW = 12;
                    typeIdentifierModule = "GSD:GSDML-V2.2-LENZE-9400PN140-20110706.XML/M/12";
                    typeIdentifierLENZE = "GSD:GSDML-V2.2-LENZE-9400PN140-20110706.XML/DAP/DIM 2";
                    typeIdentifierSafetyModule = "GSD:GSDML-V2.2-LENZE-9400PN140-20110706.XML/M/33";
                    name = "PZD(  " + pzdW + "W ) AR cons._";
                    break;

                case "FX_":
                    pzdW = 8;
                    typeIdentifierModule = "GSD:GSDML-V2.2-LENZE-9400PN140-20110706.XML/M/8";
                    typeIdentifierLENZE = "GSD:GSDML-V2.2-LENZE-9400PN140-20110706.XML/DAP/DIM 2";
                    typeIdentifierSafetyModule = "GSD:GSDML-V2.2-LENZE-9400PN140-20110706.XML/M/33";
                    name = "PZD(  " + pzdW + "W ) AR cons._";
                    break;
            }

            Device device = tiaPortal.Devices.Find(devName);
            int slotNr = 1;

            if (device == null)
            {
                Device deviceName = tiaPortal.Devices.CreateWithItem(typeIdentifierLENZE, devName, devName);
                HardwareObject obj = deviceName.DeviceItems[1].Container;

                for (int i = 1; i <= motorsList[0]; i++) //PZD
                {
                    if (obj.CanPlugNew(typeIdentifierModule, name + i.ToString(), slotNr))
                    {
                        DeviceItem deviceSubMod = obj.PlugNew(typeIdentifierModule, name + i.ToString(), slotNr);
                        slotNr++;
                        DeviceItemAssociation itemAss = deviceSubMod.Items;
                        foreach (DeviceItem deviceItem in itemAss)
                        {
                            AddressComposition AddrAss = deviceItem.Addresses;
                            foreach (Address address in AddrAss)
                            {
                                address.SetAttribute("StartAddress", int.Parse(startAddr));
                                startAddr = (int.Parse(startAddr) + 2).ToString();
                            }
                        }
                    }
                }
                for (int i = 1; i <= motorsList[1]; i++) //SAFETY
                {
                    if (obj.CanPlugNew(typeIdentifierSafetyModule, "Safety( 4W ) AR_" + i.ToString(), slotNr))
                    {
                        DeviceItem deviceSubMod = obj.PlugNew(typeIdentifierSafetyModule, "Safety( 4W ) AR_" + i.ToString(), slotNr);
                        slotNr++;
                        DeviceItemAssociation itemAss = deviceSubMod.Items;
                        foreach (DeviceItem deviceItem in itemAss)
                        {
                            AddressComposition AddrAss = deviceItem.Addresses;
                            foreach (Address address in AddrAss)
                            {
                                address.SetAttribute("StartAddress", int.Parse(startAddr));
                                startAddr = (int.Parse(startAddr) + 1).ToString();
                            }
                        }
                    }
                }

                NetworkInterface itf = ((IEngineeringServiceProvider)deviceName.Items[0].Items[0].Items[0]).GetService<NetworkInterface>();
                Subnet net = tiaPortal.Subnets.Find("PN/IE_1");
                Node node = itf.Nodes[0];
                IoConnector ioConn = itf.IoConnectors[0];
                node.SetAttribute("Address", IPAddr);
                node.ConnectToSubnet(net);
                ioConn.ConnectToIoSystem(net.IoSystems[0]);
                ioConn.SetAttribute("PnDeviceNumber", int.Parse(IPAddr.Split('.')[3]));
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="devName"></param>
        /// <param name="IPAddr"></param>
        /// <param name="tiaPortal"></param>
        public static void InsertHWMV5(string devName, string IPAddr, Project tiaPortal)
        {
            Device deviceTIA = tiaPortal.Devices.Find(devName);
            if (deviceTIA == null)
            {
                Device deviceName = tiaPortal.Devices.CreateWithItem("OrderNumber:6GF3 5**-*****/V1.0", devName, devName);
                HardwareObject obj = deviceName.DeviceItems[1].Container;

                NetworkInterface itf = ((IEngineeringServiceProvider)deviceName.Items[0].Items[0].Items[0]).GetService<NetworkInterface>();
                Subnet net = tiaPortal.Subnets.Find("PN/IE_1");
                Node node = itf.Nodes[0];
                IoConnector ioConn = itf.IoConnectors[0];
                node.SetAttribute("Address", IPAddr);
                node.ConnectToSubnet(net);
                ioConn.ConnectToIoSystem(net.IoSystems[0]);
                ioConn.SetAttribute("PnDeviceNumber", int.Parse(IPAddr.Split('.')[3]));
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="devName"></param>
        /// <param name="IPAddr"></param>
        /// <param name="tiaPortal"></param>
        public static void InsertHWIDENTControl(string devName, string IPAddr, Project tiaPortal)
        {
            Device deviceTIA = tiaPortal.Devices.Find(devName);
            if (deviceTIA == null)
            {
                Device deviceName = tiaPortal.Devices.CreateWithItem("GSD:GSDML-V2.2-PEPPERL+FUCHS-IDENTCONTROLAIDA1-20110629.XML/DAP/DAP 2", devName, devName);
                HardwareObject obj = deviceName.DeviceItems[1].Container;

                NetworkInterface itf = ((IEngineeringServiceProvider)deviceName.Items[0].Items[0].Items[0]).GetService<NetworkInterface>();
                Subnet net = tiaPortal.Subnets.Find("PN/IE_1");
                Node node = itf.Nodes[0];
                IoConnector ioConn = itf.IoConnectors[0];
                node.SetAttribute("Address", IPAddr);
                node.ConnectToSubnet(net);
                ioConn.ConnectToIoSystem(net.IoSystems[0]);
                ioConn.SetAttribute("PnDeviceNumber", int.Parse(IPAddr.Split('.')[3]));
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="devName"></param>
        /// <param name="IPAddr"></param>
        /// <param name="tiaPortal"></param>
        public static void InsertHWEuchnermgb(string devName, string IPAddr, Project tiaPortal)
        {
            Device deviceTIA = tiaPortal.Devices.Find(devName);
            if (deviceTIA == null)
            {
                Device deviceName = tiaPortal.Devices.CreateWithItem("GSD:GSDML-V2.3-EUCHNER-MGB_PN_D_110025-20150410.XML/DAP/DAP 101", devName, devName);
                HardwareObject obj = deviceName.DeviceItems[1].Container;

                NetworkInterface itf = ((IEngineeringServiceProvider)deviceName.Items[0].Items[0].Items[0]).GetService<NetworkInterface>();
                Subnet net = tiaPortal.Subnets.Find("PN/IE_1");
                Node node = itf.Nodes[0];
                IoConnector ioConn = itf.IoConnectors[0];
                node.SetAttribute("Address", IPAddr);
                node.ConnectToSubnet(net);
                ioConn.ConnectToIoSystem(net.IoSystems[0]);
                ioConn.SetAttribute("PnDeviceNumber", int.Parse(IPAddr.Split('.')[3]));
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="devName"></param>
        /// <param name="IPAddr"></param>
        /// <param name="startAddr"></param>
        /// <param name="listCoupler"></param>
        /// <param name="type"></param>
        /// <param name="tiaPortal"></param>
        public static void InsertHWCoupler(string devName, string IPAddr, string startAddr, List<int> listCoupler, string type, Project tiaPortal)
        {
            Device deviceTIA = tiaPortal.Devices.Find(devName);
            int slotNr = 1;

            if (deviceTIA == null)
            {
                Device deviceName = null;
                if (type == "EV_")
                    deviceName = tiaPortal.Devices.CreateWithItem("GSD:GSDML-V2.32-SIEMENS-PNPNIOC-20170715.XML/DAP/DAP X1 V4.0", devName, devName);
                else
                    deviceName = tiaPortal.Devices.CreateWithItem("GSD:GSDML-V2.32-SIEMENS-PNPNIOC-20170715.XML/DAP/DAP X2 V4.0", devName, devName);

                HardwareObject obj = deviceName.DeviceItems[1].Container;

                for (int i = 1; i <= listCoupler[0]; i++) // In 32 Bytes
                {
                    if (obj.CanPlugNew("GSD:GSDML-V2.32-SIEMENS-PNPNIOC-20170715.XML/M/15, Compatibility V3_x_32 Bytes Input", "*IN  32 Bytes_" + i.ToString(), slotNr))
                    {
                        DeviceItem deviceSubMod = obj.PlugNew("GSD:GSDML-V2.32-SIEMENS-PNPNIOC-20170715.XML/M/15, Compatibility V3_x_32 Bytes Input", "*IN  32 Bytes_" + i.ToString(), slotNr);
                        slotNr++;
                        DeviceItemAssociation itemAss = deviceSubMod.Items;
                        foreach (DeviceItem deviceItem in itemAss)
                        {
                            AddressComposition AddrAss = deviceItem.Addresses;
                            foreach (Address address in AddrAss)
                            {
                                address.SetAttribute("StartAddress", int.Parse(startAddr));
                                startAddr = (int.Parse(startAddr) + 2).ToString();
                            }
                        }
                    }
                }
                for (int i = 1; i <= listCoupler[1]; i++) // Out 32 Bytes
                {
                    if (obj.CanPlugNew("GSD:GSDML-V2.32-SIEMENS-PNPNIOC-20170715.XML/M/25, Compatibility V3_x_32 Bytes Output", "*OUT  32 Bytes_" + i.ToString(), slotNr))
                    {
                        DeviceItem deviceSubMod = obj.PlugNew("GSD:GSDML-V2.32-SIEMENS-PNPNIOC-20170715.XML/M/25, Compatibility V3_x_32 Bytes Output", "*OUT  32 Bytes_" + i.ToString(), slotNr);
                        slotNr++;
                        DeviceItemAssociation itemAss = deviceSubMod.Items;
                        foreach (DeviceItem deviceItem in itemAss)
                        {
                            AddressComposition AddrAss = deviceItem.Addresses;
                            foreach (Address address in AddrAss)
                            {
                                address.SetAttribute("StartAddress", int.Parse(startAddr));
                                startAddr = (int.Parse(startAddr) + 2).ToString();
                            }
                        }
                    }
                }
                for (int i = 1; i <= listCoupler[2]; i++) // PROFIsafe in/out 6 byte
                {
                    if (obj.CanPlugNew("GSD:GSDML-V2.32-SIEMENS-PNPNIOC-20170715.XML/M/1, Compatibility V3_x_6 Bytes Input / 12 Bytes Output", "*PROFIsafe IN/OUT 6 Byte / 12 Byte_" + i.ToString(), slotNr))
                    {
                        DeviceItem deviceSubMod = obj.PlugNew("GSD:GSDML-V2.32-SIEMENS-PNPNIOC-20170715.XML/M/1, Compatibility V3_x_6 Bytes Input / 12 Bytes Output", "*PROFIsafe IN/OUT 6 Byte / 12 Byte_" + i.ToString(), slotNr);
                        slotNr++;
                        DeviceItemAssociation itemAss = deviceSubMod.Items;
                        foreach (DeviceItem deviceItem in itemAss)
                        {
                            AddressComposition AddrAss = deviceItem.Addresses;
                            foreach (Address address in AddrAss)
                            {
                                address.SetAttribute("StartAddress", int.Parse(startAddr));
                                startAddr = (int.Parse(startAddr) + 2).ToString();
                            }
                        }
                    }
                }
                for (int i = 1; i <= listCoupler[3]; i++) // PROFIsafe in/out 12 Byte
                {
                    if (obj.CanPlugNew("GSD:GSDML-V2.32-SIEMENS-PNPNIOC-20170715.XML/M/2, Compatibility V3_x_12 Bytes Input / 6 Bytes Output", "*PROFIsafe IN/OUT 12 Byte / 6 Byte_" + i.ToString(), slotNr))
                    {
                        DeviceItem deviceSubMod = obj.PlugNew("GSD:GSDML-V2.32-SIEMENS-PNPNIOC-20170715.XML/M/2, Compatibility V3_x_12 Bytes Input / 6 Bytes Output", "*PROFIsafe IN/OUT 12 Byte / 6 Byte_" + i.ToString(), slotNr);
                        slotNr++;
                        DeviceItemAssociation itemAss = deviceSubMod.Items;
                        foreach (DeviceItem deviceItem in itemAss)
                        {
                            AddressComposition AddrAss = deviceItem.Addresses;
                            foreach (Address address in AddrAss)
                            {
                                address.SetAttribute("StartAddress", int.Parse(startAddr));
                                startAddr = (int.Parse(startAddr) + 2).ToString();
                            }
                        }
                    }
                }
                
                NetworkInterface itf = ((IEngineeringServiceProvider)deviceName.Items[0].Items[0].Items[0]).GetService<NetworkInterface>();
                Subnet net = tiaPortal.Subnets.Find("PN/IE_1");
                Node node = itf.Nodes[0];
                IoConnector ioConn = itf.IoConnectors[0];
                node.SetAttribute("Address", IPAddr);
                node.ConnectToSubnet(net);
                ioConn.ConnectToIoSystem(net.IoSystems[0]);
                ioConn.SetAttribute("PnDeviceNumber", int.Parse(IPAddr.Split('.')[3]));
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="library"></param>
        /// <param name="folderName"></param>
        private static MasterCopy SearchLibraryFolder(MasterCopySystemFolder library, string folderName)
        {
            MasterCopy HWfolder = null;
            foreach (var folder in library.Folders[0].Folders)
            {
                if (folder.MasterCopies.Find(folderName) != null)
                {
                    HWfolder = folder.MasterCopies.Find(folderName);
                    break;
                }
            }
            return HWfolder;
        }
    }
}