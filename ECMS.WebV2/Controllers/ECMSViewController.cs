using ECMS.Core;
using ECMS.Core.Entities;
using ECMS.Core.Framework;
using ECMS.Services;
using ECMS.WebV2.AppCode;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECMS.WebV2.Controllers
{
    [Authorize]
    public class ECMSViewController : CMSBaseController
    {
        //
        // GET: /ECMSView/
        private static ECMSViewRepository _viewRepository = new ECMSViewRepository();
        public ActionResult Index()
        {
            List<ECMSView> viewList = _viewRepository.GetAll(ECMSSettings.Current.SiteId);

            if (viewList != null)
            {
                var groupByList = DoGroupingAndSorting(viewList);
                ViewBag.Data = groupByList;
            }

            return View(GetControllerView("index"));
        }

        private Dictionary<string, IOrderedEnumerable<ECMSView>> DoGroupingAndSorting(List<ECMSView> viewList_)
        {
            var result = (from view in viewList_
             orderby view.LastModifiedOn descending
             group view by view.ViewName into grps
             select new
             {
                 Key = grps.Key,
                 Values = grps.OrderByDescending(x => x.ViewType).OrderByDescending(x => x.LastModifiedOn)
             }).ToDictionary(x => Convert.ToString(x.Key), y => y.Values);
            return result;
        }

        public ActionResult Archieved()
        {
            string viewName = this.Request.QueryString["viewName"].ToString();
            List<ECMSView> viewList = _viewRepository.GetAllArchieved(ECMSSettings.Current.SiteId, viewName).OrderByDescending(x => x.LastModifiedOn).ToList<ECMSView>();
            if (viewList != null)
            {
                var groupByList = DoGroupingAndSorting(viewList);
                ViewBag.Data = groupByList;
            }
            return View(GetControllerView("arc-index"));
        }
        public ActionResult ArcDetails(Guid id)
        {
            ECMSView view = new ECMSView();
            view.Id = id;
            view.SiteId = ECMSSettings.Current.SiteId;
            ViewData["sites"] = GetSiteList();
            ViewData["ViewTypes"] = GetViewTypeList();
            view = _viewRepository.GetArchieved(ECMSSettings.Current.SiteId, id);
            return View(GetControllerView("details"), view);
        }

        //
        // GET: /ECMSView/Details/5
        public ActionResult Details(Guid id)
        {
            ECMSView view = new ECMSView();
            view.Id = id;
            view.SiteId = ECMSSettings.Current.SiteId;
            ViewData["sites"] = GetSiteList();
            ViewData["ViewTypes"] = GetViewTypeList();
            view = _viewRepository.GetById(id);
            return View(GetControllerView("details"), view);
        }

        //
        // GET: /ECMSView/Create
        public ActionResult Save()
        {
            ViewData["sites"] = GetSiteList();
            ViewData["ViewTypes"] = GetViewTypeList();
            return View(GetControllerView("create"));
        }

        [HttpPost]        
        [ValidateInput(false)]
        [AcceptParameter(ButtonName = "Save")]
        public ActionResult Save(ECMSView view_)
        {
            try
            {
                view_.ViewType = ContentViewType.PREVIEW;
                SaveView(view_);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                LogEventInfo info = new LogEventInfo(LogLevel.Error, ECMSSettings.DEFAULT_LOGGER, ex.ToString());
                DependencyManager.Logger.Log(info);
                return View(GetControllerView("index"));
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        [ActionName("Save")]
        [AcceptParameter(ButtonName = "Publish")]
        public ActionResult Publish(ECMSView view_)
        {
            try
            {
                view_.ViewType = ContentViewType.PUBLISH;
                SaveView(view_);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                LogEventInfo info = new LogEventInfo(LogLevel.Error, ECMSSettings.DEFAULT_LOGGER, ex.ToString());
                DependencyManager.Logger.Log(info);
                return View(GetControllerView("index"));
            }
        }

        private void SaveView(ECMSView view_)
        {
            view_.SiteId = ECMSSettings.Current.SiteId;
            view_.LastModifiedBy = this.CMSUser.UserName;
            view_.LastModifiedOn = DateTime.Now;
            _viewRepository.Save(view_);
        }

        private SelectList GetViewTypeList()
        {
            var viewList = Enum.GetValues(typeof(ContentViewType)).Cast<ContentViewType>().Select(x => x.ToString());
            return new SelectList(viewList);
        }

        private SelectList GetSiteList()
        {
            return new SelectList(ECMSSettings.CMSSettingsList.Values, "SiteId", "PortalHostName");
        }

        

        //
        // GET: /ECMSView/Edit/5
        public ActionResult Edit(Guid id)
        {
            ECMSView view = new ECMSView();
            view.Id = id;
            view.SiteId = ECMSSettings.Current.SiteId;
            ViewData["sites"] = GetSiteList();
            ViewData["ViewTypes"] = GetViewTypeList();
            view = _viewRepository.GetById(id);
            return View(GetControllerView("edit"), view);
        }

        [HttpPost]
        [AcceptParameter(ButtonName = "Edit")]
        [ValidateInput(false)]
        public ActionResult Edit(Guid id, ECMSView view_)
        {
            //try
            //{
            view_.LastModifiedOn = DateTime.Now;
            view_.ViewType = ContentViewType.PREVIEW;
            view_.LastModifiedBy = this.CMSUser.UserName;
            _viewRepository.Update(view_);
            return RedirectToAction("Index");
            //}
            //catch
            //{
            // return View(GetControllerView("index"));
            //}
        }

        [HttpPost]
        [ActionName("Edit")]
        [AcceptParameter(ButtonName = "EditAndPublish")]
        [ValidateInput(false)]
        public ActionResult EditAndPublish(Guid id, ECMSView view_)
        {
            //try
            //{
            view_.LastModifiedOn = DateTime.Now;
            view_.LastModifiedBy = this.CMSUser.UserName;
            view_.ViewType = ContentViewType.PUBLISH;
            _viewRepository.Update(view_);
            return RedirectToAction("Index");
            //}
            //catch
            //{
            // return View(GetControllerView("index"));
            //}
        }

        //
        // GET: /ECMSView/Delete/5
        public ActionResult Delete(Guid id)
        {
            return View(GetControllerView("delete"));
        }

        //
        // POST: /ECMSView/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult EnterPreviewMode()
        {
            if (Request.Cookies["ECMS-View-Mode"] == null)
            {
                var c = new HttpCookie("ECMS-View-Mode", "Preview");
                c.Expires = DateTime.Now.AddDays(1);
                Response.SetCookie(c);
            }
            else
            {
                var c = Request.Cookies["ECMS-View-Mode"];
                c.Expires = DateTime.Now.AddDays(1);
                Response.Cookies.Add(c);
                Response.SetCookie(c);                
            }
            ViewBag.Message = "You entered in preview mode. Click below to exit any time.";
            ViewBag.ShowExitButton = true;

            return View(GetControllerView("EnterPreviewMode"));
        }
        [HttpGet]
        public ActionResult ExitPreviewMode()
        {
            if (Request.Cookies["ECMS-View-Mode"] != null)
            {
                var c = Request.Cookies["ECMS-View-Mode"];
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
                Response.Cookies.Remove("ECMS-View-Mode");
                ViewBag.Message = "You exit from preview mode!!!";                
            }
            ViewBag.ShowEntryButton = true;

            return View(GetControllerView("EnterPreviewMode"));
        }

        //[HttpGet]
        //public ActionResult DefaultDataAdd(Guid id)
        //{
        //    ViewBag.ECMSView = _viewRepository.Get(new ECMSView { Id = id });
        //    return View(GetControllerView("DefaultDataAdd"));
        //}

        //[HttpPost]
        //[ValidateInput(false)]
        public ActionResult DefaultDataAdd(Guid id, ContentItem item_)
        {
            ECMSView view = _viewRepository.GetById(id);
            DependencyManager.ContentRepository.Save(item_, view);
            return View(GetControllerView("DefaultDataAdd"));
        }

        [HttpGet]
        public ActionResult DefaultDataEdit(Guid id)
        {
            ECMSView ecmsView = _viewRepository.GetById(id);
            ViewBag.ViewName = ecmsView.ViewName;
            ViewBag.ViewType = ecmsView.ViewType;
            ContentItem item = DependencyManager.ContentRepository.GetContentForEditing(ecmsView);
            
            if (item != null)
            {
                return View(GetControllerView("DefaultDataEdit"), item);
            }
            else
            {
                return View(GetControllerView("DefaultDataEdit"));
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult DefaultDataEdit(Guid id, ContentItem item_)
        {
            item_.LastModifiedBy = this.CMSUser.UserName;
            item_.LastModifiedOn = DateTime.Now;
            DefaultDataAdd(id, item_);
            return Index();
        }

        [HttpGet]
        public ActionResult UrlDataEdit(Guid id, ContentViewType vm)
        {
            ValidUrl url = DependencyManager.URLRepository.GetById(this.GetSiteIdFromContext(), id, false);
            ViewBag.ViewName = url.FriendlyUrl;
            ViewBag.ViewType = vm;
            ContentItem item = null;
            try
            {
                item = DependencyManager.ContentRepository.GetContentForEditing(url, vm);
            }
            catch (FileNotFoundException) { }
            if (item != null)
            {
                return View(GetControllerView("DefaultDataEdit"), item);
            }
            else
            {
                return View(GetControllerView("DefaultDataEdit"));
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UrlDataEdit(Guid id, ContentItem item_, ContentViewType vm)
        {
            ValidUrl url = DependencyManager.URLRepository.GetById(this.GetSiteIdFromContext(), id, false);
            item_.Url = url;
            DependencyManager.ContentRepository.Save(item_, vm);
            return RedirectToAction("Index", "Urls");
        }
    }
}