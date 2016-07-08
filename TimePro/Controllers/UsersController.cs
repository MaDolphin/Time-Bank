using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimePro.Models;

namespace TimePro.Controllers
{
    public class UsersController : Controller
    {
        private TimeProContext db = new TimeProContext();

        // GET: Users
        [Filter.Filter.LoginFilter]
        public ActionResult UsersIndex()
        {           
            String usercode = Session["username"].ToString();
            int id = 0;
            foreach (var item in db.User.ToList<User>())
            {
                var Result = from a in db.User
                             where a.UserCode == usercode
                             select a;
                foreach (var result in Result)
                {
                    id = result.UserID;
                }
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            var note = (from b in db.Note.Include(n => n.User) where b.UserID != user.UserID && b.NoteFlag == 0 orderby b.NoteCreateTime descending select b).Take(3);
            int chat = (from b in db.Chat where b.ChatTo == user.UserID && b.ChatFlag == 0 select b).Count();
            ViewModel model = new ViewModel();
            model.UserModel = user;
            model.NoteModel = note.ToList();
            model.temp = chat;
            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "UserID,UserCode,UserName,UserPhoto,UserAge,UserSex,UserPwd,UserStatus,UserTel,UserEmail,UserAddress,UserHobby,UserRegisterTime,UserRole,ImageType,UserTime")] User user)
        {
            var result = from v in db.User
                         where v.UserCode == user.UserCode
                         select v;
            if (result.FirstOrDefault() == null)
            {
                if (ModelState.IsValid)
                {
                    user.UserTime = 10;
                    user.UserRole = 0;
                    user.UserRegisterTime = System.DateTime.Now;
                    db.User.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("../Users/Login");
                }
            }
            else
            {
                return Content("<script >alert('账号已存在！');history.go(-1)</script >", "text/html");
            }
            return View(user);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login([Bind(Include = "UserCode,UserPwd")] User user)
        {
            var result = from v in db.User
                         where v.UserCode == user.UserCode && v.UserPwd == user.UserPwd
                         select v;
            if (result.FirstOrDefault() != null)
            {
                if (Session["username"] != "" && Session["password"] != "")
                {
                    this.Session["username"] = user.UserCode;
                    this.Session["password"] = user.UserPwd;
                }
                Session.Timeout = 30;
                return RedirectToAction("../Users/UsersIndex");
            }
            else
            {
                return Content("<script >alert('账号或密码有误！');history.go(-1)</script >", "text/html");
            }
            return RedirectToAction("../Users/Login");
        }

        [HttpPost]
        public ActionResult LogOff()
        {
            Session["username"] = null;
            Session["password"] = null;
            return RedirectToAction("../Home/Index");
        }

        // GET: Users/Details/5
        [Filter.Filter.LoginFilter]
        public ActionResult UsersDetails()
        {
            String usercode = Session["username"].ToString();
            int id = 0;
            foreach (var item in db.User.ToList<User>())
            {
                var Result = from a in db.User
                             where a.UserCode == usercode
                             select a;
                foreach (var result in Result)
                {
                    id = result.UserID;
                }
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }


        // GET: Users/Edit/5
        [Filter.Filter.LoginFilter]
        public ActionResult UsersEdit()
        {
            String usercode = Session["username"].ToString();
            int id = 0;
            foreach (var item in db.User.ToList<User>())
            {
                var Result = from a in db.User
                             where a.UserCode == usercode
                             select a;
                foreach (var result in Result)
                {
                    id = result.UserID;
                }
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);

        }

        // POST: Users/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Filter.Filter.LoginFilter]
        public ActionResult UsersEdit(User user, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                User UserModel = db.User.Find(user.UserID);
                UserModel.UserCode = user.UserCode;
                UserModel.UserName = user.UserName;
                UserModel.UserAge = user.UserAge;
                UserModel.UserSex = user.UserSex;
                UserModel.UserPwd = user.UserPwd;
                UserModel.UserStatus = user.UserStatus;
                UserModel.UserTel = user.UserTel;
                UserModel.UserEmail = user.UserEmail;
                UserModel.UserAddress = user.UserAddress;
                UserModel.UserHobby = user.UserHobby;

                if (image != null)
                {
                    user.ImageType = image.ContentType;//获取图片类型
                    user.UserPhoto = new byte[image.ContentLength];//新建一个长度等于图片大小的二进制地址
                    image.InputStream.Read(user.UserPhoto, 0, image.ContentLength);//将image读取到ImageData中
                    UserModel.UserPhoto = user.UserPhoto;
                    UserModel.ImageType = user.ImageType;
                }

                db.Entry(UserModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("UsersIndex");
            }
            return View(user);
        }


        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.User.Find(id);
            db.User.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public FileContentResult GetImage(int userid)
        {
            String usercode = Session["username"].ToString();
            int id = 0;
            foreach (var item in db.User.ToList<User>())
            {
                var Result = from a in db.User
                             where a.UserCode == usercode
                             select a;
                foreach (var result in Result)
                {
                    id = result.UserID;
                }
            }
            User prod = db.User.FirstOrDefault(p => p.UserID == id);
            if (prod != null)
            {
                return File(prod.UserPhoto, prod.ImageType);//File方法直接将二进制转化为指定类型了。
            }
            else {
                return null;
            }
        }
    }
}
