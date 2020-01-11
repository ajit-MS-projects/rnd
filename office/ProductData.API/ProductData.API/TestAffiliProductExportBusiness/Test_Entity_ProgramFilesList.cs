using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Affilinet.Business.ProductExport.Entity;
using NUnit.Framework;
using System.IO;

namespace TestAffiliProductExportBusiness
{
    [TestFixture]
    class Test_Entity_ProgramFilesList
    {
    //    [Test]
    //    public void CheckIfFileExists_PositivTest()
    //    {
    //        string csvFilePath = @"d:\dtemp\";
    //        string prodProgId = "22";
    //        string expTimeStamp = "20100322_171822";
    //        string[] fileExt = new string[]{".product.notChanged.csv"};
    //        string filePath = csvFilePath + prodProgId + @"\" + expTimeStamp + "_" + prodProgId + fileExt[0];
    //        if(!File.Exists(filePath))
    //            File.Create(filePath);


    //        ProgramFilesList filesList = new ProgramFilesList();
    //        string result = filesList.CheckIfFileExists(expTimeStamp, prodProgId, csvFilePath);
            
    //        Assert.That(result == expTimeStamp);
    //    }

    //    [Test]
    //    public void CheckIfFileExists_NotExistingTimeStampButOtherFilesThere()
    //    {
    //        string csvFilePath = @"d:\dtemp\";
    //        string prodProgId = "22";
    //        string expTimeStamp = "20100322_171822";
    //        string notExistingExpTimeStamp = "20100322_111111";

    //        string[] fileExt = new string[] { ".product.notChanged.csv" };
    //        string path = Path.Combine(csvFilePath, prodProgId);

    //        try
    //        {
    //            if (!Directory.Exists(path))
    //                Directory.CreateDirectory(path);
    //        }
    //        catch (Exception e)
    //        {
    //            Assert.Fail();
    //        }
            
    //        path = Path.Combine(path, expTimeStamp + "_" + prodProgId + fileExt[0]);
    //        try
    //        {
    //            if (!File.Exists(path))
    //                File.Create(path);
    //        }
    //        catch (Exception e)
    //        {
    //            Assert.Fail();
    //        }
            


    //        ProgramFilesList filesList = new ProgramFilesList();
    //        string result = filesList.CheckIfFileExists(notExistingExpTimeStamp, prodProgId, csvFilePath);

    //        Assert.That(result == expTimeStamp);
    //    }

    //    [Test]
    //    public void CheckIfFileExists_NotExistingTimeStamp()
    //    {
    //        string csvFilePath = @"d:\dtemp\";
    //        string prodProgId = "23";
    //        string expTimeStamp = "20100322_171822";
    //        string notExistingExpTimeStamp = "20100000_000000";

    //        string[] fileExt = new string[] { ".product.notChanged.csv" };

    //        string path = Path.Combine(csvFilePath, prodProgId);

    //        try
    //        {
    //            if (!Directory.Exists(path))
    //                Directory.CreateDirectory(path);
    //        }
    //        catch (Exception e)
    //        {
    //            Assert.Fail();
    //        }

    //        ProgramFilesList filesList = new ProgramFilesList();
    //        string result = filesList.CheckIfFileExists(notExistingExpTimeStamp, prodProgId, csvFilePath);

    //        Assert.That(result == ".");
    //    }
    }
}
