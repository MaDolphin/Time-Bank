using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimePro.Models;

namespace TimePro.Controllers
{
    public class NotesController : Controller
    {
        private TimeProContext db = new TimeProContext();

        // GET: Notes
        [Filter.Filter.LoginFilter]
        public ActionResult NotesIndex()
        {
            String usercode = Session["username"].ToString();
            int nid = 0;
            foreach (var item in db.User.ToList<User>())
            {
                var Result = from a in db.User
                             where a.UserCode == usercode
                             select a;
                foreach (var result in Result)
                {
                    nid = result.UserID;
                }
            }
            User user = db.User.Find(nid);
            var note = from b in db.Note.Include(n => n.User) where b.UserID == user.UserID select b;
            ViewModel model = new ViewModel();
            model.UserModel = user;
            model.NoteModel = note.ToList();
            return View(model);
        }

        [Filter.Filter.LoginFilter]
        public ActionResult NotesAll()
        {
            String usercode = Session["username"].ToString();
            int nid = 0;
            foreach (var item in db.User.ToList<User>())
            {
                var Result = from a in db.User
                             where a.UserCode == usercode
                             select a;
                foreach (var result in Result)
                {
                    nid = result.UserID;
                }
            }
            User user = db.User.Find(nid);
            var note = from b in db.Note.Include(n => n.User) where b.UserID != user.UserID && b.NoteFlag == 0 orderby b.NoteCreateTime descending select b;
            ViewModel model = new ViewModel();
            model.UserModel = user;
            model.NoteModel = note.ToList();
            return View(model);
        }

        // GET: Notes/Details/5
        [Filter.Filter.LoginFilter]
        public ActionResult NotesDetails(int? id)
        {
            String usercode = Session["username"].ToString();
            int nid = 0;
            foreach (var item in db.User.ToList<User>())
            {
                var Result = from a in db.User
                             where a.UserCode == usercode
                             select a;
                foreach (var result in Result)
                {
                    nid = result.UserID;
                }
            }
            User user = db.User.Find(nid);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = db.Note.Find(id);
            if (note == null)
            {
                return HttpNotFound();
            }
            ViewModel1 model = new ViewModel1();
            model.UserModel = user;
            model.NoteModel = note;
            return View(model);
        }


        // GET: Notes/Create
        [Filter.Filter.LoginFilter]
        public ActionResult NotesCreate()
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
            if(user.UserTime ==0)
            {
                return Content("<script >alert('账号时间不足，不能创建项目！');history.go(-1)</script >", "text/html");
            }
            else
            {
                Note note = new Note();
                ViewModel1 model = new ViewModel1();
                model.UserModel = user;
                model.NoteModel = note;
                return View(model);
            }
            
        }

        // POST: Notes/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [Filter.Filter.LoginFilter]
        public ActionResult NotesCreate(ViewModel1 note)
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
            if(note.NoteModel.NoteTime > user.UserTime)
            {
                return Content("<script >alert('账号时间小于所需要的时间，不能创建项目！');history.go(-1)</script >", "text/html");
            }
            else
            {
                note.NoteModel.NoteFlag = 0;
                note.NoteModel.UserID = user.UserID;
                note.NoteModel.NoteCreateTime = System.DateTime.Now;
                if (ModelState.IsValid)
                {
                    db.Note.Add(note.NoteModel);
                    db.SaveChanges();
                    return RedirectToAction("../Notes/NotesIndex");
                }
            }
            

            ViewBag.UserID = new SelectList(db.User, "UserID", "UserCode", note.NoteModel.UserID);
            ViewModel1 model = new ViewModel1();
            model.UserModel = user;
            model.NoteModel = note.NoteModel;
            return View(model);
        }

        // GET: Notes/Edit/5
        [ValidateInput(false)]
        [Filter.Filter.LoginFilter]
        public ActionResult NotesEdit(int? id)
        {
            String usercode = Session["username"].ToString();
            int nid = 0;
            foreach (var item in db.User.ToList<User>())
            {
                var Result = from a in db.User
                             where a.UserCode == usercode
                             select a;
                foreach (var result in Result)
                {
                    nid = result.UserID;
                }
            }
            User user = db.User.Find(nid);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = db.Note.Find(id);
            if (note == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.User, "UserID", "UserCode", note.UserID);
            ViewModel1 model = new ViewModel1();
            model.UserModel = user;
            model.NoteModel = note;
            return View(model);
        }

        // POST: Notes/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [Filter.Filter.LoginFilter]
        public ActionResult NotesEdit(ViewModel1 note)
        {
            String usercode = Session["username"].ToString();
            int nid = 0;
            foreach (var item in db.User.ToList<User>())
            {
                var Result = from a in db.User
                             where a.UserCode == usercode
                             select a;
                foreach (var result in Result)
                {
                    nid = result.UserID;
                }
            }
            User user = db.User.Find(nid);

            Note NoteModel = db.Note.Find(note.NoteModel.NoteID);
            NoteModel.NoteTitle = note.NoteModel.NoteTitle;
            NoteModel.NoteLabel = note.NoteModel.NoteLabel;
            NoteModel.NoteTime = note.NoteModel.NoteTime;
            NoteModel.NoteContent = note.NoteModel.NoteContent;

            if (ModelState.IsValid)
            {
                db.Entry(NoteModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("../Notes/NotesIndex");
            }
            ViewBag.UserID = new SelectList(db.User, "UserID", "UserCode", note.UserModel.UserID);
            return View(note.NoteModel);
        }

        // GET: Notes/Delete/5
        [Filter.Filter.LoginFilter]
        public ActionResult NotesDelete(int? id)
        {
            String usercode = Session["username"].ToString();
            int nid = 0;
            foreach (var item in db.User.ToList<User>())
            {
                var Result = from a in db.User
                             where a.UserCode == usercode
                             select a;
                foreach (var result in Result)
                {
                    nid = result.UserID;
                }
            }
            User user = db.User.Find(nid);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = db.Note.Find(id);
            if (note == null)
            {
                return HttpNotFound();
            }

            ViewBag.UserID = new SelectList(db.User, "UserID", "UserCode", note.UserID);
            ViewModel1 model = new ViewModel1();
            model.UserModel = user;
            model.NoteModel = note;
            return View(model);
        }

       // POST: Notes/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Filter.Filter.LoginFilter]
        public ActionResult DeleteConfirmed(ViewModel1 model)
        {
            int id = model.NoteModel.NoteID;
            Note note = db.Note.Find(id);
            db.Note.Remove(note);
            db.SaveChanges();
            return RedirectToAction("../Notes/NotesIndex");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [Filter.Filter.LoginFilter]
        public ActionResult NotesFind(string search)
        {
            
            var query = db.Note.Where(m => m.NoteTitle.Contains(search) ||  m.NoteLabel.Contains(search) || m.NoteContent.Contains(search));

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

            ViewModel model = new ViewModel();
            model.NoteModel = query.ToList();
            model.UserModel = user;
            return View(model);
        }

        [Filter.Filter.LoginFilter]
        public ActionResult NotesFindDetails(int? id)
        {
            String usercode = Session["username"].ToString();
            int nid = 0;
            foreach (var item in db.User.ToList<User>())
            {
                var Result = from a in db.User
                             where a.UserCode == usercode
                             select a;
                foreach (var result in Result)
                {
                    nid = result.UserID;
                }
            }
            User user = db.User.Find(nid);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = db.Note.Find(id);
            if (note == null)
            {
                return HttpNotFound();
            }
            ViewModel1 model = new ViewModel1();
            model.UserModel = user;
            model.NoteModel = note;
            return View(model);
        }

        [Filter.Filter.LoginFilter]
        public ActionResult NotesAllDetails(int? id)
        {
            String usercode = Session["username"].ToString();
            int nid = 0;
            foreach (var item in db.User.ToList<User>())
            {
                var Result = from a in db.User
                             where a.UserCode == usercode
                             select a;
                foreach (var result in Result)
                {
                    nid = result.UserID;
                }
            }
            User user = db.User.Find(nid);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = db.Note.Find(id);
            if (note == null)
            {
                return HttpNotFound();
            }
            ViewModel1 model = new ViewModel1();
            model.UserModel = user;
            model.NoteModel = note;
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [Filter.Filter.LoginFilter]
        public ActionResult NotesAllDetails(ViewModel1 note){ 
            String usercode = Session["username"].ToString();
            int nid = 0;
            foreach (var item in db.User.ToList<User>())
            {
                var Result = from a in db.User
                             where a.UserCode == usercode
                             select a;
                foreach (var result in Result)
                {
                    nid = result.UserID;
                }
            }
            User user = db.User.Find(nid);

            Note NoteModel = db.Note.Find(note.NoteModel.NoteID);

            if (note.NoteModel.UserID == user.UserID)
            {
                return Content("<script >alert('不能选择自己的项目！');history.go(-1)</script >", "text/html");
            }
            else
            {
                NoteModel.NoteGetNumber = user.UserID;
                NoteModel.NoteFlag = 1;
                if (ModelState.IsValid)
                {
                    db.Entry(NoteModel).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("../Notes/NotesIng");
                }
            ViewBag.UserID = new SelectList(db.User, "UserID", "UserCode", note.UserModel.UserID);
            
            }
            return View(note);

        }


        [Filter.Filter.LoginFilter]
        public ActionResult NotesIng()
        {
            String usercode = Session["username"].ToString();
            int nid = 0;
            foreach (var item in db.User.ToList<User>())
            {
                var Result = from a in db.User
                             where a.UserCode == usercode
                             select a;
                foreach (var result in Result)
                {
                    nid = result.UserID;
                }
            }
            User user = db.User.Find(nid);

            var note = from b in db.Note.Include(n => n.User) where  b.NoteFlag == 1 || b.NoteFlag == 2 where b.NoteGetNumber == user.UserID orderby b.NoteCreateTime descending select b;
            ViewModel model = new ViewModel();
            model.UserModel = user;
            model.NoteModel = note.ToList();
            return View(model);
        }

        [Filter.Filter.LoginFilter]
        public ActionResult NotesIngDetails(int? id)
        {
            String usercode = Session["username"].ToString();
            int nid = 0;
            foreach (var item in db.User.ToList<User>())
            {
                var Result = from a in db.User
                             where a.UserCode == usercode
                             select a;
                foreach (var result in Result)
                {
                    nid = result.UserID;
                }
            }
            User user = db.User.Find(nid);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = db.Note.Find(id);
            if (note == null)
            {
                return HttpNotFound();
            }
            ViewModel1 model = new ViewModel1();
            model.UserModel = user;
            model.NoteModel = note;
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [Filter.Filter.LoginFilter]
        public ActionResult NotesIngDetails(ViewModel1 note)
        {
            String usercode = Session["username"].ToString();
            int nid = 0;
            foreach (var item in db.User.ToList<User>())
            {
                var Result = from a in db.User
                             where a.UserCode == usercode
                             select a;
                foreach (var result in Result)
                {
                    nid = result.UserID;
                }
            }
            User user = db.User.Find(nid);

            Note NoteModel = db.Note.Find(note.NoteModel.NoteID);
            NoteModel.NoteFlag = 1;
            if (ModelState.IsValid)
            {
                db.Entry(NoteModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("../Notes/NotesIndex");
            }
            ViewBag.UserID = new SelectList(db.User, "UserID", "UserCode", note.UserModel.UserID);
            return View(note);

        }

        [Filter.Filter.LoginFilter]
        public ActionResult NotesFinish()
        {
            String usercode = Session["username"].ToString();
            int nid = 0;
            foreach (var item in db.User.ToList<User>())
            {
                var Result = from a in db.User
                             where a.UserCode == usercode
                             select a;
                foreach (var result in Result)
                {
                    nid = result.UserID;
                }
            }
            User user = db.User.Find(nid);

            var note = from b in db.Note.Include(n => n.User) where b.NoteGetNumber == user.UserID && b.NoteFlag == 3 orderby b.NoteCreateTime descending select b;
            ViewModel model = new ViewModel();
            model.UserModel = user;
            model.NoteModel = note.ToList();
            return View(model);
        }

        [Filter.Filter.LoginFilter]
        public ActionResult NotesFinishDetails(int? id)
        {
            String usercode = Session["username"].ToString();
            int nid = 0;
            foreach (var item in db.User.ToList<User>())
            {
                var Result = from a in db.User
                             where a.UserCode == usercode
                             select a;
                foreach (var result in Result)
                {
                    nid = result.UserID;
                }
            }
            User user = db.User.Find(nid);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = db.Note.Find(id);
            if (note == null)
            {
                return HttpNotFound();
            }
            ViewModel1 model = new ViewModel1();
            model.UserModel = user;
            model.NoteModel = note;
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [Filter.Filter.LoginFilter]
        public ActionResult NotesFinishDetails(ViewModel1 note)
        {
            String usercode = Session["username"].ToString();
            int nid = 0;
            foreach (var item in db.User.ToList<User>())
            {
                var Result = from a in db.User
                             where a.UserCode == usercode
                             select a;
                foreach (var result in Result)
                {
                    nid = result.UserID;
                }
            }
            User user = db.User.Find(nid);

            Note NoteModel = db.Note.Find(note.NoteModel.NoteID);
            NoteModel.NoteFlag = 2;
            if (ModelState.IsValid)
            {
                db.Entry(NoteModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("../Notes/NotesIng");
            }
            ViewBag.UserID = new SelectList(db.User, "UserID", "UserCode", note.UserModel.UserID);
            return View(note);

        }


        [Filter.Filter.LoginFilter]
        public ActionResult NotesConfigDetails(int? id)
        {
            String usercode = Session["username"].ToString();
            int nid = 0;
            foreach (var item in db.User.ToList<User>())
            {
                var Result = from a in db.User
                             where a.UserCode == usercode
                             select a;
                foreach (var result in Result)
                {
                    nid = result.UserID;
                }
            }
            User user = db.User.Find(nid);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = db.Note.Find(id);
            if (note == null)
            {
                return HttpNotFound();
            }
            ViewModel1 model = new ViewModel1();
            model.UserModel = user;
            model.NoteModel = note;
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [Filter.Filter.LoginFilter]
        public ActionResult NotesConfigDetails(ViewModel1 note)
        {
            String usercode = Session["username"].ToString();
            int nid = 0;
            foreach (var item in db.User.ToList<User>())
            {
                var Result = from a in db.User
                             where a.UserCode == usercode
                             select a;
                foreach (var result in Result)
                {
                    nid = result.UserID;
                }
            }
            User user = db.User.Find(nid);

            Note NoteModel = db.Note.Find(note.NoteModel.NoteID);
            User UserModelm = db.User.Find(user.UserID);
            User UserModeli = db.User.Find(NoteModel.UserID);
            NoteModel.NoteFlag = 3;
            NoteModel.NoteFinishTime = System.DateTime.Now;
            UserModelm.UserTime = UserModelm.UserTime + note.NoteModel.NoteTime;
            UserModeli.UserTime = UserModeli.UserTime - note.NoteModel.NoteTime;
            if (ModelState.IsValid)
            {
                db.Entry(NoteModel).State = EntityState.Modified;
                db.SaveChanges();
                db.Entry(UserModelm).State = EntityState.Modified;
                db.SaveChanges();
                db.Entry(UserModeli).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("../Notes/NotesIndex");
            }
            ViewBag.UserID = new SelectList(db.User, "UserID", "UserCode", note.UserModel.UserID);
            return View(note);

        }

        [Filter.Filter.LoginFilter]
        public ActionResult NotesConfigDetails2(int? id)
        {
            String usercode = Session["username"].ToString();
            int nid = 0;
            foreach (var item in db.User.ToList<User>())
            {
                var Result = from a in db.User
                             where a.UserCode == usercode
                             select a;
                foreach (var result in Result)
                {
                    nid = result.UserID;
                }
            }
            User user = db.User.Find(nid);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = db.Note.Find(id);
            if (note == null)
            {
                return HttpNotFound();
            }
            ViewModel1 model = new ViewModel1();
            model.UserModel = user;
            model.NoteModel = note;
            return View(model);
        }

        [HttpPost, ActionName("UploadImage")]
        public ActionResult UploadImage(HttpPostedFileBase image)
        {
            if (image != null && image.ContentLength > 0)
            {
                string fileName = DateTime.Now.ToString("yyyyMMdd") + "-" + Path.GetFileName(image.FileName);
                string filePath = Path.Combine(Server.MapPath("~/Photo"), fileName);
                image.SaveAs(filePath);
                var imageUrl = "~/Photo/" + fileName;
                return Json(imageUrl);
             }
            return View();
        }

    }

}
