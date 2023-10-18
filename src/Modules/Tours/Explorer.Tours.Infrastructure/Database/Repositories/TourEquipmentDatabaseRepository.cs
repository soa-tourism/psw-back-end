﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;

namespace Explorer.Tours.Infrastructure.Database.Repositories
{
    public class TourEquipmentDatabaseRepository : ITourEquipmentRepository
    {
        private readonly ToursContext _dbContext;

        public TourEquipmentDatabaseRepository(ToursContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool Exists(int tourId, int equipmentId)
        {
            return _dbContext.TourEquipment.Any(te => te.TourId == tourId && te.EquipmentId == equipmentId);
        }

        public bool IsEquipmentExists(int id)
        {
            return _dbContext.Equipment.Any(e => e.Id == id);
        }

        public TourEquipment AddEquipment(int tourId, int equipmentId)
        {
            var tourEquipment = new TourEquipment(tourId, equipmentId);

            _dbContext.TourEquipment.Add(tourEquipment);
            _dbContext.SaveChanges();

            return tourEquipment;
        }

        public TourEquipment RemoveEquipment(int tourId, int equipmentId)
        {
            var tourEquipment = new TourEquipment(tourId, equipmentId);

            _dbContext.TourEquipment.Remove(tourEquipment);
            _dbContext.SaveChanges();

            return tourEquipment;
        }

    }
}
