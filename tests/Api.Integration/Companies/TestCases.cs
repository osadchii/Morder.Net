using Infrastructure.MediatR.Companies.Commands;

namespace Api.Integration.Companies;

public static class TestCases
{
    public static UpdateCompanyInformationRequest UpdateCompanyInformationRequest => new()
    {
        Name = "Test company",
        Shop = "Test shop",
        Url = "http://test.url"
    };
}