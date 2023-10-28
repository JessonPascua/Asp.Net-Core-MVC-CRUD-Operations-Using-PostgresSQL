using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Asp.Net_Core_MVC_CRUD_Operations_Using_PostgresSQL.Data;
using Asp.Net_Core_MVC_CRUD_Operations_Using_PostgresSQL.Models;
using Asp.Net_Core_MVC_CRUD_Operations_Using_PostgresSQL.Models.Entities;

namespace Asp.Net_Core_MVC_CRUD_Operations_Using_PostgresSQL.Controllers
{
    public class PatientController : Controller
    {
        private readonly HealthCareDbContext _healthCareDbContext;
        private readonly List<Patients> _patients = new();
        private readonly List<Physicians> _physicians = new();
        private readonly List<PatientRecordViewModel> _patientRecordViewModel = new();

        public PatientController(HealthCareDbContext healthCareDbContext)
        {
            _healthCareDbContext = healthCareDbContext;
        }

        // GET: Patient
        public async Task<IActionResult> Index()
        {
            var patients = await _healthCareDbContext.Patients
                 .Include(p => p.Physician)
                 .ThenInclude(p => p.Specialization)
                 .ToListAsync();

            foreach (var patient in patients)
            {
                var doctorId = patient.DoctorId;

                var doctorNameQuery = _healthCareDbContext.Physicians.Where(x => x.DoctorId == doctorId).Select(u => u.DoctorFullName);

                var doctorName = await doctorNameQuery.FirstOrDefaultAsync();

                _patientRecordViewModel.Add(new PatientRecordViewModel
                {
                    Id = patient.Id,
                    FristName = patient.FristName,
                    LastName = patient.LastName,
                    Address = patient.Address,
                    DoctorName = doctorName,
                    Type = patient.Physician.Specialization.Type,
                    AppointmentDate = patient.AppointmentDate
                });
            }
            return View(_patientRecordViewModel);
        }

        // GET: Patient/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _healthCareDbContext.Patients
                .FirstOrDefaultAsync(m => m.Id == id);

            if (patient == null)
            {
                return NotFound();
            }

            var doctorId = patient.DoctorId;
            var doctorNameQuery = _healthCareDbContext.Physicians.Where(x => x.DoctorId == doctorId).Select(u => u.DoctorFullName);
            var doctorName = await doctorNameQuery.FirstOrDefaultAsync();

            var patientRecordViewModel = new PatientRecordViewModel
            {
                Id = patient.Id,
                FristName = patient.FristName,
                LastName = patient.LastName,
                Address = patient.Address,
                DoctorName = doctorName,
                AppointmentDate = patient.AppointmentDate
            };

            return View(patientRecordViewModel);
        }

        // GET: Patient/Create
        public IActionResult Create()
        {
            var doctors = _healthCareDbContext.Physicians.ToList();

            // Create a SelectList to use in the dropdown list in the view
            ViewBag.Doctors = new SelectList(doctors, "DoctorId", "DoctorFullName");

            return View();
        }

        // POST: Patient/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FristName,LastName,Address,DoctorId,AppointmentDate,NickName")] PatientRecordViewModel patientRecordViewModel)
        {
            if (ModelState.IsValid)
            {
                var newPatient = new Patients
                {
                    FristName = patientRecordViewModel.FristName,
                    LastName = patientRecordViewModel.LastName,
                    Address = patientRecordViewModel.Address,
                    DoctorId = patientRecordViewModel.DoctorId,
                    AppointmentDate = patientRecordViewModel.AppointmentDate
                };

                _healthCareDbContext.Patients.Add(newPatient);
                await _healthCareDbContext.SaveChangesAsync();


                return RedirectToAction("Index");
            }

            var doctors = _healthCareDbContext.Physicians.ToList();

            ViewBag.Doctors = new SelectList(doctors, "DoctorId", "DoctorFullName");
            return View(patientRecordViewModel);
        }

        // GET: Patient/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _healthCareDbContext.Patients
                .FirstOrDefaultAsync(m => m.Id == id);

            if (patient == null)
            {
                return NotFound();
            }

            var patientRecordViewModel = new PatientRecordViewModel
            {
                Id = patient.Id,
                FristName = patient.FristName,
                LastName = patient.LastName,
                Address = patient.Address,
                DoctorId = patient.DoctorId, // Set the DoctorId property in the view model
                AppointmentDate = patient.AppointmentDate,
            };

            var doctors = _healthCareDbContext.Physicians.ToList();
            ViewBag.Doctors = new SelectList(doctors, "DoctorId", "DoctorFullName");

            return View(patientRecordViewModel);
        }

        // POST: Patient/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FristName,LastName,Address,DoctorId,AppointmentDate")] PatientRecordViewModel patientRecordViewModel)
        {
            if (id != patientRecordViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Retrieve the existing patient record from the database
                    var existingPatient = await _healthCareDbContext.Patients.FindAsync(id);

                    if (existingPatient == null)
                    {
                        return NotFound();
                    }

                    // Update the properties of the existing patient record
                    existingPatient.FristName = patientRecordViewModel.FristName;
                    existingPatient.LastName = patientRecordViewModel.LastName;
                    existingPatient.Address = patientRecordViewModel.Address;
                    existingPatient.DoctorId = patientRecordViewModel.DoctorId; // Update DoctorId
                    existingPatient.AppointmentDate = patientRecordViewModel.AppointmentDate;

                    // Mark the entity as modified
                    _healthCareDbContext.Entry(existingPatient).State = EntityState.Modified;

                    await _healthCareDbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientRecordViewModelExists(patientRecordViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(patientRecordViewModel);
        }



        // GET: Patient/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _healthCareDbContext.Patients
                .FirstOrDefaultAsync(m => m.Id == id);

            if (patient == null)
            {
                return NotFound();
            }

            var doctorId = patient.DoctorId;
            var doctorNameQuery = _healthCareDbContext.Physicians.Where(x => x.DoctorId == doctorId).Select(u => u.DoctorFullName);
            var doctorName = await doctorNameQuery.FirstOrDefaultAsync();

            var patientRecordViewModel = new PatientRecordViewModel
            {
                Id = patient.Id,
                FristName = patient.FristName,
                LastName = patient.LastName,
                Address = patient.Address,
                DoctorName = doctorName,
                AppointmentDate = patient.AppointmentDate
            };

            return View(patientRecordViewModel);
        }

        // POST: Patient/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_healthCareDbContext.Patients == null)
            {
                return Problem("Entity set 'HealthCareDbContext.Patients' is null.");
            }

            var patient = await _healthCareDbContext.Patients.FindAsync(id);
            if (patient != null)
            {
                _healthCareDbContext.Patients.Remove(patient);
                await _healthCareDbContext.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PatientRecordViewModelExists(int id)
        {
            return _healthCareDbContext.Patients.Any(e => e.Id == id);
        }
    }

}
