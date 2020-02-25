using System;

namespace DailyPlanner.Utils
{
    class Program
    {
        static void Main(string[] args)
        {
            CertificateUtil.MakeCert();
            CertificateUtil.ImportCert();
        }
    }
}
