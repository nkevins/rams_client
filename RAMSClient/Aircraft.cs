using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAMSClient
{
    public class Aircraft : IObserver
    {
        public string uid { get; set; }
        public string callsign { get; set; }
        public double alt { get; set; }
        public double ias { get; set; }
        public double hdg { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public double pitch { get; set; }
        public double bank { get; set; }
        public double vs { get; set; }

        public Aircraft()
        {
            uid = Guid.NewGuid().ToString();
        }

        public void DataUpdated()
        {
            FSData fsData = FSData.GetInstance();
            alt = fsData.alt;
            ias = fsData.ias;
            hdg = fsData.hdg;
            lat = fsData.lat;
            lon = fsData.lon;
            pitch = fsData.pitch;
            bank = fsData.bank;
            vs = fsData.vs;
        }

        public override string ToString()
        {
            string output = "";
            output += "Alt: " + alt.ToString("f0") + Environment.NewLine;
            output += "IAS: " + ias.ToString("f0") + Environment.NewLine;
            output += "HDG: " + hdg.ToString("f0") + Environment.NewLine;
            output += "Lat: " + lat.ToString("f4") + Environment.NewLine;
            output += "Lon: " + lon.ToString("f4") + Environment.NewLine;
            output += "Pitch: " + pitch.ToString("f3") + Environment.NewLine;
            output += "Bank: " + bank.ToString("f3") + Environment.NewLine;
            output += "VS: " + vs.ToString("f0") + Environment.NewLine;

            return output;
        }
    }
}
