﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Auto.Data.Entities;
using static System.Int32;

namespace Auto.Data {
    public class AutoCsvFileDatabase : IAutoDatabase {
        private static readonly IEqualityComparer<string> collation = StringComparer.OrdinalIgnoreCase;

        private readonly Dictionary<string, Manufacturer> manufacturers = new Dictionary<string, Manufacturer>(collation);
        private readonly Dictionary<string, Model> models = new Dictionary<string, Model>(collation);
        private readonly Dictionary<string, Vehicle> vehicles = new Dictionary<string, Vehicle>(collation);
        private readonly Dictionary<int, Owner> owners = new Dictionary<int, Owner>();
        private readonly ILogger<AutoCsvFileDatabase> logger;

        public AutoCsvFileDatabase(ILogger<AutoCsvFileDatabase> logger) {
            this.logger = logger;
            ReadManufacturersFromCsvFile("manufacturers.csv");
            ReadModelsFromCsvFile("models.csv");
            ReadVehiclesFromCsvFile("vehicles.csv");
            ReadOwnersFromCsvFile("owners.csv");
            ResolveReferences();
        }

        private void ResolveReferences() {
            foreach (var mfr in manufacturers.Values) {
                mfr.Models = models.Values.Where(m => m.ManufacturerCode == mfr.Code).ToList();
                foreach (var model in mfr.Models) model.Manufacturer = mfr;
            }

            foreach (var model in models.Values) {
                model.Vehicles = vehicles.Values.Where(v => v.ModelCode == model.Code).ToList();
                foreach (var vehicle in model.Vehicles) vehicle.VehicleModel = model;
            }

            foreach (var owner in owners.Values) { 
                var v = vehicles.Values.Where(v => v.Registration == owner.VehicleId).First();
                if(v != null)
                {
                    owner.Vehicle = v;
                    v.Owner = owner;
                }             
            }
        }

        private string ResolveCsvFilePath(string filename) {
            var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var csvFilePath = Path.Combine(directory, "csv-data");
            return Path.Combine(csvFilePath, filename);
        }

        private void ReadVehiclesFromCsvFile(string filename) {
            var filePath = ResolveCsvFilePath(filename);
            foreach (var line in File.ReadAllLines(filePath)) {
                var tokens = line.Split(",");
                var vehicle = new Vehicle {
                    Registration = tokens[0],
                    ModelCode = tokens[1],
                    Color = tokens[2],
                };
                if (tokens.Length >= 4 && Int32.TryParse(tokens[4].Trim(), out int owner)) vehicle.OwnerId = owner;
                if (TryParse(tokens[3], out var year)) vehicle.Year = year;
                vehicles[vehicle.Registration] = vehicle;
            }
            logger.LogInformation($"Loaded {vehicles.Count} models from {filePath}");
        }

        private void ReadModelsFromCsvFile(string filename) {
            var filePath = ResolveCsvFilePath(filename);
            foreach (var line in File.ReadAllLines(filePath)) {
                var tokens = line.Split(",");
                var model = new Model {
                    Code = tokens[0],
                    ManufacturerCode = tokens[1],
                    Name = tokens[2]
                };
                models.Add(model.Code, model);
            }
            logger.LogInformation($"Loaded {models.Count} models from {filePath}");
        }

        private void ReadManufacturersFromCsvFile(string filename) {
            var filePath = ResolveCsvFilePath(filename);
            foreach (var line in File.ReadAllLines(filePath)) {
                var tokens = line.Split(",");
                var mfr = new Manufacturer {
                    Code = tokens[0],
                    Name = tokens[1]
                };
                manufacturers.Add(mfr.Code, mfr);
            }
            logger.LogInformation($"Loaded {manufacturers.Count} manufacturers from {filePath}");
        }

        private void ReadOwnersFromCsvFile(string filename)
        {
            var filePath = ResolveCsvFilePath(filename);
            foreach (var line in File.ReadAllLines(filePath))
            {
                var tokens = line.Split(",");
                var owner = new Owner
                {
                    FullName = tokens[1],
                    VehicleId = tokens[3].Trim()
                };
                if (TryParse(tokens[0], out var id)) owner.Id = id;
                else continue;
                if (DateTime.TryParse(tokens[2], out DateTime date)) owner.BirthDate = date;
                owners.Add(owner.Id, owner);
            }
            logger.LogInformation($"Loaded {owners.Count} owners from {filePath}");
        }

        public int CountVehicles() => vehicles.Count;
        public int CountOwners() => owners.Count;

        public IEnumerable<Vehicle> ListVehicles() => vehicles.Values;

        public IEnumerable<Manufacturer> ListManufacturers() => manufacturers.Values;

        public IEnumerable<Model> ListModels() => models.Values;
        public IEnumerable<Owner> ListOwners() => owners.Values;

        public Vehicle FindVehicle(string registration) => vehicles.GetValueOrDefault(registration);

        public Model FindModel(string code) => models.GetValueOrDefault(code);

        public Manufacturer FindManufacturer(string code) => manufacturers.GetValueOrDefault(code);
        public Owner FindOwner(int id) => owners.GetValueOrDefault(id);

        public string GetVehicleNumber(Owner owner)
        {
            string vehicleNumber = null;
            foreach (var o in owners)
            {
                if (o.Value.FullName.Equals(owner.FullName)
                    && IsDateSame(o.Value.BirthDate, owner.BirthDate))
                    vehicleNumber = o.Value.VehicleId;
            }

            return vehicleNumber;
        }

        private bool IsDateSame(DateTime d1, DateTime d2)
        {
            if (d1.Day == d2.Day
                && d1.Month == d2.Month
                && d1.Year == d2.Year)
                return true;

            return false;
        }

        public void CreateVehicle(Vehicle vehicle) {
            //vehicle.ModelCode = vehicle.VehicleModel.Code;
            //vehicle.VehicleModel.Vehicles.Add(vehicle);
            //vehicle.Owner = owners.GetValueOrDefault(vehicle.OwnerId ?? -1);
            //UpdateVehicle(vehicle);
        }

        public void CreateOwner(Owner owner) {
            owner.Vehicle = vehicles.GetValueOrDefault(owner.VehicleId);
            UpdateOwner(owner);
        }

        public void UpdateVehicle(Vehicle vehicle) {
            vehicles[vehicle.Registration] = vehicle;
        }

        public void UpdateOwner(Owner owner) {
            owners[owner.Id] = owner;
        }

        public void DeleteVehicle(Vehicle vehicle) {
            var model = FindModel(vehicle.ModelCode);
            model.Vehicles.Remove(vehicle);
            vehicles.Remove(vehicle.Registration);
        }

        public void DeleteOwner(Owner owner) {
            var vehicle = FindVehicle(owner.VehicleId);
            vehicle.Owner.Vehicle = null;
            owners.Remove(owner.Id);
        }
    }
}