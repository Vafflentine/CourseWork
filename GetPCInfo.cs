using System;
using System.Management; //копонент для работы с WMI
using OpenHardwareMonitor.Hardware;

namespace PC_Monitor
{
        public class GetInfo : IVisitor
        {
        //Initialization
            public void VisitComputer(IComputer computer)
            {
                computer.Traverse(this);
            }

            public void VisitHardware(IHardware hardware)
            {
                hardware.Update();
                foreach (IHardware subHardware in hardware.SubHardware) subHardware.Accept(this);
            }

            public void VisitSensor(ISensor sensor) { }
            public void VisitParameter(IParameter parameter) { }
            Computer computer = new Computer();

        public string GetCPUTemperature()   //Getting CPU temperature in celsius
            {
                GetInfo updateVisitor = new GetInfo();
                computer.Open();
                computer.CPUEnabled = true;
                computer.Accept(updateVisitor);
                string result="";
                for (int i = 0; i < computer.Hardware.Length; i++)
                {
                    if (computer.Hardware[i].HardwareType == HardwareType.CPU)
                    {
                        for (int j = 0; j < computer.Hardware[i].Sensors.Length; j++)
                        {
                            if (computer.Hardware[i].Sensors[j].SensorType == SensorType.Temperature && computer.Hardware[i].Sensors[j].Name == "CPU Package")
                            {
                                result= computer.Hardware[i].Sensors[j].Value.ToString()+ "°C";
                                return result;
                            }
                        }
                    }
                }
                return result;
            }

            public string GetCPULoad()  //Get CPU load in percents
            {
                string Q = "SELECT LoadPercentage FROM Win32_Processor";
                string result = String.Empty;
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(Q);
                foreach (ManagementObject obj in searcher.Get())
                {
                    result = obj["LoadPercentage"].ToString().Trim();
                    result += "%";
                    
                }
                return result;
            }

           public string GetRAMUsage()  //Get RAM usage in percents
            {
                GetInfo updateVisitor = new GetInfo();
                computer.Open();
                computer.RAMEnabled = true;
                string result = "";
                float temp = 0;
                computer.Accept(updateVisitor);
                for (int i = 0; i < computer.Hardware.Length; i++)
                {
                    if (computer.Hardware[i].HardwareType == HardwareType.RAM)
                    {
                        for (int j = 0; j < computer.Hardware[i].Sensors.Length; j++)
                        {
                            if (computer.Hardware[i].Sensors[j].SensorType == SensorType.Load)
                            {
                                result = computer.Hardware[i].Sensors[j].Value.ToString();
                                temp = Convert.ToSingle(result);
                                temp=(float)Math.Truncate(100 * temp) / 100;
                                result = temp.ToString() + '%';
                                return result;
                            }
                        }
                    }
                }
            return result;
           }

           public string GetGPUTemperature()    // Get GPU Temperature in celsius
            {
                GetInfo updateVisitor = new GetInfo();
                computer.Open();
                computer.GPUEnabled = true;
                computer.Accept(updateVisitor);
                string result = "";
                for (int i = 0; i < computer.Hardware.Length; i++)
                {
                    if (computer.Hardware[i].HardwareType == HardwareType.GpuNvidia)
                    {
                        for (int j = 0; j < computer.Hardware[i].Sensors.Length; j++)
                        {
                            if (computer.Hardware[i].Sensors[j].SensorType == SensorType.Temperature)
                            {
                                result = computer.Hardware[i].Sensors[j].Value.ToString()+ "°C";
                                return result;
                            }
                        }
                    }
                }
            return result;
        }
            
           public string GetCPUVoltage()    //Get CPU voltage in volts
            {
                GetInfo updateVisitor = new GetInfo();
                computer.Open(); 
                computer.CPUEnabled = true;
                string result ="";
                double temp = 0;
                computer.Accept(updateVisitor);
                for (int i = 0; i < computer.Hardware.Length; i++)
                {
                    if (computer.Hardware[i].HardwareType == HardwareType.CPU)
                    {
                        for (int j = 0; j < computer.Hardware[i].Sensors.Length; j++)
                        {
                            if (computer.Hardware[i].Sensors[j].SensorType == SensorType.Power && computer.Hardware[i].Sensors[j].Name == "CPU Package")
                            {
                                temp = Math.Log(26, Convert.ToDouble(computer.Hardware[i].Sensors[j].Value));
                                result = Convert.ToString(Math.Truncate(1000 * temp) / 1000)+'V';
                                return result;
                            }
                        }
                    }
                }
            return result;
        }
        }
}