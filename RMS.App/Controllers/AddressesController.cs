﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
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
    public class AddressesController : Controller
    {
        private IAddressManager _addressManager;
        private IDivisionManager _divisionManager;
        private IUpazilaManager _upazilaManager;
        private IDistrictManager _districtManager;

        public AddressesController(IAddressManager addressManager,IDivisionManager divisionManager,IUpazilaManager upazilaManager,IDistrictManager districtManager)
        {
            this._addressManager = addressManager;
            this._divisionManager = divisionManager;
            this._upazilaManager = upazilaManager;
            this._districtManager = districtManager;
        }
        // GET: Addresses
        public ActionResult Index()
        {
            try
            {

                var addresses = _addressManager.GetAll();
                return View(addresses.ToList());
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Addresses", "Index"));
            }
        }

        // GET: Addresses/Details/5
        public ActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Address address = _addressManager.FindById((int)id);
                if (address == null)
                {
                    return HttpNotFound();
                }
                return View(address);
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Addresses", "Details"));
            }
        }

        // GET: Addresses/Create
        public ActionResult Create()
        {
            try
            {
                ViewBag.DistrictId = new SelectList(_districtManager.GetAll(), "Id", "Name");//district
                ViewBag.DivisionId = new SelectList(_divisionManager.GetAll(), "Id", "Name");//division
                ViewBag.UpazilaId = new SelectList(_upazilaManager.GetAll(), "Id", "Name");//upazila
                return View();
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Addresses", "Create"));
            }
        }

        // POST: Addresses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,AddressType,AddressLine1,AddressLine2,PostOffice,PostCode,DivisionId,DistrictId,UpazilaId,EmployeeId")] Address address)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    address.AddressType = "Present Address";
                    _addressManager.Add(address);
                    return RedirectToAction("Index");
                }

                return View();
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Addresses", "Create"));
            }
        }

        // GET: Addresses/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Address address = _addressManager.FindById((int)id);
                if (address == null)
                {
                    return HttpNotFound();
                }
                ViewBag.DistrictId = new SelectList(_districtManager.GetAll(), "Id", "Name", address.DistrictId);
                ViewBag.DivisionId = new SelectList(_divisionManager.GetAll(), "Id", "Name", address.DivisionId);
                ViewBag.UpazilaId = new SelectList(_upazilaManager.GetAll(), "Id", "Name", address.UpazilaId);
                return View(address);
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Addresses", "Edit"));
            }
        }

        // POST: Addresses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AddressType,AddressLine1,AddressLine2,PostOffice,PostCode,DivisionId,DistrictId,UpazilaId,EmployeeId")] Address address)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _addressManager.Update(address);
                    return RedirectToAction("Index");
                }
                ViewBag.DistrictId = new SelectList(_districtManager.GetAll(), "Id", "Name", address.DistrictId);
                ViewBag.DivisionId = new SelectList(_divisionManager.GetAll(), "Id", "Name", address.DivisionId);
                //ViewBag.EmployeeId = new SelectList(_, "Id", "FullName", address.EmployeeId);
                ViewBag.UpazilaId = new SelectList(_upazilaManager.GetAll(), "Id", "Name", address.UpazilaId);
                return View(address);
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Addresses", "Edit"));
            }
        }

        // GET: Addresses/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Address address = _addressManager.FindById((int)id);
                if (address == null)
                {
                    return HttpNotFound();
                }
                return View(address);
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Addresses", "Delete"));
            }
        }

        // POST: Addresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Address address = _addressManager.FindById((int)id);
                _addressManager.Remove(address);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Addresses", "Delete"));
            }
        }

        

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _divisionManager.Dispose();
                _districtManager.Dispose();
                _upazilaManager.Dispose();
                _addressManager.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
