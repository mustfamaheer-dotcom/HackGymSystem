using Gym.Application.Payments.DTOs;
using Gym.Application.Payments.Commands.CreatePayment;
using Gym.Application.Payments.Commands.DeletePayment;
using Gym.Application.Payments.Queries.GetAllPayments;
using Gym.Application.Payments.Queries.GetPaymentById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.API.Controllers;

[Authorize(Roles = "Owner,Receptionist")]
[Route("Payments")]
public class PaymentsMvcController : Controller
{
    private readonly IMediator _mediator;

    public PaymentsMvcController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 20, CancellationToken cancellationToken = default)
    {
        ViewData["Title"] = "Payments";

        var query = new GetAllPaymentsQuery { Page = page, PageSize = pageSize };
        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return View("Index", null);
        }

        return View(result.Data);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        ViewData["Title"] = "New Payment";
        return View();
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(CreatePaymentCommand command, CancellationToken cancellationToken)
    {
        ViewData["Title"] = "New Payment";

        if (!ModelState.IsValid)
            return View(command);

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return View(command);
        }

        TempData["Success"] = "Payment created successfully";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("details/{id}")]
    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        ViewData["Title"] = "Payment Details";

        var result = await _mediator.Send(new GetPaymentByIdQuery(id), cancellationToken);

        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        return View(result.Data);
    }

    [HttpGet("delete/{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        ViewData["Title"] = "Delete Payment";

        var result = await _mediator.Send(new GetPaymentByIdQuery(id), cancellationToken);

        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        return View(result.Data);
    }

    [HttpPost("delete/{id}")]
    public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeletePaymentCommand(id), cancellationToken);

        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        TempData["Success"] = "Payment deleted successfully";
        return RedirectToAction(nameof(Index));
    }
}
