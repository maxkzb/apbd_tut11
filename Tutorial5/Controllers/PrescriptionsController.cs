using Microsoft.AspNetCore.Mvc;
using Tutorial5.Models.DTO;
using Tutorial5.Services;

namespace Tutorial5.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PrescriptionsController : ControllerBase
{
    private readonly IPrescriptionService _prescriptionService;

    public PrescriptionsController(IPrescriptionService prescriptionService)
    {
        _prescriptionService = prescriptionService;
    }

    [HttpPost]
    public async Task<IActionResult> AddPrescription([FromBody] AddPrescriptionRequestDTO request)
    {
        var result = await _prescriptionService.AddPrescriptionAsync(request);
        if (!result.Success)
            return BadRequest(result.Message);

        return Ok(new { IdPrescription = result.IdPrescription });
    }
}

[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly IPatientService _patientService;

    public PatientsController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    [HttpGet("{idPatient}")]
    public async Task<IActionResult> GetPatientDetails(int idPatient)
    {
        var patient = await _patientService.GetPatientDetailsAsync(idPatient);
        if (patient == null)
            return NotFound();

        return Ok(patient);
    }
}