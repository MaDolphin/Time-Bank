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
    public class ChatsController : Controller
    {
        private TimeProContext db = new TimeProContext();

        // GET: Chats
        public ActionResult ChatsIndex()
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


            var chat = db.Chat.Include(c => c.User).Include(c => c.User1);
            ViewModel model = new ViewModel();
            model.UserModel = user;
            model.ChatModel = chat.ToList();
            return View(model);
        }

        // GET: Chats/Details/5
        public ActionResult ChatsDetails(int? id)
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

            Chat chat = db.Chat.Find(id);
            if (chat == null)
            {
                return HttpNotFound();
            }

            Chat ChatModel = db.Chat.Find(id);
            ChatModel.ChatFlag = 1;
            db.Entry(ChatModel).State = EntityState.Modified;
            db.SaveChanges();

            ViewModel1 model = new ViewModel1();
            model.UserModel = user;
            model.ChatModel = chat;
            return View(model);
        }

        public ActionResult ChatsDetailsFrom(int? id)
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

            Chat chat = db.Chat.Find(id);
            if (chat == null)
            {
                return HttpNotFound();
            }

            ViewModel1 model = new ViewModel1();
            model.UserModel = user;
            model.ChatModel = chat;
            return View(model);
        }

        public ActionResult ChatsDetailsTo(int? id)
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

            Chat chat = db.Chat.Find(id);
            if (chat == null)
            {
                return HttpNotFound();
            }

            Chat ChatModel = db.Chat.Find(id);
            ChatModel.ChatFlag = 1;
            db.Entry(ChatModel).State = EntityState.Modified;
            db.SaveChanges();

            ViewModel1 model = new ViewModel1();
            model.UserModel = user;
            model.ChatModel = chat;
            return View(model);
        }

        public ActionResult ChatsCreate(ViewModel1 model)
        {
            model.ChatModel.ChatFrom = model.UserModel.UserID;
            model.ChatModel.ChatTo = (int)model.NoteModel.UserID;
            model.ChatModel.ChatFlag = 0;
            model.ChatModel.ChatTime = System.DateTime.Now;
            db.Chat.Add(model.ChatModel);
            db.SaveChanges();
            return Content("<script >alert('已成功提交！');history.go(-1)</script >", "text/html");
        }

        public ActionResult ChatsCreateChat(ViewModel1 model)
        {
            model.ChatModel.ChatTo = model.ChatModel.ChatFrom;
            model.ChatModel.ChatFrom = model.UserModel.UserID;
            model.ChatModel.ChatFlag = 0;
            model.ChatModel.ChatTime = System.DateTime.Now;
            db.Chat.Add(model.ChatModel);
            db.SaveChanges();
            return Content("<script >alert('已成功提交！');history.go(-1)</script >", "text/html");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
