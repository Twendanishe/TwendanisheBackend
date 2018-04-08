using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Twendanishe.Data;
using Twendanishe.ViewModels;

namespace Twendanishe.Controllers
{
    [Produces("application/json")]
    [Route("api/Lookups")]
    public class LookupsController : Controller
    {
        private readonly TwendanisheContext _context;

        public LookupsController(TwendanisheContext context)
        {
            _context = context;
        }

        // GET: api/Lookups/Types
        [HttpGet]
        [Route("types")]
        public IEnumerable<TypeViewModel> GetTypes()
        {
            return _context.Types.Cast<TypeViewModel>();
        }

        // GET: api/Lookups/States
        [HttpGet]
        [Route("states")]
        public IEnumerable<StateViewModel> GetStates()
        {
            return _context.Types.Cast<StateViewModel>();
        }
    }
}