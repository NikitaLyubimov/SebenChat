using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;


using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using Core.Interfaces.UseCases;
using Core.DTO.UseCaseRequests;
using API.Presenters;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VerifyController : ControllerBase
    {
        private IVerifyEmailTokenUseCase _verifyEmailTokenUseCase;
        private VerifyEmailTokenPresenter _verifyEmailTokenPresenter;
        public VerifyController(IVerifyEmailTokenUseCase verifyEmailTokenUseCase, VerifyEmailTokenPresenter verifyEmailTokenPresenter)
        {
            _verifyEmailTokenUseCase = verifyEmailTokenUseCase;
            _verifyEmailTokenPresenter = verifyEmailTokenPresenter;
        }
        public async Task<ActionResult> Index([FromQuery]string token)
        {
            await _verifyEmailTokenUseCase.Handle(new VerifyEmailTokenRequest(token), _verifyEmailTokenPresenter);
            return _verifyEmailTokenPresenter.ContentResult;

        }
    }
}