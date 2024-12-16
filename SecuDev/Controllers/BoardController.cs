using FrameWork.DB;
using SecuDev.Helper;
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
        public ActionResult Index()
        {
            return View();
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
                                fileDir = dir + newFileName;
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

            return Json(new { dbFilePath, altFileName });
        }

        [HttpPost]
        public ActionResult FileDelete(string dbFilePath)
        {

            string sRtn = "Fail";

            dbFilePath = crypto.Decrypt(dbFilePath);

            if (System.IO.File.Exists($"{Server.MapPath("/")}/Upload/File/" + dbFilePath))
            {

                System.IO.File.Delete($"{Server.MapPath("/")}/Upload/File/" + dbFilePath);

            }

            return Json(new { });
        }

    }
}