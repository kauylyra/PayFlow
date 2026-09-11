using AutoMapper;
using PayFlow.application.Crosscuttint.Dtos.Request.Customers;
using PayFlow.application.Crosscuttint.Dtos.Request.Payments;
using PayFlow.application.Crosscuttint.Dtos.Response.Customers;
using PayFlow.application.Crosscuttint.Dtos.Response.Payments;
using PayFlow.domain.Models.Payment;
using PayFlow.Domain.Entities;

namespace PayFlow.application.Mapping;

public class MappingToDto : Profile
{
    public MappingToDto()
    {
        CreateMap<Customers, AddCustomersRequest>().ReverseMap();

        CreateMap<Customers, CustomersResponse>().ReverseMap();

        CreateMap<Customers, UpdateCustomerRequest>().ReverseMap();

        CreateMap<Payments, AddPaymentRequest>().ReverseMap();

        CreateMap<Payments, CustomerPaymentResponse>();

        CreateMap<Payments, PaymentItemResponse>();

        CreateMap<Customers, CustomerPaymentResponse>();

        CreateMap<PaymentMethod, PaymentMethodResponse>();

        CreateMap<FilterPayment, FilterPaymentRequest>().ReverseMap();
    }
}