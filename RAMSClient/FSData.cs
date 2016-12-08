using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSUIPC;

namespace RAMSClient
{
    public class FSData : Subject
    {
        private static FSData _instance;
        private bool _connectionOpen = false;

        public double ias { get; private set; }
        public double hdg { get; private set; }
        public double lat { get; private set; }
        public double lon { get; private set; }
        public double alt { get; private set; }
        public double pitch { get; private set; }
        public double bank { get; private set; }
        public double vs { get; private set; }

        private Offset<int> _ias = new Offset<int>(0x02BC);
        private Offset<double> _hdg = new Offset<double>(0x02CC);
        private Offset<long> _lat = new Offset<long>(0x0560);
        private Offset<long> _lon = new Offset<long>(0x0568);
        private Offset<long> _alt = new Offset<long>(0x0570);
        private Offset<int> _pitch = new Offset<int>(0x0578);
        private Offset<int> _bank = new Offset<int>(0x057C);
        private Offset<short> _vs = new Offset<short>(0x0842);

        private FSData() { }

        public static FSData GetInstance()
        {
            if (_instance == null)
            { 
                _instance = new FSData();
            }

            return _instance;
        }

        public void OpenFSUIPC()
        {
            if (_connectionOpen)
                return;

            try
            {
                FSUIPCConnection.Open();
                _connectionOpen = true;
            }
            catch (Exception ex)
            {
                FSUIPCConnection.Close();
                _connectionOpen = false;
                throw ex;
            }
        }

        public void CloseFSUIPC()
        {
            if (_connectionOpen)
            { 
                FSUIPCConnection.Close();
                _connectionOpen = false;
            }
        }

        public void UpdateData()
        {
            try
            {
                FSUIPCConnection.Process();

                ias = _ias.Value / 128;
                hdg = _hdg.Value;
                FsLongitude fsLon = new FsLongitude(_lon.Value);
                FsLatitude fsLat = new FsLatitude(_lat.Value);
                lon = fsLon.DecimalDegrees;
                lat = fsLat.DecimalDegrees;
                alt = _alt.Value / (65536.0 * 65536.0) * 3.28084;
                pitch = _pitch.Value * 360.0 / (65536.0 * 65536.0) * -1;
                bank = _bank.Value * 360.0 / (65536.0 * 65536.0);
                vs = _vs.Value * 3.28084 * -1;

                // update observers
                Notify();
            }
            catch (FSUIPCException ex)
            {
                if (ex.FSUIPCErrorCode == FSUIPCError.FSUIPC_ERR_SENDMSG)
                {
                    FSUIPCConnection.Close();
                    throw ex;
                }
                else
                {
                    throw ex;
                }
            }
            catch (Exception)
            {
                // Sometime when the connection is lost, bad data gets returned 
                // and causes problems with some of the other lines.  
                // This catch block just makes sure the user doesn't see any
                // other Exceptions apart from FSUIPCExceptions.
            }
        }
    }
}
