using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace DailyPlanner.Utils
{
    public class CertificateUtil
    {
        public static void MakeCert()
        {
            var ecdsa = ECDsa.Create(); // generate asymmetric key pair
            var req = new CertificateRequest("cn=dailyplanner", ecdsa, HashAlgorithmName.SHA256);
            var cert = req.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(5));

            // Create PFX (PKCS #12) with private key
            File.WriteAllBytes("f:\\dp.pfx", cert.Export(X509ContentType.Pfx, "nMu2scN32QAWJwar"));

            // Create Base 64 encoded CER (public key only)
            File.WriteAllText("f:\\dp.cer",
                "-----BEGIN CERTIFICATE-----\r\n"
                + Convert.ToBase64String(cert.Export(X509ContentType.Cert), Base64FormattingOptions.InsertLineBreaks)
                + "\r\n-----END CERTIFICATE-----");
        }

        public static void ImportCert()
        {
            var fileName = "f:\\dp.cer";
            var cert = new X509Certificate2(fileName);
            var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadWrite);

            try
            {
                var contentType = X509Certificate2.GetCertContentType(fileName);
                var pfx = cert.Export(contentType);
                cert = new X509Certificate2(pfx, (string)null, X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);
                store.Add(cert);
            }
            finally
            {
                store.Close();

            }
        }
    }
}