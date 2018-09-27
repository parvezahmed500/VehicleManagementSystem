﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using RMS.App.ViewModels;
using RMS.BLL.Contracts;
using RMS.Models.DatabaseContext;
using RMS.Models.EntityModels;

namespace RMS.App.Controllers
{
    [Authorize(Roles = "Controller,Administrator")]
    public class MailServicesController : Controller
    {
        private IMailServiceManager _mailServiceManager;
        private IRequisitionManager _requisitionManager;
        private IAssignRequisitionManager _assignRequisitionManager;

        public MailServicesController(IMailServiceManager mailServiceManager,IRequisitionManager requisitionManager,IAssignRequisitionManager assignRequisitionManager)
        {
            this._mailServiceManager = mailServiceManager;
            this._requisitionManager = requisitionManager;
            this._assignRequisitionManager = assignRequisitionManager;
        }

        // GET: MailServices
        public ActionResult Index()
        {
            ICollection<MailService> mailServices = _mailServiceManager.GetAll();
            IEnumerable<MailServiceViewModel> mailServiceViewModels =
                Mapper.Map<IEnumerable<MailServiceViewModel>>(mailServices);
            return View(mailServiceViewModels);
        }

        // GET: MailServices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MailService mailService = _mailServiceManager.FindById((int)id);
            if (mailService == null)
            {
                return HttpNotFound();
            }
            MailServiceViewModel mailServiceViewModel = Mapper.Map<MailServiceViewModel>(mailService);
            return View(mailServiceViewModel);
        }

        // GET: MailServices/Create
        public ActionResult Create()
        {
            //ViewBag.RequisitionId = new SelectList(_requisitionManager.GetAllWithEmployee(), "Id", "RequisitionNumber");
            return View();
        }

        // POST: MailServices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,To,Subject,Body")] MailServiceViewModel mailServiceViewModel)
        {
            if (ModelState.IsValid)
            {
                MailService mailService = Mapper.Map<MailService>(mailServiceViewModel);
                mailService.MailSendingDateTime=DateTime.Now;
                mailService.From = "demowork9999@gmail.com";
                var result = _mailServiceManager.Add(mailService);
                if (result)
                {
                    try
                    {
                        SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                        smtpClient.Credentials = new NetworkCredential("demowork9999@gmail.com", "~Aa123456");
                        smtpClient.EnableSsl = true;


                        MailMessage mailMessage = new MailMessage();
                        mailMessage.From = new MailAddress("demowork9999@gmail.com");
                        mailMessage.To.Add(mailService.To);
                        mailMessage.Subject = mailService.Subject;
                        mailMessage.Body = mailService.Body;
                        smtpClient.Send(mailMessage);

                        TempData["msg"] = "Mail has been save and send successfully";
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        TempData["msg1"] = "Mail sending failed. The error message is -"+ "<br/>"+" [ " + ex.Message +" Helpline"+" ] ";

                        return RedirectToAction("Index");
                    }
                   
                }
                
                //ViewBag.RequisitionId = new SelectList(_requisitionManager.GetAllWithEmployee(), "Id", "RequisitionNumber", mailServiceViewModel.RequisitionId);
                return View(mailServiceViewModel);

            }

            ViewBag.RequisitionId = new SelectList(_requisitionManager.GetAllWithEmployee(), "Id", "RequisitionNumber", mailServiceViewModel.RequisitionId);
            return View(mailServiceViewModel);
        }

        // GET: MailServices/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    MailService mailService = _mailServiceManager.FindById((int) id);
        //    if (mailService == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    MailServiceViewModel mailServiceViewModel = Mapper.Map<MailServiceViewModel>(mailService);
        //    ViewBag.RequisitionId = new SelectList(_requisitionManager.GetAllWithEmployee(), "Id", "RequisitionNumber", mailServiceViewModel.RequisitionId);
        //    return View(mailServiceViewModel);
        //}

        // POST: MailServices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,To,From,Subject,Body,MailSendingDateTime,RequisitionId")] MailServiceViewModel mailServiceViewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        MailService mailService = Mapper.Map<MailService>(mailServiceViewModel);
        //        _mailServiceManager.Update(mailService);
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.RequisitionId = new SelectList(_requisitionManager.GetAllWithEmployee(), "Id", "RequisitionNumber", mailServiceViewModel.RequisitionId);
        //    return View(mailServiceViewModel);
        //}

        // GET: MailServices/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    MailService mailService = _mailServiceManager.FindById((int) id);
        //    if (mailService == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    MailServiceViewModel mailServiceViewMode = Mapper.Map<MailServiceViewModel>(mailService);
        //    return View(mailServiceViewMode);
        //}

        // POST: MailServices/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    MailService mailService = _mailServiceManager.FindById(id);
        //    _mailServiceManager.Remove(mailService);
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _mailServiceManager.Dispose();
                _requisitionManager.Dispose();
                _assignRequisitionManager.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
