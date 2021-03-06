using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    // controllers beed attributes
    // Route in this case start with api then controller
    // place holder use api/users
    // [ApiController]
    // [Route("api/[controller]")]
    // we need not derive from ControllerBase. We now 
    // can derive from BaseApiController due to 
    //everything. The attributes 
    // on line 15 and 16 being inherited in the 
    // BaseApiController
    public class UsersController : BaseApiController
    {
        private readonly DataContext _context;
        public UsersController(DataContext context)
        {
            _context = context;
        }
    // add endpoint to get all users and specific users
    // use an httpget to get users
    // in Action Result we wanna return users
    [HttpGet]
    // [AllowAnonymous]lets users who have not been authenticated 
    // access the action or controller. In short, it knows based 
    // on the token it receives from the client
    [AllowAnonymous]
    // we want to specify a type of thing that being users
    // IEnumerable need system.collections.generic
    // appusers needs entities API to make use of appuser class
    // iEnum allows simple iteration of a specified type
    public async Task <ActionResult<IEnumerable<AppUser>>> GetUsers()
    {
    // we want the below line of code to wait
    // you must add the await keyword
    // we need to use the async version of ToList
    // ToListAsync needs Microsoft entity core
    // to get result of task we await it. It unwrapd itself
    // and return itself to the user
            return await _context.Users.ToListAsync();

        }
    // this adds an authorization attribute that protectd 
    // authentization

    [Authorize]



    // below specifies a route param. When someone hits this endpoint
    // they are saying api/users/3. When user or client hits this endpoint
    // itll be fetching from the database
    [HttpGet("{id}")]
    // below we need to pass in a param to the GetUser method
    // itll take an int id which we get from the route param
    // we s
    public async Task<ActionResult<AppUser>> GetUser(int id)
    {
    // we dont need to devlare a var for user if we arent using it
    // we can return result of operation
            return _context.Users.Find(id);

            
        }
    }
}
