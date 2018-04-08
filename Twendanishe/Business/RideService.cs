using Microsoft.EntityFrameworkCore;
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
    public class RideService
    {
        public TwendanisheContext _context { get; private set; }

        public RideService(TwendanisheContext context)
        {
            _context = context;
        }

        public async Task<Destination> Do(DestinationViewModel destination)
        {
            try
            {
                var state = _context.States.Where(stateSr => stateSr.Name == "Active").FirstOrDefault();

                var destinationSave = new Destination()
                {
                    UserId = destination.UserId,
                    TypeId = destination.TypeId,
                    SourceCoordinates = destination.SourceCoordinates,
                    SourceName = destination.SourceName,
                    DestinationCoordinates = destination.DestinationCoordinates,
                    DestinationName = destination.DestinationName,
                    StateId = state.Id
                };
                _context.Destinations.Add(destinationSave);
                await _context.SaveChangesAsync();
                
                _context.Orders.Add(new Order()
                {
                    DestinationId = destinationSave.Id,
                    StateId = state.Id
                });
                await _context.SaveChangesAsync();

                return destinationSave;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// The driver accepts the ride for a passenger.
        /// We change the ride's state and decrement the current capacity for 
        /// the driver's destination.
        /// </summary>
        /// <param name="rideId">The ride from the user.</param>
        /// <param name="driverId">The driver accepting the ride.</param>
        /// <returns></returns>
        public async Task<bool> Accept(RideStatusViewModel acceptRide)
        {
            try
            {
                var statePickup = _context.States.Where(stateSr => stateSr.Name == "To Pickup").FirstOrDefault();
                var stateAwaitingPickup = _context.States.Where(stateSr => stateSr.Name == "Awaiting Pickup").FirstOrDefault();

                var ride = await _context.Destinations.FindAsync(acceptRide.RideId);

                if (ride == null)
                {
                    return false;
                }

                ride.StateId = stateAwaitingPickup.Id;
                _context.Entry(ride).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                var destination = await _context.Destinations
                    .Where(dest => dest.UserId == acceptRide.UserId && 
                    (dest.State.Name == "Awaiting Departure" || dest.State.Name == "In Transit" 
                    || dest.State.Name == "To Pickup") && dest.AvailableCapacity > 0)
                    .FirstOrDefaultAsync();

                if (destination == null)
                {
                    return false;
                }

                destination.AvailableCapacity -= 1;

                if (destination.State.Name != "To Pickup")
                {
                    destination.State = statePickup;
                }

                _context.Entry(destination).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// The driver completes the ride for a passenger.
        /// We change the ride's state and inrement the current capacity for 
        /// the driver's destination.
        /// </summary>
        /// <param name="destinationId">The id for the driver's destination.</param>
        /// <param name="idViewModel">The model with the id of the ride.</param>
        /// <returns></returns>
        public async Task<RideCompleteViewModel> Complete(int destinationId, IdViewModel idViewModel)
        {
            try
            {
                var stateCompleted = _context.States.Where(stateSr => stateSr.Name == "Completed").FirstOrDefault();
                var stateInTransit = _context.States.Where(stateSr => stateSr.Name == "In Transit").FirstOrDefault();

                var ride = await _context.Destinations.FindAsync(idViewModel.Id);

                if (ride == null)
                {
                    return null;
                }

                ride.StateId = stateCompleted.Id;
                _context.Entry(ride).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                var destination = await _context.Destinations.FindAsync(destinationId);

                if (destination == null)
                {
                    return null;
                }

                if (destination.Vehicle.Capacity <= destination.AvailableCapacity + 1)
                {
                    destination.AvailableCapacity += 1;
                }

                if (destination.Vehicle.Capacity == destination.AvailableCapacity)
                {
                    destination.State = stateInTransit;
                }

                _context.Entry(destination).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                return new RideCompleteViewModel() {
                    RideId = idViewModel.Id,
                    DriverId = destination.UserId,
                    Amount = 0
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// The driver confirms that a passenger has boarded the vehicle.
        /// </summary>
        /// <param name="rideStatus">The id of the passenger's destination and the driver id.</param>
        /// <returns></returns>
        public async Task<bool> Board(RideStatusViewModel rideStatus)
        {
            try
            {
                var stateCompleted = _context.States.Where(stateSr => stateSr.Name == "Completed").FirstOrDefault();
                var stateInTransit = _context.States.Where(stateSr => stateSr.Name == "In Transit").FirstOrDefault();

                var ride = await _context.Destinations.FindAsync(rideStatus.RideId);

                if (ride == null)
                {
                    return false;
                }

                ride.StateId = stateInTransit.Id;
                _context.Entry(ride).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// The passenger gets an approximation of the fare for their destination
        /// </summary>
        /// <param name="destinationId">The id for the passenger's destination.</param>
        /// <returns></returns>
        public async Task<decimal> Approximate(int destinationId)
        {
            try
            {
                var destination = await _context.Destinations.FindAsync(destinationId);
                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// The passenger gets an approximation of the fare for their destination
        /// </summary>
        /// <param name="destinationId">The id for the passenger's destination.</param>
        /// <returns></returns>
        public async Task<int> Drivers(CoordinateViewModel coordinate)
        {
            try
            {
                var destination = await _context.Destinations.FindAsync(0);
                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
