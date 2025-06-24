using AutoMapper;
using InvoinceModule.Application.Dtos;
using InvoinceModule.Application.Ports.Out;
using InvoinceModule.Application.UseCases;
using InvoinceModule.Domain.Entities;
using Xunit;
using Moq;

public class InvoiceHeaderUseCaseTests
{
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IInvoiceRepositoryOutPort> _repoMock = new();
    private readonly Mock<IInvoiceNumberGeneratorOutPort> _numberGenMock = new();

    private readonly InvoiceHeaderUseCase _useCase;

    public InvoiceHeaderUseCaseTests()
    {
        _useCase = new InvoiceHeaderUseCase(_mapperMock.Object, _repoMock.Object, _numberGenMock.Object);
    }

    [Fact]
    public async Task CreateInvoiceAsync_ShouldCallValidationAndRepositories()
    {
        // Arrange
        var invoiceDto = new InvoiceDto();
        var invoiceEntity = new InvoiceHeader
        {
            Details = new List<InvoiceDetail> { new InvoiceDetail() }
        };

        _mapperMock.Setup(m => m.Map<InvoiceHeader>(invoiceDto)).Returns(invoiceEntity);
        _numberGenMock.Setup(n => n.GetNextSequenceAsync()).ReturnsAsync(123);
        _mapperMock.Setup(m => m.Map<InvoiceDto>(invoiceEntity)).Returns(invoiceDto);

        // Act
        var result = await _useCase.CreateInvoiceAsync(invoiceDto);

        // Assert
        _mapperMock.Verify(m => m.Map<InvoiceHeader>(invoiceDto), Times.Once);
        _numberGenMock.Verify(n => n.GetNextSequenceAsync(), Times.Once);
        _repoMock.Verify(r => r.AddAsync(invoiceEntity), Times.Once);
        _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        _mapperMock.Verify(m => m.Map<InvoiceDto>(invoiceEntity), Times.Once);
    }
}
