using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace PC_Monitor
{
    public partial class Form1 : Form
    {
        private GetInfo _computer = new GetInfo();
        private LogHelper _toLog = new LogHelper();
        private List<string> _checkedSensors = new List<string>();
        private Thread _checkThread;

        //Initialization
        public Form1()
        {
            InitializeComponent();
        }
  
        //Outputting to from info about computers sensors
        public void OutputToForm(string value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(OutputToForm), new object[] { value });
                return;
            }
            label2.Text = _computer.GetCPULoad();
            label4.Text = _computer.GetCPUTemperature();
            label5.Text = _computer.GetRAMUsage();
            label7.Text = _computer.GetGPUTemperature();
            label9.Text = _computer.GetCPUVoltage();
        }

        void OutputToFormWrapper()
        {
            while(true)
            { 
                OutputToForm(" ");
                Thread.Sleep(2000);
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            Thread mainThread = new Thread(OutputToFormWrapper) { IsBackground=true};
            mainThread.Start();
        }
        // Adding sensors to control it
        private void button1_Click(object sender, EventArgs e)
        {
            foreach (Control control in this.Controls)
            {
                if (control as CheckBox != null)
                {
                    if ((control as CheckBox).Checked)
                    {
                        _checkedSensors.Add(control.Text);
                    }
                }
            }
            _checkThread = new Thread(StartCheckWrapper) { IsBackground=true};
            _checkThread.Start();
        }

        public void StartCheck(string value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(StartCheck), new object[] { value });
                return;
            }

            foreach (string control in _checkedSensors)
            {
                if (control == "CPU Load" && !String.IsNullOrEmpty(textBox1.Text))
                {
                    _toLog.SetValuesToCheck("CPU Load", Convert.ToSingle(textBox1.Text));
                    _toLog.GetValuesToCheck(control, Convert.ToSingle(label2.Text.Replace("%", "")));
                }
                else if (control == "CPU Temperature" && !String.IsNullOrEmpty(textBox2.Text))
                {
                    _toLog.SetValuesToCheck("CPU Temperature", Convert.ToSingle(textBox2.Text));
                    _toLog.GetValuesToCheck(control, Convert.ToSingle(label4.Text.Replace("°C", "")));
                }
                else if (control == "RAM Usage" && !String.IsNullOrEmpty(textBox3.Text))
                {
                    _toLog.SetValuesToCheck("RAM Usage", Convert.ToSingle(textBox3.Text));
                    _toLog.GetValuesToCheck(control, Convert.ToSingle(label5.Text.Replace("%", "")));
                }
                else if (control == "GPU Temperature" && !String.IsNullOrEmpty(textBox4.Text))
                {
                    _toLog.SetValuesToCheck("GPU Temperature", Convert.ToSingle(textBox4.Text));
                    _toLog.GetValuesToCheck(control, Convert.ToSingle(label7.Text.Replace("°C", "")));
                }
                else if (control == "CPU Voltage" && !String.IsNullOrEmpty(textBox5.Text))
                {
                    _toLog.SetValuesToCheck("CPU Voltage", Convert.ToSingle(textBox5.Text));
                    _toLog.GetValuesToCheck(control, Convert.ToSingle(label9.Text.Replace("V", "")));
                }
                else
                {
                    MessageBox.Show("eror");
                }
            }
        }

        private void StartCheckWrapper()
        {
            while (true)
            {
                Thread.Sleep(20000);
                MessageBox.Show("Threshold value exceeded.\nPlease check your log-list or make a request to database.");
                StartCheck(" ");
            }
        }     
    }
}
