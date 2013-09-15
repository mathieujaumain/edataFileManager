using System;
using System.IO;
using System.Linq;
using EdataFileManager.NdfBin;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdataFileManager.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            const string everythingPath = @"C:\Users\enohka\Desktop\Teststuff\everything.ndfbin";
            byte[] data;


            using (var fs = new FileStream(everythingPath, FileMode.Open))
            {
                using (var ms = new MemoryStream())
                {
                    var buffer = new byte[4096];
                    int size = 1;

                    while (size > 0)
                    {
                        size = fs.Read(buffer, 0, buffer.Length);
                        ms.Write(buffer, 0, size);
                    }
                    data = ms.ToArray();
                }
            }

            var m = new NdfbinManager(data);

            m.ParseData();
        }

        [TestMethod]
        public void RepackIt()
        {
            const string path = @"E:\Programme\Steam\SteamApps\common\Wargame Airland Battle\Data\wargame\PC\2100001472_bak\NDF_Win.dat";

            var mgr = new EdataManager(path);

            mgr.ParseEdataFile();

            mgr.ReplaceFile(mgr.Files.Single(x => x.Path.Contains("everything.ndfbin")), new byte[1000]);
        }
    }
}
