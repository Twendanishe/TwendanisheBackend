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
    public class DestinationService
    {
        public TwendanisheContext _context { get; private set; }

        public DestinationService(TwendanisheContext context)
        {
            _context = context;
        }

        public async Task<bool> Do(DestinationViewModel destination)
        {
            try
            {
                var state = _context.States
                    .Where(stateSr => stateSr.Name == "Awaiting Departure").FirstOrDefault();

                var destinationSave = new Destination()
                {
                    UserId = destination.UserId,
                    TypeId = destination.TypeId,
                    SourceCoordinates = destination.SourceCoordinates,
                    SourceName = destination.SourceName,
                    DestinationCoordinates = destination.DestinationCoordinates,
                    DestinationName = destination.DestinationName,
                    VehicleId = destination.VehicleId,
                    StateId = state.Id
                };
                _context.Destinations.Add(destinationSave);
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
