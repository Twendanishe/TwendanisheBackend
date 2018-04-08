using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Twendanishe.Business;
using Twendanishe.Data;
using Twendanishe.Models;
using Twendanishe.ViewModels;

namespace Twendanishe.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/Travels")]
    public class TravelsController : Controller
    {
        private readonly TwendanisheContext _context;
        private readonly IConfiguration _configuration;

        #region BusinessServices

        private readonly RideService _rideService;
        private readonly DestinationService _destinationService;
        private readonly LocationService _locationService;

        #endregion

        public TravelsController(
            TwendanisheContext context, IConfiguration configuration,
            RideService rideService, DestinationService destinationService,
            LocationService locationService
            )
        {
            _context = context;
            _configuration = configuration;

            // Business Services
            _rideService = rideService;
            _destinationService = destinationService;
            _locationService = locationService;
        }

        /// <summary>
        /// Gets the drivers within this radius
        /// </summary>
        /// <param name="coordinate">Latitude, Longitude item</param>
        /// <returns></returns>
        [HttpGet]
        [Route("drivers")]
        public async Task<IActionResult> GetDrivers([FromBody] CoordinateViewModel coordinate)
        {
            return Ok(new {
                drivers = await _rideService.Drivers(coordinate)
            });
        }

        /// <summary>
        /// Request a ride for a passenger
        /// </summary>
        /// <param name="destination">The destination for the user</param>
        /// <returns></returns>
        [HttpPost]
        [Route("ride")]
        public async Task<IActionResult> PostRide([FromBody] DestinationViewModel destination)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ride = await _rideService.Do(destination);

            if (ride != null)
            {
                return Ok(new
                {
                    status = "success",
                    rideId = ride.Id
                });
            }
            else
            {
                return BadRequest();
            }
            
        }

        /// <summary>
        /// A driver accepts a ride from a passenger
        /// </summary>
        /// <param name="rideAccept">
        /// The ride details. UserId forthe driver and the RideId for the user request.
        /// </param>
        /// <returns></returns>
        [HttpPost]
        [Route("accept-ride")]
        public async Task<IActionResult> PostAcceptRide([FromBody] RideStatusViewModel rideAccept)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ride = await _rideService.Accept(rideAccept);

            if (ride)
            {
                return Ok(new
                {
                    status = "success"
                });
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// A driver confirms a passenger boarding the given ride.
        /// </summary>
        /// <param name="id">
        /// The ride details. UserId forthe driver and the RideId for the user request.
        /// </param>
        /// <returns></returns>
        [HttpPost]
        [Route("board")]
        public async Task<IActionResult> PostBoardRide([FromBody] RideStatusViewModel rideStatus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ride = await _rideService.Board(rideStatus);

            if (ride)
            {
                return Ok(new
                {
                    status = "success"
                });
            }
            else
            {
                return BadRequest();
            }

        }

        /// <summary>
        /// A driver completes a ride for a specific passenger
        /// </summary>
        /// <param name="rideAccept">
        /// The ride details. UserId forthe driver and the RideId for the user request.
        /// </param>
        /// <returns></returns>
        [HttpPost]
        [Route("complete-ride/{id}")]
        public async Task<IActionResult> PostCompleteRide([FromRoute] int id, [FromBody] IdViewModel idViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ride = await _rideService.Complete(id, idViewModel);

            if (ride != null)
            {
                return Ok(new
                {
                    status = "success",
                    amount = ride.Amount
                });
            }
            else
            {
                return BadRequest();
            }

        }

        /// <summary>
        /// A driver completes a ride for a specific passenger
        /// </summary>
        /// <param name="rideAccept">
        /// The ride details. UserId forthe driver and the RideId for the user request.
        /// </param>
        /// <returns></returns>
        [HttpPost]
        [Route("fare/{id}")]
        public async Task<IActionResult> PostApproximateRide([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rideAmount = await _rideService.Approximate(id);

            if (rideAmount > 0)
            {
                return Ok(new
                {
                    status = "success",
                    amount = rideAmount
                });
            }
            else
            {
                return BadRequest();
            }

        }

        /// <summary>
        /// Sets a destination for a user. This is done probably by the driver so that we can
        /// triangulate there details.
        /// </summary>
        /// <param name="destination">The destination for the user</param>
        /// <returns></returns>
        [HttpPost]
        [Route("destination")]
        public async Task<IActionResult> PostDestination([FromBody] DestinationViewModel destination)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dest = await _destinationService.Do(destination);

            if (dest)
            {
                return Ok(new
                {
                    status = "success"
                });
            }
            else
            {
                return BadRequest();
            }

        }

        /// <summary>
        /// Sets a location for a user. This is done probably by the driver so that we can
        /// triangulate there details.
        /// </summary>
        /// <param name="location">The location for the user</param>
        /// <returns></returns>
        [HttpPost]
        [Route("location")]
        public async Task<IActionResult> PostLocation([FromBody] UserLocationViewModel location)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var loc = await _locationService.Do(location);

            if (loc)
            {
                return Ok(new
                {
                    status = "success"
                });
            }
            else
            {
                return BadRequest();
            }

        }
    }
}