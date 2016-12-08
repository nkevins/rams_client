using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RAMSClient
{
    public partial class Form1 : Form, IObserver
    {
        private FSData fsData;
        private Aircraft aircraft;
        private Websocket websocket;

        public Form1()
        {
            InitializeComponent();
            fsData = FSData.GetInstance();
            aircraft = new Aircraft();
            websocket = new Websocket(aircraft);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            fsData.OpenFSUIPC();
            tmrDataUpdate.Enabled = true;
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            aircraft.callsign = tbxCallsign.Text;
            tbxCallsign.Enabled = false;

            fsData.Attach(this);
            fsData.Attach(aircraft);
            fsData.Attach(websocket);
        }

        private void tmrDataUpdate_Tick(object sender, EventArgs e)
        {
            fsData.UpdateData();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            fsData.CloseFSUIPC();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            tmrDataUpdate.Enabled = false;
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            tbxCallsign.Enabled = true;
            fsData.CloseFSUIPC();
        }

        public void DataUpdated()
        {
            lblData.Text = aircraft.ToString();
        }
    }
}
