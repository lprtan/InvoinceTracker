using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using InvoinceModule.Application.Dtos;
using InvoinceModule.Domain.Entities;
using InvoinceModule.Domain.ValueObject;
using InvoinceModule.Application.Helpers;
using InvoinceModule.Application.Ports.In;
using InvoinceModule.Application.Ports.Out;

namespace InvoinceModule.Application.UseCases
{
    public class InvoiceHeaderUseCase: IInvoiceHeaderInputPort
    {
        private readonly IMapper _mapper;
        private readonly IInvoiceRepositoryOutPort _invoiceRepository;
        private readonly IInvoiceNumberGeneratorOutPort _invoiceNumberGeneratorRepository;

        public InvoiceHeaderUseCase(IMapper mapper, IInvoiceRepositoryOutPort invoiceRepository, IInvoiceNumberGeneratorOutPort invoiceNumberGeneratorRepository)
        {
            this._mapper = mapper;
            this._invoiceRepository = invoiceRepository;
            this._invoiceNumberGeneratorRepository = invoiceNumberGeneratorRepository;
        }

        public async Task<InvoiceDto> CreateInvoiceAsync(InvoiceDto invoiceDto)
        {
            var invoiceEntity = _mapper.Map<InvoiceHeader>(invoiceDto);

            InvoinceHeaderVO.Validate(invoiceEntity);

            int number = await _invoiceNumberGeneratorRepository.GetNextSequenceAsync();

            invoiceEntity.InvoiceNumber = InvoiceNumberGenerator.Generate(number);

            await _invoiceRepository.AddAsync(invoiceEntity);
            await _invoiceRepository.SaveChangesAsync();

            var createdInvoiceDto = _mapper.Map<InvoiceDto>(invoiceEntity);

            return createdInvoiceDto;
        }

        public async Task<InvoiceDto> GetInvoiceByIdAsync(string invoiceNumber)
        {

            var invoiceEntity = await _invoiceRepository.GetByInvoiceNumberAsync(invoiceNumber);

            if (invoiceEntity == null)
                return null;

            var invoiceDto = _mapper.Map<InvoiceDto>(invoiceEntity);
            return invoiceDto;
        }

        public async Task<IEnumerable<InvoiceHeaderDto>> GetInvoiceHeadersAsync()
        {
            var headers = await _invoiceRepository.GetHeadersAsync();

            var headerDtos = _mapper.Map<IEnumerable<InvoiceHeaderDto>>(headers);
            return headerDtos;
        }
    }
}
