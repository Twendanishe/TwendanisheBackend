using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twendanishe.Data;
using Twendanishe.Models;
using Twendanishe.ViewModels;

namespace Twendanishe.Business
{
    /// <summary>
    /// This is the class for handling the ride for a user.
    /// Posts the destination and order into the DB.
    /// </summary>
    public class LocationService
    {
        public TwendanisheContext _context { get; private set; }

        public LocationService(TwendanisheContext context)
        {
            _context = context;
        }

        public async Task<bool> Do(UserLocationViewModel userLocation)
        {
            try
            {
                var state = _context.States.Where(stateSr => stateSr.Name == "Active").FirstOrDefault();

                var location = new UserLocation()
                {
                    UserId = userLocation.UserId,
                    Latitude = userLocation.Latitude,
                    Longitude = userLocation.Longitude,
                    StateId = state.Id
                };
                _context.UserLocations.Add(location);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
