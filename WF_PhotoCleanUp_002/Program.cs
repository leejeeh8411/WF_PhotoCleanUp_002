using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using MetadataExtractor.IO;
using MetadataExtractor.Util;
using MetadataExtractor.Formats.QuickTime;
using System.Globalization;

namespace WF_PhotoCleanUp_002
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
         

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

    }
    class MyWorker
    {
        public struct MyPhoto
        {
            public int nIdx;
            public int nDate;
            public long nDateFull;
            public bool bExifDate;
            public string filePath;
            public string fileName;
            public string folderName;
            public bool bDone;
            public string fileExt;
            public bool bImage;
            public bool bVideo;
        }

        public struct MyExifData
        {
            public DateTime dtDate;
            public bool bSuccessExifData;
            public string strFileExt;
            public bool bVideo;
            public bool bImage;
        }

        Thread threadReadFolder = null;
        public string m_strSrcPath = string.Empty;
        public string m_strDstPath = string.Empty;
        public int m_nCntAll = 0;
        public int m_nCnt = 0;

        public Queue<string> m_queueLogMsg = new Queue<string>();

        List<MyPhoto> listPhoto = new List<MyPhoto>();
        PhotoFile JpgFile = new PhotoFile();
        PhotoFile NonJpgFile = new PhotoFile();

        string[] strListUseExt = { "MP4", "MOV" };

        private void ThreadReadFolder1()
        {
           
            //파일 탐색
            if (string.IsNullOrEmpty(m_strSrcPath) == false)
            {
                DirectoryInfo di = new DirectoryInfo(m_strSrcPath);
                int nCnt = 0;

                //아래 안들어갈 수도 있음. 바로 파일이 있을때.

                foreach (System.IO.FileInfo f in di.GetFiles())
                {
                    nCnt++;
                }

                foreach (var item in di.GetDirectories())
                {
                    nCnt += item.GetFiles("*.*", System.IO.SearchOption.AllDirectories).Length;
                }


                m_nCntAll = nCnt;

                listPhoto.Clear();

                for(int i=0; i<1000; i++)
                {
                    loopNew();
                    m_queueLogMsg.Enqueue(string.Format("{0}번 호출", i));
                }
                //find_photo2(m_strSrcPath);
            }
           
        }

        public void RunWorkThread()
        {
            threadReadFolder = new Thread(new ThreadStart(ThreadReadFolder1));
            threadReadFolder.Start();
            threadReadFolder.Join();
        }

        void loopNew()
        {
            for(int i=0; i<1000000; i++)
            {
                int ii = i;
            }
        }
            void find_photo2(string path)
        {
            System.IO.DirectoryInfo diInfo = new System.IO.DirectoryInfo(path);

            // 현재 디렉토리의 총 파일 갯수 구하기
            foreach (System.IO.FileInfo File in diInfo.GetFiles())
            {
                MyExifData myExifData = get_exif_data(File.FullName);
                MyPhoto myPhoto = new MyPhoto();

                if (myExifData.bSuccessExifData)
                {
                    myPhoto.bExifDate = false;
                    myPhoto.filePath = File.FullName;
                    myPhoto.fileName = File.Name;
                    listPhoto.Add(myPhoto);
                }
                else
                {
                    DateTime dtCreatedDate = myExifData.dtDate;
                    string myConvertedDate = dtCreatedDate.ToString("yyyy-MM-dd");
                    int nCovertedDate = Convert.ToInt32(dtCreatedDate.ToString("yyyyMMdd"));
                    long nCovertedDateFull = Convert.ToInt64(dtCreatedDate.ToString("yyyyMMddHHmmss"));

                    myPhoto.nDate = nCovertedDate;
                    myPhoto.nDateFull = nCovertedDateFull;
                    myPhoto.fileName = File.Name;
                    myPhoto.bExifDate = true;
                    myPhoto.filePath = File.FullName;
                    myPhoto.folderName = myConvertedDate;
                    myPhoto.bDone = false;
                    myPhoto.fileExt = myExifData.strFileExt;
                    myPhoto.bImage = myExifData.bImage;
                    myPhoto.bVideo = myExifData.bVideo;

                    listPhoto.Add(myPhoto);
                }
            }


            System.IO.DirectoryInfo[] dirs = diInfo.GetDirectories();                       // 현재 path경로의 디렉터리들을 검색.       
            if (dirs.Length > 0)
            {
                foreach (System.IO.DirectoryInfo item in diInfo.GetDirectories())
                    find_photo2(item.FullName);
            }
        }

        MyExifData get_exif_data(string str_file)
        {
            MyExifData myExifData;
            myExifData.bSuccessExifData = false;
            myExifData.strFileExt = null;
            myExifData.bImage = false;
            myExifData.bVideo = false;
            myExifData.dtDate = new DateTime();

            try
            {
                FileStream myStream = new FileStream(str_file, FileMode.Open);
                FileType fileType = FileTypeDetector.DetectFileType(myStream);
                myStream.Close();

                IEnumerable<MetadataExtractor.Directory> directories;
                directories = ImageMetadataReader.ReadMetadata(str_file);
                DateTime dtTime;

                if (fileType == FileType.Jpeg || fileType == FileType.Png || fileType == FileType.Bmp)
                {
                    var subIfdDirectory = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();
                    var dateTime = subIfdDirectory?.GetDescription(ExifDirectoryBase.TagDateTimeOriginal);

                    if (dateTime != null)
                    {
                        dtTime = ConvertDateExif(dateTime);
                        myExifData.bSuccessExifData = true;
                        myExifData.strFileExt = Path.GetExtension(str_file);
                        myExifData.bImage = true;
                        myExifData.bVideo = false;
                        myExifData.dtDate = dtTime;
                    }

                }
                else if (fileType == FileType.QuickTime)//동영상일 경우 
                {
                    bool bUseVideoFile = UseVideoFile(str_file);
                    if (bUseVideoFile)
                    {
                        var subIfdDirectory = directories.OfType<QuickTimeMovieHeaderDirectory>().FirstOrDefault();
                        var dateTime = subIfdDirectory?.GetDescription(QuickTimeMovieHeaderDirectory.TagCreated);
                        if (dateTime != null)
                        {
                            dtTime = ConvertDateVideo(dateTime);
                            myExifData.bSuccessExifData = true;
                            myExifData.strFileExt = Path.GetExtension(str_file);
                            myExifData.bImage = false;
                            myExifData.bVideo = true;
                            myExifData.dtDate = dtTime;
                        }
                    }
                }
            }
            catch (MetadataExtractor.ImageProcessingException e)
            {
                //WriteLog("Exception-" + str_file);
                string strMsg = string.Format("Exception-{0}", str_file);
                m_queueLogMsg.Enqueue(strMsg);
                return myExifData;
            }
            catch (System.UnauthorizedAccessException e)
            {
                //WriteLog("Exception-" + str_file);
                string strMsg = string.Format("Exception-{0}", str_file);
                m_queueLogMsg.Enqueue(strMsg);
                return myExifData;
            }
            catch (System.IO.IOException e)
            {
                //WriteLog("Exception-" + str_file);
                string strMsg = string.Format("Exception-{0}", str_file);
                m_queueLogMsg.Enqueue(strMsg);
                return myExifData;
            }

            return myExifData;
        }

        bool UseVideoFile(string strFile)
        {
            bool bRet = false;

            string strParseExt = Path.GetExtension(strFile);
            strParseExt = strParseExt.Replace(".", "");

            foreach (string strExt in strListUseExt)
            {
                if (strExt == strParseExt)
                {
                    bRet = true;
                    break;
                }
            }
            return bRet;
        }

        DateTime ConvertDateExif(string strDate)
        {
            //Exif Date string -> DateTime 
            DateTime dtDate;
            CultureInfo provider = CultureInfo.InvariantCulture;
            string strExposureDate2 = strDate.Replace("-", ":");
            string format = "yyyy:MM:dd HH:mm:ss";
            dtDate = DateTime.ParseExact(strExposureDate2, format, provider);
            //string myConvertedDate = dtDate.ToString("yyyy-MM-dd");
            //int nCovertedDate = Convert.ToInt32(dtDate.ToString("yyyyMMdd"));
            //long nCovertedDateFull = Convert.ToInt64(dtDate.ToString("yyyyMMddHHmmss"));

            return dtDate;
        }

        DateTime ConvertDateVideo(string strDate)
        {
            //목 8 08 17:00:03 2019
            //Exif Date string -> DateTime 
            DateTime dtDate;
            CultureInfo provider = CultureInfo.InvariantCulture;
            string strTemp1 = strDate.Substring(4, strDate.Length - 4);
            strTemp1 = strTemp1.Trim();
            int strTempMonth = Convert.ToInt32(strDate.Substring(2, 2));
            string strTempAll = string.Format("{0:d2} {1}", strTempMonth, strTemp1);
            strTempAll = strTempAll.Replace("-", ":");
            string format = "MM dd HH:mm:ss yyyy";//"yyyy:MM:dd HH:mm:ss";
            dtDate = DateTime.ParseExact(strTempAll, format, provider);

            return dtDate;
        }

    }
}
