using LightNap.Core.Api;
using LightNap.Core.Data;
using LightNap.Core.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LightNap.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AccountsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all accounts in the system.
        /// </summary>
        /// <returns>A list of all account records.</returns>
        /// <response code="200">Returns the list of accounts.</response>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<Account>>), 200)]
        public async Task<ApiResponseDto<IEnumerable<Account>>> GetAll()
        {
            var accounts = await _context.Accounts.ToListAsync();
            return new ApiResponseDto<IEnumerable<Account>>(accounts);
        }


        /// <summary>
        /// Retrieves a single account by its ID.
        /// </summary>
        /// <param name="id">The ID of the account to retrieve.</param>
        /// <returns>The matching account if found.</returns>
        /// <response code="200">Returns the requested account.</response>
        /// <response code="404">If the account is not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponseDto<Account>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ApiResponseDto<Account>>> Get(int id)
        {
            var account = await _context.Accounts.FindAsync(id);

            if (account == null)
                return NotFound();

            return Ok(new ApiResponseDto<Account>(account));
        }

        /// <summary>
        /// Creates a new account.
        /// </summary>
        /// <param name="account">The account to create.</param>
        /// <returns>The newly created account with its assigned ID.</returns>
        /// <response code="201">Account successfully created.</response>
        /// <response code="400">If the account data is invalid.</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseDto<Account>), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ApiResponseDto<Account>>> Create([FromBody] Account account)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = account.Id }, new ApiResponseDto<Account>(account));
        }

        /// <summary>
        /// Updates an existing account.
        /// </summary>
        /// <param name="id">The ID of the account to update.</param>
        /// <param name="account">The updated account object.</param>
        /// <returns>No content if update succeeds.</returns>
        /// <response code="204">Account successfully updated.</response>
        /// <response code="400">If the ID mismatch or model is invalid.</response>
        /// <response code="404">If the account is not found.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int id, [FromBody] Account account)
        {
            if (id != account.Id)
                return BadRequest("ID in URL does not match ID in request body.");

            var exists = await _context.Accounts.AnyAsync(a => a.Id == id);
            if (!exists)
                return NotFound();

            _context.Entry(account).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Deletes an account by ID.
        /// </summary>
        /// <param name="id">The ID of the account to delete.</param>
        /// <returns>True if the account was deleted.</returns>
        /// <response code="200">Account successfully deleted.</response>
        /// <response code="404">If the account does not exist.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponseDto<bool>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ApiResponseDto<bool>>> Delete(int id)
        {
            var account = await _context.Accounts.FindAsync(id);

            if (account == null)
                return NotFound();

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponseDto<bool>(true));
        }

    }
}
