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
        public DateTime dtCreatedDate;
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
        string m_strTbDstPath = string.Empty;

        System.Windows.Forms.Timer myTimer = new System.Windows.Forms.Timer();

        int m_nCntAll = 0;
        int m_nCnt = 0;
        bool m_bClean = false;

        string strSrcPath = string.Empty;
        string strDestPath = string.Empty;
        string strDefaultFolder = "C:\\";
        string[] strListUseExt = { "MP4", "MOV" };


        List<MyPhoto> listPhoto = new List<MyPhoto>();

        public Form1()
        {
            InitializeComponent();

            myTimer.Interval = 100;
            myTimer.Tick += new EventHandler(timer_tick);
            myTimer.Start();

        }
        public void DrawInfo()
        {
            string strLog1 = string.Empty;
            string strLog2 = string.Empty;

            if (m_bClean)
            {
                strLog1 = string.Format("{0}", m_nCnt);
                strLog2 = string.Format("{0}", m_nCntAll);
            }
            else
            {
                strLog1 = string.Format("{0}", listPhoto.Count);
                strLog2 = string.Format("{0}", m_nCntAll);
            }

            tb_cnt1.Text = strLog1;
            tb_cnt2.Text = strLog2;

            if (m_bClean == true)
            {
                btn_clean.Enabled = false;
            }
            else if (listPhoto.Count >= m_nCntAll && m_nCntAll != 0)
            {
                btn_clean.Enabled = true;
            }
            else
            {
                btn_clean.Enabled = false;
            }

        }



        public void timer_tick(object sender, System.EventArgs e)
        {
            DrawInfo();

            if (m_queueLogMsg.Count() > 0)
            {
                string strMsg = string.Empty;
                for (int i = 0; i < m_queueLogMsg.Count(); i++)
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

            if (m_nCnt != 0 && m_nCntAll != 0)
            {
                int nPercent = Convert.ToInt32((((float)m_nCnt / (float)m_nCntAll) * 100.0));
                SetProgressBar(nPercent);
            }
        }

        private void btn_sel_src_folder_Click(object sender, EventArgs e)
        {
            strSrcPath = string.Empty;
            strSrcPath = open_folder_dialog(strDefaultFolder);
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
            listPhoto.Clear();
            m_bClean = false;
            
            Thread thread = new Thread(new ThreadStart(delegate () // thread 생성
            {
                m_strTbSrcPath = textBox_src_path.Text;
                ReadFolder(m_strTbSrcPath);
                this.Invoke(new Action(delegate ()
                {

                }));
            }));

            thread.Start();

        }

        private void btn_clean_Click(object sender, EventArgs e)
        {
           

            //path 설정 판단
            if (ValidPath() == false)
            {
                WriteLog("경로설정을 확인해 주세요");
            }
            else
            {
                Thread thread = new Thread(new ThreadStart(delegate () // thread 생성
                {
                    m_strTbSrcPath = textBox_src_path.Text;
                    m_strTbDstPath = textBox_dest_path.Text;
                    //func
                    Clean_Photo();
                    this.Invoke(new Action(delegate ()
                    {

                    }));
                }));

                thread.Start();

            }
        }

        public void Clean_Photo()
        {
            m_bClean = true;

            string strFileCnt = string.Empty;

            if (string.IsNullOrEmpty(textBox_src_path.Text) == false && string.IsNullOrEmpty(textBox_dest_path.Text) == false)
            {
                strFileCnt = string.Format("{0}", m_nCntAll);
                if (MessageBox.Show(strFileCnt + "개의 파일을 정리 실행하시겠습니까?", "Yes Or No", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    m_queueLogMsg.Enqueue("정리 스레드 시작.....");

                    m_nCnt = 0;

                    //정렬
                    listPhoto.Sort(delegate (MyPhoto A, MyPhoto B)
                    {
                        if (A.nDateFull > B.nDateFull) return 1;
                        else if (A.nDateFull < B.nDateFull) return -1;
                        return 0;
                    });

                    int nCntNone = 0;
                    int nCnt = 0;
                    string strOldFolderName = string.Empty;
                    int nCleanedCnt = 0;

                    for (int nIndex = 0; nIndex < listPhoto.Count; nIndex++)
                    {
                        int nDate = listPhoto[nIndex].nDate;
                        bool bHaveExif = listPhoto[nIndex].bExifDate;
                        string strDate = Convert.ToString(nDate);
                        string strDestFolder;
                        string strDestFileName;

                        if (bHaveExif)
                        {
                            //해당 폴더에서 최초인지 체크
                            string strCurrentForderName = string.Empty;
                            string strCurrentFileName = string.Empty;
                            strCurrentForderName = listPhoto[nIndex].folderName;
                            //파일이름:yyyymmsshhmmss
                            DateTime dtCrDate = listPhoto[nIndex].dtCreatedDate;
                            string strExt = listPhoto[nIndex].fileExt;
                            strCurrentFileName = dtCrDate.ToString("yyyyMMdd_hhmmss");

                            if (string.IsNullOrEmpty(strOldFolderName) || strOldFolderName != strCurrentForderName)
                            {
                                strOldFolderName = strCurrentForderName;
                                nCnt = 0;
                            }
                            strOldFolderName = listPhoto[nIndex].folderName;
                            strDestFolder = string.Format("{0}\\{1}", textBox_dest_path.Text, strCurrentForderName);
                            CreateFolder(strDestFolder);
                            strDestFileName = string.Format("{0}\\{1}\\{2}_{3}{4}", textBox_dest_path.Text, strCurrentForderName, strCurrentFileName, nCnt, strExt);
                            FileRename(textBox_dest_path.Text, listPhoto[nIndex].filePath, strDestFileName);
                            nCnt++;

                        }
                        else
                        {
                            strDestFolder = string.Format("{0}\\{1}", textBox_dest_path.Text, "None");
                            CreateFolder(strDestFolder);
                            strDestFileName = string.Format("{0}\\{1}\\{2}_{3}.jpg", textBox_dest_path.Text, "None", "None", nCntNone);
                            FileRename(textBox_dest_path.Text, listPhoto[nIndex].filePath, strDestFileName);
                            nCntNone++;
                        }
                        //Thread.Sleep(1000);
                        nCleanedCnt++;
                        m_nCnt = nCleanedCnt;
                    }

                    btn_clean.Enabled = false;
                    m_queueLogMsg.Enqueue("정리 완료.....");

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

            m_bClean = true;
        }


        private static DateTime Delay(int ms)
        {
            DateTime ThisMoment = DateTime.Now;
            TimeSpan duration = new TimeSpan(0, 0, 0, 0, ms);
            DateTime AfterWatds = ThisMoment.Add(duration);
            while (AfterWatds >= ThisMoment)
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
            if (nPercent > progressBar_clean.Maximum)
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
            if (System.IO.File.Exists(oldFile))
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        private bool ValidPath()
        {
            string strSrc = textBox_src_path.Text;
            string strDst = textBox_dest_path.Text;

            bool bRet = false;

            if (string.IsNullOrEmpty(strSrc) == false && string.IsNullOrEmpty(strDst) == false)
            {
                bRet = true;
            }

            return bRet;

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
                m_queueLogMsg.Enqueue("검색 스레드 시작.....");

                DirectoryInfo di = new DirectoryInfo(strFolderPath);
                int nCnt = 0;

                if (di.Exists)
                {
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

                    m_queueLogMsg.Enqueue("검색 완료.....");
                }

                else
                {
                    m_queueLogMsg.Enqueue(string.Format("{0} 해당 경로가 없습니다.....", strFolderPath));
                }

            }
        }

        private void open_file_dialog(string str_default_path)
        {
            openFileDialog1.InitialDirectory = str_default_path;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string file_path = openFileDialog1.FileName;
                
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
                    myPhoto.bExifDate = true;

                }
                else
                {
                    myPhoto.bExifDate = false;
                }

                DateTime dtCreatedDate = myExifData.dtDate;
                string myConvertedDate = dtCreatedDate.ToString("yyyy-MM-dd");
                int nCovertedDate = Convert.ToInt32(dtCreatedDate.ToString("yyyyMMdd"));
                long nCovertedDateFull = Convert.ToInt64(dtCreatedDate.ToString("yyyyMMddHHmmss"));

                myPhoto.dtCreatedDate = dtCreatedDate;
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
                if (strExt == strParseExt)
                {
                    bRet = true;
                    break;
                }
            }
            return bRet;
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
}
