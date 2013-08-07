﻿using CMZero.Web.Models.ViewModels;

namespace CMZero.Web.Services.ViewModelGetters
{
    public interface IApplicationViewModelGetter
    {
        ApplicationViewModel Get(string applicationId);
    }
}