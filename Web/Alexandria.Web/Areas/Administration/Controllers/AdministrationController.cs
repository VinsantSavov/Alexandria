﻿namespace Alexandria.Web.Areas.Administration.Controllers
{
    using Alexandria.Common;
    using Alexandria.Web.Controllers;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area(GlobalConstants.AdministrationArea)]
    public class AdministrationController : BaseController
    {
    }
}
