using CoreDAL.Configuration.Interface;
using CoreDAL.ORM;
using CoreDAL.ORM.Extensions;
using CryptoManager;
using PagedList;
using SecuDev.Helper;
using SecuDev.Models;
using SingletonManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace SecuDev.Controllers
{
    public class BoardController : Controller
    {
        ICryptoManager crypto = Singletons.Instance.GetKeyedSingleton<ICryptoManager>(MvcApplication.AES256);
        IDatabaseSetup ConnDB = Singletons.Instance.GetKeyedSingleton<IDatabaseSetup>(MvcApplication.ConnDB);

        // GET: Board
        public ActionResult Index(int? Page, int PageSize = 10)
        {

            int PageNo = Page ?? 1;

            List<Board> list = new List<Board>();

            SQLResult result = ConnDB.DAL.ExecuteProcedure(ConnDB, "PROC_BOARD_LIST");

            DataSet ds = result.DataSet;

            foreach (DataRow b in ds.Tables[0].Rows)
            {
                Board tb = b.ToObject<Board>();
                Users tu = b.ToObject<Users>();
                Category tc = b.ToObject<Category>();

                tb.Users = tu;
                tb.Category = tc;

                list.Add(tb);
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
                            var fileDir = Path.Combine(dir + fileName);

                            int filecnt = 1;
                            string newFileName = string.Empty;
                            while (new FileInfo(fileFullPath).Exists)
                            {
                                var idx = formFile.FileName.LastIndexOf('.');
                                var tmp = formFile.FileName.Substring(0, idx);
                                newFileName = tmp + String.Format("({0})", filecnt++) + formFile.FileName.Substring(idx);
                                fileFullPath = uploadDir + newFileName;
                                fileDir = dir + newFileName;    // TODO: 여기에서 다시 파일 경로를 조합하고 있음
                            }

                            formFile.SaveAs(fileFullPath);

                            // 마지막 배열일경우 | 를 표시하지 않음
                            if (formFile.Equals(file.Last()))
                            {
                                dbFilePath = crypto.Encrypt(fileDir);   // TODO: += 연산자가 필요 없어 보임
                                altFileName = fileName;
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
        public ActionResult FileDelete(string uniqueFileId) {

            string sRtn = "Fail";

            uniqueFileId = $"{Server.MapPath("/")}/Upload/File/" + crypto.Decrypt(uniqueFileId);

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

            if (FilePath != null)
            {
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
            }

            Dictionary<string, object> param = new Dictionary<string, object>
            {
                { "CID", b.Category.CID },
                { "UID", Session["UID"] },
                { "Title", b.Title },
                { "Content", b.Content },
                { "FilePath", dbFilePath },
                { "FileName", FileName },
                { "IPAddress", Session["IPAddress"] }
            };

            SQLResult result = ConnDB.DAL.ExecuteProcedure(ConnDB, "PROC_BOARD_WRITE", param);

            Rtn = result.ReturnValue;

            return Rtn;

        }

    }
}