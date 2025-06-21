using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Webapi.Application.VoucherCQRS.Commands.CreateVoucher;
using Webapi.Application.VoucherCQRS.Commands.DeleteVoucher;
using Webapi.Application.VoucherCQRS.Commands.ImportVouchers;
using Webapi.Application.VoucherCQRS.Commands.UpdateVoucher;
using Webapi.Application.VoucherCQRS.Queries.GetVoucherById;
using Webapi.Application.VoucherCQRS.Queries.GetVouchers;
using Webapi.SharedKernel.DTOs.Voucher;

namespace Webapi.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VouchersController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public VouchersController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<VoucherDto>>> GetVouchers()
    {
        var result = await _mediator.Send(new GetVouchersQuery());
        return Ok(result);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<VoucherDto>> GetVoucherById(Guid id)
    {
        var result = await _mediator.Send(new GetVoucherByIdQuery(id));
        return Ok(result);
    }
    
    [HttpPost]
    // [Authorize(Roles = "Admin")]
    public async Task<ActionResult<VoucherDto>> CreateVoucher(CreateVoucherDto voucherDto)
    {
        var result = await _mediator.Send(new CreateVoucherCommand(voucherDto));
        return CreatedAtAction(nameof(GetVoucherById), new { id = result.Id }, result);
    }
    
    [HttpPut("{id:guid}")]
    // [Authorize(Roles = "Admin")]
    public async Task<ActionResult<VoucherDto>> UpdateVoucher(Guid id, UpdateVoucherDto voucherDto)
    {
        var result = await _mediator.Send(new UpdateVoucherCommand(id, voucherDto));
        return Ok(result);
    }
    
    [HttpDelete("{id:guid}")]
    // [Authorize(Roles = "Admin")]
    public async Task<ActionResult<bool>> DeleteVoucher(Guid id)
    {
        var result = await _mediator.Send(new DeleteVoucherCommand(id));
        return Ok(result);
    }
    
    [HttpPost("import")]
    // [Authorize(Roles = "Admin")]
    public async Task<ActionResult<int>> ImportVouchers([FromForm] ImportVoucherDto importDto)
    {
        var result = await _mediator.Send(new ImportVouchersCommand(importDto));
        return Ok(new { ImportedCount = result });
    }
}