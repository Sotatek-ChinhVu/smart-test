using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BinaryDataController : ControllerBase
    {
        // POST api/values
        [HttpPost]
        public ActionResult<BinaryStructure> Post()
        {
            //var stream = Request.Body;

            string txtPath = @"C:\Users\Admin\Downloads\ImageByte.txt";
            string text = System.IO.File.ReadAllText(txtPath);
            string[] chars = text.Split(" ");
            byte[] bytes = chars.Select(c => StringToByteArray(c)).ToArray();

            //List<byte> byteArray = new List<byte>(); 
            //foreach (var item in chars) 
            //{
            //    byteArray.Add(Convert.ToByte(long.Parse(item)));
            //}

            Stream stream = new MemoryStream(bytes);

            BinaryStructure result = ProcessBinaryData(stream);
            return new ActionResult<BinaryStructure>(result);
        }

        public static byte StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .First();
        }

        private BinaryStructure ProcessBinaryData(Stream? inputStream)
        {
            string root = System.AppDomain.CurrentDomain.BaseDirectory;
            if (inputStream != null)
            {
                bool isPrefixOK = true;
                Byte[] bt = new Byte[10];
                inputStream.Seek(0, System.IO.SeekOrigin.Begin);
                inputStream.Read(bt, 0, 10);
                if (isPrefixOK && 'H' != bt[0]) isPrefixOK = false;
                if (isPrefixOK && 'T' != bt[1]) isPrefixOK = false;
                if (isPrefixOK && 'T' != bt[2]) isPrefixOK = false;
                if (isPrefixOK && 'P' != bt[3]) isPrefixOK = false;
                if (isPrefixOK && 'S' != bt[4]) isPrefixOK = false;
                if (isPrefixOK && 'T' != bt[5]) isPrefixOK = false;
                if (isPrefixOK && 'R' != bt[6]) isPrefixOK = false;
                if (isPrefixOK && 'E' != bt[7]) isPrefixOK = false;
                if (isPrefixOK && 'A' != bt[8]) isPrefixOK = false;
                if (isPrefixOK && 'M' != bt[9]) isPrefixOK = false;
                if (!isPrefixOK)
                {
                    Console.WriteLine("The prefix is wrong: " + bt);
                    Console.WriteLine("ASUploadLargeFile  End ");
                    return new BinaryStructure("The prefix is wrong: " + bt + "ASUploadLargeFile  End ");
                }
                string prefix = ByteArrayToString(bt.ToList());

                bt = new Byte[4];
                inputStream.Read(bt, 0, 4);
                Int32 length = BitConverter.ToInt32(bt, 0);
                bt = new Byte[length];
                inputStream.Read(bt, 0, length);
                string fileName = System.Text.Encoding.UTF8.GetString(bt);
                String filepath = System.Text.Encoding.UTF8.GetString(bt);
                if (!String.IsNullOrEmpty(root))
                {
                    filepath = root + "\\" + filepath.TrimStart('\\');
                    string dir = System.IO.Path.GetDirectoryName(filepath);
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                }
                FileStream fs = new FileStream(filepath, FileMode.Create);
                BinaryWriter bw = new BinaryWriter(fs);
                List<byte> contentFile = new List<byte>();
                try
                {
                    bt = new byte[4096];
                    int nRead = 0;
                    int nTotal = 0; 
                    
                    while ((nRead = inputStream.Read(bt, 0, 4096)) > 0)
                    {
                        bw.Write(bt, 0, nRead); nTotal = nTotal + nRead;

                        if (nRead != 4096)
                        {
                            for (int i = 0; i < nRead; i++)
                            {
                                contentFile.Add(bt[i]);
                            }
                        }
                        else
                        {
                            contentFile.AddRange(bt);
                        }
                        
                        Console.WriteLine("Reading : " + nRead + "/" + nTotal);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return new BinaryStructure(ex.Message);
                }
                finally
                {
                    bw.Close();
                    fs.Close();
                }
                return new BinaryStructure(prefix, length, fileName, ByteArrayToString(contentFile), "Convert done");
            }
            return new BinaryStructure("Stream is null or empty");
        }

        public static string ByteArrayToString(List<byte> ba)
        {
            StringBuilder hex = new StringBuilder(ba.Count * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2},", b);
            return hex.ToString();
        }
    }

    public class BinaryStructure
    {
        public string Prefix { get; private set; }

        public int PathLength { get; private set; }

        public string Path { get; private set; }

        public string ContentHex { get;  private set; }

        public string Message { get; private set; }

        public BinaryStructure(string prefix, int pathLength, string path, string contentHex, string message)
        {
            Prefix = prefix;
            PathLength = pathLength;
            Path = path;
            ContentHex = contentHex;
            Message = message;
        }

        public BinaryStructure(string message)
        {
            Prefix = string.Empty;
            PathLength = 0;
            Path = string.Empty;
            ContentHex = string.Empty;
            Message = message;
        }
    }
}
