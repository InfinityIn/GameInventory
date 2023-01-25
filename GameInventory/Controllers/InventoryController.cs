using GameInventory.Dto;
using GameInventory.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GameInventory.Controllers
{
    [Authorize]
    [ApiController]
    
    public class InventoryController : ControllerBase
    {        
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<InventoryController> _logger;
        readonly IMediator _mediator;

        public InventoryController(ILogger<InventoryController> logger, 
            UserManager<IdentityUser> userManager, 
            IMediator mediator)
        {
            _logger = logger;
            _userManager = userManager;
            _mediator = mediator;
        }

        [Route("fill/{count}")]
        [HttpGet]
        public async Task<ActionResult<InventoryItem>> GetItems([FromRoute] int count)
        {
            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            if (Guid.TryParse(user.Id, out var permanentId))
            {
                var query = new FillInventoryQuery.Request() { UserId = permanentId, Count = count };
                var response = await _mediator.Send(query);
                return Ok(response);
            }

            return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Ошибка авторизации пользователя" });

        }
        
        [Route("items")]
        [HttpGet]
        public async Task<ActionResult<InventoryItem>> GetItems()
        {           
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            if (Guid.TryParse(user.Id, out var permanentId))
            {
                var query = new GetItemListQuery.Request() { UserId = permanentId };
                var response = await _mediator.Send(query);
                return Ok(response);
            }

            return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Ошибка авторизации пользователя" });

        }
    }
}