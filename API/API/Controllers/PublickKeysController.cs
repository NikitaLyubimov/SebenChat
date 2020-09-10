using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using API.Presenters;
using API.ViewModels.Request;
using Core.Interfaces.UseCases;
using Core.DTO.UseCaseRequests;

namespace API.Controllers
{
    [Authorize(Policy ="ApiUser")]
    [Route("api/[controller]")]
    [ApiController]
    public class PublickKeysController : ControllerBase
    {
        private IPublickKeyUseCase _publickKeyUseCase;
        private PublickKeyPresenter _publickKeyPresenter;

        public PublickKeysController(IPublickKeyUseCase publickKeyUseCase, PublickKeyPresenter publickKeyPresenter)
        {
            _publickKeyUseCase = publickKeyUseCase;
            _publickKeyPresenter = publickKeyPresenter;
        }

        [HttpPost]
        public async Task<ActionResult> Index([FromBody]API.ViewModels.Request.PublickKeyRequest request)
        {
            await _publickKeyUseCase.Handle(new Core.DTO.UseCaseRequests.PublickKeyRequest(request.UserName, request.KeyValue), _publickKeyPresenter);
            return _publickKeyPresenter.ContentResult;
        }
    }
}