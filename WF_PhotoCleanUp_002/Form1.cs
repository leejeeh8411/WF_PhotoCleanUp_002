using System;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using Microsoft.WindowsAPICodePack.Dialogs;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using MetadataExtractor.IO;
using MetadataExtractor.Util;
using MetadataExtractor.Formats.QuickTime;



namespace WF_PhotoCleanUp_002
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

    public partial class Form1 : Form
    {

        Queue<string> m_queueLogMsg = new Queue<string>();
        string m_strTbSrcPath = string.Empty;


        System.Windows.Forms.Timer myTimer = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer myTimerProgress = new System.Windows.Forms.Timer();

        int m_nCntAll = 0;
        int m_nCnt = 0;

        string strSrcPath = string.Empty;
        string strDestPath = string.Empty;
        string strDefaultFolder = "C:\\";
        string[] strListUseExt = { "MP4", "MOV" };


        List<MyPhoto> listPhoto = new List<MyPhoto>();

        PhotoFile JpgFile = new PhotoFile();
        PhotoFile NonJpgFile = new PhotoFile();

        //Thread #1
        public void ThreadReadFolder()
        {
            ReadFolder(m_strTbSrcPath);
        }
        public void ThreadDrawInfo()
        {
            DrawInfo();
        }


        public Form1()
        {
            InitializeComponent();

            myTimer.Interval = 100;
            myTimer.Tick += new EventHandler(timer_tick);
            myTimer.Start();

            //myTimerProgress.Interval = 200;
            //myTimerProgress.Tick += new EventHandler(timer_tick_progress);
            //myTimerProgress.Start();

        }
        public void DrawInfo()
        {
            while (true)
            {
                string strLog1 = string.Format("{0}", m_nCnt);
                string strLog2 = string.Format("{0}", m_nCntAll);
                tb_cnt1.Text = strLog1;
                tb_cnt2.Text = strLog2;
                Delay(1);
            } 
        }

        public void SetCntInfo(int nCnt, int nCntAll)
        {
            m_nCnt = nCnt;
            m_nCntAll = nCntAll;
        }


        public void timer_tick(object sender, System.EventArgs e)
        {
            string strLog1 = string.Format("{0}", listPhoto.Count);
            string strLog2 = string.Format("{0}", m_nCntAll);
            tb_cnt1.Text = strLog1;
            tb_cnt2.Text = strLog2;

            


            if (m_queueLogMsg.Count() > 0)
            {
                string strMsg = string.Empty;
                for (int i=0; i< m_queueLogMsg.Count(); i++)
                {
                    strMsg = m_queueLogMsg.Dequeue();
                    WriteLog(strMsg);
                }
                
                
            }
        }

        public void timer_tick_progress(object sender, System.EventArgs e)
        {
            int nCntAll = m_nCntAll;
            int nCnt = m_nCnt;

            if(m_nCnt != 0 && m_nCntAll != 0)
            {
                int nPercent = Convert.ToInt32((((float)m_nCnt / (float)m_nCntAll) * 100.0));
                SetProgressBar(nPercent);
            }
        }

        private void btn_sel_src_folder_Click(object sender, EventArgs e)
        {
            strSrcPath = string.Empty;
            strSrcPath =  open_folder_dialog(strDefaultFolder);
            textBox_src_path.Text = strSrcPath;
        }

        private void btn_sel_dest_folder_Click(object sender, EventArgs e)
        {
            strDestPath = string.Empty;
            strDestPath = open_folder_dialog(strDefaultFolder);
            textBox_dest_path.Text = strDestPath;
        }

        private void btn_reserch_Click(object sender, EventArgs e)
        {
            ResetVariable();

            Thread thread = new Thread(new ThreadStart(delegate () // thread 생성
            {
                m_strTbSrcPath = textBox_src_path.Text;
                ReadFolder(m_strTbSrcPath);
                this.Invoke(new Action(delegate ()
                {
                   
                }));
            }));
            m_queueLogMsg.Enqueue("스레드 시작");
            thread.Start();
        }
       
        private void btn_clean_Click(object sender, EventArgs e)
        {
            return;

            string strFileCnt = string.Empty;

            if (string.IsNullOrEmpty(textBox_src_path.Text) == false && string.IsNullOrEmpty(textBox_dest_path.Text) == false)
            {
                strFileCnt = GetFileCnt();
                if (MessageBox.Show(strFileCnt + "정리를 실행하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    JpgFile.SetFileCnt(0);
                    NonJpgFile.SetFileCnt(0);
                    //yes
                    find_photo(textBox_src_path.Text, 1);
                    
                    //정렬
                    listPhoto.Sort(delegate(MyPhoto A, MyPhoto B)
                    {
                        if (A.nDateFull > B.nDateFull) return 1;
                        else if (A.nDateFull < B.nDateFull) return -1;
                        return 0;
                    });

                    int nCntNone = 0;
                    int nCnt = 0;
                    string strOldFolderName = string.Empty;
                    int nCleanedCnt = 0;

                    for(int nIndex = 0; nIndex < listPhoto.Count; nIndex++)
                    {
                        int nDate = listPhoto[nIndex].nDate;
                        string strDate = Convert.ToString(nDate);
                        string strDestFolder;
                        string strDestFileName;

                        int nNoneExif = 0;

                        if (nDate == nNoneExif)
                        {
                            strDestFolder = string.Format("{0}\\{1}", textBox_dest_path.Text, "None");
                            CreateFolder(strDestFolder);
                            strDestFileName = string.Format("{0}\\{1}\\{2}_{3}.jpg", textBox_dest_path.Text, "None", "None", nCntNone);
                            FileRename(textBox_dest_path.Text, listPhoto[nIndex].filePath, strDestFileName);
                            nCntNone++;
                        }
                        else
                        {
                            //해당 폴더에서 최초인지 체크
                            string strCurrentForderName = string.Empty;
                            strCurrentForderName = listPhoto[nIndex].folderName;

                            if (string.IsNullOrEmpty(strOldFolderName) || strOldFolderName != strCurrentForderName)
                            {
                                strOldFolderName = strCurrentForderName;
                                nCnt = 0;
                            }
                            strOldFolderName = listPhoto[nIndex].folderName;
                            strDestFolder = string.Format("{0}\\{1}", textBox_dest_path.Text, strCurrentForderName);
                            CreateFolder(strDestFolder);
                            strDestFileName = string.Format("{0}\\{1}\\{2}_{3}.jpg", textBox_dest_path.Text, strCurrentForderName, strCurrentForderName, nCnt);
                            FileRename(textBox_dest_path.Text, listPhoto[nIndex].filePath, strDestFileName);
                            nCnt++;           
                        }

                        nCleanedCnt++;
                    }
                    string strLog = string.Format("처리 갯수 : [{0}] / [{1}] ", nCleanedCnt, listPhoto.Count);
                    WriteLog(strLog);
                    Delay(1000);
                }
                else
                {
                    //no
                }
            }
            else
            {
                //메시지-경로를 설정해 주세요
            }
        }
        private static DateTime Delay(int ms)
        {
            DateTime ThisMoment = DateTime.Now;
            TimeSpan duration = new TimeSpan(0, 0, 0, 0, ms);
            DateTime AfterWatds = ThisMoment.Add(duration);
            while(AfterWatds >= ThisMoment)
            {
                System.Windows.Forms.Application.DoEvents();
                ThisMoment = DateTime.Now;
            }
            return DateTime.Now;
        }

        private void SetProgressBarBlockStyle()
        {
            // 디폴트값 사용 (Maximum=100, Minimum=0, Step=10)
            // 최대,최소,간격을 임의로 조정
            progressBar_clean.Style = ProgressBarStyle.Blocks;
            progressBar_clean.Minimum = 0;
            progressBar_clean.Maximum = 100;
            progressBar_clean.Step = 1;
            progressBar_clean.Value = 0;
        }

        private void SetProgressBar(int nPercent)
        {
            // 디폴트값 사용 (Maximum=100, Minimum=0, Step=10)
            // 최대,최소,간격을 임의로 조정
            if(nPercent > progressBar_clean.Maximum)
            {
                nPercent = progressBar_clean.Maximum;
            }
            progressBar_clean.Value = nPercent;
        }


        private string FileRename(string filePath, string oldFile, string newFile)
        {
            //oldFile = filePath + "\\" + oldFile;
            //newFile = filePath + "\\" + newFile;

            if (FileExistsCheck(oldFile))
            {
                System.IO.File.Move(oldFile, newFile);
                return "File Name Change :: " + oldFile + " >> " + newFile;
            }
            else
            {
                return "File Not Exists";
            }
        }

        private bool FileExistsCheck(string oldFile)
        {
           if(System.IO.File.Exists(oldFile))
           {
               return true;
           }
           else
           {
               return false;
           }
        }

        private string GetFileCnt()
        {
            string strText = string.Empty;
            JpgFile.SetFileCnt(0);
            NonJpgFile.SetFileCnt(0);
            listPhoto.Clear();
            //파일 탐색
            if (string.IsNullOrEmpty(textBox_src_path.Text) == false)
            {
                find_photo(textBox_src_path.Text, 0);
                int nJpgFileCnt = JpgFile.GetFileCnt();
                int nNOnJpgFileCnt = NonJpgFile.GetFileCnt();
                strText = string.Format("찾은 파일 갯수 : Jpg[{0}] / Non-Jpg[{1}] ", nJpgFileCnt, nNOnJpgFileCnt);
            }
            return strText;
        }

        private void ResetVariable()
        {
            string strText = string.Empty;
            JpgFile.SetFileCnt(0);
            NonJpgFile.SetFileCnt(0);
            listPhoto.Clear();
        }

        private void ProgressBarMarStart()
        {
            progressBar_clean.Enabled = true;
            progressBar_clean.MarqueeAnimationSpeed = 50;
            progressBar_clean.Style = ProgressBarStyle.Marquee;
        }

        private void ProgressBarMarStop()
        {
            progressBar_clean.Enabled = false;
            progressBar_clean.MarqueeAnimationSpeed = 0;
            progressBar_clean.Style = ProgressBarStyle.Blocks;
            progressBar_clean.Value = progressBar_clean.Minimum;
        }


        public void ReadFolder(string strFolderPath)
        {
            //파일 탐색
            if (string.IsNullOrEmpty(strFolderPath) == false)
            {
                DirectoryInfo di = new DirectoryInfo(strFolderPath);
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

                find_photo2(m_strTbSrcPath);
            }
        }

        private void open_file_dialog(string str_default_path)
        {
            openFileDialog1.InitialDirectory = str_default_path;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string file_path = openFileDialog1.FileName;
                string str_exposure_date = get_exif_info(file_path);
            }
        }

        private string open_folder_dialog(string str_default_path)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = str_default_path;
            dialog.IsFolderPicker = true;
            string fileName = string.Empty;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                fileName = dialog.FileName;
            }
            return fileName;
        }

        void find_photo(string path, int nMode)
        {
            System.IO.DirectoryInfo diInfo = new System.IO.DirectoryInfo(path);
            DateTime dtDate;

            int nJpgFileCnt = 0;
            int nNonJpgFileCnt = 0;
            // 현재 디렉토리의 총 파일 갯수 구하기
            foreach (System.IO.FileInfo File in diInfo.GetFiles())
            {
                if ((File.Extension.ToLower().CompareTo(".jpg") == 0))
                {
                    nJpgFileCnt++;
                    if (nMode == 1)
                    {
                        string strExposureDate = get_exif_info(File.FullName);

                        MyPhoto myPhoto = new MyPhoto();

                        //찍은날짜 없으면
                        if (string.IsNullOrEmpty(strExposureDate) == true)
                        { 
                            myPhoto.bExifDate = false;
                            myPhoto.filePath = File.FullName;
                            myPhoto.fileName = File.Name;
                            listPhoto.Add(myPhoto);
                        }
                        else
                        {
                            //Exif Date string -> DateTime 
                            CultureInfo provider = CultureInfo.InvariantCulture;
                            string strExposureDate2 = strExposureDate.Replace("-", ":");
                            string format = "yyyy:MM:dd HH:mm:ss";
                            dtDate = DateTime.ParseExact(strExposureDate2, format, provider);
                            string myConvertedDate = dtDate.ToString("yyyy-MM-dd");
                            int nCovertedDate = Convert.ToInt32(dtDate.ToString("yyyyMMdd"));
                            long  nCovertedDateFull = Convert.ToInt64(dtDate.ToString("yyyyMMddHHmmss"));
                           
                            myPhoto.nDate = nCovertedDate;
                            myPhoto.nDateFull = nCovertedDateFull;
                            myPhoto.fileName = File.Name;
                            myPhoto.bExifDate = true;
                            myPhoto.filePath = File.FullName;
                            myPhoto.folderName = myConvertedDate;
                            listPhoto.Add(myPhoto);
                        }
                    }
                }
                else
                {
                    nNonJpgFileCnt++;
                }

            }

            JpgFile.AddFileCnt(nJpgFileCnt);
            NonJpgFile.AddFileCnt(nNonJpgFileCnt);

            string strLog = string.Format("파일로딩중 : [{0}] / [{1}] ", JpgFile.GetFileCnt(), NonJpgFile.GetFileCnt());
            WriteLog(strLog);
            Delay(10);

            System.IO.DirectoryInfo[] dirs = diInfo.GetDirectories();                       // 현재 path경로의 디렉터리들을 검색.       
            if (dirs.Length > 0)
            {
                foreach (System.IO.DirectoryInfo item in diInfo.GetDirectories())
                    find_photo(item.FullName, nMode);
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

        void CreateFolder(string strFolderPath)
        {
            DirectoryInfo di = new DirectoryInfo(strFolderPath);

            if (di.Exists == false)
            {
                di.Create();
            }
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
                if(strExt == strParseExt)
                {
                    bRet = true;
                    break;
                }
            }
            return bRet;
        }


        string get_exif_info(string str_file)
        {
            string str_exposure_date = string.Empty;
            try
            {
                Image image = new Bitmap(str_file);
                PropertyItem[] propItems = image.PropertyItems;

                int nDateOrgTag = 0x9003;
                //int nMakerNameTag = 0x010F;

                int nFileLength = propItems.Length;
                str_exposure_date = string.Empty;

                foreach (PropertyItem propItem in propItems)
                {
                    if (propItem.Id == nDateOrgTag)  //EXIF 촬상시간이 있는 파일들만
                    {
                        System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                        //str_exposure_date = encoding.GetString(propItem.Value);
                        str_exposure_date = encoding.GetString(propItem.Value, 0, propItem.Len - 1);

                    }
                }

                image.Dispose();
            }
            catch(System.ArgumentException e)
            {
                string strExcepLog;
                strExcepLog = string.Format("예외처리:{0}", str_file);
                WriteLog(strExcepLog);
            }
            
            
            return str_exposure_date;
        }

        void WriteLog(string str_log_data)
        {
            textBox_log_1.AppendText(str_log_data);
            textBox_log_1.AppendText(System.Environment.NewLine);
            textBox_log_1.SelectionStart = textBox_log_1.TextLength + 1; // 시작할 위치 설정.
            textBox_log_1.ScrollToCaret(); // 캐럿 이동.
            textBox_log_1.Refresh();
        }
    }

    public class PhotoFile
    {
        int nFileCnt = 0;

        public int GetFileCnt()
        {
            return nFileCnt;
        }

        public void SetFileCnt(int nValue)
        {
            nFileCnt = nValue;
        }

        public void AddFileCnt(int nValue)
        {
            nFileCnt += nValue;
        }
    }
}
