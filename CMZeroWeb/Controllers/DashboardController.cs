﻿using System;
using System.Web.Mvc;
using CMZero.API.ServiceAgent;
using CMZero.Web.Models.ViewModels;
using CMZero.Web.Services.Labels;
using CMZero.Web.Services.Login;
using CMZero.Web.Services.ViewModelGetters;

namespace CMZeroWeb.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardViewModelGetter _dashboardViewModelGetter;
        private readonly ILabelCollectionRetriever _labelCollectionRetriever;

        public DashboardController(IDashboardViewModelGetter dashboardViewModelGetter, ILabelCollectionRetriever labelCollectionRetriever)
        {
            _dashboardViewModelGetter = dashboardViewModelGetter;
            _labelCollectionRetriever = labelCollectionRetriever;
        }

        public ViewResult Index()
        {
            var model = _dashboardViewModelGetter.Get();

            return View("Index", model);
        }

        public ViewResult Error()
        {
            var model = new DashboardErrorViewModel();
            model.Labels = _labelCollectionRetriever.Get("DashboardErrorPage");

            return View("Error", model);
        }

    }
}
