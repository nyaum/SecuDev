using FrameWork.DB;
using PagedList;
using SecuDev.Helper;
using SecuDev.Models;
using SecuDEV.Manager;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAdmin.Models;

namespace SecuDev.Controllers
{
    public class BoardController : Controller
    {
        // GET: Board
        public ActionResult Index(int? Page, int PageSize = 10)
        {

            int PageNo = Page ?? 1;

            SqlParamCollection param = new SqlParamCollection();

            DataSet ds = (new Common().MdlList(param, "PROC_BOARD_LIST"));

            List<Board> list = new List<Board>();

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Board b = new Board();

                b.BID = Int32.Parse(ds.Tables[0].Rows[i]["BID"].ToString());
                b.Category.CID = Int32.Parse(ds.Tables[0].Rows[i]["CID"].ToString());
                b.Category.CategoryName = ds.Tables[0].Rows[i]["CategoryName"].ToString();
                b.Category.BackgroundColor = ds.Tables[0].Rows[i]["BackgroundColor"].ToString();
                b.Category.FontColor = ds.Tables[0].Rows[i]["FontColor"].ToString();
                b.Users.UID = ds.Tables[0].Rows[i]["UID"].ToString();
                b.Users.UserName = ds.Tables[0].Rows[i]["UserName"].ToString();
                b.Title = ds.Tables[0].Rows[i]["Title"].ToString();

                list.Add(b);
            }

            ViewBag.list = list;

            return View(list.ToPagedList(PageNo, PageSize));
        }

        public ActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FileUpload(IEnumerable<HttpPostedFileBase> file)
        {

            string Rtn = "FAIL";

            string today = Utility.DateTimeFormat(Utility.GetNowDate(), 8);
            string uploadDir = $"{Server.MapPath("/")}/Upload/File/" + today + "/";
            string dir = "/" + today + "/";

            string dbFilePath = "";
            string altFileName = "";

            // 경로 확인
            try
            {
                if (file != null)
                {
                    // 경로 확인
                    DirectoryInfo di = new DirectoryInfo(uploadDir);

                    if (di.Exists == false)
                    {
                        di.Create();
                    }

                    foreach (var formFile in file)
                    {
                        if (formFile.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(formFile.FileName);
                            var fileFullPath = Path.Combine(uploadDir + fileName);
                            var fileDir = CryptoManager.AESEncrypt256(Path.Combine(dir + fileName));

                            int filecnt = 1;
                            string newFileName = string.Empty;
                            while (new FileInfo(fileFullPath).Exists)
                            {
                                var idx = formFile.FileName.LastIndexOf('.');
                                var tmp = formFile.FileName.Substring(0, idx);
                                newFileName = tmp + String.Format("({0})", filecnt++) + formFile.FileName.Substring(idx);
                                fileFullPath = uploadDir + newFileName;
                                fileDir = CryptoManager.AESEncrypt256(dir + newFileName);
                            }

                            formFile.SaveAs(fileFullPath);

                            // 마지막 배열일경우 | 를 표시하지 않음
                            if (formFile.Equals(file.Last()))
                            {
                                dbFilePath += fileDir;
                                altFileName += fileName;
                            }
                            else
                            {
                                dbFilePath += fileDir + "|";
                                altFileName += fileName + "|";
                            }
                        }
                    }
                }

                Rtn = "OK";

            }
            catch (Exception ex)
            {

            }

            return Json(new { uniqueFileId = dbFilePath, FileName = altFileName });
        }

        [HttpPost]
        public ActionResult FileDelete(string uniqueFileId)
        {

            string sRtn = "Fail";

            uniqueFileId = $"{Server.MapPath("/")}/Upload/File/" + CryptoManager.AESDecrypt256(uniqueFileId);

            if (System.IO.File.Exists(uniqueFileId))
            {

                System.IO.File.Delete(uniqueFileId);

            }

            return Json(new { });
        }

        [HttpPost]
        [ValidateInput(false)]
        public int Write(Board b, string[] FilePath)
        {

            int Rtn = -1;

            string dbFilePath = "";
            string FileName = "";

            for (int i = 0; i < FilePath.Length; i++)
            {
                if (i == 0)
                {
                    dbFilePath = FilePath[i].Split(',')[0];
                    FileName = FilePath[i].Split(',')[1];
                }
                else
                {
                    dbFilePath += "|" + FilePath[i].Split(',')[0];
                    FileName += "|" + FilePath[i].Split(',')[1];
                }
            }

            SqlParamCollection param = new SqlParamCollection();

            param.Add("@CID", b.Category.CID);
            param.Add("@UID", Session["UID"]);
            param.Add("@IPAddress", Session["IPAddress"]);
            param.Add("@Title", b.Title);
            param.Add("@Content", b.Content);
            param.Add("@FilePath", dbFilePath);
            param.Add("@FileName", FileName);

            Rtn = (new Common()).MdlRegIntRtn(param, "PROC_BOARD_WRITE");

            return Rtn;
        }

    }
}